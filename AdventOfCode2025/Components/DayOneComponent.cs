using System;
using System.Collections.Generic;
using System.Globalization;
using AoCHelper;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Graphics;
using MLEM.Maths;
using MonoGame.Aseprite;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.Tweening;

namespace AdventOfCode2025.Components;

public class DayOneComponent : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private Tweener _tweener;
    private Sprite _lockSprite;
    private Sprite _dialSprite;
    private Queue<(Direction2, int)> _instructionsQueue;
    private bool _rotating;
    private bool _interactive;
    private int _toDegrees;
    private int _zeroes;
    private (Direction2, int) _currentInstruction;
    private SpriteFont _font;
    private int _instructionCount;
    private int _clicks;
    private bool _passedZero;

    /// <inheritdoc />
    public DayOneComponent(Game game) : base(game)
    {
        
    }

    /// <inheritdoc />
    protected override void LoadContent()
    {
        using var stream = TitleContainer.OpenStream("Content/combination_lock.aseprite");
        var asepriteFile = AsepriteFileLoader.FromStream("combination_lock", stream, preMultiplyAlpha: true);
        var atlas = asepriteFile.CreateTextureAtlas(GraphicsDevice, mergeDuplicateFrames: true);
        _lockSprite = atlas.CreateSprite(regionIndex: 0);
        _dialSprite = atlas.CreateSprite(regionIndex: 1);
        _lockSprite.Origin = _dialSprite.Origin = new Vector2(128, 128);
        _dialSprite.Rotation = _dialSprite.Rotation = MathHelper.ToRadians(180);
    }

    /// <inheritdoc />
    public override void Initialize()
    {
        base.Initialize();
        GraphicsDevice.PresentationParameters.BackBufferWidth = 600;
        GraphicsDevice.PresentationParameters.BackBufferHeight = 600;
        _spriteBatch = Game.Services.GetService<SpriteBatch>();
        _font = Game.Services.GetService<SpriteFont>();
        _tweener = new Tweener();
        _toDegrees = 1800;

        var downLoader = Game.Services.GetService<InputDownloader>();
        // var input = downLoader.GetInput(1, 2025).Result;
        var input = "L68\nL30\nR48\nL5\nR60\nL55\nL1\nL99\nR14\nL82";
        _instructionsQueue = new Queue<(Direction2, int)>();
        foreach (var line in input.ReplaceLineEndings().TrimEnd().Split(Environment.NewLine))
        {
            var direction = line[0] switch
            {
                'L' => Direction2.Left,
                'R' => Direction2.Right,
                _ => throw new ArgumentOutOfRangeException()
            };
            var number = int.Parse(line[1..]);
            _instructionsQueue.Enqueue((direction, number));
        }

        _instructionCount = _instructionsQueue.Count;
    }

    /// <inheritdoc />
    public override void Update(GameTime gameTime)
    {
        var keyboardState = KeyboardExtended.GetState();
        if (!_rotating)
        {
            var previousKey = Keys.None;
            if (_interactive)
            {
                float turnValue = 0;
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    turnValue -= 0.5f;
                    if (turnValue < -1)
                    {
                        _toDegrees -= 36;
                    }

                    previousKey = Keys.Left;
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    turnValue += 0.5f;
                    if (turnValue > 1)
                    {
                        _toDegrees += 36;
                    }

                    previousKey = Keys.Right;
                }
            }
            else
            {
                // int iterationEnd = 0;
                // if (_instructionsQueue.Count < _instructionCount - 100)
                // {
                //     iterationEnd = (_instructionCount - _instructionsQueue.Count) - 100;
                // }
                //
                // for (int i = 0; i <= iterationEnd; i++)
                {
                    if (_instructionsQueue.Count == 0) return;
                    _currentInstruction = _instructionsQueue.Dequeue();
                    var degreeDelta = _currentInstruction.Item2 * 36;
                    if (_currentInstruction.Item1 == Direction2.Left)
                    {
                        _toDegrees -= degreeDelta;
                    }
                    else if (_currentInstruction.Item1 == Direction2.Right)
                    {
                        _toDegrees += degreeDelta;
                    }

                    var toNumber = calculateNumber(_toDegrees);

                    if (toNumber == 0)
                    {
                        _zeroes++;
                    }
                }
            }

            if (!_interactive || (_interactive && keyboardState.WasKeyReleased(previousKey)))
            {
                _tweener.TweenTo(target: _dialSprite, expression: dial => _dialSprite.Rotation,
                        toValue: MathHelper.ToRadians(_toDegrees / 10f),
                        duration: 1f,
                        delay: 0.0f)
                    .OnEnd(_ => _rotating = false);
                _rotating = true;
                _passedZero = false;
            }
        }
        else
        {
            _tweener.Update(gameTime.GetElapsedSeconds());
            var angle = new Angle(_dialSprite.Rotation);
            if (!_passedZero && angle.Radians.Equals(MathHelper.ToRadians(0), 10f / MathHelper.Pi))
            {
                _clicks++;
                _passedZero = true;
            }
        }
    }

    /// <inheritdoc />
    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.DrawString(_font, $"Zeroes {_zeroes.ToString(CultureInfo.InvariantCulture)}", Vector2.Zero,
            Color.White);
        _spriteBatch.DrawString(_font, _currentInstruction.Item1 + " " + _currentInstruction.Item2, new Vector2(200, 0),
            Color.White);
        _spriteBatch.DrawString(_font, _instructionCount - _instructionsQueue.Count + "/", new Vector2(0, 30),
            Color.White);
        _spriteBatch.DrawString(_font, _instructionCount.ToString(), new Vector2(200, 30), Color.White);
        _lockSprite.Draw(_spriteBatch, new Vector2(160, 200));
        _dialSprite.Draw(_spriteBatch, new Vector2(160, 200));
        _spriteBatch.DrawString(_font,
            calculateNumber(MathHelper.ToDegrees(_dialSprite.Rotation) * 10)
                .ToString(CultureInfo.InvariantCulture), new Vector2(160, 200),
            Color.White, 0, new Vector2(20, 20), 1.0f, SpriteEffects.None, 1f);
        _spriteBatch.DrawString(_font, _clicks.ToString(), new Vector2(160, 360), Color.White);
        _spriteBatch.End();
    }

    private static int calculateNumber(float degrees)
    {
        var returnNumber = (int)(degrees / 36f) % 100;
        if (returnNumber < 0)
        {
            returnNumber += 100;
        }
        else if (returnNumber > 99)
        {
            returnNumber -= 100;
        }

        return returnNumber;
    }
}
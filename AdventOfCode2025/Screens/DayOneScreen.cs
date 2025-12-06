using System;
using System.Collections.Generic;
using System.Globalization;
using AsepriteDotNet.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;

namespace AdventOfCode2025.Screens;

public class DayOneScreen : SolutionScreen
{
    private SpriteBatch _spriteBatch;
    private Sprite _lockSprite;
    private Sprite _dialSprite;
    private SpriteFont _font;
    private Queue<(Keys, int)> _instructionsQueue;
    private bool _rotating;
    private int _zeroes;
    private int _instructionCount;
    private Keys _turnKey;
    private int _clicksToGo;
    private int _zeroClicks;
    private int _number = 50;
    private bool _doorsOpen;
    private bool _run;
    private SoundEffect _clickSoundEffect;
    private SoundEffect _safeOpenSoundEffect;
    private SoundEffect _zeroClickSoundEffect;

    /// <inheritdoc />
    public DayOneScreen(Game game) : base(game)
    {
        _year = 2025;
        _day = 1;
        _isExample = false;
        _isInteractive = false;
        _exampleInput = "L68\nL30\nR48\nL5\nR60\nL55\nL1\nL99\nR14\nL82";
    }

    /// <inheritdoc />
    public override void LoadContent()
    {
        using var stream = TitleContainer.OpenStream("Content/combination_lock.aseprite");
        var asepriteFile = AsepriteFileLoader.FromStream("combination_lock", stream, preMultiplyAlpha: true);
        var atlas = asepriteFile.CreateTextureAtlas(GraphicsDevice, mergeDuplicateFrames: true);
        _lockSprite = atlas.CreateSprite(regionIndex: 0);
        _dialSprite = atlas.CreateSprite(regionIndex: 1);
        _lockSprite.Origin = _dialSprite.Origin = new Vector2(128, 128);
        _dialSprite.Rotation = _dialSprite.Rotation = MathHelper.ToRadians(180);
        _clickSoundEffect = Content.Load<SoundEffect>("Sounds/Effects/dial_turn_click");
        _safeOpenSoundEffect = Content.Load<SoundEffect>("Sounds/Effects/unlock_safe");
        _zeroClickSoundEffect = Content.Load<SoundEffect>("Sounds/Effects/zero_click");
    }

    /// <inheritdoc />
    public override void Initialize()
    {
        base.Initialize();
        _spriteBatch = Game.Services.GetService<SpriteBatch>();
        _font = Game.Services.GetService<SpriteFont>();

        var input = _isExample ? _exampleInput : _puzzleInput;

        _instructionsQueue = new Queue<(Keys, int)>();
        foreach (var line in input.ReplaceLineEndings().TrimEnd().Split(Environment.NewLine))
        {
            var direction = line[0] switch
            {
                'L' => Keys.Left,
                'R' => Keys.Right,
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
        base.Update(gameTime);
        if (_doorsOpen)
        {
            return;
        }

        if (!_isExample && !_isInteractive)
        {
            while (_instructionsQueue.Count != 0)
            {
                (_turnKey, _clicksToGo) = _instructionsQueue.Dequeue();
                while (_clicksToGo != 0) {
                    TurnKey(_turnKey);
                }
            }

            return;
        }

        if (!_rotating)
        {
            if (_instructionsQueue.Count != 0)
            {
                (_turnKey, _clicksToGo) = _instructionsQueue.Dequeue();
                _rotating = true;
            }
        }

        if (_isInteractive)
        {
            var keys = Keys.None;
            
            if (_inputHandler.IsPressed(Keys.Left))
            {
                keys = Keys.Left;
            }
            else if (_inputHandler.IsPressed(Keys.Right))
            {
                keys = Keys.Right;
            }

            TurnKey(keys);
        }
        else
        {
            for (int i = 0; i < _clicksToGo; i++)
            {
                TurnKey(_turnKey);
            }
        }
    }

    /// <inheritdoc />
    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.DrawString(_font, $"Zeroes {_zeroes.ToString(CultureInfo.InvariantCulture)}", Vector2.Zero,
            Color.White);
        _spriteBatch.DrawString(_font, _turnKey + " " + _clicksToGo, new Vector2(200, 0),
            Color.White);
        _spriteBatch.DrawString(_font, _instructionCount - _instructionsQueue.Count + "/", new Vector2(0, 30),
            Color.White);
        _spriteBatch.DrawString(_font, _instructionCount.ToString(), new Vector2(200, 30), Color.White);
        _lockSprite.Draw(_spriteBatch, new Vector2(160, 200));
        _dialSprite.Draw(_spriteBatch, new Vector2(160, 200));
        _spriteBatch.DrawString(_font,
            _number.ToString(), new Vector2(160, 200),
            Color.White, 0, new Vector2(20, 20), 1.0f, SpriteEffects.None, 1f);
        _spriteBatch.DrawString(_font, "0 Clicks", new Vector2(40, 360), Color.White);
        _spriteBatch.DrawString(_font, _zeroClicks.ToString(), new Vector2(160, 360), Color.White);
        _spriteBatch.End();
    }

    private void TurnKey(Keys keys)
    {
        if (keys != Keys.None)
        {
            if (keys is Keys.Left or Keys.Right && _isInteractive)
            {
                _zeroClickSoundEffect.Play();
            }

            if (keys == Keys.Left)
            {
                _number--;
            }
            else
            {
                _number++;
            }

            _number = _number switch
            {
                < 0 => 99,
                > 99 => 0,
                _ => _number
            };
            if (keys == _turnKey)
            {
                _clicksToGo--;
                if (_number == 0)
                {
                    _zeroClicks++;
                }
            }
            else
            {
                _clicksToGo++;
                if (_number == 0)
                {
                    _zeroClicks--;
                }
            }

            if (_clicksToGo == 0)
            {
                _clickSoundEffect.Play();
                if (_number == 0)
                {
                    _zeroes++;
                }

                _rotating = false;
                if (_instructionsQueue.Count == 0)
                {
                    _doorsOpen = true;
                    _safeOpenSoundEffect.Play();
                }
            }

            _dialSprite.Rotation = MathHelper.ToRadians(_number * 3.6f);
        }
    }
}
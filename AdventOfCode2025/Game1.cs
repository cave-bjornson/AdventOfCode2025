using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.NetworkInformation;
using AdventOfCode2025.Components;
using AoCHelper;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;

namespace AdventOfCode2025;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        // _tweener = new Tweener();
        Components.Add(new DayOneComponent(this));
    }

    protected override void Initialize()
    {
        // Add your initialization logic here
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Services.AddService(_spriteBatch);
        _font = Content.Load<SpriteFont>("font");
        Services.AddService(_font);
        var sessionId =
            "";
        Services.AddService(new InputDownloader(sessionId));
        base.Initialize();
        // _toDegrees = 1800;
    }

    protected override void LoadContent()
    {
        // Use this.Content to load your game content here

        // AsepriteFile asepriteFile;
        // using var stream = TitleContainer.OpenStream("Content/combination_lock.aseprite");
        // asepriteFile = AsepriteFileLoader.FromStream("combination_lock", stream, preMultiplyAlpha: true);
        // _atlas = asepriteFile.CreateTextureAtlas(GraphicsDevice, mergeDuplicateFrames: true);
        // _lockSprite = _atlas.CreateSprite(regionIndex: 0);
        // _dialSprite = _atlas.CreateSprite(regionIndex: 1);
        // _lockSprite.Origin = _dialSprite.Origin = new Vector2(128, 128);
        // _dialSprite.Rotation = _dialSprite.Rotation = MathHelper.ToRadians(180);
        //
        // List<(Direction, int)> _instructions =
        // [
        //     (Direction.Left, 68),
        //     (Direction.Left, 30),
        //     (Direction.Right, 48),
        //     (Direction.Left, 5),
        //     (Direction.Right, 60),
        //     (Direction.Left, 55),
        //     (Direction.Left, 1),
        //     (Direction.Left, 99),
        //     (Direction.Right, 14),
        //     (Direction.Left, 82)
        // ];
        // _instructionsStack = new Queue<(Direction, int)>(_instructions);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // Add your update logic here
        KeyboardExtended.Update();
        // var keyboardState = KeyboardExtended.GetState();
        //
        // if (!_rotating)
        // {
        //     if (_interactive)
        //     {
        //         if (keyboardState.IsKeyDown(Keys.Left))
        //         {
        //             _turnValue -= 0.5f;
        //             if (_turnValue < -1)
        //             {
        //                 _toDegrees -= 36;
        //                 _turnValue = 0;
        //             }
        //
        //             previousKey = Keys.Left;
        //         }
        //         else if (keyboardState.IsKeyDown(Keys.Right))
        //         {
        //             _turnValue += 0.5f;
        //             if (_turnValue > 1)
        //             {
        //                 _toDegrees += 36;
        //                 _turnValue = 0;
        //             }
        //
        //             previousKey = Keys.Right;
        //         }
        //     }
        //     else
        //     {
        //         if (_instructionsStack.Count == 0) return;
        //         (_currentDirection, _currentNumber) = _instructionsStack.Dequeue();
        //         if (_currentDirection == Direction.Left)
        //         {
        //             _toDegrees -= _currentNumber * 36;
        //         }
        //         else if (_currentDirection == Direction.Right)
        //         {
        //             _toDegrees += _currentNumber * 36;
        //         }
        //     }
        //
        //     _toNumber = calculateNumber(_toDegrees);
        //
        //     if (_toNumber == 0)
        //     {
        //         _zeroes++;
        //     }
        //
        //     if (!_interactive || (_interactive && keyboardState.WasKeyReleased(previousKey)))
        //     {
        //         _tweener.TweenTo(target: _dialSprite, expression: dial => _dialSprite.Rotation,
        //                 toValue: MathHelper.ToRadians(_toDegrees / 10f), duration: 0.5f,
        //                 delay: 0.1f)
        //             .Easing(EasingFunctions.SineInOut).OnEnd(_ => _rotating = false);
        //         _rotating = true;
        //         previousKey = Keys.None;
        //     }
        // }
        // else
        // {
        //     _tweener.Update(gameTime.GetElapsedSeconds());
        // }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Add your drawing code here
        // _spriteBatch.Begin();
        // _spriteBatch.DrawString(_font, $"Zeroes {_zeroes.ToString(CultureInfo.InvariantCulture)}", Vector2.Zero,
        //     Color.White);
        // _lockSprite.Draw(_spriteBatch, new Vector2(200, 200));
        // _dialSprite.Draw(_spriteBatch, new Vector2(200, 200));
        // _spriteBatch.DrawString(_font,
        //     calculateNumber(MathHelper.ToDegrees(_dialSprite.Rotation) * 10)
        //         .ToString(CultureInfo.InvariantCulture), new Vector2(200, 200),
        //     Color.White, 0, new Vector2(20, 20), 1.0f, SpriteEffects.None, 1f);
        // _spriteBatch.DrawString(_font, _currentDirection + " " + _currentNumber, new Vector2(200, 0), Color.White);
        // _spriteBatch.End();
        base.Draw(gameTime);
    }

    // public int calculateNumber(float degrees)
    // {
    //     var returnNumber = (int)(degrees / 36f) % 100;
    //     if (returnNumber < 0)
    //     {
    //         returnNumber += 100;
    //     }
    //     else if (returnNumber > 99)
    //     {
    //         returnNumber -= 100;
    //     }
    //
    //     return returnNumber;
    // }
}
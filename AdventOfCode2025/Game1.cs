using AdventOfCode2025.Screens;
using AdventOfCode2025.Settings;
using AoCHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Input;
using MonoGame.Extended.Screens;

namespace AdventOfCode2025;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private readonly ScreenManager _screenManager;
    private readonly InputHandler _inputHandler;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private readonly SettingsManager<SessionSettings> _settingsManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);

        // Share GraphicsDeviceManager as a service.
        Services.AddService(typeof(GraphicsDeviceManager), _graphics);

        // Determine the appropriate settings storage based on the platform.
        ISettingsStorage storage = new DesktopSettingsStorage();
        _graphics.IsFullScreen = false;
        IsMouseVisible = true;

        // // Initialize settings managers.
        _settingsManager = new SettingsManager<SessionSettings>(storage);
        Services.AddService(typeof(SettingsManager<SessionSettings>), _settingsManager);

        Content.RootDirectory = "Content";
        _screenManager = new ScreenManager();
        Components.Add(_screenManager);
        _inputHandler = new InputHandler(this);
        Services.AddService(_inputHandler);
    }

    protected override void Initialize()
    {
        // Add your initialization logic here
        base.Initialize();
        var sessionId = _settingsManager.Settings.sessionKey;
        if (sessionId != null)
        { 
            Services.AddService(new InputDownloader(sessionId));
        }
        _screenManager.ShowScreen(new MainMenuScreen(this));
    }

    protected override void LoadContent()
    {
        // Use this.Content to load your game content here
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Services.AddService(_spriteBatch);
        _font = Content.Load<SpriteFont>("font");
        Services.AddService(_font);
    }

    protected override void Update(GameTime gameTime)
    {
        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
        //     Keyboard.GetState().IsKeyDown(Keys.Escape))
        //     Exit();
        // Add your update logic here
        // KeyboardExtended.Update();
        _inputHandler.Update();
        base.Update(gameTime);
    }
}
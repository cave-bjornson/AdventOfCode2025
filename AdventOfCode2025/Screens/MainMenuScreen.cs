using System;
using System.Collections.Generic;
using System.Globalization;
using AdventOfCode2025.Settings;
using AoCHelper;
using Gum.Forms;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGameGum;

namespace AdventOfCode2025.Screens;

public class MainMenuScreen : GameScreen
{
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private Vector2 _titlePosition;
    private SettingsManager<SessionSettings> _settingsManager;
    private Dictionary<int, Screen> _solutions;
    
    GumService GumUI => GumService.Default;

    /// <inheritdoc />
    public MainMenuScreen(Game game) : base(game)
    {
        _solutions = new Dictionary<int, Screen>()
        {
            [1] = new DayOneScreen(Game)
        };
    }

    /// <inheritdoc />
    public override void LoadContent()
    {
        _spriteBatch = Services.GetService<SpriteBatch>();
        _font = Services.GetService<SpriteFont>();
        _titlePosition = new Vector2(100, 50);
    }

    /// <inheritdoc />
    public override void Initialize()
    {
        _settingsManager = Services.GetService<SettingsManager<SessionSettings>>();
        _settingsManager.SettingsSaved += settings =>
        {
            Services.RemoveService(typeof(InputDownloader));
            Services.AddService(new InputDownloader(settings.sessionKey));
        };
        
        GumUI.Initialize(Game, DefaultVisualsVersion.V2);

        var textBox = new TextBox
        {
            X = 50,
            Y = 50,
            Width = 200,
            Height = 34,
            Placeholder = "SessionId...",
            Text = _settingsManager.Settings.sessionKey ?? ""
        };
        textBox.AddToRoot();
        
        var button = new Button
        {
            X = 50,
            Y = 100,
            Width = 200,
            Height = 50,
            Text = "Save Session Key"
        };
        button.Click += (_, _) =>
        {
            _settingsManager.Settings.sessionKey = textBox.Text;
            _settingsManager.Save();
        };
        button.AddToRoot();
        
        var listBox = new ListBox
        {
            X = 300,
            Y = 50,
            Width = 200,
            Height = 400
        };
        listBox.AddToRoot();
        foreach (var (key, _) in _solutions)
        {
            listBox.Items.Add(key);
        }

        listBox.ItemClicked += (sender, args) =>
        {
            var item = (ListBoxItem)sender;
            var day = (int)item.BindingContext;
            ScreenManager.ShowScreen(_solutions[day]);
        };
    }

    /// <inheritdoc />
    public override void Update(GameTime gameTime)
    {
        GumUI.Update(gameTime);
    }

    /// <inheritdoc />
    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        GumUI.Draw();
    }
}
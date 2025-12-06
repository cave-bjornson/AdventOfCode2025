using AoCHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MLEM.Input;
using MonoGame.Extended.Screens;

namespace AdventOfCode2025.Screens;

public class SolutionScreen : GameScreen
{
    protected readonly InputHandler _inputHandler;
    protected readonly InputDownloader _downloader;

    protected bool _isExample;
    protected bool _isInteractive;
    protected string _exampleInput;
    protected string _puzzleInput;
    protected int _day, _year;

    /// <inheritdoc />
    protected SolutionScreen(Game game) : base(game)
    {
        _inputHandler = Services.GetService<InputHandler>();
        _downloader = Services.GetService<InputDownloader>();
    }

    /// <inheritdoc />
    public override void Initialize()
    {
        _puzzleInput = _downloader.GetInput(_day, _year).Result;
    }

    /// <inheritdoc />
    public override void Update(GameTime gameTime)
    {
        if (_inputHandler.IsPressed(Keys.Escape))
        {
            ScreenManager.CloseScreen();
        }
    }

    /// <inheritdoc />
    public override void Draw(GameTime gameTime)
    {
        
    }
}
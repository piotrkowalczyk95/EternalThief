using EternalThief.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EternalThief
{
    //TODO Wykorzystanie klasy Resolution w celu dostowania dowolnej wielkości ekranu (do potrzeb)  
    // Poprawienie kamery w taki sposób by nie było widać obszaru po za tilemapą


    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _uiSpriteBatch;

        private bool pausedGame = false;
        private bool pauseButtenPressed = false;
        private Level1 level;
        private Menu menu;

        static int ResolutionWidth = 1200; //rozmiar okna
        static int ResolutionHeight = 800;

        GameState gameState = GameState.MENU;
        GameUI gameUI;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = ResolutionWidth;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = ResolutionHeight;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            Resolution.Init(ref _graphics);
            Window.AllowUserResizing = false;
            Resolution.SetResolution(ResolutionWidth, ResolutionHeight, false);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            menu = new Menu(ResolutionWidth, ResolutionHeight, this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            menu.Load(delegate
            {
                level = new Level1(ResolutionWidth, ResolutionHeight, this);
                level.Initialize();
                level.LoadContent(GraphicsDevice);
                gameUI = new GameUI(GraphicsDevice);
                gameUI.initUI(null, 200000);
                gameUI.Load(Content);
                gameState = GameState.RUN;
            });
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _uiSpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (gameState == GameState.MENU)
            {
                menu.Update(gameTime);
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    gameState = GameState.MENU;
                    level.stopGame();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.P) && !pauseButtenPressed)
                {
                    pauseButtenPressed = true;
                    pausedGame = !pausedGame;
                    gameUI.gameIsPaused(pausedGame);
                }
                else if (Keyboard.GetState().IsKeyUp(Keys.P) && pauseButtenPressed)
                {
                    pauseButtenPressed = false;
                }
                if (!pausedGame)
                {
                    unPausedGameUpdate(gameTime);
                }
            }
            base.Update(gameTime);
        }

        private void unPausedGameUpdate(GameTime gameTime)
        {
            gameUI.Update(gameTime);
            level.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (gameState == GameState.RUN)
            {
                level.Draw(gameTime, _spriteBatch);
                gameUI.draw(_uiSpriteBatch);
            }
            else
            {
                menu.Draw(_spriteBatch);
            }
        }
    }
}
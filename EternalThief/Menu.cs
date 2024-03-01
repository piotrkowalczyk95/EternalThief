using EternalThief.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EternalThief
{
    class Menu
    {
        private int resolutionWidth;
        private int resolutionHeight;
        private Game game;
        private Button buttonStartGame;
        private Button buttonExit;

        public Menu(int resolutionWidth, int resolutionHeight, Game game)
        {
            this.resolutionWidth = resolutionWidth;
            this.resolutionHeight = resolutionHeight;
            this.game = game;
        }

        public void Load(EventHandler Click)
        {
            var font = game.Content.Load<SpriteFont>("Font/VCR_OSD_MONO_1.001");
            buttonStartGame = new Button(font, game.GraphicsDevice, 100, 100, "Start");
            buttonExit = new Button(font, game.GraphicsDevice, 100, 500, "Exit");
            buttonStartGame.Click += Click;
            buttonExit.Click += delegate
            {
                game.Exit();
            };
        }

        internal void Update(GameTime gameTime)
        {
            buttonStartGame.Update(gameTime);
            buttonExit.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            buttonStartGame.Draw(spriteBatch);
            buttonExit.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
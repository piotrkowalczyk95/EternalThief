using EternalThief.actors.characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EternalThief.UI
{
    class GameUI
    {
        private GraphicsDevice graphicsDevice;
        private Hero hero;
        private CountDown countDown;
        private SpriteFont font;
        private Vector2 timerPosition = new Vector2(0, 10);
        private Vector2 pausePosition = new Vector2(0, 0);
        private Texture2D blackEffectRectangle;
        private Effect effect;
        private long maxProcentageR;
        private bool gamePaused = false;

        public GameUI(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            blackEffectRectangle = new Texture2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            Color[] data = new Color[graphicsDevice.Viewport.Height * graphicsDevice.Viewport.Width];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Black;
            blackEffectRectangle.SetData(data);
        }

        public void initUI(Hero hero, long timeForCountDown)
        {
            this.hero = hero;
            countDown = new CountDown(timeForCountDown);
            maxProcentageR = timeForCountDown;
        }

        public void Update(GameTime gameTime)
        {
            countDown.Update(gameTime);
            timerPosition.X = graphicsDevice.Viewport.Width - 50;
            pausePosition.X = graphicsDevice.Viewport.Width / 2;
            pausePosition.Y = graphicsDevice.Viewport.Height / 2;
            var time = countDown.getCurrentTime();
            var procentage = time / maxProcentageR;
            effect.Parameters["r"].SetValue(procentage);
        }

        internal void Load(ContentManager content)
        {
            font = content.Load<SpriteFont>("Font/VCR_OSD_MONO_1.001");
            effect = content.Load<Effect>("Shaders/UiBlackScreeneffect");
            effect.Parameters["r"].SetValue((float)1);
        }

        internal void gameIsPaused(bool pausedGame)
        {
            gamePaused = pausedGame;
        }
        public void draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, effect, null);
            _spriteBatch.Draw(blackEffectRectangle, new Vector2(0, 0), Color.Black);
            _spriteBatch.End();
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, countDown.getCurrentTimeString(), timerPosition, Color.White);
            if (gamePaused)
            {
                _spriteBatch.DrawString(font, "PAUSE", pausePosition, Color.White);
            }
            _spriteBatch.End();
        }
    }
}
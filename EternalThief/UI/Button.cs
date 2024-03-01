using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EternalThief.UI
{
    public class Button
    {

        private MouseState _currentMouse;

        private SpriteFont _font;

        private bool _isHovering;

        private MouseState _previousMouse;

        private Texture2D buttoRectangle;

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }


        public string Text { get; set; }

        private int width = 300;

        private int height = 150;

        private int px = 0;
        private int py = 0;

        Rectangle Rectangle
        {
            get
            {
                return new Rectangle(px, py, width, height);
            }
        }

        public Button(SpriteFont font, GraphicsDevice graphicsDevice, int x, int y, String text)
        {
            buttoRectangle = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            buttoRectangle.SetData(data);
            this._font = font;

            this.Text = text;
            this.px = x;
            this.py = y;

            PenColour = Color.Green;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(buttoRectangle, new Vector2(px, py), Color.White);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (px + (width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (py + (height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
        }

        public void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
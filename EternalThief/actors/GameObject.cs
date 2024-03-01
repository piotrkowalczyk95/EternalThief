using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EternalThief.actors
{
    public class GameObject
    {
        protected Texture2D texture;
        public Vector2 position;
        protected Vector2 center;
        private Color tintColor = Color.White;
        public float scale = 0.5f;
        public float rotation = 0.0f;
        public float layerDepth = 0.5f;
        public bool active = true;

        public bool isCollidable = true;
        protected int boundingBoxWidth, boundingBoxHeight;
        protected Vector2 boundingBoxOffset = Vector2.Zero;

        public Vector2 direction = new Vector2(1, 0);
        public Vector2 startPosition = new Vector2(-1, -1);

        public virtual Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)(position.X + boundingBoxOffset.X), (int)(position.Y + boundingBoxOffset.Y), boundingBoxWidth, boundingBoxHeight);
            }
        }

        public virtual void Initialize()
        {
            if (startPosition == new Vector2(-1, -1))
            {
                startPosition = position;
            }
        }

        public virtual void SetToDefaultPosition()
        {
            position = startPosition;
        }

        public virtual void Load()
        {
            CalculateCenter();

            if (texture != null)
            {
                boundingBoxWidth = texture.Width;
                boundingBoxHeight = texture.Height;
            }
        }

        public virtual bool CheckCollision(Rectangle input)
        {
            return BoundingBox.Intersects(input);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, tintColor, rotation, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        }

        private void CalculateCenter()
        {
            if (texture == null)
                return;
            center.X = texture.Width / 2;
            center.Y = texture.Height / 2;
        }
    }
}

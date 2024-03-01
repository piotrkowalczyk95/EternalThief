using Microsoft.Xna.Framework;

namespace EternalThief
{
    public class Camera
    {
        Vector2 position;
        Matrix viewMatrix;
        int screenWidth, screenHeight;
        public Rectangle heroBoundingBox;
        public Camera(int screenWidth, int screenHeight, Rectangle heroBoundingBox)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }
        public Matrix VievMatrix
        {
            get { return viewMatrix; }
        }

        public void Update(Vector2 playerPosition, float scale, TiledMap tiledMap)
        {
            position.X = playerPosition.X + heroBoundingBox.Width / 2 - screenWidth / 2;
            position.Y = playerPosition.Y + heroBoundingBox.Height / 2 - screenHeight / 2;

            if (position.X < 0)
                position.X = 0;

            if (position.Y < 0)
                position.Y = 0;

            if (position.X > tiledMap.size.Width - screenWidth)
                position.X = tiledMap.size.Width - screenWidth;

            if (position.Y > tiledMap.size.Height - screenHeight)
                position.Y = tiledMap.size.Height - screenHeight;

            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0)) * Matrix.CreateScale(scale);
        }
    }
}
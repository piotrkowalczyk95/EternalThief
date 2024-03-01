using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EternalThief.actors.characters;

namespace EternalThief.actors
{
    class StunDart: GameObject
    {
        Texture2D texture;
        public StunDart(Vector2 position, Vector2 direction, ContentManager content)
        {
            this.position = position;
            this.direction = direction;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Load(ContentManager content)
        {
            if (direction.X == -1)
            {
                texture = content.Load<Texture2D>("Items/DartLeft");
            }
            else
            {
                texture = content.Load<Texture2D>("Items/DartRight");
            }
            boundingBoxWidth = (int)texture.Width;
            boundingBoxHeight = (int)texture.Height;
        }

        public void Update(List<GameObject> gameObjects, GameTime gameTime, TiledMap map)
        {
            if (direction.X == -1)
            {
                this.position.X -= 3.0f;
            }
            else
            {
                this.position.X += 3.0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Begin(transformMatrix: camera.VievMatrix);
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White);
            spriteBatch.End();
        }

        public bool MapCollision(TiledMap tiledMap)
        {
            Rectangle dart = new Rectangle((int)position.X, (int)position.Y, boundingBoxWidth, boundingBoxHeight);
            if (tiledMap.CheckCollision(dart) != Rectangle.Empty){
                return true;
            }
            return false;
        }

        public bool SkeletonCollision(Skeleton skeleton)
        {
            Rectangle dart = new Rectangle((int)position.X, (int)position.Y, boundingBoxWidth, boundingBoxHeight);
            if(dart.Y + boundingBoxHeight > skeleton.position.Y + 10 && dart.Y < skeleton.position.Y + skeleton.height)
            {
                if(direction.X == -1)
                {
                    if(dart.X < skeleton.position.X + skeleton.width)
                    {
                        return true;
                    }
                }
                else
                {
                    if(dart.X + boundingBoxWidth > skeleton.position.X)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

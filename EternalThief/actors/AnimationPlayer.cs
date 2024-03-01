using EternalThief.actors.characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EternalThief.actors
{
    public class AnimationPlayer
    {
        private Sprite _sprite;
        private float _timer;
        private bool isJumping = false;
        private int direction = 1;
        private int temp = 0;
        
        public AnimationPlayer(Sprite sprite)
        {
            _sprite = sprite;
        }

        public Vector2 GetTextureSize()
        {
            return (new Vector2(_sprite.FrameWidth, _sprite.FrameHeight));
        }

        public void Play(Sprite sprite)
        {
            if (_sprite == sprite)
            {
                return;
            }

            _sprite = sprite;

            if (_sprite.Texture.Name.Contains("Left") && !(_sprite.Texture.Name.Contains("Fall")))
            {
                if(isJumping == true)
                {
                    _sprite.CurrentFrame = temp;
                }
                else
                {
                   _sprite.CurrentFrame = _sprite.FrameCount - 1;
                }
            }
            else if (!(_sprite.Texture.Name.Contains("Fall")))
            {
                if(isJumping == true)
                {
                    _sprite.CurrentFrame = temp;
                }
                else
                {
                    _sprite.CurrentFrame = 0;
                }
            }

            _timer = 0;
        }

        public void oneFrame(Sprite sprite, int frame) {
            _sprite = sprite;
            _sprite.CurrentFrame = frame;
            _timer = 0;
        }

        public void Stop()
        {
            _timer = 0f;

            _sprite.CurrentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Effect effect, Camera camera)
        {
            if(effect != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, effect, camera.VievMatrix);
                spriteBatch.Draw(_sprite.Texture,
                                 position,
                                 new Rectangle(_sprite.CurrentFrame * _sprite.FrameWidth,
                                               0,
                                               _sprite.FrameWidth,
                                              _sprite.FrameHeight),
                                 Color.White);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(transformMatrix: camera.VievMatrix);
                spriteBatch.Draw(_sprite.Texture,
                                 position,
                                 new Rectangle(_sprite.CurrentFrame * _sprite.FrameWidth,
                                               0,
                                               _sprite.FrameWidth,
                                              _sprite.FrameHeight),
                                 Color.White);
                spriteBatch.End();
            }
            
        }

        public virtual void Update(List<GameObject> gameobjects, GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > _sprite.FrameSpeed)
            {
                _timer = 0f;

                if (_sprite.Texture.Name.Contains("Left"))
                {
                    _sprite.CurrentFrame--;

                    if (_sprite.Texture.Name.Contains("Jump"))
                    {
                        if (_sprite.CurrentFrame < 0)
                        {
                            _sprite.CurrentFrame = 0;
                        }
                        if(direction == 1)
                        {
                            temp = (_sprite.FrameCount - _sprite.CurrentFrame - 1) % _sprite.FrameCount;
                        }
                        
                        isJumping = true;
                    }
                    else
                    {
                        isJumping = false;
                        if (_sprite.CurrentFrame < 0)
                        {
                            _sprite.CurrentFrame = _sprite.FrameCount - 1;
                        }
                    }

                    direction = -1;

                }
                else
                {
                    _sprite.CurrentFrame++;

                    if (_sprite.Texture.Name.Contains("Jump"))
                    {
                        if (_sprite.CurrentFrame >= _sprite.FrameCount)
                        {
                            _sprite.CurrentFrame  = _sprite.FrameCount - 1;
                        }

                        if (direction == -1)
                        {
                            temp = (_sprite.FrameCount - _sprite.CurrentFrame - 1) % _sprite.FrameCount;
                        }
                       
                        isJumping = true;
                    }
                    else
                    {
                        isJumping = false;
                        if (_sprite.CurrentFrame >= _sprite.FrameCount)
                        {
                            _sprite.CurrentFrame = 0;
                        }
                    }

                    direction = 1;
                }

            }
        }
    }
}

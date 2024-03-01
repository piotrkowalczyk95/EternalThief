using EternalThief.actors.characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EternalThief.actors
{
    class AnimatedObject : GameObject
    {
        protected AnimationPlayer _animationPlayer;

        protected Dictionary<string, Sprite> _animations;

        private bool action = false;

        public Effect effect;

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Load(ContentManager content)
        {
            base.Load();
        }

        public void Draw(SpriteBatch spriteBatch, Effect effect, Camera camera)
        {

            if (texture != null)
            {
                base.Draw(spriteBatch);
            }
            else if (_animationPlayer != null)
            {
                _animationPlayer.Draw(spriteBatch, position, effect, camera);
            }
            else
            {
                throw new Exception("Error loading");
            }
        }

        protected void SetAnimation(Vector2 velocity, Vector2 direction)
        {
            if (action)
            {
                if (direction.X == 1)
                {
                    _animationPlayer.Play(_animations["Throw"]);
                }
                else
                {
                    _animationPlayer.Play(_animations["ThrowLeft"]);
                }
            }
            else if (velocity.Y > 0)
            {
                if (direction.X == 1)
                {
                    _animationPlayer.Play(_animations["Fall"]);
                }
                else
                {
                    _animationPlayer.Play(_animations["FallLeft"]);
                }
            }
            else if (velocity.Y < 0)
            {
                if (direction.X == 1)
                {
                    _animationPlayer.Play(_animations["JumpRight"]);
                }
                else
                {
                    _animationPlayer.Play(_animations["JumpLeft"]);
                }
            }
            else
            {
                if (velocity.X > 0)
                {
                    _animationPlayer.Play(_animations["Right"]);
                }
                else if (velocity.X < 0)
                {
                    _animationPlayer.Play(_animations["Left"]);
                }
                else if (velocity.X == 0 && velocity.Y == 0 && action == false)
                {
                    if (direction.X == 1)
                    {
                        _animationPlayer.Play(_animations["Idle"]);
                    }
                    else if (direction.X == -1)
                    {
                        _animationPlayer.Play(_animations["IdleLeft"]);
                    }
                }
            }
        }

        protected void SetAttackAnimation(Vector2 direction)
        {
            if(direction.X == -1)
            {
                _animationPlayer.Play(_animations["AttackLeft"]);
            }
            else
            {
                _animationPlayer.Play(_animations["AttackRight"]);
            }
        }

        protected void setAnimationClimbing(Vector2 moving) 
        {
            if (moving.Y != 0) 
            {
                _animationPlayer.Play(_animations["Climb"]);
            } 
            else 
            {
                _animationPlayer.oneFrame(_animations["Climb"], 3);
            }
        }
 
        protected virtual void Update(List<GameObject> gameObjects, GameTime gameTime, Vector2 velocity, Vector2 direction, Movment movment, bool action)
        {
            if (_animationPlayer != null && _animations != null)
            {
                this.action = action;
                switch (movment) {
                    case Movment.IDLE: 
                        {
                            SetAnimation(velocity, direction);                       
                            break;
                        }
                    case Movment.CLIMBING: 
                        {
                            setAnimationClimbing(direction);
                            break;
                        }
                    case Movment.ATTACKING:
                        {
                            SetAttackAnimation(direction);
                            break;
                        }
                }
            }
        }
    }
}

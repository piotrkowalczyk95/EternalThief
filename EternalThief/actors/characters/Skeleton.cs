using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace EternalThief.actors.characters
{
    class Skeleton : Character
    {
        public bool stunned = false;
        private const float _stunDelay = 3000;
        private float _remainingStunDelay = _stunDelay;
        private float stunTimer;
        public int height;
        public int width;

        public Skeleton(Vector2 startingPosition)
        {
            this.startPosition = startingPosition;
            position = startingPosition;
            this.acceleration = 0.8f;
            this.deceleration = 0.54f;
            this.maxSpeed = 4.0f;
            this.isCollidable = false;
            this._delay = 3000;
            this._cooldownTime = 10000;
            this._remainingDelay = _delay;
            this._remainingCooldownTime = _cooldownTime;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Load(ContentManager content)
        {
            _animations = new Dictionary<string, Sprite>()
            {
                {"Idle", new Sprite(content.Load<Texture2D>("Undead/Idle"),18) },
                {"IdleLeft", new Sprite(content.Load<Texture2D>("Undead/IdleLeft"),18) },
                {"Right", new Sprite(content.Load<Texture2D>("Undead/WalkRight"),20) },
                {"Left", new Sprite(content.Load<Texture2D>("Undead/WalkLeft"),20) },
                {"AttackLeft", new Sprite(content.Load<Texture2D>("Undead/AttackLeft"),20) },
                {"AttackRight", new Sprite(content.Load<Texture2D>("Undead/AttackRight"),20) },
                {"JumpRight", new Sprite(content.Load<Texture2D>("Undead/WalkRight"),20) },
                {"JumpLeft", new Sprite(content.Load<Texture2D>("Undead/AttackRight"),20) },
                {"Fall", new Sprite(content.Load<Texture2D>("Undead/WalkRight"),20) },
                {"FallLeft", new Sprite(content.Load<Texture2D>("Undead/AttackRight"),20) },
                {"Throw", new Sprite(content.Load<Texture2D>("Undead/AttackLeft"),20) },
                {"ThrowLeft", new Sprite(content.Load<Texture2D>("Undead/AttackRight"),20) },
            };

            _animationPlayer = new AnimationPlayer(_animations.First().Value);

            effect = content.Load<Effect>("Shaders/EnemyStun");
        
            base.Load();

        }

        public void Update(List<GameObject> gameObjects, GameTime gameTime, Vector2 heroPosition, TiledMap map)
        {
            _animationPlayer.Update(gameObjects, gameTime);
            boundingBoxWidth = (int)_animationPlayer.GetTextureSize().X;
            boundingBoxHeight = (int)_animationPlayer.GetTextureSize().Y;
            width = boundingBoxWidth;
            height = boundingBoxHeight;
            if (stunned)
            {
                stunTimer = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                _remainingStunDelay -= stunTimer;
                if(_remainingStunDelay <= 0)
                {
                    stunned = false;
                    _remainingStunDelay = _stunDelay;
                }
            }
            else
            {
                if (!actionTaken)
                {
                    Move(gameObjects, heroPosition, map);
                }
            }
            base.Update(gameObjects, gameTime, map);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            if (stunned)
            {
                effect.Parameters["sinFunc"].SetValue((float)Math.Sin((float)_remainingStunDelay/100.0));
                effect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
                effect.CurrentTechnique.Passes[0].Apply();
                base.Draw(spriteBatch, effect, camera);
            }
            else
            {
                base.Draw(spriteBatch, null, camera);
            }
        }

        private void Move(List<GameObject> gameObjects, Vector2 heroPosition, TiledMap map)
        {
            var distanceX = Math.Abs((this.position.X + 24) - heroPosition.X);
            var distanceY = Math.Abs(heroPosition.Y - (this.position.Y + 17));

            if (distanceX >= 100 || distanceY >= 40)
            {
                if(direction.X == -1)
                {
                    MoveLeft();
                    if (NearWall(map) || !PatrolGround(map))
                    {
                        direction.X = 1;
                    }
                }
                else
                {
                    MoveRight();
                    if (NearWall(map) || !PatrolGround(map))
                    {
                        direction.X = -1;
                    }
                }
            }
            else if ((distanceX < 100 && distanceX > 34) && distanceY < 40)
            {
                movment = Movment.IDLE;
                if (this.position.X < heroPosition.X)
                {
                    MoveRight();
                }
                else if (this.position.X > heroPosition.X)
                {
                    MoveLeft();
                }
            }
            else if(distanceX <= 34 && distanceY < 30)
            {
                movment = Movment.ATTACKING;
                Attack(heroPosition);
            }
        }
    }
}

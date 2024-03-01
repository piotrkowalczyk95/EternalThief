using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace EternalThief.actors.characters
{
    class Hero : Character
    {
        public Dictionary<string, Texture2D> _projTextures;
        public Hero(Vector2 startingPosition)
        {
            this.startPosition = startingPosition;
            position = startingPosition;
            this.acceleration = 1.8f;
            this.deceleration = 0.78f;
            this.maxSpeed = 8.0f;
            this.isCollidable = false;
            this._delay = 1000;
            this._cooldownTime = 10000;
            this._remainingDelay = _delay;
            this._remainingCooldownTime = _cooldownTime;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private Input input;

        public float elapsedTime;

        public void Load(ContentManager content)
        {
            _animations = new Dictionary<string, Sprite>()
            {
                {"Idle", new Sprite(content.Load<Texture2D>("Player/Idle"),4) },
                {"IdleLeft", new Sprite(content.Load<Texture2D>("Player/IdleLeft"),4) },
                {"Right", new Sprite(content.Load<Texture2D>("Player/WalkRight"),6) },
                {"Left", new Sprite(content.Load<Texture2D>("Player/WalkLeft"),6) },
                {"JumpRight", new Sprite(content.Load<Texture2D>("Player/JumpRight"),4) },
                {"JumpLeft", new Sprite(content.Load<Texture2D>("Player/JumpLeft"),4) },
                {"Fall", new Sprite(content.Load<Texture2D>("Player/Fall"),2) },
                {"FallLeft", new Sprite(content.Load<Texture2D>("Player/FallLeft"),2) },
                {"Climb", new Sprite(content.Load<Texture2D>("Player/Climb"),4) },
                {"Throw", new Sprite(content.Load<Texture2D>("Player/Throw"),4) },
                {"ThrowLeft", new Sprite(content.Load<Texture2D>("Player/ThrowLeft"),4) }
            };

            input = new Input()
            {
                Right = Keys.D,
                Left = Keys.A,
                Up = Keys.W,
                Jump = Keys.Space,
                Down = Keys.S,
                Throw = Keys.LeftControl
            };

            _animationPlayer = new AnimationPlayer(_animations.First().Value);
            
            base.Load();
        }

        public void Update(List<GameObject> gameObjects, GameTime gameTime, TiledMap map)
        {
            CheckInput(gameObjects, map);
            _animationPlayer.Update(gameObjects, gameTime);
            boundingBoxWidth = (int)_animationPlayer.GetTextureSize().X - 16;
            boundingBoxHeight = (int)_animationPlayer.GetTextureSize().Y;
            elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameObjects, gameTime, map);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            base.Draw(spriteBatch, null, camera);
        }

        private void CheckInput(List<GameObject> gameObjects, TiledMap map)
        {
            if (Keyboard.GetState().IsKeyDown(input.Right))
                MoveRight();
            if (Keyboard.GetState().IsKeyDown(input.Left))
                MoveLeft();
            if (Keyboard.GetState().IsKeyDown(input.Jump))
            {
                Jump(map);
            }
            if (Keyboard.GetState().IsKeyDown(input.Throw))
            {
                Throw();
            }

            MoveUp(Keyboard.GetState().IsKeyDown(input.Up));
            MoveDown(Keyboard.GetState().IsKeyDown(input.Down));
        }
    }
}
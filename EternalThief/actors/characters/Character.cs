using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace EternalThief.actors
{
    class Character : AnimatedObject
    {

        public Vector2 velocity = Vector2.Zero;
        protected float acceleration;
        protected float deceleration;
        protected float maxSpeed;
        const float gravity = 2.0f;
        protected bool isJumping = false;
        protected float jumpVelocity = 4.0f;
        protected float tempVelocity = 1.0f;
        protected float elapsedTime = 1.0f;
        protected Movment movment = Movment.IDLE;
        private bool moveUp = false;
        private bool moveDown = false;
        private bool isRunCalled = false;
        private bool isHeroIdle = true;
        protected float timer;
        protected float _delay;
        protected float _cooldownTime;
        protected float _remainingDelay;
        protected float _remainingCooldownTime;
        public bool actionTaken = false;
        public bool cooldown = false;
        private Vector2 tempDirection;

        public override void Initialize()
        {
            velocity = Vector2.Zero;
            base.Initialize();
        }

        public void Update(List<GameObject> gameObjects, GameTime gameTime, TiledMap map)
        {
            switch (movment) {
                case Movment.IDLE: {
                        UpdateMovement(gameObjects, map, gameTime);
                        break;
                    }
                case Movment.CLIMBING: {
                        UpdateClimbing(gameObjects, map, gameTime);
                        break;
                    }
                case Movment.ATTACKING:
                    {
                        UpdateAttacking(gameObjects, map, gameTime);
                        break;
                    }
            }
            base.Update(gameObjects, gameTime, velocity, direction, movment, actionTaken);
            velocity = Vector2.Zero;

            timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (actionTaken)
            {
                _remainingDelay -= timer;
                if (_remainingDelay <= 0)
                {
                    actionTaken = false;
                    _remainingDelay = _delay;
                }
            }
            if (cooldown)
            {
                _remainingCooldownTime -= timer;
                if(_remainingCooldownTime <= 0)
                {
                    cooldown = false;
                    _remainingCooldownTime = _cooldownTime;
                }
            }
        }

        private void UpdateMovement(List<GameObject> gameObjects, TiledMap map, GameTime gameTime)
        {
            GameObject obj = checkIfColidedWithActiveObject(gameObjects);
            switch (obj) {
                case Ladder: {
                        var result = this.BoundingBox.Y < obj.BoundingBox.Y;
                        if (moveDown && result) 
                        {
                            movment = Movment.CLIMBING;
                            position.Y += 20f;
                            position.X = obj.position.X - 30;
                        } 
                        else if (moveUp && !result) 
                        {
                            movment = Movment.CLIMBING;
                            position.Y -= 20f;
                            position.X = obj.position.X - 30;
                        }
                        break;
                    }
                default: break;
            }

            int timeElapsed = Environment.TickCount & Int32.MaxValue;
            if (isRunCalled == true && !isJumping && !moveDown && !moveUp)
            {
                if (timeElapsed % 400 == 0)
                {
                    SoundManager.PlayHeroRun(true);
                }
                isRunCalled = false;
                SoundManager.PlayHeroRun(false);
            }

                if (timeElapsed % 30000 == 0)
                {
                    if (isHeroIdle == true && isJumping == false && !moveDown && !moveUp)
                    {
                    SoundManager.PlayHeroIdle();
                    }
                }

            if (movment != Movment.IDLE) {
                return;
            }

            ApplyGravity(map, gameTime);

            if (velocity.X != 0 && CheckCollisions(gameObjects, true, map))
            {
                velocity.X = 0;
            }

            if (actionTaken)
            {
                direction = tempDirection;
                velocity.X = 0;
            }

            position.X += velocity.X;

            if (velocity.Y > 0 && CheckCollisions(gameObjects, false, map))
            {
                velocity.Y = 0;
            }


            if (actionTaken)
            {
                velocity.Y = 0;
            }

            position.Y += velocity.Y;
        }

        private void UpdateClimbing(List<GameObject> gameObjects, TiledMap map, GameTime gameTime) 
        {
            var colision = CheckCollisions(gameObjects, false, map);
            if (colision) 
            {
                movment = Movment.IDLE;
                return;
            }
            if(isJumping) 
            {
                isJumping = false;
            }
            if(moveUp) 
            {
                position.Y -= 0.1f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                
            }
            if (moveDown) 
            {
                position.Y += 0.1f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
               
            }
            if(!moveDown && !moveUp) 
            {
                direction.Y = 0;
              
            }

            if (isJumping == false && moveUp || moveDown)
            {
                int timeElapsed = Environment.TickCount & Int32.MaxValue;
                if (timeElapsed % 1000 == 0)
                {
                    SoundManager.PlayHeroClimb(true);
                }
                SoundManager.PlayHeroClimb(false);
            }

        }

        private void UpdateAttacking(List<GameObject> gameObjects, TiledMap map, GameTime gameTime)
        {
            if (actionTaken)
            {
                direction = tempDirection;
                velocity.X = 0;
                velocity.Y = 0;
            }
            else
            {
                movment = Movment.IDLE;
                return;
            }
        }

        protected bool Throw()
        {
            if(actionTaken == true || cooldown == true)
            {
                return false;
            }
            if(velocity.Y == 0 && isJumping == false)
            {
                velocity.X = 0;
                tempDirection = direction;
                actionTaken = true;
                return true;
            }
            return false;
        }


        protected void MoveRight() {
            if (movment != Movment.IDLE) {
                return;
            }
            isRunCalled = true;
            isHeroIdle = false;
            velocity.X += acceleration + deceleration;
            velocity.X = Math.Min(velocity.X, maxSpeed);
            direction.X = 1;
            direction.Y = 0;
        }

        protected void MoveLeft()
        {
            if (movment != Movment.IDLE) {
                return;
            }
            isRunCalled = true;
            isHeroIdle = false;
            velocity.X -= acceleration + deceleration;
            velocity.X = Math.Max(velocity.X, -maxSpeed);
            direction.X = -1;
            direction.Y = 0;
        }


        protected void MoveDown(bool presed)
        {
            moveDown = presed;
            direction.Y = -1;
        }


        protected void MoveUp(bool presed)
        {
            moveUp = presed;
            direction.Y = 1;
        }

        protected void Attack(Vector2 heroPosition)
        {
           if (actionTaken == true)
           {
                return;
           }
           velocity.X = 0;
           tempDirection = direction;
           actionTaken = true;
        }

        protected void Jump(TiledMap map)
        {
            if(movment != Movment.IDLE) {
                return;
            }
            if(isJumping == true && OnGround(map) == false)
            {
                return;
            }
            if(elapsedTime < 1.0f)
            {
                return;
            }

            if (velocity.Y == 0)
            {
                velocity.Y -= jumpVelocity;
                isJumping = true;
            }
            SoundManager.PlayHeroJump();
            return;
        }

        protected bool OnGround(TiledMap map)
        {
            Rectangle groundBoundingBox;
            if (velocity.Y >= 0)
            {
                groundBoundingBox = new Rectangle((int)(position.X + 17), (int)(position.Y + boundingBoxHeight + 1), 21, 3);
            }
            else
            {
                groundBoundingBox = new Rectangle((int)(position.X + 17), (int)(position.Y + boundingBoxHeight - 1), 21, 3);
            }
            

            Rectangle rectangle = map.CheckCollision(groundBoundingBox);
            if (rectangle != Rectangle.Empty)
            {
                if (elapsedTime < 1.0f)
                {
                    elapsedTime += 0.1f;
                }
                if (rectangle.Y <= (position.Y + boundingBoxHeight) && rectangle.Y > position.Y)
                {
                    position.Y = rectangle.Top - boundingBoxHeight + 1;
                }
                return true;
            }

            return false;
        }

        protected bool PatrolGround(TiledMap map)
        {
            Rectangle patrolBoundingBox;
            if (direction.X == -1)
            {
                patrolBoundingBox = new Rectangle((int)(position.X + 15), (int)(position.Y + boundingBoxHeight + 1), 19, 3);
            }
            else
            {
                patrolBoundingBox = new Rectangle((int)(position.X + 19), (int)(position.Y + boundingBoxHeight + 1), 21, 3);
            }
            Rectangle rectangle = map.CheckCollision(patrolBoundingBox);
            if(patrolBoundingBox.X <= rectangle.X - 1 || (patrolBoundingBox.X + 17) >= (rectangle.X + 18))
            {
                return true;
            }
            return false;
        }

        protected bool NearWall(TiledMap map)
        {
            Rectangle wallBoundingBox = new Rectangle((int)position.X + 16, (int)position.Y + 10, 22, boundingBoxHeight / 2);
            if (direction.X == -1)
            {
                wallBoundingBox.X -= 2;
                wallBoundingBox.Width -= 2;
            }
            else if (direction.X == 1)
            {
                wallBoundingBox.X += 2;
                wallBoundingBox.Width += 2;
            }

            if (map.CheckCollision(wallBoundingBox) != Rectangle.Empty)
            {
                return true;
            }

            return false;
        }

        protected bool RoofHit(TiledMap map)
        {
            Rectangle roofBoundingBox = new Rectangle((int)position.X + 17, (int)position.Y, 21, 3);
            if (map.CheckCollision(roofBoundingBox) != Rectangle.Empty)
            {
                velocity.Y = 0;
                elapsedTime = 3.0f;
                return true;
            }

            return false;
        }

        private void ApplyGravity(TiledMap map, GameTime gameTime)
        {
            if (isJumping == true)
            {
                if (elapsedTime < 6.0f)
                {
                    elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds * 3;
                }
                if (isJumping == true && elapsedTime < 3f && RoofHit(map) == false)
                {
                    velocity.Y -= jumpVelocity / elapsedTime;
                }
                else
                {
                    velocity.Y += gravity * (elapsedTime - 2f);
                    if (OnGround(map) == true)
                    {
                        velocity.Y = 0;
                        isJumping = false;
                        elapsedTime = 0f;
                    }
                }

            }
            else if (OnGround(map) == false)
            {
                if (elapsedTime < 3.0f)
                {
                    elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds * 3;
                }
                velocity.Y += gravity * (elapsedTime);
            }
        }

        protected virtual GameObject checkIfColidedWithActiveObject(List<GameObject> gameObjects) {
            foreach (var gameObject in gameObjects) {
                if(gameObject is Ladder && CheckCollision(gameObject.BoundingBox)) {
                    return gameObject;
                }
            }
            return null;
        }

        protected virtual bool CheckCollisions(List<GameObject> gameObjects, bool xAxis, TiledMap map)
        {

            if (xAxis == true && NearWall(map))
            {
                return true;
            }

            if (xAxis == false && OnGround(map))
            {
                return true;
            }

            //TODO do fix this
            foreach(var gameObject in gameObjects)
            {
                if(gameObject != this && gameObject.isCollidable)
                {
                    if(this.CheckCollision(gameObject.BoundingBox)) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

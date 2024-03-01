using EternalThief.actors;
using EternalThief.actors.characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using System;
using System.Collections.Generic;
using System.Text;

namespace EternalThief
{
    class Level1
    {
        private Hero hero;

        public Vector2 heroPosition;

        private List<Skeleton> Skeletons = new List<Skeleton>();

        private StunDart stunDart;

        public TiledMap tiledMap;

        public Camera camera;

        public List<Hull> Hulls = new List<Hull>();

        PenumbraComponent penumbra;

        float scale = 1f;

        Light light = new PointLight
        {
            Scale = new Vector2(1200f),
            ShadowType = ShadowType.Solid
        };
        int ResolutionWidth = 0; //rozmiar okna
        int ResolutionHeight = 0;
        Game game;
        public Level1(int ResolutionWidth, int ResolutionHeight, Game game)
        {
            this.ResolutionWidth = ResolutionWidth;
            this.ResolutionHeight = ResolutionHeight;
            this.game = game;
        }

        internal void Initialize()
        {
            tiledMap = new TiledMap(ResolutionWidth, ResolutionHeight);
            penumbra = new PenumbraComponent(game);

            //camera = new OrthographicCamera(viewportAdapter);
            game.Components.Add(penumbra);
        }

        internal void LoadContent(GraphicsDevice graphicsDevice)
        {
            tiledMap.Load(game.Content, @"Tilemaps/moj_poziom.tmx");

            hero = new Hero(tiledMap.playerPosition);
            foreach (var opponentPos in tiledMap.enemyPositions)
            {
                Skeletons.Add(new Skeleton(opponentPos));
            }

            hero.Load(game.Content);
            SoundManager.Load(game.Content);
            SoundManager.PlayBgMusic();
            foreach (var skeleton in Skeletons)
            {
                skeleton.Load(game.Content);
            }

            camera = new Camera(ResolutionWidth, ResolutionHeight, hero.BoundingBox);

            penumbra.Lights.Add(light);
            penumbra.AmbientColor = new Color(45, 45, 70);//new Color(r, g, b);
            light.Color = new Color(107, 143, 133);
            foreach (var rect in tiledMap.Hulls)
            {
                Hull hull = new Hull(new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height), new Vector2(rect.X, rect.Y + rect.Height))
                {
                    Scale = new Vector2(1.0f)
                };

                Hulls.Add(hull);
            }


            foreach (var hull in Hulls)
            {
                penumbra.Hulls.Add(hull);
            }
        }

        internal void stopGame()
        {
            game.Components.Clear();
            SoundManager.StopMusic();
        }

        internal void Update(GameTime gameTime)
        {
            var a = new List<GameObject>();
            a.AddRange(tiledMap.acttiveGameObject);

            hero.Update(a, gameTime, tiledMap);
            var heroPosition = new Vector2(hero.position.X + 26, hero.position.Y + 17);

            foreach (var skeleton in Skeletons)
            {
                skeleton.Update(a, gameTime, heroPosition, tiledMap);
                a.Add(skeleton);
            }

            if (stunDart == null)
            {
                if (hero.actionTaken)
                {
                    if (hero.cooldown == false)
                    {
                        stunDart = new StunDart(heroPosition, hero.direction, game.Content);
                        stunDart.Load(game.Content);
                        hero.cooldown = true;
                    }
                }
            }
            else
            {
                stunDart.Update(a, gameTime, tiledMap);
                foreach (var skeleton in Skeletons)
                {
                    if (stunDart != null)
                    {
                        if (stunDart.SkeletonCollision(skeleton))
                        {
                            skeleton.stunned = true;
                            stunDart = null;
                        }
                    }
                }
                if (stunDart != null)
                {
                    if (stunDart.MapCollision(tiledMap))
                    {
                        stunDart = null;
                    }
                }

            }

            camera.heroBoundingBox = hero.BoundingBox;
            camera.Update(heroPosition, scale, tiledMap);
            tiledMap.Update(gameTime, heroPosition);
            light.Position = heroPosition;

            penumbra.Transform = camera.VievMatrix;
        }

        internal void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            penumbra.BeginDraw();
            _spriteBatch.Begin(transformMatrix: camera.VievMatrix);
            tiledMap.Draw(_spriteBatch);
            _spriteBatch.End();
            hero.Draw(_spriteBatch, camera);
            if (stunDart != null)
            {
                stunDart.Draw(_spriteBatch, camera);
            }
            foreach (var skeleton in Skeletons)
            {
                skeleton.Draw(_spriteBatch, camera);
            }
            penumbra.Draw(gameTime);
        }
    }
}
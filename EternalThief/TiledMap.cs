using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TiledSharp;
using EternalThief.actors;
using EternalThief.actors.characters;


namespace EternalThief
{
    public class TiledMap
    {
        TmxMap tiledMap;
        List<int> firstGids = new List<int>();
        List<Texture2D> tilesets = new List<Texture2D>();

        List<TileLayer> tileLayers = new List<TileLayer>();
        List<Rectangle> collisionRectangles = new List<Rectangle>();

        public List<Rectangle> Hulls = new List<Rectangle>();

        int VResWidth, VResHeight;

        public Vector2 playerPosition;

        public List<Vector2> enemyPositions = new List<Vector2>();
      
        public List<Vector2> enemy = new List<Vector2>();
        public List<GameObject> acttiveGameObject = new List<GameObject>();

        public TiledMap() {
        }
        public TiledMap(int screenWidth, int screenHeight)
        {
            VResWidth = screenWidth;
            VResHeight = screenHeight;
        }
        public Rectangle size
        {
            get
            {
                return new Rectangle(0, 0, tiledMap.TileWidth * tiledMap.Width, tiledMap.TileHeight * tiledMap.Height);
            }
        }
        public void Load(ContentManager content, string filePath)
        {
            tiledMap = new TmxMap(@content.RootDirectory + @"\" + filePath);
            foreach (var tileset in tiledMap.Tilesets)
            {

                String filepath = System.IO.Path.GetDirectoryName(filePath) + @"\" + tileset.Name.ToString();
                tilesets.Add(content.Load<Texture2D>(filepath));
                firstGids.Add(tileset.FirstGid);
            }
            float j = 0f;
            foreach (var layer in tiledMap.Layers)
            {
                TileLayer mapLayer = new TileLayer(VResWidth, VResHeight);

                mapLayer.layerDepth = j;
                j += 0.1f;
                mapLayer.name = layer.Name;
                int i = 0;

                foreach (var tile in layer.Tiles)
                {
                    int gid = tile.Gid;

                    if (gid == 0) { }
                    else
                    {
                        var index = firstGids.IndexOf(firstGids.Where(n => n <= gid).Max());
                        int tileWidth = tiledMap.Tilesets[index].TileWidth;
                        int tileHeight = tiledMap.Tilesets[index].TileHeight;
                        int tilesetTilesWide = tilesets[index].Width / tileWidth;

                        String name = tiledMap.Tilesets[index].Name;

                        int tileFrame = gid - firstGids[index];
                        int column = tileFrame % tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                        float x = (i % tiledMap.Width) * tiledMap.TileWidth;
                        float y = (float)Math.Floor(i / (double)tiledMap.Width) * tiledMap.TileHeight;

                        Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

                        Tile mapTile = new Tile(new Rectangle((int)x, (int)y, tileWidth, tileHeight), index, tilesetRec);
                        mapLayer.tiles.Add(mapTile);
                    }
                    i++;
                }
                tileLayers.Add(mapLayer);
            }

            foreach (var objectLayer in tiledMap.ObjectGroups)
            {
                if (objectLayer.Name.Equals("collisions"))
                {
                    addColisions(objectLayer);
                }
                else if (objectLayer.Name.Equals("characters"))
                {
                    addPositions(objectLayer);
                } else if(objectLayer.Name.Equals("activeGameObject")) {
                    addActiveGameObject(objectLayer);
                } else {
                    return;
                }

            }
        }

        private void addColisions(TmxObjectGroup objectLayer)
        {
            foreach (var rect in objectLayer.Objects)
            {
                collisionRectangles.Add(new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
                Hulls.Add(new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
            }
        }

        private void addPositions(TmxObjectGroup objectLayer)
        {
            foreach (var rect in objectLayer.Objects)
            {
                if (rect.Name.Equals("player"))
                {
                    playerPosition = new Vector2((float)rect.X, (float)rect.Y);
                } else if (rect.Name.Contains("enemy"))
                {
                    enemyPositions.Add(new Vector2((float)rect.X, (float)rect.Y));
                }
            }
        }

        private void addActiveGameObject(TmxObjectGroup objectLayer) {
            foreach (var rect in objectLayer.Objects) {
                switch (Enum.Parse(typeof(Type), rect.Type)) {
                    case Type.LADDER: {
                            acttiveGameObject.Add(new Ladder((float) rect.X, (float) rect.Y, (float) rect.Width, (float) rect.Height));
                            break;
                    }
                }
            }
        }

        public Rectangle CheckCollision(Rectangle input) {
            //TODO Collisions with vectors
            foreach (var rectangle in collisionRectangles) {
                if (rectangle.Intersects(input)) {
                    return rectangle;
                }
            }

            return Rectangle.Empty;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var layer in tileLayers)
            {
                layer.Draw(tilesets, spriteBatch);
            }
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            foreach (var layer in tileLayers)
            {
                layer.Update(gameTime, playerPosition);
            }
        }
    }
    public class TileLayer
    {

        public String name;
        public List<Tile> tiles = new List<Tile>();

        private List<Tile> drawableTiles = new List<Tile>();
        public float layerDepth = 1.0f;

        int VResHeight, VResWidth;
        Vector2 prevoiusPlayerPos = new Vector2(int.MinValue, int.MaxValue);
        public TileLayer()
        {

        }
        public TileLayer(int screenWidth, int screenHeight)
        {
            VResWidth = screenWidth;
            VResHeight = screenHeight;
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            if (playerPos != prevoiusPlayerPos)
            {
                drawableTiles.Clear();

                foreach (var tile in tiles)
                {
                    if (tile.position.X > playerPos.X - 2 * VResWidth &&
                        tile.position.X < playerPos.X + 2 * VResWidth &&
                        tile.position.Y > playerPos.Y - 2 * VResHeight &&
                        tile.position.Y < playerPos.Y + 2 * VResHeight)
                    {
                        drawableTiles.Add(tile);
                    }
                }
            }
            prevoiusPlayerPos = playerPos;
        }

        public void Draw(List<Texture2D> tilesets, SpriteBatch spriteBatch)
        {
            foreach (var tile in drawableTiles)
            {
                spriteBatch.Draw(tilesets[tile.tilesetIndex], tile.position, tile.sourceRect, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);
            }
        }
    }

    public class Tile
    {
        public Rectangle position, sourceRect;
        public int tilesetIndex;

        public Tile() { }

        public Tile(Rectangle position, int tilesetIndex, Rectangle sourceRect)
        {
            this.position = position;
            this.tilesetIndex = tilesetIndex;
            this.sourceRect = sourceRect;
        }
    }
}

enum Type {
    LADDER
}
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EternalThief.actors {
    class Ladder: GameObject {

        public float height = 0f;
        public float width = 0f;

        public override Rectangle BoundingBox {
            get {
                return new Rectangle((int) position.X, (int) (position.Y), (int) width, (int) height);
            }
        }

        public Ladder() {
            Initialize();
            isCollidable = false;
        }
        public Ladder(float x, float y, float width, float height) {
            this.position = new Vector2(x, y);
            isCollidable = false;
            this.height = height;
            this.width = width;
        }
    }
}

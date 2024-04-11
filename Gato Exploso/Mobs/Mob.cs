using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.Mobs
{
    public abstract class Mob : Entity
    {
        public MoveDirection facing = new MoveDirection();
        public int speed = 0;
        public bool hasBeenTalkedTo = false;
        protected Mob()
        {

        }
        public abstract void Draw(SpriteBatch spritebatch, int offX, int offY);
    }
}

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.Mobs
{
    public abstract class Mob : Entity
    {
        public MoveDirection facing = new MoveDirection();
        public int speed = 0;
        public double hp = 9999;
        public bool hasBeenTalkedTo = false;
        double dx = 0;
        double dy = 0;
        double angle2;
        protected Mob()
        {

        }
        public void Move(double angle)
        {
            
            if(Game1.Instance.GetTime() % 8 == 0)
            {
                angle2 = angle;
            }
            // uses formula to decide where to move the mob
            dy = speed * Math.Sin(Game1.Instance.radiansToDegrees(angle2));
            dx = speed * Math.Cos(Game1.Instance.radiansToDegrees(angle2));
            // moves the mob accordingly
            x += (int)dx;
            y += (int)dy;
        }
        public abstract void Draw(SpriteBatch spritebatch, int offX, int offY);
    }
}

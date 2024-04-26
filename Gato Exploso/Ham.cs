using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    public class Ham : Entity
    {
        int speed = 10;
        public Ham()
        {
            
        }
        public void Move(double angle)
        {
            double dx = 0;
            double dy = 0;
            // uses formula to decide where to move the mob
            dy = speed * Math.Sin(angle);
            dx = speed * Math.Cos(angle);
            // moves the mob accordingly
            int proposedX = x + (int)dx;
            int proposedY = y + (int)dy;
             
        }
    }
}

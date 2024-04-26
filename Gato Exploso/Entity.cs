using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    public abstract class Entity
    {
        // variables
        public int x = 0;
        public int y = 0;
        public int width = 32;
        public int height = 32;
        
        // constructor
        public Entity()
        {
        }

       // gives the hitbox of an entity
       public Rectangle GetEntityRect()
        {
            return new Rectangle(x, y, width, height);
        }


    }
}

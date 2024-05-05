using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.TileObjects
{
    public abstract class TileObject
    {
        // sets a variable for if the variable exists
        protected bool deleted = false;
        public string type = "nuh uh";
        public abstract void Draw(SpriteBatch spriteBatch, int x, int y);
    }
}

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.TileObjects
{
    internal abstract class TileObject
    {
        protected bool deleted = false;

        public abstract void Draw(SpriteBatch spriteBatch, int x, int y);
    }
}

using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Mime;

namespace Gato_Exploso
{
    internal abstract class Tile : Entity
    {
        public int width = 32;
        public int height = 32;
        // variables
        protected Texture2D tileTexture;

        // methods
        public abstract void Load();
        public abstract void Draw(SpriteBatch spriteBatch, int x, int y);

    }
}

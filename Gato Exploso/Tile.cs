using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Mime;

namespace Gato_Exploso
{
    internal abstract class Tile
    {
        protected Texture2D tileTexture;
        public abstract void Load();
        public abstract void Draw(SpriteBatch spriteBatch, int x, int y);

    }
}

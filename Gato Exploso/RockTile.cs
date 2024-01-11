using Gato_Exploso;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Mime;

namespace Gato_Exploso
{
    internal class RockTile : Tile
    {
        // variables
        public bool solid = true;
        public int tileID = 2;
        // methods 
        public override void Load()
        {
            tileTexture = Game1.GameContent.Load<Texture2D>("TemporaryRockTile");

        }
        public RockTile()
        {
            Load();

        }
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            var pos = new Vector2(x, y);
            spriteBatch.Draw(tileTexture, pos, null, Color.White);
        }


    }
}

using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Mime;

namespace Gato_Exploso.Tiles
{
    internal class HammyTile : Tile
    {
        // variables
        public bool solid = false;

        // methods
        public override void Load()
        {
            tileTexture = Game1.GameContent.Load<Texture2D>("GrassTile");
        }
        public HammyTile(int gameTime)
        {
            Load();
            tileID = 6;
        }
        public HammyTile()
        {
            Load();
            tileID = 6;
        }
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            var pos = new Vector2(x, y);
            if (isExploding)
            {
                spriteBatch.Draw(tileTexture, pos, null, Color.Red);
            }
            else
            {
                spriteBatch.Draw(tileTexture, pos, null, Color.White);
            }
        }


    }
}

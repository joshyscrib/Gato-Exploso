using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.TileObjects
{
    internal class Rock : TileObject
    {
        protected Texture2D rockTexture;
        public Rock()
        {
            Load();
        }
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(rockTexture, new Vector2(x,y), Color.White);
        }


        public void Load()
        {
            rockTexture = Game1.GameContent.Load<Texture2D>("TemporaryRockTile");

        }

    }
}


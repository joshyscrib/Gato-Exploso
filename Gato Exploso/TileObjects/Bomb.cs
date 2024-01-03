using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.TileObjects
{
    internal class Bomb : TileObject
    {
        public double createTime = 0;
        protected Texture2D bombTexture;
        public Bomb(double ticks)
        {
            Load();
            createTime = ticks;
        }
        public void Load()
        {
            bombTexture = Game1.GameContent.Load<Texture2D>("bomb");
        }
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            var pos = new Vector2(x, y);
            spriteBatch.Draw(bombTexture, pos, null, Color.White);

            
        }
    }
}

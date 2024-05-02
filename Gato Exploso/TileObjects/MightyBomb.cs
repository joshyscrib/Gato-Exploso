using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.TileObjects
{
    public class MightyBomb : Bomb
    {
        public MightyBomb(double ticks) : base(ticks)
        {
            
            Load();
            range = 4;
            // sets the time that the bomb was created
            createTime = ticks;
        }
        public void Load()
        {
            bombTexture = Game1.GameContent.Load<Texture2D>("MightyBomb");
        }
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            // draws the bomb
            var pos = new Vector2(x, y);
            spriteBatch.Draw(bombTexture, pos, null, Color.White);


        }
        public void DetonateBomb()
        {
            createTime = 0;
        }
    }
}

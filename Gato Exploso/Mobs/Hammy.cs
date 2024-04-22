using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.Mobs
{
    public class Hammy : Mob
    {
        protected ContentManager Content;
        protected Texture2D texture;
        public Hammy(ContentManager cont)
        {
            Content = cont;
            Load();
            speed = 3;
            hp = 40;
        }
        public override void Draw(SpriteBatch spriteBatch, int offX, int offY)
        {
            spriteBatch.Draw(texture, new Vector2(x + offX, y + offY), Color.White);
        }
        public void Load()
        {
            texture = Content.Load<Texture2D>("Hammy");
        }
    }
}

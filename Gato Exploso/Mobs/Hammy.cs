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
            hp = 300;
            strength = 60;
        }
        public override void Draw(SpriteBatch spriteBatch, int offX, int offY)
        {
            double healthPercent = hp / 300;
            spriteBatch.Draw(texture, new Vector2(x + offX, y + offY), Color.White);
            spriteBatch.Draw(drawTexture, new Rectangle(900,150,750,36), Color.Gray);
            spriteBatch.Draw(drawTexture, new Rectangle(904,154, (int)(healthPercent * 742.0), 28), Color.Red);
        }
        public void Load()
        {
            texture = Content.Load<Texture2D>("Hammy");
        }
    }
}

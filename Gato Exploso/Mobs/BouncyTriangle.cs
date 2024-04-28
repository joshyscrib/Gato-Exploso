using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Gato_Exploso.Mobs
{
    public class BouncyTriangle : Mob
    {
        protected ContentManager Content;
        protected Texture2D texture;
         
        // bouncy triangle is good during the day
        const bool defaultGood = true;
        bool good = defaultGood;
        
        // methods
        public BouncyTriangle(ContentManager cont)
        {
            Content = cont;
            Load();
            speed = 4;
            hp = 55;
            strength = 15;
        }
        public override void Draw(SpriteBatch spriteBatch, int offX, int offY)
        {
            double healthPercent = hp / 55;
            spriteBatch.Draw(texture, new Vector2(x + offX,y + offY),Color.White);
            spriteBatch.Draw(drawTexture, new Rectangle(x - 8 + offX, y + 68 + offY, 84, 12), Color.Gray);
            spriteBatch.Draw(drawTexture, new Rectangle(x - 7 + offX, y + 69 + offY, (int)(healthPercent * 84.0), 10), Color.Red);
        }

        public int Attack()
        {
            Random r = new Random();
            return r.Next() * 20;
        }
        public void Load()
        {
            texture = Content.Load<Texture2D>("BouncyTriangle");
        }


    }

}

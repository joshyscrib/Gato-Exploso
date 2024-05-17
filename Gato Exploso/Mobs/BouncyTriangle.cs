using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net;

namespace Gato_Exploso.Mobs
{
    public class BouncyTriangle : Mob
    {
        protected ContentManager Content;
        protected Texture2D texture;
        protected Texture2D texture2;
        protected Texture2D texture3;
        protected Texture2D texture4;
        protected Texture2D oppTexture;
        protected Texture2D oppTexture2;
        protected Texture2D oppTexture3;
        protected Texture2D oppTexture4;
        protected Texture2D curTexture;

        // bouncy triangle is good during the day


        // methods
        public BouncyTriangle(ContentManager cont)
        {
            Content = cont;
            Load();
            speed = 3; 
            maxHp = 15 + (Game1.difficultyNumber * 20);
            hp = maxHp;
            strength = 4 + (Game1.difficultyNumber * 8);
            width = 64;
            height = 64;
            defaultGood = true;
            good = defaultGood;
        }
        public override void Draw(SpriteBatch spriteBatch, int offX, int offY)
        {
            if (good)
            {
                curTexture = texture;
            }
            else
            {
                curTexture = oppTexture;
            }
            double healthPercent = hp / maxHp;
            spriteBatch.Draw(curTexture, new Rectangle(x + offX, y + offY, 64, 64),Color.White);
            spriteBatch.Draw(drawTexture, new Rectangle(x - 8 + offX, y + 68 + offY, 84, 12), Color.Gray);
            spriteBatch.Draw(drawTexture, new Rectangle(x - 7 + offX, y + 69 + offY, (int)(healthPercent * 84.0), 10), Color.Red);
        }

        public void Load()
        {
            texture = Content.Load<Texture2D>("BouncyTriangle");
            texture2 = Content.Load<Texture2D>("BouncyTriangle2");
            texture3 = Content.Load<Texture2D>("BouncyTriangle3");
            texture4 = Content.Load<Texture2D>("BouncyTriangle4");
            oppTexture = Content.Load<Texture2D>("BadBouncyTriangle");
            oppTexture2 = Content.Load<Texture2D>("BadBouncyTriangle2");
            oppTexture3 = Content.Load<Texture2D>("BadBouncyTriangle3");
            oppTexture4 = Content.Load<Texture2D>("BadBouncyTriangle4");
        }


    }

}

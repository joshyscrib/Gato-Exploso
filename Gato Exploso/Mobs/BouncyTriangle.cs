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
        protected Texture2D oppTexture;
        protected Texture2D curTexture;
        int curFrame = 1;
        int bounceAmount = 0;

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
            if (Game1.Instance.GetTime() % 5 == 0)
            {
                curFrame++;
                if (curFrame > 15)
                {
                    curFrame = 1;
                }
            }
            switch (curFrame)
            {
                case 1:
                    bounceAmount = 0;
                    break;
                case 2:
                    bounceAmount = 2;
                    break;
                case 3:
                    bounceAmount = 4;
                    break;
                case 4:
                    bounceAmount = 5;
                    break;
                case 5:
                    bounceAmount = 7;
                    break;
                case 6:
                    bounceAmount = 9;
                    break;
                case 7:
                    bounceAmount = 11;
                    break;
                case 8:
                    bounceAmount = 13;
                    break;
                case 9:
                    bounceAmount = 13;
                    break;
                case 10:
                    bounceAmount = 11;
                    break;
                case 11:
                    bounceAmount = 9;
                    break;
                case 12:
                    bounceAmount = 7;
                    break;
                case 13:
                    bounceAmount = 5;
                    break;
                case 14:
                    bounceAmount = 3;
                    break;
                case 15:
                    bounceAmount = 1;
                    break;

            }
            if (good)
            {
                curTexture = texture;
            }
            else
            {
                curTexture = oppTexture;
            }
            double healthPercent = hp / maxHp;
            spriteBatch.Draw(curTexture, new Rectangle(x + offX, y + offY - bounceAmount, 64, 64), Color.White);
            spriteBatch.Draw(drawTexture, new Rectangle(x - 8 + offX, y + 68 + offY, 84, 12), Color.Gray);
            spriteBatch.Draw(drawTexture, new Rectangle(x - 7 + offX, y + 69 + offY, (int)(healthPercent * 84.0), 10), Color.Red);
        }

        public void Load()
        {
            texture = Content.Load<Texture2D>("BouncyTriangle");
            oppTexture = Content.Load<Texture2D>("BadBouncyTriangle");
        }


    }

}

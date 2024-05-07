using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Gato_Exploso.Mobs
{
    public class Porcupine : Mob
    {
        protected ContentManager Content;
        protected Texture2D texture;
        protected Texture2D oppTexture;
        protected Texture2D curTexture;

        // porcupine is bad during the day


        // methods
        public Porcupine(ContentManager cont)
        {
            Content = cont;
            Load();
            speed = 5 + (Game1.difficultyNumber / 4);
            maxHp = 15 + (Game1.difficultyNumber * 20);
            hp = maxHp;
            strength = 4 + (Game1.difficultyNumber * 7);
            width = 64;
            height = 64;
            defaultGood = false;
            good = defaultGood;
        }
        public override void Draw(SpriteBatch spriteBatch, int offX, int offY)
        {
            if (!good)
            {
                curTexture = texture;
            }
            else
            {
                curTexture = oppTexture;
            }
            double healthPercent = hp / maxHp;
            spriteBatch.Draw(curTexture, new Rectangle(x + offX - 8, y + offY - 8, 80, 80), Color.White);
            spriteBatch.Draw(drawTexture, new Rectangle(x - 8 + offX, y + 68 + offY, 84, 12), Color.Gray);
            spriteBatch.Draw(drawTexture, new Rectangle(x - 7 + offX, y + 69 + offY, (int)(healthPercent * 84.0), 10), Color.Red);
        }

        public void Load()
        {
            texture = Content.Load<Texture2D>("BadPorcupine");
            oppTexture = Content.Load<Texture2D>("Porcupine");
        }


    }

}

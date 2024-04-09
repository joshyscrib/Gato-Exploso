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
        // variables   
        int health = 40;
        const bool defaultGood = true;
        bool good = defaultGood;

        // methods

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(texture, new Vector2(x,y),Color.White);
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

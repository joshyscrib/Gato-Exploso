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
        
        const bool defaultGood = true;
        bool good = defaultGood;

        // methods
        public BouncyTriangle(ContentManager cont)
        {
            Content = cont;
            Load();
            speed = 6;
            hp = 40;
        }
        public override void Draw(SpriteBatch spriteBatch, int offX, int offY)
        {
            spriteBatch.Draw(texture, new Vector2(x + offX,y + offY),Color.White);
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

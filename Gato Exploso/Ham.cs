using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    public class Ham : Entity
    {
        ContentManager Content;
        int speed = 4;
        public double angle = 0;
        Texture2D texture;
        public Ham(ContentManager cont)
        {
            Content = cont;
            Load();
        }
        public void Move()
        {
            double dx = 0;
            double dy = 0;
            // uses formula to decide where to move the mob
            dy = speed * Math.Sin(angle);
            dx = speed * Math.Cos(angle);
            // moves the mob accordingly
            int proposedX = x + (int)dx;
            int proposedY = y + (int)dy;
            x = proposedX;
            y = proposedY;
             
        }
        public void Draw(SpriteBatch spriteBatch, int offX, int offY)
        {
            spriteBatch.Draw(texture, new Vector2(x + offX, y + offY), Color.White);
        }
        public void Load()
        {
            texture = Content.Load<Texture2D>("Ham");
        }
    }
}

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
            speed = 2;
            hp = 300;
            strength = 60;
            width = 96;
            height = 96;
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
        public double Target(int gx, int gy)
        {
            double sx = gx - x;
            double sy = gy - y;
            // uses trig to find the angle at which the mob should moves
            return Math.Atan2(sy, sx);
        }
        public Ham ShootHam(int gx, int gy, ContentManager cont)
        {
            Ham ham = new Ham(cont);
            ham.x = x;
            ham.y = y;
            ham.angle = Target(gx, gy);
            return ham;
        }
    }
}

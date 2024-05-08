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
        // to write title
        SpriteFont titleFont;
        public Hammy(ContentManager cont)
        {
            Content = cont;
            Load();
            speed = 2;
            maxHp = 250 + (Game1.difficultyNumber * 30);
            hp = maxHp;
            strength = 30 + (Game1.difficultyNumber * 13);
            width = 96;
            height = 96;
            defaultGood = false;
            good = defaultGood;
        }
        public override void Draw(SpriteBatch spriteBatch, int offX, int offY)
        {
            double healthPercent = hp / maxHp;
            spriteBatch.Draw(texture, new Vector2(x + offX, y + offY), Color.White);
            spriteBatch.Draw(drawTexture, new Rectangle(900,150,750,36), Color.Gray);
            spriteBatch.Draw(drawTexture, new Rectangle(904,154, (int)(healthPercent * 742.0), 28), Color.Red);
            spriteBatch.DrawString
                (
                titleFont,
                "Hammy",
                new Vector2(1100, 30),
                Color.Black
                );
        }
        public void Load()
        {
            texture = Content.Load<Texture2D>("Hammy");

            titleFont = Content.Load<SpriteFont>("Title");
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
            ham.angle = Target(gx, gy);
            ham.x = x;
            ham.y = y;
            return ham;
        }
    }
}

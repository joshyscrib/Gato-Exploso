using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gato_Exploso
{
    public class Turtle : Player
    {
        public Turtle(ContentManager context, double createTime) : base(context, createTime)
        {
            hp = 999999;
            speed = 4;
            Load();
            width = 64;
            height = 64;
        }

        public override void Attack()
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(PTextureLeft, new Rectangle(x, y, 64, 64), Color.White);
        }

        public override void Load()
        {
            PTextureLeft = Content.Load<Texture2D>("Squirrell");
        }
    }
}

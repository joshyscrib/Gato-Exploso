using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gato_Exploso.TileObjects
{
    public class Landmine : Bomb
    {
        public Landmine(double ticks) : base(ticks)
        {

            Load();
            range = 4;
            // sets the time that the bomb was created
            createTime = ticks;
            // so that other classes can easily tell what type of bomb it is
            type = "land";
        }
        public void Load()
        {
            bombTexture = Game1.GameContent.Load<Texture2D>("Landmine");
        }
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            // draws the bomb
            var pos = new Vector2(x, y);
            spriteBatch.Draw(bombTexture, pos, null, Color.White);


        }
        public void DetonateBomb()
        {
            createTime = 0;
        }
    }
}

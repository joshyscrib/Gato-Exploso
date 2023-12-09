using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Mime;


namespace Gato_Exploso
{
    internal class Player
    {
        int x = 0;
        int y = 0;
        int speed = 5;

        public void Move(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.W))
            {
                y -= speed;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                x -= speed;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                y += speed;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                x += speed;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }

    }
}

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
        public int x = 50;
        public int y = 50;
        int speed = 5;
        private ContentManager Content;
        Texture2D playerTexture;
        public Player(ContentManager context)
        {
            Content = context;
        }

        public Player()
        {
        }

        public void Load()
        {
            playerTexture = Content.Load<Texture2D>("gato");
        }
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
        // draws the player in the middle of the screen, no matter where they are in the world
        public void Draw(SpriteBatch spriteBatch)
        {

            Vector2 playerLocation = new Vector2(1230, 650);
            Rectangle playerRect = new Rectangle(10,10,32,32);
            spriteBatch.Draw(playerTexture,playerLocation,Color.White);
        }

    }
}

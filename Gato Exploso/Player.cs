using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Mime;


namespace Gato_Exploso
{
    internal class Player : Entity
    {
        public int x = 500;
        public int y = 500;
        public int width = 32;
        public int height = 64;
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
            int targetX = 0;
            int targetY = 0;
            if (keyState.IsKeyDown(Keys.W))
            {
                targetY -= speed;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                targetX -= speed;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                targetY += speed;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                targetX += speed;
            }
            if (keyState.IsKeyDown(Keys.E))
            {
                x = 400;
                y = 400;
            }
            x = targetX;
            y = targetY;
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

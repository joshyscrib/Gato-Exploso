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
        public int x = 300;
        public int y = 300;
        public int width = 32;
        public int height = 64;
        int targetX = 300;
        int targetY = 300;
        int speed = 10;
        private ContentManager Content;
        Texture2D playerTexture;
        public Player(ContentManager context)
        {
            Content = context;
        }

        public Player()
        {
        }
        // loads player texture
        public void Load()
        {
            playerTexture = Content.Load<Texture2D>("gato");
        }
        // checks if the new locatin collides with an object and decides whether or not to move the player
        public void Move(KeyboardState keyState)
        {
            targetX = x;
            targetY = y;
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
            if (targetX >= 32 && targetX <= 31968)
            {
                x = targetX;
            }
            if (targetY >= 64 && targetY <= 31936)
            {
                y = targetY;
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

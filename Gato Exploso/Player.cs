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
        public int speed = 3;
        public MoveDirection facing = new MoveDirection();
        private ContentManager Content;
        Texture2D PTextureUp;
        Texture2D PTextureRight;
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
            PTextureUp = Content.Load<Texture2D>("TempGatoUp");
            PTextureRight = Content.Load<Texture2D>("TempGatoRight");
        }

        // moves player to an X or Y
        // there is a seperate function for each so that the player can still move horizontally  if they are touching a wall above them, or vice-versa
        public void MoveX(int locX)
        {
            x = locX;
        }
        public void MoveY(int locY)
        {
            y = locY;
        }
        // draws the player in the middle of the screen, no matter where they are in the world
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D curTexture;
            if (facing.Up || facing.Down) { curTexture = PTextureUp; }
            else if (facing.Right || facing.Left) { curTexture = PTextureRight; }
            else
            {
                curTexture = PTextureUp;
            }
            if(facing.Left || facing.Down)
            {
                
            }
            Vector2 playerLocation = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
            spriteBatch.Draw(curTexture,playerLocation,Color.White);
        }

    }
}

using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Mime;


namespace Gato_Exploso
{
    public abstract class Player : Entity
    {
        public int x = 300;
        public int y = 300;
        public int width = 32;
        public int height = 64;
        public int speed = 3;
        public MoveDirection facing = new MoveDirection();
        protected ContentManager Content;
        protected Texture2D PTextureUp;
        protected Texture2D PTextureRight;
        public bool moving = false;
        public Player(ContentManager context)
        {
            Content = context;
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
        public void FacePlayer(MoveDirection face)
        {
            facing = face;
        }
        public void StartMoving()
        {
            moving = true;
        }
        public void StopMoving()
        {
            moving = false;
        }
        // draws the player in the middle of the screen, no matter where they are in the world
        public abstract void Draw(SpriteBatch spriteBatch, int x, int y);




    }
}

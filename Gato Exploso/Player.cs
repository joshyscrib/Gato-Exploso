using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Net.Mime;


namespace Gato_Exploso
{
    public abstract class Player : Entity
    {
        private double currentTime = 0;
        private double lastMoveTime = 0;
        public int x = 2000;
        public int y = 2000;
        public int width = 32;
        public int height = 64;
        public int speed = 3;
        // which direcion the player is facing
        public MoveDirection facing = new MoveDirection();
        protected ContentManager Content;
        protected Texture2D PTextureUp;
        protected Texture2D PTextureRight;
        public bool moving = false;
        // player health
        public double hp = 100;
        public Player(ContentManager context, double createTime)
        {
            Content = context;
            lastMoveTime = createTime;
        }

        public void UpdateTime(double milliseconds)
        {
            currentTime = milliseconds;
        }

        public string  Name { get; set; }

        public bool IsTimedOut()
        {
            // if player hasn't moved for 60 seconds
            if(currentTime > 0 && lastMoveTime + 60000 < currentTime)
            {
                return true;
            }

            return false;
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
            lastMoveTime = currentTime;
            x = locX;

        }
        public void MoveY(int locY)
        {
            lastMoveTime = currentTime;
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

        public abstract void Attack();
        


    }
}

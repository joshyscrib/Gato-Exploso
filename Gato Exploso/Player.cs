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
        
        public int width = 60;
        public int height = 48;
        public int speed = 3;
        public int points = 0;
        // which direcion the player is facing
        public MoveDirection facing = new MoveDirection();
        protected ContentManager Content;
        protected Texture2D PTextureLeft;
        protected Texture2D PTextureRight;
        protected Texture2D PTexture2Left;
        protected Texture2D PTexture2Right;
        public bool moving = false;
        public double lastTimeFired = 0;
        // player health
        public double hp = 100;
        public Player(ContentManager context, double createTime)
        {
            Content = context;
            lastMoveTime = createTime;
            x = 2000;
            y = 2000;
    }

        public void UpdateTime(double milliseconds)
        {
            currentTime = milliseconds;
        }

        public string Name { get; set; }

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
        public abstract void Load();

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
        // takes damage but only every 1/3 of a second
        double lastDamageTime = 0;
        public void TakeDamage(int damage)
        {
            if(Game1.Instance.GetTime() - lastDamageTime > 600)
            {
                hp -= damage;
                lastDamageTime = Game1.Instance.GetTime();
            }
            
        }

    }
}

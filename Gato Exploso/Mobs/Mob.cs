using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Gato_Exploso.Mobs
{
    public abstract class Mob : Entity
    {
        public MoveDirection facing = new MoveDirection();
        public int speed = 0;
        public double hp = 9999;
        public bool hasBeenTalkedTo = false;
        double dx = 0;
        double dy = 0;
        double angle2 = -1;
        Random rand = new Random();
        int moveReactionDelay = 100;
        int lastChangeDirectionTime = 0;
        protected GraphicsDevice context;
        protected Texture2D drawTexture;
        protected GraphicsDeviceManager graphic;
        public int strength = 0;
        // when the mob hit a player
        double knockBackStartTime = 0;
        protected Mob()
        {
            Load();
        }
        public void Load()
        {
            graphic = Game1.Instance._graphics;
            context = graphic.GraphicsDevice;
            drawTexture = new Texture2D(context, 1, 1);
            drawTexture.SetData(new Color[] { Color.White });
        }
        double lastKnockTime = 0;
        public void Move(double angle)
        {
            
            if(Game1.Instance.GetTime()- lastChangeDirectionTime > moveReactionDelay)
            {
                lastChangeDirectionTime = Game1.Instance.GetTime();
                angle2 = angle;
                moveReactionDelay = 100 + rand.Next(800);
            }
            int curSpeed = speed;
            if(Game1.Instance.GetTime() - knockBackStartTime < 100)
            {
                curSpeed = 35;
            }
            // uses formula to decide where to move the mob
            dy = curSpeed * Math.Sin(angle2);
            dx = curSpeed * Math.Cos(angle2);
            // moves the mob accordingly
            int proposedX = x+  (int)dx;
            int proposedY = y + (int)dy;

            HashSet<Entity> colldingEntities = Game1.Instance.GetCollidingEntities(new Rectangle(x, y, width, height));

            foreach (Entity entity in colldingEntities)
            {
                if (Game1.Instance.GetTime() - lastKnockTime > 600)
                {
                    if (entity.GetType() == typeof(MainPlayer))
                    {
                        knockBackStartTime = Game1.Instance.GetTime();
                        lastChangeDirectionTime = Game1.Instance.GetTime();
                        moveReactionDelay = 120;
                        MainPlayer p = entity as MainPlayer;
                        p.TakeDamage(strength);
                        double reverseAngle = angle2 + 3;
                        angle2 += 3;
                        // reverses the angle(makes it face away from the player)
                        dy = 20 * Math.Sin(reverseAngle);
                        dx = 20 * Math.Cos(reverseAngle);
                        // moves the mob accordingly
                        proposedX = x + (int)dx;
                        proposedY = y + (int)dy;
                    }
                }

                
            }

                x = proposedX;
                y = proposedY;
         

        }
        public abstract void Draw(SpriteBatch spritebatch, int offX, int offY);
    }
}

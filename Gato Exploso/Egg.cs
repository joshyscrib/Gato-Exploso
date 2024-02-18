using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    public class Egg : Entity
    {
        // location of egg
        public int x = 0;
        public int y = 0;

        // if the egg has reached its max distance travelled
        public bool doneTraveling = false;

        // when the egg was fired
        public int createTime = 0;
        // egg image
        protected static Texture2D eggTexture;

        // when the egg should stop moving
        public int endTime = 0;

        // speed and direction of egg
        public int speed = 0;
        public MoveDirection direction = new MoveDirection();
        public Egg(int startX, int startY, MoveDirection dir, int speed, int maxTime)
        {
            // defaults direction to left to avoid any errors
            direction.Left = true;
            Load();
            x = startX;
            y = startY;
            direction = dir;
            createTime = Game1.Instance.GetTime();
            endTime = Game1.Instance.GetTime() + maxTime;
            this.speed = speed;
        }
        public void Load()
        {
            // loads Egg's image if it isn't already loaded
            if(eggTexture == null)
            {
                eggTexture = Game1.GameContent.Load<Texture2D>("Egg");
            }
            
        }
        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            var pos = new Vector2(x, y);
            spriteBatch.Draw(eggTexture, pos, null, Color.White);
        }
        public void Tick()
        {
            if(Game1.Instance.GetTime() >= endTime)
            {
                doneTraveling = true;
            }
        }
    }
}

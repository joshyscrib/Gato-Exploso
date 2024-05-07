using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.HUD
{
    public class Clock
    {
        ContentManager Content;
        // rotation to draw the clock at
        public double time = 270;
        double rotation = 0;
        Texture2D clockTexture;
        Texture2D arrowTexture;
        public Clock(ContentManager context)
        {
            Content = context;
            Load();
        }
        public bool GetDay()
        {
            if (time >= 270 || time < 90)
            {
                return true;
            }
            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            rotation = time * (Math.PI / 180);
            spriteBatch.Draw(
                clockTexture,
                new Rectangle(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 128, 136, 256, 256),
                null,
                Color.White,
                (float)rotation,
                new Vector2(128, 128),
                SpriteEffects.None,
                0
                );
            spriteBatch.Draw(arrowTexture, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 256, 8), Color.White);
        }
        public void Load()
        {
            clockTexture = Content.Load<Texture2D>("Clock");
            arrowTexture = Content.Load<Texture2D>("ClockArrow");
        }
        public void Tick()
        {
            time += 0.1d;
            if (time > 359)
            {
                time -= 360;
            }

            Console.WriteLine("Clock: " + time);
        }
    }
}

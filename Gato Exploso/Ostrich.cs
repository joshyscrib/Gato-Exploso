using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    internal class Ostrich : Player
    {
        public int speed = 1;
        Texture2D curTexture;
        protected GraphicsDevice context;
        protected Texture2D drawTexture;
        protected GraphicsDeviceManager graphic;
        public Ostrich(ContentManager context, double createTime) : base(context, createTime)
        {

        }
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {

            if (curTexture == null)
            {
                curTexture = PTextureLeft;
            }
            
            if (facing.Right) { curTexture = PTextureRight; }
            if (facing.Left) { curTexture = PTextureLeft; }
            Vector2 playerLocation = new Vector2(x, y);
            spriteBatch.Draw(curTexture, playerLocation,  Color.White);
            double healthPercent = hp / 100;
            spriteBatch.Draw(drawTexture, new Rectangle(x - 8, y + 68, 84, 12), Color.Gray);
            spriteBatch.Draw(drawTexture, new Rectangle(x - 7, y + 69, (int)(healthPercent * 84.0), 10), Color.Red);
        }
        public override void Attack()
        {
            
        }
        public void Shoot()
        {

        }

        public override void Load()
        {
            graphic = Game1.Instance._graphics;
            context = graphic.GraphicsDevice;
            drawTexture = new Texture2D(context, 1, 1);
            PTextureLeft = Content.Load<Texture2D>("ostrichL1");
            PTextureRight = Content.Load<Texture2D>("ostrichR1");
            drawTexture.SetData(new Color[] { Color.White });
        }
    }

    }

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
        }
        public override void Attack()
        {
            
        }
        public void Shoot()
        {

        }

        public override void Load()
        {
            PTextureLeft = Content.Load<Texture2D>("ostrichL1");
            PTextureRight = Content.Load<Texture2D>("ostrichR1");
        }
    }

    }

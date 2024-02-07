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
        public Ostrich(ContentManager context, double createTime) : base(context, createTime)
        {

        }
            public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            Texture2D curTexture;
            if (facing.Up || facing.Down) { curTexture = PTextureUp; }
            else if (facing.Right || facing.Left) { curTexture = PTextureRight; }
            else
            {
                curTexture = PTextureUp;
            }
            if (facing.Left || facing.Down)
            {

            }
            Vector2 playerLocation = new Vector2(x, y);
            spriteBatch.Draw(curTexture, playerLocation, Color.Red);
        }
        public override void Attack()
        {
            
        }
    }

    }

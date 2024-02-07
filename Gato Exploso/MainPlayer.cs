using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    internal class MainPlayer : Player
    {
        public int hp = 83;
        public MainPlayer(ContentManager context) : base(context, 0)
        {
            Name = "gato";
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
            Vector2 playerLocation = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
            //Vector2 playerLocation = new Vector2 (x, y);
            spriteBatch.Draw(curTexture, playerLocation, Color.White);
        }
        public override void Attack()
        {

        }
    }

        
}


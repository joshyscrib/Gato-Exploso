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
        Texture2D curTexture;
        public MainPlayer(ContentManager context) : base(context, 0)
        {
            Name = "gato";
            speed = 5;
        }
        
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            if (curTexture == null)
            {
                curTexture = PTextureLeft;
            }
            if (facing.Right) { curTexture = PTextureRight; }
            if (facing.Left) { curTexture = PTextureLeft; }
            Vector2 playerLocation = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
            //Vector2 playerLocation = new Vector2 (x, y);
            spriteBatch.Draw(curTexture, playerLocation, Color.White);
        }
        public override void Attack()
        {

        }

        public override void Load()
        {
            PTextureLeft = Content.Load<Texture2D>("gato3L");
            PTextureRight = Content.Load<Texture2D>("gato3R");
        }
    }

        
}


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    internal class MainPlayer : Player
    {
        public int hp = 83;
        Texture2D curTexture;
        int curFrame = 1;
        protected Texture2D PTexture3Left;
        protected Texture2D PTexture3Right;
        protected Texture2D PTexture4Left;
        protected Texture2D PTexture4Right;
        char face = 'r';
        public MainPlayer(ContentManager context) : base(context, 0)
        {
            Name = "gato";
            speed = 3;
        }
        
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            if (facing.Right)
            {
                face = 'r';
            }
            if (facing.Left)
            {
                face = 'l';
            }
            if (Game1.Instance.GetTime() % 250 == 0)
            {
                curFrame++;
                if(curFrame > 4)
                {
                    curFrame = 1;
                }
            }
            if (curTexture == null)
            {
                curTexture = PTextureLeft;
            }
            switch (curFrame)
            {
                case 1:
                    if (face == 'r') { curTexture = PTextureRight; }
                    if (face == 'l') { curTexture = PTextureLeft; }
                    break;
                case 2:
                    if (face == 'r') { curTexture = PTexture2Right; }
                    if (face == 'l') { curTexture = PTexture2Left; }
                    break;
                case 3:
                    if (face == 'r') { curTexture = PTexture3Right; }
                    if (face == 'l') { curTexture = PTexture3Left; }
                    break;
                case 4:
                    if (face == 'r') { curTexture = PTexture4Right; }
                    if (face == 'l') { curTexture = PTexture4Left; }
                    break;
            }
            if (!moving)
            {
                if (face == 'r') { curTexture = PTexture4Right; }
                if (face == 'l') { curTexture = PTexture4Left; }
            }
            Vector2 playerLocation = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
            //Vector2 playerLocation = new Vector2 (x, y);
            spriteBatch.Draw(curTexture, playerLocation, Color.White);
        }
        public override void Attack()
        {

        }

        public override void Load()
        {
            PTextureLeft = Content.Load<Texture2D>("Gato4L1");
            PTextureRight = Content.Load<Texture2D>("Gato4R1");
            PTexture2Left = Content.Load<Texture2D>("Gato4L2");
            PTexture2Right = Content.Load<Texture2D>("Gato4R2");
            PTexture3Left = Content.Load<Texture2D>("Gato4L3");
            PTexture3Right = Content.Load<Texture2D>("Gato4R3");
            PTexture4Left = Content.Load<Texture2D>("Gato4L4");
            PTexture4Right = Content.Load<Texture2D>("Gato4R4");
        }
    }

        
}


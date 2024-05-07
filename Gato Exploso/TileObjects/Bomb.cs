using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.TileObjects
{
    public class Bomb : TileObject
    {
    
        public double createTime = 0;
        protected Texture2D bombTexture1;
        protected Texture2D bombTexture2;
        protected Texture2D bombTexture;
        // determines which bomb sprite to use
        public bool bombText = false;
        public int range = 1;
        
        public Bomb(double ticks)
        {
            Load();
            type = "bomb";
            // sets the time that the bomb was created
            createTime = ticks;
        }
        public void Load()
        {
            bombTexture1 = Game1.GameContent.Load<Texture2D>("Bomb1");
            bombTexture2 = Game1.GameContent.Load<Texture2D>("Bomb2");
        }
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            // switches which bomb texture to use each few frames
            if(Game1.Instance.GetTime() % 500 == 0)
            {
                bombText = !bombText;
            }
            if(bombText)
            {
                bombTexture = bombTexture1;
            }
            else
            {
                bombTexture = bombTexture2;
            }
            // draws the bomb
            var pos = new Vector2(x, y);
            spriteBatch.Draw(bombTexture, pos, null, Color.White);

            
        }
        public void DetonateBomb()
        {
            createTime = 0;
        }
    }
}

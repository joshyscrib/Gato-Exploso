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
    public class Hud
    {
        GraphicsDevice context;
        public int[,] miniMapData = new int[256, 256];

        ContentManager Content;
        // clock to tell the player if it is day or night
        public Clock clock;
        // spritefonts to write text on the screen
        private SpriteFont font;
        public Hud(ContentManager c)
        {
            Content = c;
            clock = new Clock(Content);
        }

        // scroll wheel amount
        public int scrollAmt = 0;

        // draws rects 
        Texture2D texture;

        // inventory
        Texture2D inventexture;

        // inventory selected item
        Texture2D invecture;
        public int 
            
            
            
            Amt = 0;

        // Bomb sprites for inventory
        Texture2D bombTexture;
        Texture2D mightyTexture;
        Texture2D grenadeTexture;
        Texture2D landMineTexture;
        Texture2D missileTexture;
        Texture2D gravTexture;

        // filter to put over the screen at night
        Color nightyTime = new Color(0, 0, 30, 120);
        public void Load(GraphicsDeviceManager mgr)
        {
            context = mgr.GraphicsDevice;
            texture = new Texture2D(context, 1, 1);
            texture.SetData(new Color[] { Color.White });
            font = Content.Load<SpriteFont>("Norm");
            inventexture = Content.Load<Texture2D>("Inventory");
            invecture = Content.Load<Texture2D>("Invect");
            bombTexture = Content.Load<Texture2D>("Bomb1");
            landMineTexture = Content.Load<Texture2D>("Landmine");
            mightyTexture = Content.Load<Texture2D>("MightyBomb");
            gravTexture = Content.Load<Texture2D>("GravBomb");
        }

        public void Reset()
        {

        }
        public void Draw(SpriteBatch spriteBatch, int health, int gatoX, int gatoY, string quest, List<Mobs.Mob> mobList, List<Player> playerList, int mig, int land, int grav)
        {
            int minimapWidth = 270;
            int minimapHeight = 295;
            int minimapLeft = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - minimapWidth;
            int minimapTop = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - minimapHeight;
            // draws health bar
            spriteBatch.Draw(texture, new Rectangle(10, 10, 510, 72), Color.Gray);
            spriteBatch.Draw(texture, new Rectangle(14, 14, health * 5, 64), Color.Lime);

            // background of minimap
            spriteBatch.Draw(texture, new Rectangle(minimapLeft - 5, minimapTop - 5, 266, 266), Color.Black);
            // Decides tiles-colors and draws minimap
            Color miniColor;
            int campX = -1;
            int campY = -1;
            for (int i = 0; i < miniMapData.GetLength(0); i++)
            {
                for (int j = 0; j < miniMapData.GetLength(1); j++)
                {
                    switch (miniMapData[i, j])
                    {
                        case 1:
                            miniColor = new Color(0, 0, 255);
                            break;
                        case 2:
                            miniColor = new Color(255, 255, 0);
                            break;
                        case 3:
                            miniColor = new Color(0, 255, 0);
                            break;
                        case 4:
                            miniColor = new Color(0, 150, 30);
                            break;
                        case 5:
                            campX = i;
                            campY = j;
                            miniColor = new Color(0, 0, 0);
                            break;
                        default:
                            miniColor = new Color(255, 255, 255);
                            break;
                    }
                    spriteBatch.Draw(texture, new Rectangle(i + (minimapLeft), j + (minimapTop), 1, 1), miniColor);
                }
            }

            if (campX >= 0)
            {
                Color campColor = new Color(255, 255, 255); ;
                if (Game1.Instance.GetTime() % 500 < 250)
                {
                    campColor = new Color(111, 111, 0);
                }
               
                spriteBatch.Draw(texture, new Rectangle(campX + (minimapLeft) + 14, campY + (minimapTop) + 11, 7, 7), campColor);
            }


            // draws gato location on minimap
            spriteBatch.Draw(texture, new Rectangle(gatoX / 32 + (minimapLeft) - 3, gatoY / 32 + (minimapTop) - 3, 6, 6), Color.Blue);

            // draws mobs on minimap
            foreach(var mob in mobList)
            {
                spriteBatch.Draw(texture, new Rectangle(mob.x / 32 + (minimapLeft) - 1, mob.y / 32 + (minimapTop) - 1, 3, 3), Color.Red);
                if(mob.GetType() == typeof(Mobs.Hammy))
                {
                    spriteBatch.Draw(texture, new Rectangle(mob.x / 32 + (minimapLeft) - 3, mob.y / 32 + (minimapTop) - 3, 6, 6), Color.Gold);
                
                }
            }
            // draws ostriches on minimap
            foreach(Player player in playerList)
            // draws a clock that shows if it is day or night
            clock.Draw(spriteBatch);

            // dark blue overlay if it is night
            if (!clock.GetDay())
            {
                spriteBatch.Draw(texture, new Rectangle(0, 0, 3000, 3000), nightyTime);
            }
            // writes current quest on screen
            spriteBatch.DrawString
                (
                font,
                quest,
                new Vector2(55, 25),
                Color.Black
                );
            // draw inventory
            spriteBatch.Draw(inventexture, new Vector2(990,1255), Color.White);
            spriteBatch.Draw(invecture, new Rectangle(990 + (scrollAmt * 136), 1255, 144, 144), Color.White);
            spriteBatch.Draw(bombTexture, new Rectangle(998 + (0 * 136), 1259, 128, 128), Color.White);
            if (mig > 0)
            {
                spriteBatch.Draw(mightyTexture, new Rectangle(998 + (1 * 136), 1259, 128, 128), Color.White);
                spriteBatch.DrawString
                (
                font,
                mig.ToString(),
                new Vector2(998 + (1 * 136) + 108, 1259 + 108),
                Color.White
                );
            }
            if (land > 0)
            {
                spriteBatch.Draw(landMineTexture, new Rectangle(998 + (2 * 136), 1259, 128, 128), Color.White);
                spriteBatch.DrawString
                (
                font,
                land.ToString(),
                new Vector2(998 + (2 * 136) + 108, 1259 + 108),
                Color.White
                );
            }
            if (grav > 0)
            {
                spriteBatch.Draw(gravTexture, new Rectangle(998 + (3 * 136), 1259, 128, 128), Color.White);
                spriteBatch.DrawString
                (
                font,
                grav.ToString(),
                new Vector2(998 + (3 * 136) + 108, 1259 + 108),
                Color.White
                );
            }
        }
    }
}

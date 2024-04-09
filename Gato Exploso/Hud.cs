using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    public class Hud
    {
        GraphicsDevice context;
        public int[,] miniMapData = new int[256, 256];
        ContentManager Content;
        // clock to tell the player if it is day or night
        public Clock clock;
        public Hud(ContentManager c)
        {
            Content = c;
            clock = new Clock(Content);
        }
        Texture2D texture;
        public void Load(GraphicsDeviceManager mgr)
        {
            context = mgr.GraphicsDevice;
            texture = new Texture2D(context, 1, 1);
            texture.SetData(new Color[] { Color.White });
        }
        public void Draw(SpriteBatch spriteBatch, int health, int gatoX, int gatoY)
        {
            // draws health bar
            spriteBatch.Draw(texture, new Rectangle(1100, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 290, 408, 75), Color.Gray);
            spriteBatch.Draw(texture, new Rectangle(1104, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 286, health * 4, 67), Color.Lime);

            // background of minimap
            spriteBatch.Draw(texture, new Rectangle(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 275, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 300, 266, 266), Color.Black);
            // Decides tiles-colors and draws minimap
            Color miniColor;
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
                        default:
                            miniColor = new Color(255, 255, 255);
                            break;
                    }
                    spriteBatch.Draw(texture, new Rectangle(i + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 270), j + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 295), 1, 1), miniColor);
                }
            }
            // draws gato location on minimap
            spriteBatch.Draw(texture, new Rectangle((gatoX / 32) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 272) - 2, (gatoY / 32) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 297) - 2, 6, 6), Color.Red);

            // draws a clock that shows if it is day or night
            clock.Draw(spriteBatch);
            
            // TODO draw inventory
        }
    }
}

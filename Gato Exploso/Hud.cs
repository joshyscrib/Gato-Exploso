using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{ 
    public class Hud
    {
        GraphicsDevice context;
        public Hud()
        {
            
        }
        Texture2D texture;
        public void Load(GraphicsDeviceManager mgr)
        {
            context = mgr.GraphicsDevice;
            texture = new Texture2D(context, 1, 1);
            texture.SetData(new Color[] { Color.White });
        }
        public void Draw(SpriteBatch spriteBatch, int health, int bombX, int bombY)
        {
            spriteBatch.Draw(texture, new Rectangle(1100, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 290, 408, 100), Color.Gray);
            spriteBatch.Draw(texture, new Rectangle(1104, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 286, health * 4, 92), Color.Lime);
            spriteBatch.Draw(texture, new Rectangle(bombX, bombY, 32, 32), Color.Red);
        }
    }
}

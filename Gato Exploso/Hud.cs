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
        Texture2D greyTexture;
        Texture2D limeTexture;
        public void Load(GraphicsDeviceManager mgr)
        {
            context = mgr.GraphicsDevice;
            greyTexture = new Texture2D(context, 1, 1);
            greyTexture.SetData(new Color[] { Color.Gray });
            limeTexture = new Texture2D(context, 1, 1);
            greyTexture.SetData(new Color[] { Color.Lime });
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(greyTexture, new Rectangle(10, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 158, 200, 128), Color.Gray);
            spriteBatch.Draw(limeTexture, new Rectangle(15, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 163, 190, 118), Color.Lime);
        }
    }
}

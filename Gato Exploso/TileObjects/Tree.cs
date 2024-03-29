using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.TileObjects
{
    public class Tree : TileObject
    {
        protected Texture2D treeTextureB;
        protected Texture2D treeTextureT;
        public Tree()
        {
            Load();
            
        }
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(treeTextureB, new Vector2(x, y), Color.White);
        }
        public void DrawTop(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(treeTextureB, new Vector2(x, y), Color.White);
        }


        public void Load()
        {

            treeTextureB = Game1.GameContent.Load<Texture2D>("TreeBottom");
            treeTextureT = Game1.GameContent.Load<Texture2D>("TreeTop");


        }

    }
}


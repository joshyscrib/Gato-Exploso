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
        protected Texture2D treeTexture;
        public Tree(char type)
        {
            Load(type);
        }
        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(treeTexture, new Vector2(x, y), Color.White);
        }


        public void Load(char type)
        {
            if(type == 'L')
            {
               treeTexture = Game1.GameContent.Load<Texture2D>("LTree");
            }
            else
            {
                treeTexture = Game1.GameContent.Load<Texture2D>("DTree");
            }

        }

    }
}


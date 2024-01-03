using Gato_Exploso.TileObjects;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Net.Mime;

namespace Gato_Exploso
{
    internal abstract class Tile : Entity
    {
        public const int tileSide = 32;
        public int width = tileSide;
        public int height = tileSide;
        public bool solid;
        public List<TileObject> objects = new List<TileObject>();
        // variables
        protected Texture2D tileTexture;

        // methods
        public abstract void Load();
        public abstract void Draw(SpriteBatch spriteBatch, int x, int y);

        public void DrawTileObjects(SpriteBatch spriteBatch, int x, int y)
        {
            // draw each object
            foreach (var obj in objects)
            {
                obj.Draw(spriteBatch, x, y);
            }
        }
        public void UpdateGameTime(double curTime)
        {
            HashSet<Bomb> bombsToDelete = new HashSet<Bomb>();
            foreach (Bomb bmb in objects)
            {
                if(curTime - 3000 > bmb.createTime)
                {
                    bombsToDelete.Add(bmb);
                }
            }
            foreach (Bomb bmb in bombsToDelete)
            {
                objects.Remove(bmb);
            }
        }

    }
}

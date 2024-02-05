using Gato_Exploso.TileObjects;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography;

namespace Gato_Exploso
{
    internal abstract class Tile : Entity
    {
        // variables

        // sets variables for the height and width of tiles
        public const int tileSide = 32;
        public int width = tileSide;
        public int height = tileSide;
        // var for if the player can walk through the tile
        public bool solid;
        // vars for collision with bomb explosions
        public bool bombExploded = false;
        public bool bombExplodeRad = false;
        // Id for each different tile type(e.g. grass=1,forest=1, etc)
        public int tileID;
        // list of tile objects
        public List<TileObject> objects = new List<TileObject>();
        // bool for if the tile has a bomb that is currently explodng
        protected bool isExploding = false;
        // tick when the explosion started
        int explosionStartTime = 0;
        protected Texture2D tileTexture;
        protected int curTickCount = 0;
        // methods
        public abstract void Load();
        public abstract void Draw(SpriteBatch spriteBatch, int x, int y);
        public void BombExplode()
        {
            bombExploded = true;
        }

        public void DrawTileObjects(SpriteBatch spriteBatch, int x, int y)
        {
            // draw each object
            foreach (var obj in objects)
            {
                obj.Draw(spriteBatch, x, y);
            }
        }
        public bool isActive()
        {
            if(objects.Count > 0)
            {
                return true;
            }
            if (isExploding)
            {
                return true;
            }
            if (bombExploded)
            {
                return true;
            }
            
            return false;
        }
        public void AddBomb(Bomb b)
        {
            bool hasBomb = false;
            foreach (var tileObj in objects)
            {
                if (tileObj.GetType() == typeof(Bomb)) ;
                {
                    hasBomb = true;
                }
            }
            if (!hasBomb)
            {
                objects.Add(b);
            }
        }
        // starts one-frame animation of bomb explosion
        public void startExplosion()
        {
            explosionStartTime = curTickCount;
            isExploding = true;
        }
        // updates current game time and updates time based objects
        public void UpdateGameTime(double curTime)
        {
            curTickCount = (int)curTime;
            // uses a Hashset to remove every bomb that is past his life time
            HashSet<Bomb> bombsToDelete = new HashSet<Bomb>();
            foreach (Bomb bmb in objects)
            {
                if (curTime - 3000 > bmb.createTime)
                {
                    bombsToDelete.Add(bmb);
                }
            }
            foreach (Bomb bmb in bombsToDelete)
            {
                BombExplode();
                objects.Remove(bmb);
            }
            if (isExploding)
            {
                if (curTickCount - explosionStartTime > 100)
                {
                    isExploding = false;
                }
            }
        }

    }
}

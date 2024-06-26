﻿using Gato_Exploso.TileObjects;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

namespace Gato_Exploso.Tiles
{
    internal abstract class Tile : Entity
    {
        // variables

        // location of tile
        int x = 0;
        int y = 0;

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
        private List<TileObject> objects = new List<TileObject>();

        // bool for if the tile has a bomb that is currently explodng
        protected bool isExploding = false;

        // tick when the explosion started
        int explosionStartTime = 0;
        protected Texture2D tileTexture;

        // range to explode a bomb when it is placed on the tile
        public int range = 0;

        protected int lastModifiedTime = 0;
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
                if (obj.GetType() != typeof(GravityBomb))
                {
                    obj.Draw(spriteBatch, x, y);
                }
                else
                {
                    obj.Draw(spriteBatch, x - 32, y - 32);
                }
            }
        }
        // returns list of tile objects
        public List<TileObject> GetTileObjects()
        {
            return objects;
        }
        public int GetLastUpdatedTick()
        {
            return lastModifiedTime;
        }
        public bool IsExploding()
        {
            return isExploding;
        }

        // checks if the tile is solid 
        public bool IsSolid()
        {
            for (int i = 0; i < objects.Count; i++)
            {
             if (objects[i].GetType() == typeof(Rock) || objects[i].GetType() == typeof(Tree) || this.GetType() == typeof(WaterTile))

                {
                    return true;
                }
            }
            if (solid)
            {
                return true;
            }
            return false;
        }
        public bool isActive()
        {
            if (objects.Count > 0)
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
                if (tileObj.GetType() == typeof(Bomb))
                {
                    hasBomb = true;
                }
                
            }
            if (!hasBomb)
            {
                objects.Add(b);
            }
            foreach (var tileObj in objects)
            {

                if (tileObj.GetType() == typeof(MightyBomb))
                {
                    range = 4;
                }
                else if (tileObj.GetType() == typeof(Bomb))
                {
                    range = 2;
                }
                else if (tileObj.GetType() == typeof(Landmine))
                {
                    range = 3;
                }
            }
            lastModifiedTime = Game1.Instance.GetTime();
        }
        // places a rock
        public void PlaceRock()
        {
            objects.Add(new Rock());
            lastModifiedTime = Game1.Instance.GetTime();
        }
        // places a tree
        public void PlantTree()
        {
            objects.Add(new Tree());
            lastModifiedTime = Game1.Instance.GetTime();
        }
        // starts one-frame animation of bomb explosion
        public void startExplosion()
        {

            explosionStartTime = Game1.Instance.GetTime(); ;
            isExploding = true;

            lastModifiedTime = Game1.Instance.GetTime();
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                TileObject tileObj = objects[i];
                if (tileObj.GetType() == typeof(Bomb))
                {
                    Bomb b = tileObj as Bomb;
                    b.DetonateBomb();
                }
                else if (tileObj.GetType() == typeof(MightyBomb))
                {
                    MightyBomb b = tileObj as MightyBomb;
                    b.DetonateBomb();
                }
                else if (tileObj.GetType() == typeof(Landmine))
                {
                    Landmine b = tileObj as Landmine;
                    b.DetonateBomb();
                }
                else if(tileObj.GetType() != typeof(GravityBomb) && tileObj.GetType() != typeof(Landmine))
                {
                    objects.RemoveAt(i);
                }
            }
        }
        // updates current game time and updates time based objects
        public void UpdateGameTime(double curTime2)
        {

            // uses a Hashset to remove every bomb that is past his life time
            HashSet<Bomb> bombsToDelete = new HashSet<Bomb>();
            foreach (TileObject bmb in objects)
            {
                switch (bmb.type)
                {
                    case "bomb":
                        Bomb bomb = (Bomb)bmb;
                        if (Game1.Instance.GetTime() - 1000 > bomb.createTime)
                        {
                            bombsToDelete.Add(bomb);
                            lastModifiedTime = Game1.Instance.GetTime();
                        }
                        break;
                    case "mighty":
                        MightyBomb mighty = (MightyBomb)bmb;
                        if (Game1.Instance.GetTime() - 1400 > mighty.createTime)
                        {
                            bombsToDelete.Add(mighty);
                            lastModifiedTime = Game1.Instance.GetTime();
                        }
                        break;
                    case "grav":
                        GravityBomb grav = (GravityBomb)bmb;
                        if (Game1.Instance.GetTime() - 6000 > grav.createTime)
                        {
                            bombsToDelete.Add(grav);
                            lastModifiedTime = Game1.Instance.GetTime();
                        }
                        break;
                        
                }
                    
                
            }
            foreach (Bomb bmb in bombsToDelete)
            {
                BombExplode();
                objects.Remove(bmb);
            }
            if (isExploding)
            {
                if (Game1.Instance.GetTime() - explosionStartTime > 2000)
                {
                    isExploding = false;
                    lastModifiedTime = Game1.Instance.GetTime();
                }
            }
        }

    }
}

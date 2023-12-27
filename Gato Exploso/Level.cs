
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Gato_Exploso.TileObjects;
using Microsoft.Xna.Framework.Content;

namespace Gato_Exploso
{
    internal class Level
    {
        // variables

        // amount of tiles per row/column
        public const int xTiles = 1000;
        public const int yTiles = 1000;
        // width/height of each tile
        public const int tileSide = 32;
        // matrix of tiles
        public Tile[,] tiles = new Tile[xTiles, yTiles];

        // constructor
        public Level()
        {
            
        }
        // methods

        // Assigns each tile a type
        public void InitTiles()
        {
            
            for (int i = 0; i < xTiles; i++)
            {
                for (int j = 0; j < yTiles; j++)
                {
                    GrassTile tile = new GrassTile();
                    tiles[i, j] = tile;
                }
            }
        }
        // Draws all of the tiles relative to the player's position
        public void Draw(SpriteBatch spritebatch, Vector2 TLPixel, int SWidth, int SHeight, int playerX, int playerY)
        {
            // sets variables for the offset of tiles
            int tileXOffset = 0 - (int)TLPixel.X;
            int tileYOffset = 0 - (int)TLPixel.Y;
            for (int i = ((playerX - SWidth / 2) / tileSide) - 1; i <= ((playerX + SWidth / 2) / tileSide); i++)
            {
                // checks if tiles are outside of the screen
                if (i < 0)
                {
                    continue;
                }
                if (i > xTiles - 1)
                {
                    continue;
                }
                for (int j = ((playerY - SHeight / 2) / tileSide) - 1; j <= ((playerY + SHeight / 2) / tileSide); j++)
                {
                    // checks if tiles are outside of the screen
                    if (j < 0)
                    {
                        continue;
                    }
                    if (j > yTiles - 1)
                    {
                        continue;
                    }
                    // draws the on-screen tiles
                    if (isTileOnScreen(i, j, SWidth, SHeight, TLPixel.X, TLPixel.Y))
                    {
                        tiles[i, j].Draw(spritebatch, (i * tileSide) + tileXOffset, (j * tileSide) + tileYOffset);
                        tiles[i, j].DrawTileObjects(spritebatch, (i * tileSide) + (playerX - (SWidth/2)), (j * tileSide) + (playerY - (SHeight / 2)));
                    }
                    
                    
                }
            }
        }
        // places a new rock tile on left click
        public void PlaceRock(int offsetX, int offsetY)
        {

            MouseState cursor = new MouseState();
            cursor = Mouse.GetState();
            RockTile rock = new RockTile();
            int placeX = (cursor.X-16) / tileSide;
            int placeY = (cursor.Y - 16) / tileSide;
            tiles[placeX + offsetX, placeY + offsetY] = rock;
//            tiles[(cursor.X / 32) - offsetX, (cursor.Y / 32) - offsetY] = rock;

        }
        public void PlaceBomb()
        {
            MouseState cursor = new MouseState();
            cursor = Mouse.GetState();
            Bomb b = new Bomb(tiles[(int)GetTilePosition(cursor.X, cursor.Y).X, (int)GetTilePosition(cursor.X, cursor.Y).Y].tickCount);
            tiles[(int)GetTilePosition(cursor.X, cursor.Y).X, (int)GetTilePosition(cursor.X, cursor.Y).Y].objects.Add(b);
            
        }
        // two overloads to find which tile the pixel is in
        Vector2 GetTilePosition(int x, int y)
        {
            return new Vector2(x / tileSide, y / tileSide);
        }

        Vector2 GetTilePosition(Vector2 vec)
        {
            return new Vector2((int)vec.X / tileSide, (int)vec.Y / tileSide);
        }

        // Checks if the tile is currently shown on screen
        public bool isTileOnScreen(int tileX, int tileY, int width, int height, float XPixel, float YPixel)
        {
            if ((tileX * tileSide > XPixel && tileX * tileSide < XPixel + width && tileY * tileSide > YPixel && tileY * tileSide < YPixel + height) ||
                (tileX * tileSide + tileSide > XPixel && tileX * tileSide + tileSide < XPixel + width && tileY * tileSide > YPixel && tileY < YPixel + height) ||
                (tileX * tileSide > XPixel && tileX * tileSide < XPixel + width && tileY * tileSide + tileSide > YPixel && tileY * tileSide + tileSide < YPixel + height) ||
                (tileX * tileSide + tileSide > XPixel && tileX * tileSide + tileSide < XPixel + width && tileY * tileSide + tileSide > YPixel && tileY * tileSide + tileSide < YPixel + height))
            {
                return true;
            }
            else { return false; }
        }

    }
}

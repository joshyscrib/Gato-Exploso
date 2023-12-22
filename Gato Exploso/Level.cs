using DigCraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
namespace Gato_Exploso
{
    internal class Level
    {
        // variables

        // amount of tiles per row/column
        public const int xTiles = 1000;
        public const int yTiles = 1000;
        public Tile[,] tiles = new Tile[xTiles, yTiles];

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
            for (int i = ((playerX - SWidth / 2) / 32) - 1; i <= ((playerX + SWidth / 2) / 32); i++)
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
                for (int j = ((playerY - SHeight / 2) / 32) - 1; j <= ((playerY + SHeight / 2) / 32); j++)
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
                        tiles[i, j].Draw(spritebatch, (i * 32) + tileXOffset, (j * 32) + tileYOffset);
                    }
                }
            }
        }
        // places a new rock tile if 'E' is pressed
        public void PlaceRock(int offsetX, int offsetY)
        {

            MouseState cursor = new MouseState();
            cursor = Mouse.GetState();
            RockTile rock = new RockTile();
            int placeX = cursor.X / 32;
            int placeY = cursor.Y / 32;
            tiles[placeX, placeY] = rock;
//            tiles[(cursor.X / 32) - offsetX, (cursor.Y / 32) - offsetY] = rock;

        }
        // two overloads to find which tile the pixel is in
        Vector2 GetTilePosition(int x, int y)
        {
            return new Vector2(x / 32, y / 32);
        }

        Vector2 GetTilePosition(Vector2 vec)
        {
            return new Vector2((int)vec.X / 32, (int)vec.Y / 32);
        }

        // Checks if the tile is currently shown on screen
        public bool isTileOnScreen(int tileX, int tileY, int width, int height, float XPixel, float YPixel)
        {
            if ((tileX * 32 > XPixel && tileX * 32 < XPixel + width && tileY * 32 > YPixel && tileY * 32 < YPixel + height) ||
                (tileX * 32 + 32 > XPixel && tileX * 32 + 32 < XPixel + width && tileY * 32 > YPixel && tileY < YPixel + height) ||
                (tileX * 32 > XPixel && tileX * 32 < XPixel + width && tileY * 32 + 32 > YPixel && tileY * 32 + 32 < YPixel + height) ||
                (tileX * 32 + 32 > XPixel && tileX * 32 + 32 < XPixel + width && tileY * 32 + 32 > YPixel && tileY * 32 + 32 < YPixel + height))
            {
                return true;
            }
            else { return false; }
        }

    }
}

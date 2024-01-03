
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
using System.Diagnostics.CodeAnalysis;

namespace Gato_Exploso
{
    internal class Level
    {
        // variables

        // amount of tiles per row/column
        public const int xTiles = 100;
        public const int yTiles = 100;
        // width/height of each tile
        public const int tileSide = 32;
        // matrix of tiles
        public Tile[,] tiles = new Tile[xTiles, yTiles];

        int offsetX = 0;
        int offsetY = 0;

        double gameTime = 0;
        // constructor
        public Level()
        {
            
        }
        // methods

        public void UpdateOffset(int x, int y)
        {
            offsetX = x;
            offsetY = y;
        }

        public void UpdateTime(double time)
        {
            gameTime = time;
            for(int i = 0; i < xTiles; i++)
            {
                for(int j = 0; j < yTiles; j++)
                {
                    if (IsTileOnScreen(i,j,GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height,offsetX,offsetY))
                    {
                        tiles[i, j].UpdateGameTime(time);
                    }
                    else
                    {
                        int pop = 9;
                    }
                }
            }

        }
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
                    if (IsTileOnScreen(i, j, SWidth, SHeight, TLPixel.X, TLPixel.Y))
                    {
                        int drawX = (i * tileSide) + tileXOffset;
                        int drawY = (j * tileSide) + tileYOffset;
                        tiles[i, j].Draw           (spritebatch, drawX, drawY);
                        tiles[i, j].DrawTileObjects(spritebatch, drawX, drawY);
                    }
                    
                    
                }
            }
        }
        // places a new rock tile on left click
        public void PlaceRock()
        {

            Vector2 vec = GetTileUnderMouse();
            if (vec.X < 0 || vec.Y < 0) { return; }
            RockTile rock = new RockTile();
            tiles[(int)vec.X, (int)vec.Y] = rock;
//            tiles[(cursor.X / 32) - offsetX, (cursor.Y / 32) - offsetY] = rock;

        }

        // Places a bomb where the mouse is at when space is pressed
        public void PlaceBomb()
        {
            Vector2 vec = GetTileUnderMouse();
            if(vec.X < 0 || vec.Y < 0) { return; }
            Bomb b = new Bomb(gameTime);
            Tile curTile = tiles[(int)vec.X, (int)vec.Y];
            curTile.objects.Add(b);
            
        }

        // finds what tile the mouse is on
        public Vector2 GetTileUnderMouse()
        {
            MouseState cursor = new MouseState();
            cursor = Mouse.GetState();
            int tileX = (cursor.X + offsetX) / tileSide;
            int tileY = (cursor.Y + offsetY) / tileSide;
            if ( tileX < 0 || tileX >= tiles.Length || tileY < 0 || tileY >= tiles.Length)
            {
                return new Vector2( -1, -1 );
            }
            return new Vector2( tileX, tileY );

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
        public bool IsTileOnScreen(int tileX, int tileY, int width, int height, float XPixel, float YPixel)
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

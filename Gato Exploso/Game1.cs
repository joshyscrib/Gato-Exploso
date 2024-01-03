using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gato_Exploso
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player gato;
        public static ContentManager GameContent;

        // Move position for collision detection
        int targetX = 300;
        int targetY = 300;
        public const int tileSide = 32;
        Level level1 = new Level();
        int tickCount = 0;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            gato = new Player(Content);
            GameContent = Content;
        }

        protected override void Initialize()
        {

            base.Initialize();
            level1.InitTiles();
        }

        // loads images for different classes
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gato.Load();
        }
        // checks if the new location collides with an object and decides whether or not to move the player
        public void MovePlayer(KeyboardState keyState)
        {
            targetX = gato.x;
            targetY = gato.y;
            if (keyState.IsKeyDown(Keys.W))
            {
                targetY -= gato.speed;
                Tile l = getTileAt(gato.x, targetY - 1);
                Tile r = getTileAt(gato.x + gato.width - 1, targetY - 1);
                if(l is not RockTile && r is not RockTile)
                {
                    gato.MoveY(targetY);
                }
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                targetX -= gato.speed;
                Tile t = getTileAt(targetX, gato.y);
                Tile b = getTileAt(targetX, gato.y + gato.height - 1);
                Tile m = getTileAt(targetX, gato.y + 32);
                if (t is not RockTile && b is not RockTile && m is not RockTile)
                {
                    gato.MoveX(targetX);
                }
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                targetY += gato.speed;
                Tile l = getTileAt(gato.x, targetY + gato.height - 1);
                Tile r = getTileAt(gato.x + gato.width -1, targetY + gato.height - 1);
                if (l is not RockTile && r is not RockTile)
                {
                    gato.MoveY(targetY);
                };
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                targetX += gato.speed;
                Tile t = getTileAt(targetX + gato.width - 1, gato.y);
                Tile b = getTileAt(targetX + gato.width - 1, gato.y + gato.height - 1);
                Tile m = getTileAt(targetX + gato.width - 1, gato.y + 32);
                if (t is not RockTile && b is not RockTile && m is not RockTile)
                {
                    gato.MoveX(targetX);
                }
            }
            // updates the offset between screen and world coordinates
            level1.UpdateOffset(gato.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), gato.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
            // places bomb when space is pressed
            if(keyState.IsKeyDown(Keys.Space))
            {
                level1.PlaceBomb();
            }
            
        }
        // gets the tile at a position
        private Tile getTileAt(int x, int y)
            int tileX = x / 32;
            int tileY = y / 32;
            if (tileX >= 0 && tileY >= 0 && tileX < 1000 && tileY < 1000)
            {
                return level1.tiles[tileX, tileY];
            }

            return null;
        }

        // converts a point on the screen to a location in the world
        public Vector2 ScreenToWorldPoint(Vector2 oldPoint)
        {
            return new Vector2(oldPoint.X + gato.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), oldPoint.Y + gato.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
        }
    
        // main update function, gets called about every 30 milliseconds
        protected override void Update(GameTime gameTime)
        {
            level1.UpdateTime(gameTime.TotalGameTime.TotalMilliseconds);
            var state = Keyboard.GetState();
            MouseState cursor = new MouseState();
            cursor = Mouse.GetState();
            MovePlayer(state);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (cursor.LeftButton == ButtonState.Pressed)
            {
                level1.PlaceRock();
            }
            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }

        // tells each class to draw themselves
        protected override void Draw(GameTime gameTime)
        {
            // finding top-left pixel of the screen
            var topLeftPixel = new Vector2(gato.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), gato.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
            _spriteBatch.Begin();
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

            level1.Draw(_spriteBatch, topLeftPixel, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, gato.x, gato.y);
            gato.Draw(_spriteBatch);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
            
            _spriteBatch.End();
        }

    }
}

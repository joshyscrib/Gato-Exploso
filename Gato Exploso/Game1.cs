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
        MoveDirection lastNetDirection = new MoveDirection();

        // makes a new webserver
        WebServer server = new WebServer();
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
            server.Start();
            server.PlayerAction += Server_PlayerAction;
        }

        private void Server_PlayerAction(object sender, PlayerActionArgs args)
        {
            MovePlayer(args.direction);
            lastNetDirection = args.direction;
        }

        // loads images for different classes
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gato.Load();
        }
        // checks if the new location collides with an object and decides whether or not to move the player
        public void MovePlayer(MoveDirection dir)
        {
            targetX = gato.x;
            targetY = gato.y;
            if (dir.Up)
            {
                targetY -= gato.speed;
                Tile l = getTileAt(gato.x, targetY - 1);
                Tile r = getTileAt(gato.x + gato.width - 1, targetY - 1);
                if (l is not RockTile && r is not RockTile)
                {
                    gato.MoveY(targetY);
                }
                gato.facing = dir;
            }
            if (dir.Left)
            {
                targetX -= gato.speed;
                Tile t = getTileAt(targetX, gato.y);
                Tile b = getTileAt(targetX, gato.y + gato.height - 1);
                Tile m = getTileAt(targetX, gato.y + 32);
                if (t is not RockTile && b is not RockTile && m is not RockTile)
                {
                    gato.MoveX(targetX);
                }
                gato.facing = dir;
            }
            if (dir.Down)
            {
                targetY += gato.speed;
                Tile l = getTileAt(gato.x, targetY + gato.height - 1);
                Tile r = getTileAt(gato.x + gato.width - 1, targetY + gato.height - 1);
                if (l is not RockTile && r is not RockTile)
                {
                    gato.MoveY(targetY);
                }
                gato.facing = dir;
            }
            if (dir.Right)
            {
                targetX += gato.speed;
                Tile t = getTileAt(targetX + gato.width - 1, gato.y);
                Tile b = getTileAt(targetX + gato.width - 1, gato.y + gato.height - 1);
                Tile m = getTileAt(targetX + gato.width - 1, gato.y + 32);
                if (t is not RockTile && b is not RockTile && m is not RockTile)
                {
                    gato.MoveX(targetX);
                }
                gato.facing = dir;
            }
            // updates the offset between screen and world coordinates
            level1.UpdateOffset(gato.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), gato.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));


        }
        // gets the tile at a position
        private Tile getTileAt(int x, int y)
        {
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
            MoveDirection direction = new MoveDirection();
            cursor = Mouse.GetState();
            PlayerActionArgs actionArgs = new PlayerActionArgs();

            if (state.IsKeyDown(Keys.W)) { direction.Up = true; }
            if (state.IsKeyDown(Keys.A)) { direction.Left = true; }
            if (state.IsKeyDown(Keys.S)) { direction.Down = true; }
            if (state.IsKeyDown(Keys.D)) { direction.Right = true; }
            if (state.IsKeyDown(Keys.Space)) { actionArgs.placeBomb = true; }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (cursor.LeftButton == ButtonState.Pressed)
            {
                level1.PlaceRock();
            }
            MovePlayer(direction);
            MovePlayer(lastNetDirection);
            ExexutePlayerAction(actionArgs);


            base.Update(gameTime);
        }
        public void ExexutePlayerAction(PlayerActionArgs act)
        {
            // places bomb when space is pressed
            if (act.placeBomb)
            {
                level1.PlaceBomb();
            }
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

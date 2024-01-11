using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Gato_Exploso
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        // private Player gato;
        private Dictionary<string, Player> _players = new Dictionary<string, Player>();
        private string mainPlayerName = "gato";
        public static ContentManager GameContent;
        public static Game1 Instance;

        // Move position for collision detection
        int targetX = 300;
        int targetY = 300;
        public const int tileSide = 32;
        Level level1 = new Level();
        int tickCount = 0;
        MoveDirection lastNetDirection = new MoveDirection();
        // Makes a Heads-Up Display
        Hud hud = new Hud();
        // makes a new webserver
        WebServer server = new WebServer();
        public Game1()
        {
            Instance = this;
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            GameContent = Content;
            _players.Add(mainPlayerName, new MainPlayer(Content));
        }
        public List<PlayerInfo> GetPlayerInfos()
        {
            var list = new List<PlayerInfo>();
            foreach(var name in _players.Keys)
            {
                var player = _players[name];
                var curPlayer = new PlayerInfo();
                curPlayer.Name = name;
                curPlayer.X = player.x;
                curPlayer.Y = player.y;
                list.Add (curPlayer);

            }
            return list;
        }

        protected Player GetMainPlayer()
        {
            return _players[mainPlayerName];
        }

        protected override void Initialize()
        {

            base.Initialize();
            level1.InitTiles();
            server.Start();
            server.PlayerAction += Server_PlayerAction;
            server.PlayerRegister += Server_PlayerRegister;
        }

        private void Server_PlayerRegister(object sender, RegisterPlayerArgs args)
        {
            string playerName = args.Name;
            if (_players.ContainsKey(playerName))
            {
                return;
            }
            Ostrich ost = new Ostrich(Content);
            ost.Load();
            _players.Add(playerName, ost);

        }

        private void Server_PlayerAction(object sender, PlayerActionArgs args)
        {
            if (_players.ContainsKey(args.name))
            {
                Player pl = _players[args.name];
                if(args.direction.IsDirectionSet())
                {
                    pl.StartMoving();
                    pl.FacePlayer(args.direction);
                }
                else
                {
                    pl.StopMoving();
                }
                MovePlayer(pl);
            }
        }


        // loads images for different classes
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GetMainPlayer().Load();
            hud.Load(_graphics);
        }
        // checks if the new location collides with an object and decides whether or not to move the player
        public void MovePlayer(Player gato)
        {
            if(!gato.moving)
            {
                return;
            }
            targetX = gato.x;
            targetY = gato.y;
            MoveDirection dir = gato.facing;
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
            if (gato is MainPlayer)
            {
                level1.UpdateOffset(gato.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), gato.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
            }


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
            Player p = GetMainPlayer();
            return new Vector2(oldPoint.X + p.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), oldPoint.Y + p.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
        }
        public Vector2 WorldToScreenPoint(Vector2 oldPoint)
        {
            Player p = GetMainPlayer();
            return new Vector2(oldPoint.X + p.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), oldPoint.Y + p.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
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
            Player player = GetMainPlayer();

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
            if (direction.IsDirectionSet())
            {
                player.FacePlayer(direction);
                player.StartMoving();
            }
            else
            {
                player.StopMoving();
            }
            foreach(Player p in _players.Values)
            {
                MovePlayer(p);
            }
            ExexutePlayerAction(actionArgs);


            base.Update(gameTime);
        }
        public void ExexutePlayerAction(PlayerActionArgs act)
        {
            // places bomb when space is pressed
            if (act.placeBomb)
            {
                if (_players[mainPlayerName].facing.Up)
                {
                    level1.PlaceBomb(_players[mainPlayerName].x, _players[mainPlayerName].y - 32);
                }
                if (_players[mainPlayerName].facing.Left)
                {
                    level1.PlaceBomb(_players[mainPlayerName].x - 32, _players[mainPlayerName].y);

                }
                if (_players[mainPlayerName].facing.Down)
                {
                    level1.PlaceBomb(_players[mainPlayerName].x - 24, _players[mainPlayerName].y + 32);
                }
                if (_players[mainPlayerName].facing.Right)
                {
                    level1.PlaceBomb(_players[mainPlayerName].x + 16, _players[mainPlayerName].y);
                }
                
            }
        }

        // tells each class to draw themselves
        protected override void Draw(GameTime gameTime)
        {
            Player p = GetMainPlayer();
            // finding top-left pixel of the screen
            var topLeftPixel = new Vector2(p.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), p.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
            _spriteBatch.Begin();
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

            level1.Draw(_spriteBatch, topLeftPixel, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, p.x, p.y);

            foreach (Player play in _players.Values)
            {
                if(play is Ostrich)
                {
                    play.Draw(_spriteBatch, play.x - (int)topLeftPixel.X, play.y - (int)topLeftPixel.Y);
                }
                else
                {
                    play.Draw(_spriteBatch, play.x, play.y);
                }
            }
            hud.Draw(_spriteBatch);
            base.Draw(gameTime);

            _spriteBatch.End();
        }

    }
}

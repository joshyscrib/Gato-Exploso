using Gato_Exploso.Infos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gato_Exploso.TileObjects;
using System.Security.Cryptography.X509Certificates;

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
        // Location for the bomb placement indicator
        public int bombIndX = 0;
        public int bombIndY = 0;
        // Move position for collision detection
        int targetX = 300;
        int targetY = 300;
        // Number for how many rows of tiles there are
        const int tileRows = 100;
        public const int tileSide = 32;
        Level level1 = new Level();
        int tickCount = 0;
        MoveDirection lastNetDirection = new MoveDirection();
        // Makes a Heads-Up Display
        Hud hud = new Hud();
        // makes a new webserver
        WebServer server = new WebServer();
        double currentTime = 0;
        // for when ostriches die
        Random rand = new Random();
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
            this.Exiting += Game1_Exiting;
        }

        private void Game1_Exiting(object sender, EventArgs e)
        {
            server.Stop();
        }
        // returns current time
        public int GetTime()
        {
            return (int)currentTime;
        }

        public GameInfo GetGameInfo(string playerName, int time)
        {
            GameInfo info = new GameInfo();
            var list = new List<PlayerInfo>();
            foreach (var name in _players.Keys)
            {
                var player = _players[name];
                var curPlayer = new PlayerInfo();
                curPlayer.Name = name;
                curPlayer.X = player.x;
                curPlayer.Y = player.y;
                list.Add(curPlayer);
                curPlayer.Health = (int)player.hp;
                curPlayer.Facing = player.facing;

            }
            if (!_players.ContainsKey(playerName))
            {
                return null;
            }
            Player curplayer = _players[playerName];
            List<Tile> updatedTiles = level1.GetUpdatedTiles(getTileAt(curplayer.x, curplayer.y).x, getTileAt(curplayer.x, curplayer.y).y, 17, time);
            List<TileInfo> tileInfos = new List<TileInfo>();
            // optimize by only sending new stuff
            foreach (Tile tile in updatedTiles)
            {
                if (tile.GetLastUpdatedTick() < time)
                {
                    continue;
                }
                TileInfo information = new TileInfo();
                information.X = tile.x;
                information.Y = tile.y;
                if (tile.IsExploding())
                {
                    information.State = 1;
                }

                if (tile.GetTileObjects().Count > 0)
                {

                    List<ObjectInfo> objectInfos = new List<ObjectInfo>();
                    foreach (var obj in tile.GetTileObjects())
                    {
                        string objType = "";
                        if (obj.GetType() == typeof(Bomb))
                        {
                            objType = "bomb";
                        }
                        if (obj.GetType() == typeof(Rock))
                        {
                            objType = "rock";
                        }
                        ObjectInfo objectInfo = new ObjectInfo();
                        objectInfo.ObjectType = objType;
                        objectInfos.Add(objectInfo);
                    }
                    information.ObjectInfos = objectInfos;

                }
                tileInfos.Add(information);
            }

            info.TileInfos = tileInfos;
            info.PlayerInfos = list;
            info.GameTime = (int)currentTime;
            return info;
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
            Ostrich ost = new Ostrich(Content, currentTime);
            ost.Name = playerName;
            ost.Load();
            _players.Add(playerName, ost);

        }

        private void Server_PlayerAction(object sender, PlayerActionArgs args)
        {
            if (_players.ContainsKey(args.name))
            {
                Player pl = _players[args.name];
                if (args.direction != null && args.direction.IsDirectionSet())
                {
                    pl.StartMoving();
                    pl.FacePlayer(args.direction);
                }
                else
                {
                    pl.StopMoving();
                }
                //      MovePlayer(pl);
                if (args.attack)
                {
                    Attack(pl);
                }
            }
        }
        // ostrich attack
        public void Attack(Player player)
        {
            HashSet<Vector2> attackCoords = new HashSet<Vector2>();

            if (player.facing.Up)
            {
                attackCoords.Add(new Vector2(player.x / 32, (player.y / 32) - 1));
                attackCoords.Add(new Vector2((player.x / 32) + 1, (player.y / 32) - 1));
                attackCoords.Add(new Vector2((player.x / 32) - 1, (player.y / 32) - 1));
            }
            if (player.facing.Left)
            {
                attackCoords.Add(new Vector2((player.x / 32) - 1, (player.y / 32) - 1));
                attackCoords.Add(new Vector2((player.x / 32) - 1, (player.y / 32)));
                attackCoords.Add(new Vector2((player.x / 32) - 1, (player.y / 32) + 1));
            }
            if (player.facing.Down)
            {
                attackCoords.Add(new Vector2((player.x / 32) - 1, (player.y / 32) + 1));
                attackCoords.Add(new Vector2((player.x / 32), (player.y / 32) + 1));
                attackCoords.Add(new Vector2((player.x / 32) + 1, (player.y / 32) + 1));
            }
            if (player.facing.Right)
            {
                attackCoords.Add(new Vector2((player.x / 32) + 1, (player.y / 32) - 1));
                attackCoords.Add(new Vector2((player.x / 32) + 1, (player.y / 32)));
                attackCoords.Add(new Vector2((player.x / 32) + 1, (player.y / 32) + 1));
            }

            foreach (Player play in _players.Values)
            {
                if (play != player)
                {

                    HashSet<Vector2> playerCoords = GetPlayerTiles(play);
                    if (attackCoords.Intersect(playerCoords).Count() > 0)
                    {
                        play.hp -= 10;
                    }
                    if (play.Name == "gato")
                    {
                        play.hp += 20;
                    }

                }
            }
        }

        // loads images for different classes
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GetMainPlayer().Load();
            hud.Load(_graphics);
        }
        private bool isPassableTile(Tile t)
        {
            if (t == null || t.IsSolid())
            {
                return false;
            }
            return true;
        }
        // checks if the new location collides with an object and decides whether or not to move the player
        public void MovePlayer(Player gato)
        {
            lock (this)
            {
                if (!gato.moving)
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
                    if (isPassableTile(l) && isPassableTile(r) && gato.y > 0)
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
                    if (isPassableTile(t) && isPassableTile(b) && isPassableTile(m) && gato.x > 0)
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
                    if (isPassableTile(l) && isPassableTile(r) && gato.y < 3168)
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

                    if (isPassableTile(t) && isPassableTile(b) && isPassableTile(m) && gato.x < 3168)
                    {
                        gato.MoveX(targetX);
                    }
                    gato.facing = dir;
                }

                // updates the offset between screen and world coordinates
                if (gato is MainPlayer)
                {
                    level1.UpdateOffset(gato.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), gato.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
                    if (gato.facing.Up)
                    {
                        bombIndX = (_players[mainPlayerName].x + 16) / 32;
                        bombIndY = (_players[mainPlayerName].y - 16) / 32;


                    }
                    if (gato.facing.Left)
                    {
                        bombIndX = (_players[mainPlayerName].x - 16) / 32;
                        bombIndY = (_players[mainPlayerName].y + 48) / 32;

                    }
                    if (gato.facing.Down)
                    {
                        bombIndX = (_players[mainPlayerName].x + 16) / 32;
                        bombIndY = (_players[mainPlayerName].y + 80) / 32;
                    }
                    if (gato.facing.Right)
                    {
                        bombIndX = (_players[mainPlayerName].x + 48) / 32;
                        bombIndY = (_players[mainPlayerName].y + 48) / 32;
                    }
                }
            }

        }
        // gets the tile at a position
        private Tile getTileAt(int x, int y)
        {
            int tileX = x / 32;
            int tileY = y / 32;
            if (tileX >= 0 && tileY >= 0 && tileX < tileRows && tileY < tileRows)
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
            HashSet<string> playersToDie = new HashSet<string>();
            foreach (Player play in _players.Values)
            {
                if (play.hp <= 0)
                {
                    playersToDie.Add(play.Name);
                }
                else
                {
                    MovePlayer(play);
                }
            }


            foreach (string s in playersToDie)
            {

                Random ry = new Random();
                _players[s].x = rand.Next((Level.xTiles - 4) * 32);
                _players[s].y = ry.Next((Level.yTiles - 4) * 32);
            }
            currentTime = gameTime.TotalGameTime.TotalMilliseconds;
            if (_players["gato"].hp < 81)
            {
                _players["gato"].hp += 20;
            }

            List<Player> playersToRemove = new List<Player>();
            foreach (var curPlayer in _players.Values)
            {
                curPlayer.UpdateTime(gameTime.TotalGameTime.TotalMilliseconds);
                if (curPlayer.hp < 100)
                {
                    curPlayer.hp += 0.1;
                }
                if (curPlayer.IsTimedOut() && curPlayer.Name != "gato")
                {
                    playersToRemove.Add(curPlayer);
                }
            }

            foreach (var curPlayer in playersToRemove)
            {
                _players.Remove(curPlayer.Name);
            }

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
            foreach (Player p in _players.Values)
            {
                MovePlayer(p);
            }
            ExexutePlayerAction(actionArgs);
            HashSet<Vector2> activeTiles = level1.GetActiveTileCoords();
            HashSet<Vector2> explodingTiles = new HashSet<Vector2>();
            foreach (Vector2 coord in activeTiles)
            {
                Tile curTile = level1.GetTile(coord);
                if (curTile != null && curTile.IsExploding())
                {
                    explodingTiles.Add(coord);
                }
            }
            foreach (Player p in _players.Values)
            {
                // set of all tiles the player is touching
                HashSet<Vector2> playerTiles = GetPlayerTiles(p);

                if (playerTiles.Intersect(explodingTiles).Count() > 0)
                {
                    p.hp -= 0.4;
                }
            }

            base.Update(gameTime);
        }

        private HashSet<Vector2> GetPlayerTiles(Player p)
        {
            // set of all tiles the player is touching
            HashSet<Vector2> playerTiles = new HashSet<Vector2>
                {
                        // top left
                    new Vector2(p.x / tileSide, p.y / tileSide),
                        // mid left
                    new Vector2(p.x / tileSide, (p.y + (p.height / 2)) / tileSide),
                        // bottom left
                    new Vector2(p.x / tileSide, (p.y + p.height) / tileSide),
                        // top right
                    new Vector2((p.x + p.width) / tileSide, p.y / tileSide),
                        // mid right
                    new Vector2(p.x / tileSide, p.y + (p.height / tileSide)),
                        //bottom right
                    new Vector2(p.x / tileSide, (p.y + p.height) / tileSide)
                };
            return playerTiles;
        }
        public void ExexutePlayerAction(PlayerActionArgs act)
        {
            // places bomb when space is pressed
            if (act.placeBomb)
            {
                if (_players[mainPlayerName].facing.Up)
                {
                    level1.PlaceBomb(_players[mainPlayerName].x + 16, _players[mainPlayerName].y - 16);
                }
                else if (_players[mainPlayerName].facing.Left)
                {
                    level1.PlaceBomb(_players[mainPlayerName].x - 16, _players[mainPlayerName].y + 48);

                }
                else if (_players[mainPlayerName].facing.Down)
                {
                    level1.PlaceBomb(_players[mainPlayerName].x + 16, _players[mainPlayerName].y + 80);
                }
                else if (_players[mainPlayerName].facing.Right)
                {
                    level1.PlaceBomb(_players[mainPlayerName].x + 48, _players[mainPlayerName].y + 48);
                }
            }
        }
        public string GetGameWorld()
        {
            StringBuilder world = new StringBuilder();

            for (int i = 0; i < Level.xTiles; i++)
            {
                for (int j = 0; j < Level.xTiles; j++)
                {
                    world.Append(level1.tiles[i, j].tileID.ToString());
                }
            }
            return world.ToString();
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
                if (play is Ostrich)
                {
                    play.Draw(_spriteBatch, play.x - (int)topLeftPixel.X, play.y - (int)topLeftPixel.Y);
                }
                else
                {
                    play.Draw(_spriteBatch, play.x, play.y);
                }
            }

            base.Draw(gameTime);

            // draws HUD
            hud.Draw(_spriteBatch, (int)_players[mainPlayerName].hp, (bombIndX * 32) - (level1.offsetX), (bombIndY * 32) - (level1.offsetY));
            _spriteBatch.End();
        }
    }
}

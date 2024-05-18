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
using Gato_Exploso.Tiles;
using Gato_Exploso.HUD;
using Gato_Exploso.Mobs;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Reflection.Metadata;
using System.Net.Http.Headers;
using System.Data;
using System.Threading;

namespace Gato_Exploso
{
    // test git
    public class Game1 : Game
    {

        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Song music;
        public Song menuMusic;
        public Song curSong;
        public Song campSong;
        public Song hammySong;

        // private Player gato;
        private Dictionary<string, Player> _players = new Dictionary<string, Player>();
        private string mainPlayerName = "gato";
        public static ContentManager GameContent;
        public static Game1 Instance;

        // pausing the game
        bool paused = true;
        double lastPausedTime = 0;

        // Location for the bomb placement indicator
        public int bombIndX = 0;
        public int bombIndY = 0;

        // Move position for collision detection
        int targetX = 300;
        int targetY = 300;



        // difficulty
        public static int difficultyNumber = 2;
        // Number for how many rows of tiles there are
        const int tileRows = 256;
        public const int tileSide = 32;
        Level level1 = new Level();
        int tickCount = 0;

        // random seed or not
        public static bool randSeed = false;

        // Makes a Heads-Up Display
        Hud hud;
        // makes a menu
        Menu menu;

        // if the boss fight has been initiated
        public bool bossFightStarted = false;

        // makes a new webserver
        WebServer server = new WebServer();
        double currentTime = 0;

        // for when ostriches die
        Random rand = new Random();

        // list of eggs(bullets)
        public List<Egg> eggs = new List<Egg>();

        // list of monsters(mobs)
        public List<Mob> mobs = new();
        // if it is day or night
        public static bool day = true;
        // boss
        public Hammy hammy = null;

        // how many bombs the player has
        public int mightys = 0;
        public int mines = 0;
        public int gravs = 0;

        // if it is the first time the game has been unpaused
        bool hasCooled = false;
        bool turtleIntroStarted = false;

        bool isMousePressed = false;
        // current quest to show and all other quests
        string curQuest = "";
        string quest1 = "Find Timmy the Turtle at the campsite";
        string quest3 = "Defeat Hammy at the marked location";

        // current squirrell dialog
        int curDiall = 1;
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
            hud = new Hud(Content);
            menu = new Menu(Content);
            menu.SetMenuType("splash");
            menu.PlayerAction += Menu_PlayerAction;
            ResetGame();
        }

        private void Menu_PlayerAction(object sender, string act)
        {
            if (act == "cool")
            {
                ResumeGame();

            }
            if (act == "start")
            {
                MediaPlayer.Play(music);
                ResetGame();
            }

        }

        // spawn turtle
        public void SpawnTurtle(string which)
        {
            if (which == "squirrell")
            {
                if (!_players.ContainsKey("squirrell"))
                {
                    var newTurtle = new Turtle(Content, 0, "squirrell");
                    _players.Add("squirrell", newTurtle);
                }

                Turtle squirrell = (Turtle)_players["squirrell"];
                squirrell.x = GetMainPlayer().x - 130;
                squirrell.y = GetMainPlayer().y - 130;
            }
            if (which == "timmy")
            {
                if (!_players.ContainsKey("timmy"))
                {
                    var newTurtle = new Turtle(Content, 0, "timmy");
                    _players.Add("timmy", newTurtle);
                }

                Turtle squirrell = (Turtle)_players["timmy"];
                squirrell.x = 800;
                squirrell.y = 7000;
            }
        }
        // hams
        public List<Ham> hams = new List<Ham>();
        private void Game1_Exiting(object sender, EventArgs e)
        {
            // stops running the server on the web
            server.Stop();
        }
        // returns current time
        public int GetTime()
        {
            return (int)currentTime;
        }

        public void ResetGame()
        {
            hud.clock.time = 270;
            hud.clock.rotation = 0;
            foreach(Player p in _players.Values)
            {
                SpawnPlayer(p);
            }
            beginBossFight = false;
            turtleIntroStarted = false;
            lastFullUpdateTime.Clear();
            level1.ResetLevel();
            bossFightStarted = false;
            hammy = null;
            hams.Clear();
            // set players scores to 0
            mobs.Clear();
            for (int i = 0; i < tileRows; i++)
            {
                for (int j = 0; j < tileRows; j++)
                {
                    hud.miniMapData[i, j] = level1.tiles[i, j].tileID;
                }
            }
            foreach (Player p in _players.Values)
            {
                if (p.GetType() != typeof(Turtle))
                {
                    SpawnPlayer(p);

                }
            }
            paused = false;
            if (!randSeed)
            {
                SpawnTurtle("squirrell");
            }
            mightys = 0;
            mines = 0;
            gravs = 0;
            curDiall = 0;
        }

        // spawns players/mobs
        public void SpawnMob(Mob dude)
        {
            bool landFound = false;
            Random random = new Random();
            while (!landFound)
            {
                int nx = random.Next(256);
                int ny = random.Next(256);
                if (level1.tiles[nx, ny].GetType() != typeof(WaterTile))
                {
                    landFound = true;
                    dude.x = nx * 32;
                    dude.y = ny * 32;
                    dude.spawnX = dude.x;
                    dude.spawnY = dude.y;
                }
            }
        }
        // spawns players/mobs
        public void SpawnPlayer(Player dude)
        {
            if (!randSeed && dude.Name == "gato")
            {
                dude.x = 7500;
                dude.y = 7800;
            }
            else if(dude.GetType() != typeof(Turtle))
            {
                bool landFound = false;
                Random random = new Random();
                while (!landFound)
                {
                    int nx = random.Next(256);
                    int ny = random.Next(256);
                    if (level1.tiles[nx, ny].GetType() != typeof(WaterTile))
                    {
                        landFound = true;
                        _players[dude.Name].x = nx * 32;
                        _players[dude.Name].y = ny * 32;
                    }
                }

            }
            dude.points = 0;
            _players[dude.Name].hp = 100;
        }
        public void SpawnPlayer(Player dude, int spx, int spy)
        {
            _players[dude.Name].x = spx * 32;
            _players[dude.Name].y = spy * 32;
            dude.points = 0;
            _players[dude.Name].hp = 100;
        }

        private Dictionary<String, int> lastFullUpdateTime = new Dictionary<string, int>();

        public GameInfo GetGameInfo(string playerName, int time)
        {
            GameInfo info = new GameInfo();
            var list = new List<PlayerInfo>();
            var playerInfoStrings = new List<String>();
            foreach (var name in _players.Keys)
            {
                // sets data for each player to return to the server
                var player = _players[name];
                var curPlayer = new PlayerInfo();
                curPlayer.Name = name;
                curPlayer.X = player.x;
                curPlayer.Y = player.y;
                list.Add(curPlayer);

                curPlayer.Health = (int)player.hp;
                curPlayer.Facing = player.facing;
                curPlayer.Points = player.points;
                playerInfoStrings.Add(curPlayer.CreateInfoString());
            }
            if (!_players.ContainsKey(playerName))
            {
                return null;
            }
            Player curplayer = _players[playerName];
            List<Tile> updatedTiles = level1.GetUpdatedTiles(getTileAt(curplayer.x, curplayer.y).x, getTileAt(curplayer.x, curplayer.y).y, 30, time);
            List<TileInfo> tileInfos = new List<TileInfo>();
            bool fullUpdate = false;
            if (!lastFullUpdateTime.ContainsKey(playerName))
            {
                fullUpdate = true;
                lastFullUpdateTime.Add(playerName, (int)currentTime);
            }

            var tileInfoStrings = new List<string>();
            var objectInfoStrings = new List<string>();
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
                        // returns the type of tile objects for the server
                        string objType = "";
                        if (obj.GetType() == typeof(Bomb) || obj.GetType() == typeof(MightyBomb) || obj.GetType() == typeof(GravityBomb))
                        {
                            objType = "bomb";
                        }
                        if (obj.GetType() == typeof(Rock))
                        {
                            objType = "rock";
                        }
                        if (obj.GetType() == typeof(Tree))
                        {
                            objType = "tree";
                        }
                        // adds the object to a list of objects to give the webserF
                        ObjectInfo objectInfo = new ObjectInfo();
                        objectInfo.ObjectType = objType;
                        objectInfos.Add(objectInfo);

                        objectInfoStrings.Add(objectInfo.CreateInfoString(tile.x, tile.y));
                    }
                    if (tile.GetTileObjects().Count == 0)
                    {
                        String noObject = String.Format("{0},{1},{2}", tile.x, tile.y, 0);
                        objectInfoStrings.Add(noObject);
                    }
                    information.ObjectInfos = objectInfos;
                    tileInfos.Add(information);

                }

                //  if (fullUpdate)
                {
                    tileInfos.Add(information);
                    // tileInfoStrings.Add(information.CreateInfoString());
                }
            }

            List<MobInfo> mobInfos = new List<MobInfo>();
            var mobInfoStrings = new List<string>();

            foreach (var curMob in mobs)
            {
                // if (FindDistance(curMob.x, curMob.y, _players[playerName].x, _players[playerName].y) < 600)
                {
                    MobInfo mobInfo = new MobInfo();
                    mobInfo.X = curMob.x;
                    mobInfo.Y = curMob.y;
                    mobInfo.Id = curMob.id;
                    if (curMob.GetType() == typeof(BouncyTriangle))
                    {
                        mobInfo.Type = MobType.Triangle;
                    }
                    if (curMob.GetType() == typeof(Porcupine))
                    {
                        mobInfo.Type = MobType.Porcupine;
                    }


                    mobInfos.Add(mobInfo);
                    mobInfoStrings.Add(mobInfo.CreateInfoString());
                }

            }


            List<ProjectileInfo> eggInfos = new List<ProjectileInfo>();
            for (int i = eggs.Count - 1; i >= 0; i--)
            {
                Egg curEgg = eggs[i];
                ProjectileInfo pInfo = new ProjectileInfo();
                pInfo.StartX = curEgg.startX;
                pInfo.StartY = curEgg.startY;
                pInfo.Id = curEgg.Id; pInfo.Speed = curEgg.speed + 5;
                if (curEgg.direction.Up)
                {
                    pInfo.Direction = 1;
                }
                if (curEgg.direction.Left)
                {
                    pInfo.Direction = 2;
                }
                if (curEgg.direction.Down)
                {
                    pInfo.Direction = 3;
                }
                if (curEgg.direction.Right)
                {
                    pInfo.Direction = 4;
                }
                pInfo.EndTime = curEgg.endTime;
                pInfo.Type = 1;
                eggInfos.Add(pInfo);
            }
            info.ProjectileInfos = eggInfos;
            //  info.TileInfos = tileInfos;
            //  info.MobInfos = mobInfos;
            //  info.PlayerInfos = list;
            info.GameTime = (int)currentTime;



            StringBuilder tileInfoBuilder = new StringBuilder();
            for (int i = 0; i < tileInfoStrings.Count; i++)
            {
                tileInfoBuilder.Append(tileInfoStrings[i]);
                if (i < tileInfoStrings.Count - 1)
                {
                    tileInfoBuilder.Append(':');
                }
            }
            info.TileInfoString = tileInfoBuilder.ToString();

            StringBuilder playerInfoBuilder = new StringBuilder();
            for (int i = 0; i < playerInfoStrings.Count; i++)
            {
                playerInfoBuilder.Append(playerInfoStrings[i]);
                if (i < playerInfoStrings.Count - 1)
                {
                    playerInfoBuilder.Append(':');
                }
            }
            info.PlayerInfoString = playerInfoBuilder.ToString();

            StringBuilder mobInfoBuilder = new StringBuilder();
            for (int i = 0; i < mobInfoStrings.Count; i++)
            {
                mobInfoBuilder.Append(mobInfoStrings[i]);
                if (i < mobInfoStrings.Count - 1)
                {
                    mobInfoBuilder.Append(':');
                }
            }
            info.MobInfoString = mobInfoBuilder.ToString();


            StringBuilder objectInfoBuilder = new StringBuilder();
            for (int i = 0; i < objectInfoStrings.Count; i++)
            {
                objectInfoBuilder.Append(objectInfoStrings[i]);
                if (i < objectInfoStrings.Count - 1)
                {
                    objectInfoBuilder.Append(':');
                }
            }
            info.ObjectInfoString = objectInfoBuilder.ToString();
            return info;

        }

        public Player GetMainPlayer()
        {
            return _players[mainPlayerName];
        }
        public List<Player> GetPlayers()
        {
            List<Player> li = new List<Player>();
            try
            {
                foreach (Player p in _players.Values)
                {
                    li.Add(p);
                }
            }
            catch (Exception e) { }
            return li;
        }

        protected override void Initialize()
        {
            // starts main game sequences (makes tiles, begins events, runs web server)
            base.Initialize();
            // level1.InitTiles();

            server.Start();
            server.PlayerAction += Server_PlayerAction;
            server.PlayerRegister += Server_PlayerRegister;
            if (!randSeed)
            {
                SpawnPlayer(GetMainPlayer(), 7810, 7112);
            }
            else
            {
                SpawnPlayer(GetMainPlayer());
            }
        }
        public HashSet<Entity> GetEntities()
        {
            HashSet<Entity> entities = new HashSet<Entity>();
            entities.Add(GetMainPlayer());
            return entities;
        }

        // dialog after kazoo
        public void ShowPastDial()
        {
            switch (curDiall)
            {
                case 4:
                    PauseGame("QintroDial2");
                    SpawnTurtle("squirrell");
                    break;
                case 6:
                    PauseGame("QtimmyDial2");
                    SpawnTurtle("squirrell");
                    break;
                case 7:
                    PauseGame("QbossDial");
                    SpawnTurtle("squirrell");

                    break;
                default:
                    break;
            }
        }
        public void PlayerTouchedTurle()
        {
            switch (curDiall)
            {
                case 0:
                    PauseGame("introDial");
                    curDiall = 2;
                    return;
                case 1:
                    PauseGame("introDial");
                    curDiall = 2;
                    return;

                case 2:
                    PauseGame("introDial2");
                    curDiall = 3;
                    return;
                case 3:
                    PauseGame("introDial3");
                    curDiall = 4;
                    return;


                case 4:
                    if (GetMainPlayer().points >= (10 * difficultyNumber))
                    {
                        PauseGame("bossDial");
                        curDiall = 7;
                        return;
                    }
                    else
                    {
                        PauseGame("timmyDial");
                        curDiall = 5;
                        return;
                    }
                case 5:
                    if (GetMainPlayer().points >= (10 * difficultyNumber))
                    {
                        PauseGame("bossDial");
                        curDiall = 7;
                        return;
                    }
                    else
                    {
                        PauseGame("timmyDial2");
                        curDiall = 6;
                        return;
                    }

                case 6:
                    PauseGame("bossDial");
                    curDiall = 7;
                    return;

            }
        }
        public HashSet<Entity> GetCollidingEntities(Rectangle rect, Player play)
        {
            HashSet<Entity> entities = new HashSet<Entity>();
            if (CollisionDetection.AreRectsInEachOther(rect.X, rect.Y, rect.Width, rect.Height, play))
            {
                entities.Add(play);
            }
            /* foreach (Player ost in _players.Values)
            {
                if (ost.GetType() != typeof(MainPlayer) && CollisionDetection.AreRectsInEachOther(rect.X, rect.Y, rect.Width, rect.Height, ost))
                {
                    entities.Add(ost);
                }

            }
            */
            return entities;
        }
        // registers players on the web
        private void Server_PlayerRegister(object sender, RegisterPlayerArgs args)
        {
            // gives players information
            string playerName = args.Name;
            if (_players.ContainsKey(playerName))
            {
                return;
            }
            Ostrich ost = new Ostrich(Content, currentTime);
            ost.Name = playerName;
            ost.Load();

            // adds new player to the list of player
            _players.Add(playerName, ost);
            SpawnPlayer(ost);

        }
        // handles player action like moving and attacking
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
                if (args.shoot)
                {
                    Shoot(pl);
                }
            }
        }
        // ostrich attack
        public void Attack(Player player)
        {
            HashSet<Vector2> attackCoords = new HashSet<Vector2>();
            // decides which tiles to attack based on which direction the player is facing
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
                    // 
                    HashSet<Vector2> playerCoords = GetPlayerTiles(play);
                    if (attackCoords.Intersect(playerCoords).Count() > 0)
                    {
                        /* remove*/
                        play.hp -= 6;
                        if (play.Name == "gato")
                        {
                            play.hp -= 10;
                        }
                    }


                }
            }
        }
        public void Shoot(Player player)
        {
            // sets cooldown between shots
            if (Instance.currentTime - player.lastTimeFired >= 400)
            {
                player.lastTimeFired = Instance.currentTime;
                Egg egg = new Egg(player.x, player.y, player.facing, player.speed + 7, 3000, player.Name);
                // sets egg's location, direction and speed to match the player that shot it

                eggs.Add(egg);
            }

        }

        // loads images for different classes
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GetMainPlayer().Load();
            // loads menu and hud
            hud.Load(_graphics);
            menu.Load(_graphics);

            music = Content.Load<Song>("ExplosoFields");
            menuMusic = Content.Load<Song>("MenuMusic");
            campSong = Content.Load<Song>("CampSite");
            hammySong = Content.Load<Song>("HamMusica");
            curSong = music;
            MediaPlayer.Play(menuMusic);
            MediaPlayer.IsRepeating = true;
            PauseGame("splash");
            MediaPlayer.Volume = (float).5;
            if (!randSeed)
            {
                SpawnTurtle("timmy");
            }
        }

        public bool IsPassableCoord(int x, int y)
        {
            Tile t = getTileAt(x, y);
            if (t == null)
            {
                return false;
            }

            return isPassableTile(t);
        }

        // checks if the provide tile is solid
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
            // makes sure only 1 thread can access function at a time
            lock (this)
            {
                if (!gato.moving)
                {
                    return;
                }
                targetX = gato.x;
                targetY = gato.y;
                MoveDirection dir = gato.facing;
                // moves and sets facing direction based on which key is pressed
                if (dir.Up)
                {
                    targetY -= gato.speed;
                    Tile l = getTileAt(gato.x, targetY - 1);
                    Tile r = getTileAt(gato.x + gato.width - 1, targetY - 1);
                    Tile m = getTileAt(gato.x + (gato.width / 2), gato.y);
                    if (isPassableTile(l) && isPassableTile(r) && isPassableTile(m) && gato.y > 0 && l.GetType() != typeof(WaterTile) && r.GetType() != typeof(WaterTile) && m.GetType() != typeof(WaterTile))
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
                    Tile m = getTileAt(targetX, gato.y + (gato.height / 2));
                    if (isPassableTile(t) && isPassableTile(b) && isPassableTile(m) && gato.x > 0 && t.GetType() != typeof(WaterTile) && b.GetType() != typeof(WaterTile) && m.GetType() != typeof(WaterTile))
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
                    Tile m = getTileAt(gato.x + (gato.width / 2), gato.y + gato.height);
                    if (isPassableTile(l) && isPassableTile(r) && isPassableTile(m) && gato.y < (tileRows * 32) - gato.height && l.GetType() != typeof(WaterTile) && r.GetType() != typeof(WaterTile) && m.GetType() != typeof(WaterTile))
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

                    if (isPassableTile(t) && isPassableTile(b) && isPassableTile(m) && gato.x < (tileRows * 32) - gato.width && t.GetType() != typeof(WaterTile) && b.GetType() != typeof(WaterTile) && m.GetType() != typeof(WaterTile))
                    {
                        gato.MoveX(targetX);
                    }
                    gato.facing = dir;
                }

                // updates the offset between screen and world coordinates
                if (gato is MainPlayer)
                {
                    if (gato.x < 1500 && gato.y > 6500)
                    {
                        if (curSong != campSong)
                        {
                            MediaPlayer.Play(campSong);
                            curSong = campSong;
                        }
                    }
                    else
                    {
                        if (curSong != music)
                        {
                            MediaPlayer.Play(music);
                            curSong = music;
                        }
                    }
                    level1.UpdateOffset(gato.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), gato.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
                    // sets location of the bomb placement indicator
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
        // moves all eggs
        public void MoveEggs()
        {
            for (int i = eggs.Count - 1; i >= 0; i--)
            {
                // removes eggs that have traveled too far
                Egg curEgg = eggs[i];
                curEgg.Tick();
                if (curEgg.doneTraveling)
                {
                    BreakEgg(curEgg);

                    continue;
                    /*  (:  */
                }
                // checks what direction the egg is moving and moves it in that direction
                if (curEgg.direction.Up)
                {
                    curEgg.y -= curEgg.speed;
                }
                if (curEgg.direction.Left)
                {
                    curEgg.x -= curEgg.speed;
                }
                if (curEgg.direction.Down)
                {
                    curEgg.y += curEgg.speed;
                }
                if (curEgg.direction.Right)
                {
                    curEgg.x += curEgg.speed;
                }
                // damages players and removes egg if it collides with smth
                foreach (Player hooman in _players.Values)
                {
                    if (CollisionDetection.AreRectsInEachOther(hooman.x, hooman.y, hooman.width, hooman.height, curEgg) && hooman.Name != curEgg.nameOfLauncher)
                    {

                        hooman.hp -= 17;
                        if (hooman.hp <= 0)
                        {
                            _players[curEgg.nameOfLauncher].points++;
                        }
                        BreakEgg(curEgg);
                    }
                    // checks if the egg collides with a solid tile

                }
                if (level1.GetTile(curEgg.x / 32, curEgg.y / 32).IsSolid())
                {
                    BreakEgg(curEgg);
                }
                else if (level1.GetTile((curEgg.x + 32) / 32, curEgg.y / 32).IsSolid())
                {
                    BreakEgg(curEgg);
                }
                else if (level1.GetTile(curEgg.x / 32, (curEgg.y + 32) / 32).IsSolid())
                {
                    BreakEgg(curEgg);
                }
                else if (level1.GetTile((curEgg.x + 32) / 32, (curEgg.y + 32) / 32).IsSolid())
                {
                    BreakEgg(curEgg);
                }

            }

        }
        // uses triginometry to find the distance between 2 things
        public double FindDistance(double x1, double y1, double x2, double y2)
        {
            // finds the distance in terms of X and Y, which represent 2 legs of a right triangle
            double sideX = x1 - x2;
            double sideY = y1 - y2;
            // squares both sides
            double sx2 = sideX * sideX;
            double sy2 = sideY * sideY;
            // adds them and takes the square root to find the hypotenuse, which is the distance
            double hyp = Math.Sqrt(sx2 + sy2);
            return hyp;
        }
        // moves all mobs
        public void MoveMobs()
        {

            // moves mobs toward gravity bomb(if it is near)
            for (int i = 0; i < mobs.Count; i++)
            {
                // changes whether the mob is good or bad mased on day/night
                if (day)
                {
                    mobs[i].good = mobs[i].defaultGood;
                }
                else
                {
                    mobs[i].good = !mobs[i].defaultGood;
                }
                bool isGravNearMob = false;
                int gravX = 0;
                int gravY = 0;
                HashSet<Vector2> gravCoords = level1.GetCoordsAroundTile(mobs[i].x / 32, mobs[i].y / 32, 15);
                foreach(Vector2 cord in gravCoords)
                {
                    if (level1.GetTile(cord).GetTileObjects().Count > 0)
                    {
                        if (level1.GetTile(cord).GetTileObjects()[0].GetType() == typeof(GravityBomb))
                        {
                            isGravNearMob = true;
                            gravX = (int)cord.X * 32;
                            gravY = (int)cord.Y * 32;
                        }
                    }
                }
                if (isGravNearMob)
                {
                    Mob curMob = mobs[i];
                    if (gravX > mobs[i].x)
                    {
                        mobs[i].x += 3;
                    }
                    if (gravX < mobs[i].x)
                    {
                        mobs[i].x -= 3;
                    }
                    if (gravY > mobs[i].y)
                    {
                        mobs[i].y += 3;
                    }
                    if (gravY < mobs[i].y)
                    {
                        mobs[i].y -= 3;
                    }
                    if (CollisionDetection.AreRectsInEachOther(curMob.x, curMob.y, 64, 64, GetMainPlayer()))
                    {
                        GetMainPlayer().TakeDamage(curMob.strength);
                    }
                    try
                    {
                        if (getTileAt((int)curMob.x, (int)curMob.y).GetTileObjects().Count > 0)
                        {
                            if (getTileAt((int)curMob.x, (int)curMob.y / 32).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)curMob.x, (int)curMob.y).bombExploded = true;
                            }
                        }
                        else if (getTileAt(curMob.x + 32, curMob.y).GetTileObjects().Count > 0)
                        {
                            if (getTileAt(curMob.x + 32, curMob.y).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)(curMob.x + 32), (int)curMob.y).bombExploded = true;
                            }
                        }
                        else if (getTileAt((int)curMob.x, (int)(curMob.y + 32)).GetTileObjects().Count > 0)
                        {
                            if (getTileAt((int)curMob.x, (int)(curMob.y + 32)).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)curMob.x, (int)(curMob.y + 32)).bombExploded = true;
                            }
                        }
                        else if (getTileAt((int)(curMob.x + 32), (int)(curMob.y + 32)).GetTileObjects().Count > 0)
                        {
                            if (getTileAt((int)(curMob.x + 32), (int)(curMob.y + 32)).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)(curMob.x + 32), (int)(curMob.y + 32)).bombExploded = true;
                            }
                        }
                        else if (getTileAt((int)curMob.x, (int)curMob.y).GetTileObjects().Count > 0)
                        {
                            if (getTileAt((int)curMob.x, (int)curMob.y).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)curMob.x, (int)curMob.y).bombExploded = true;
                            }
                        }
                        else if ((getTileAt(curMob.x + 64, curMob.y).GetTileObjects().Count > 0))
                        {
                            if (getTileAt(curMob.x + 64, curMob.y).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt(curMob.x + 64, curMob.y).bombExploded = true;
                            }
                        }
                        else if (getTileAt((int)curMob.x, (int)(curMob.y + 64)).GetTileObjects().Count > 0)
                        {
                            if (getTileAt((int)curMob.x, (int)(curMob.y + 64)).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)curMob.x, (int)(curMob.y + 64)).bombExploded = true;
                            }
                        }
                        else if (getTileAt((int)(curMob.x + 64), (int)(curMob.y + 64)).GetTileObjects().Count > 0)
                        {
                            if (getTileAt(curMob.x + 64, curMob.y + 64).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)(curMob.x + 64), (int)(curMob.y + 64)).bombExploded = true;
                            }
                        }
                    }
                    catch (Exception e) { }



                }
                else
                {
                    Mob curMob = mobs[i];
                    
                    try
                    {
                        if (getTileAt((int)curMob.x, (int)curMob.y).GetTileObjects().Count > 0)
                        {
                            if (getTileAt((int)curMob.x, (int)curMob.y / 32).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)curMob.x, (int)curMob.y).bombExploded = true;
                            }
                        }
                        else if (getTileAt(curMob.x + 32, curMob.y).GetTileObjects().Count > 0)
                        {
                            if (getTileAt(curMob.x + 32, curMob.y).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)(curMob.x + 32), (int)curMob.y).bombExploded = true;
                            }
                        }
                        else if (getTileAt((int)curMob.x, (int)(curMob.y + 32)).GetTileObjects().Count > 0)
                        {
                            if (getTileAt((int)curMob.x, (int)(curMob.y + 32)).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)curMob.x, (int)(curMob.y + 32)).bombExploded = true;
                            }
                        }
                        else if (getTileAt((int)(curMob.x + 32), (int)(curMob.y + 32)).GetTileObjects().Count > 0)
                        {
                            if (getTileAt((int)(curMob.x + 32), (int)(curMob.y + 32)).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)(curMob.x + 32), (int)(curMob.y + 32)).bombExploded = true;
                            }
                        }
                        else if (getTileAt((int)curMob.x, (int)curMob.y).GetTileObjects().Count > 0)
                        {
                            if (getTileAt((int)curMob.x, (int)curMob.y).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)curMob.x, (int)curMob.y).bombExploded = true;
                            }
                        }
                        else if ((getTileAt(curMob.x + 64, curMob.y).GetTileObjects().Count > 0))
                        {
                            if (getTileAt(curMob.x + 64, curMob.y).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt(curMob.x + 64, curMob.y).bombExploded = true;
                            }
                        }
                        else if (getTileAt((int)curMob.x, (int)(curMob.y + 64)).GetTileObjects().Count > 0)
                        {
                            if (getTileAt((int)curMob.x, (int)(curMob.y + 64)).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)curMob.x, (int)(curMob.y + 64)).bombExploded = true;
                            }
                        }
                        else if (getTileAt((int)(curMob.x + 64), (int)(curMob.y + 64)).GetTileObjects().Count > 0)
                        {
                            if (getTileAt(curMob.x + 64, curMob.y + 64).GetTileObjects()[0].GetType() == typeof(Landmine))
                            {
                                getTileAt((int)(curMob.x + 64), (int)(curMob.y + 64)).bombExploded = true;
                            }
                        }
                    }
                    catch (Exception e) { }
                    {
                    }
                    // moves mobs if they are in range of the player
                    if (curMob.GetType() == typeof(BouncyTriangle) || curMob.GetType() == typeof(Porcupine))
                    {
                        if (!curMob.good)
                        {
                            MoveMobTowardPlayer(curMob, GetMainPlayer());
                        }
                        else
                        {
                            MoveMobTowardSpawn(curMob);
                        }
                    }
                    if (curMob.GetType() == typeof(Hammy))
                    {
                        MoveMobTowardPlayer(curMob, GetMainPlayer());
                    }
                    
                }

            }
        }
        public Player FindNearestOstrich(Mob mob)
        {
            int distance = int.MaxValue;
            Player nearestPlayer = null;
            foreach (Player p in _players.Values)
            {
                if (p.GetType() != typeof(MainPlayer))
                {
                    double sx = p.x - mob.x;
                    double sy = p.y - mob.y;
                    double dx = sx * sx;
                    double dy = sy * sy;
                    int curDist = (int)Math.Sqrt(dx + dy);
                    if (curDist < distance)
                    {
                        distance = curDist;
                        nearestPlayer = p;
                    }
                }
            }
            return nearestPlayer;
        }
        public void MoveMobTowardPlayer(Mob curMob, Player player)
        {
            if (player == null)
            {
                return;
            }
            if (FindDistance(player.x, player.y, curMob.x, curMob.y) <= 2800 + (difficultyNumber * 500) || curMob.GetType() == typeof(Hammy))
            {
                Player p = player;
                // finds X and Y distance between player and mob
                double sx = p.x - curMob.x;
                double sy = p.y - curMob.y;
                // uses trig to find the angle at which the mob should moves
                double angleInRadians = Math.Atan2(sy, sx);

                curMob.Move(angleInRadians);
            }
        }
        // mobs go back to where they came from
        public void MoveMobTowardSpawn(Mob curMob)
        {
            double sx = curMob.spawnX - curMob.x;
            double sy = curMob.spawnY - curMob.y;
            // uses trig to find the angle at which the mob should moves
            double angleInRadians = Math.Atan2(sy, sx);
            curMob.Move(angleInRadians);
        }
        public double radiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }
        // what to do when the given egg breaks
        public void BreakEgg(Egg eg)
        {
            eggs.Remove(eg);
        }
        // gets the tile at a position
        private Tile getTileAt(int x, int y)
        {
            int tileX = x / 32;
            int tileY = y / 32;
            if (tileY >= 256)
            {
                tileY = 255;
            }

            if (tileX >= 256)
            {
                tileX = 255;
            }

            if (tileX < 0)
            {
                tileX = 0;
            }

            if (tileY < 0)
            {
                tileY = 0;
            }

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
        // vice versa
        public Vector2 WorldToScreenPoint(Vector2 oldPoint)
        {
            Player p = GetMainPlayer();
            return new Vector2(oldPoint.X + p.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), oldPoint.Y + p.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
        }

        // main update function, gets called about every 30 milliseconds
        Random randy = new Random();
        bool beginBossFight = false;
        // dtermines what type of bomb the player will get
        Random bombRand = new Random();
        protected void ResetScrollAmt()
        {
            if (hud.scrollAmt > 3)
            {
                hud.scrollAmt -= 4;
                ResetScrollAmt();
            }
            else if (hud.scrollAmt < 0)
            {
                hud.scrollAmt += 4;
                ResetScrollAmt();
            }
            else
            {
                return;
            }
        }
        protected override void Update(GameTime gameTime)
        {

            bool hasBeenTouched = false;
            MouseState cursor = new MouseState();
            cursor = Mouse.GetState();
            hud.scrollAmt = -cursor.ScrollWheelValue / 120;
            ResetScrollAmt();
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape) && gameTime.TotalGameTime.TotalMilliseconds - lastPausedTime > 250)
            {
                lastPausedTime = gameTime.TotalGameTime.TotalMilliseconds;
                if (paused)
                {
                    if (menu.menuType != "splash" && menu.menuType != "dead")
                    {
                        ResumeGame();
                    }
                    if ((menu.menuType == "introDial3" || menu.menuType == "bossDial") && _players.ContainsKey("squirrell"))
                    {
                        _players.Remove("squirrell");
                    }
                    if (menu.menuType == "timmyDial2" && _players.ContainsKey("timmy"))
                    {
                        _players.Remove("timmy");
                    }
                    if (menu.menuType == "QintroDial2" && _players.ContainsKey("squirrell"))
                    {
                        _players.Remove("squirrell");
                    }
                    if (menu.menuType == "QtimmyDial2" && _players.ContainsKey("squirrell"))
                    {
                        _players.Remove("squirrell");
                    }
                    if (menu.menuType == "bossDial" && _players.ContainsKey("squirrell"))
                    {
                        _players.Remove("squirrell");
                    }
                }
                else
                {
                    PauseGame("pause");
                }
            }
            if (state.IsKeyDown(Keys.Q) && gameTime.TotalGameTime.TotalMilliseconds - lastPausedTime > 250)
            {
                ShowPastDial();
            }
            if (paused)
            {
                if (cursor.LeftButton == ButtonState.Pressed)
                {
                    menu.Click(cursor.X, cursor.Y);
                }
                return;
            }
            if (tickCount % 4 == 0)
            {
                MoveMobs();
            }
            if (mobs.Count < 6 * (1 + (difficultyNumber / 2)))
            {

                BouncyTriangle tri = new BouncyTriangle(Content);
                SpawnMob(tri);
                mobs.Add(tri);

                Porcupine pork = new Porcupine(Content);
                SpawnMob(pork);
                mobs.Add(pork);

            }


            if (tickCount % 10 == 0)
            {
                hud.clock.Tick();
                day = hud.clock.GetDay();
            }

            // moves hams
            if (bossFightStarted)
            {
                if (GetTime() % 4000 == 0)
                {
                    hams.Add(hammy.ShootHam(GetMainPlayer().x, GetMainPlayer().y, Content));
                }

                for (int i = hams.Count - 1; i >= 0; i--)
                {
                    Ham ham = hams[i];

                    ham.Move();
                    if (CollisionDetection.AreRectsInEachOther(GetMainPlayer().x, GetMainPlayer().y, GetMainPlayer().width, GetMainPlayer().height, ham))
                    {
                        hams.Remove(ham);
                        GetMainPlayer().TakeDamage(40);
                    }
                }
                if (hammy.hp <= 0)
                {
                    PauseGame("win");
                }

            }
            MoveEggs();
            HashSet<string> playersToDie = new HashSet<string>();
            foreach (Player play in _players.Values)
            {

                if (play.hp <= 0)
                {
                    if (play.Name == "gato")
                    {
                        PauseGame("dead");
                    }
                    playersToDie.Add(play.Name);


                }
                else
                {
                    MovePlayer(play);
                }
            }

            // removes dead mobs
            HashSet<Mob> mobsToDie = new HashSet<Mob>();
            foreach (Mob mm in mobs)
            {

                if (mm.hp <= 0)
                {
                    mobsToDie.Add(mm);
                    GetMainPlayer().points += difficultyNumber;
                    if (GetMainPlayer().points >= (10 * difficultyNumber))
                    {
                        if (!randSeed)
                        {
                            SpawnTurtle("squirrell");
                        }
                        beginBossFight = true;
                    }
                    int br = bombRand.Next(1, 4);

                    switch (br)
                    {
                        case 1:
                            mightys++;
                            break;
                        case 2:
                            mines++;
                            break;
                        case 3:
                            gravs++;
                            break;
                        default: break;

                    }

                }
            }

            if (_players.ContainsKey("squirrell"))
            {
                var turtle = _players["squirrell"];
                if (CollisionDetection.AreRectsInEachOther(GetMainPlayer().x, GetMainPlayer().y, 90, 90, turtle) && !hasBeenTouched)
                {
                    PlayerTouchedTurle();
                }
            }
            if (_players.ContainsKey("timmy"))
            {
                var turtle = _players["timmy"];
                if (CollisionDetection.AreRectsInEachOther(GetMainPlayer().x, GetMainPlayer().y, 90, 90, turtle))
                {
                    PlayerTouchedTurle();
                }
            }


            if (beginBossFight && !bossFightStarted)
            {
                StartBossFight();
            }
            foreach (Mob mob in mobsToDie)
            {
                mobs.Remove(mob);
            }
            // resets players' healths and randomly teleports them
            foreach (string s in playersToDie)
            {

                Random ry = new Random();
                
            }
            currentTime = gameTime.TotalGameTime.TotalMilliseconds;
            if (_players["gato"].hp < 100)
            {
                _players["gato"].hp += 0.023;
            }

            List<Player> playersToRemove = new List<Player>();
            foreach (var curPlayer in _players.Values)
            {

                curPlayer.UpdateTime(gameTime.TotalGameTime.TotalMilliseconds);
                // regenerates health
                if (curPlayer.hp < 100 && curPlayer.Name != "gato")
                {
                    curPlayer.hp += 0.015;
                }
                // waits to remove idle players as not to disrupt the rest of the loop
                if (curPlayer.IsTimedOut() && curPlayer.GetType() == typeof(Ostrich))
                {
                    playersToRemove.Add(curPlayer);
                }
            }
            // removes idle players
            foreach (var curPlayer in playersToRemove)
            {
                _players.Remove(curPlayer.Name);
            }

            level1.UpdateTime(gameTime.TotalGameTime.TotalMilliseconds);


            MoveDirection direction = new MoveDirection();

            PlayerActionArgs actionArgs = new PlayerActionArgs();
            Player player = GetMainPlayer();
            // sets key pressed variables based on recieved key inputs
            if (state.IsKeyDown(Keys.W)) { direction.Up = true; }
            if (state.IsKeyDown(Keys.A)) { direction.Left = true; }
            if (state.IsKeyDown(Keys.S)) { direction.Down = true; }
            if (state.IsKeyDown(Keys.D)) { direction.Right = true; }
            if (state.IsKeyDown(Keys.Space)) { actionArgs.placeBomb = true; }
            if (state.IsKeyDown(Keys.P)) { _players["gato"].hp += 5; }
            if (state.IsKeyDown(Keys.B)) { if (!bossFightStarted) { StartBossFight(); } }

            if (state.IsKeyDown(Keys.Space)) { }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();
            if (cursor.LeftButton == ButtonState.Pressed)
            {
                if (!isMousePressed)
                {
                    actionArgs.placeBomb = true;
                }
                isMousePressed = true;
            }
            if (cursor.RightButton == ButtonState.Pressed)
            {
                if (!isMousePressed)
                {
                    actionArgs.placeRock = true;
                }
                isMousePressed = true;
            }
            else
            {
                isMousePressed = false;
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
            foreach (Mob m in mobs)
            {
                // set of all tiles the player is touching
                HashSet<Vector2> mobTiles = GetMobTiles(m);

                if (mobTiles.Intersect(explodingTiles).Any())
                {
                    m.hp -= 0.475;
                }
            }
            base.Update(gameTime);
        }
        // pauses the game
        public void PauseGame(string type)
        {
            menu.SetMenuType(type);
            paused = true;


        }
        // resumes the game
        public void ResumeGame()
        {
            paused = false;
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
        private HashSet<Vector2> GetMobTiles(Mob p)
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

                        // bottom right
                    new Vector2(p.x / tileSide, (p.y + p.height) / tileSide)

                };
            return playerTiles;
        }
        double lastBombTime = 0;
        // if the player has enough bombs to place one
        bool hasEnoughBombs = false;
        public void ExexutePlayerAction(PlayerActionArgs act)
        {
            // place rock
            if (act.placeRock)
            {
                if (currentTime - lastBombTime >= 1000)
                {
                    if (_players[mainPlayerName].facing.Down)
                    {
                        level1.PlaceRock(_players[mainPlayerName].x + 16, _players[mainPlayerName].y - 16);
                    }
                    else if (_players[mainPlayerName].facing.Right)
                    {
                        level1.PlaceRock(_players[mainPlayerName].x - 16, _players[mainPlayerName].y + 48);

                    }
                    else if (_players[mainPlayerName].facing.Up)
                    {
                        level1.PlaceRock(_players[mainPlayerName].x + 16, _players[mainPlayerName].y + 80);
                    }
                    else if (_players[mainPlayerName].facing.Left)
                    {
                        level1.PlaceRock(_players[mainPlayerName].x + 48, _players[mainPlayerName].y + 48);
                    }
                    lastBombTime = currentTime;
                }
            }
            // places bomb when space is pressed
            if (act.placeBomb)
            {
                switch (hud.scrollAmt)
                {
                    case 0:
                        hasEnoughBombs = true;
                        break;
                    case 1:
                        if (mightys > 0)
                        {
                            hasEnoughBombs = true;
                            if (currentTime - lastBombTime >= 1000)
                            {
                                mightys--;
                            }

                        }
                        break;
                    case 2:
                        if (mines > 0)
                        {
                            hasEnoughBombs = true;
                            if (currentTime - lastBombTime >= 1000)
                            {
                                mines--;
                            }

                        }
                        break;
                    case 3:
                        if (gravs > 0)
                        {
                            hasEnoughBombs = true;
                            if (currentTime - lastBombTime >= 1000)
                            {
                                gravs--;
                            }

                        }
                        break;
                }
                if (currentTime - lastBombTime >= 1000 && hasEnoughBombs)
                {
                    if (_players[mainPlayerName].facing.Up)
                    {
                        level1.PlaceBomb(_players[mainPlayerName].x + 16, _players[mainPlayerName].y - 16, hud.scrollAmt);
                    }
                    else if (_players[mainPlayerName].facing.Left)
                    {
                        level1.PlaceBomb(_players[mainPlayerName].x - 16, _players[mainPlayerName].y + 48, hud.scrollAmt);

                    }
                    else if (_players[mainPlayerName].facing.Down)
                    {
                        level1.PlaceBomb(_players[mainPlayerName].x + 16, _players[mainPlayerName].y + 80, hud.scrollAmt);
                    }
                    else if (_players[mainPlayerName].facing.Right)
                    {
                        level1.PlaceBomb(_players[mainPlayerName].x + 48, _players[mainPlayerName].y + 48, hud.scrollAmt);
                    }
                    lastBombTime = currentTime;
                }
            }
            hasEnoughBombs = false;
        }
        // spawns hammy and initiates the boss fight
        public void StartBossFight()
        {
            bossFightStarted = true;
            hammy = new Hammy(Content);
            hammy.x = 7800;
            hammy.y = 200;
            mobs.Add(hammy);
            MediaPlayer.Play(hammySong);
        }
        // returns the full list of tiles in a string
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

        //returns the list of all Eggs as a string
        public string GetEggs()
        {
            StringBuilder eggString = new StringBuilder();
            for (int i = 0; i < eggs.Count; i++)
            {
                eggString.Append(eggs[i]);
            }
            return eggString.ToString();
        }

        // players in a list instead of a dictionary
        List<Player> playerL = new List<Player>();
        // tells each class to draw themselves
        protected override void Draw(GameTime gameTime)
        {
            Player p = GetMainPlayer();
            // finding top-left pixel of the screen
            var topLeftPixel = new Vector2(p.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), p.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
            _spriteBatch.Begin();
            GraphicsDevice.Clear(Color.SeaGreen);

            if (curDiall < 2)
            {
                level1.Draw(_spriteBatch, topLeftPixel, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, p.x, p.y, "camp");
            }
            else
            {
                level1.Draw(_spriteBatch, topLeftPixel, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, p.x, p.y, "boss");
            }

            foreach (Player play in _players.Values)
            {
                if (play is Ostrich)
                {
                    play.Draw(_spriteBatch, play.x - (int)topLeftPixel.X, play.y - (int)topLeftPixel.Y);
                }
                else if (play is Turtle)
                {
                    play.Draw(_spriteBatch, play.x - (int)topLeftPixel.X, play.y - (int)topLeftPixel.Y);
                }
                else
                {
                    play.Draw(_spriteBatch, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - 8, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - 12);
                }
            }
            // draws eggs
            for (int i = 0; i < eggs.Count; i++)
            {
                Egg curEgg = eggs[i];
                curEgg.Draw(_spriteBatch, curEgg.x - (int)topLeftPixel.X, curEgg.y - (int)topLeftPixel.Y);
            }
            base.Draw(gameTime);

            // draws monsters/mobs
            for (int i = 0; i < mobs.Count; i++)
            {
                Mob curMob = mobs[i];
                curMob.Draw(_spriteBatch, -(int)topLeftPixel.X, -(int)topLeftPixel.Y);
            }
            // draws ham
            foreach (Ham hum in hams)
            {
                hum.Draw(_spriteBatch, -(int)topLeftPixel.X, -(int)topLeftPixel.Y);
            }
            MainPlayer player = new MainPlayer(Content);
            if (p.GetType() == typeof(MainPlayer))
            {
                player = (MainPlayer)p;
            }
            // draws HUD
            if (bossFightStarted)
            {
                hud.Draw(_spriteBatch, (int)_players[mainPlayerName].hp, _players["gato"].x, _players["gato"].y, curQuest, mobs, GetPlayers(), mightys, mines, gravs, true);
            }
            else
            {
                hud.Draw(_spriteBatch, (int)_players[mainPlayerName].hp, _players["gato"].x, _players["gato"].y, curQuest, mobs, GetPlayers(), mightys, mines, gravs, false);
            }
            if (paused)
            {
                if (_players.ContainsKey("squirrell"))
                {
                    foreach (Player pp in _players.Values)
                    {
                        playerL.Add(pp);

                    }
                    menu.Draw(_spriteBatch, new Vector2(_players["squirrell"].x - topLeftPixel.X, _players["squirrell"].y - topLeftPixel.Y), level1.randSeedDisp, playerL, topLeftPixel);
                }
                else
                {
                    menu.Draw(_spriteBatch, new Vector2(3000, 3000), level1.randSeedDisp, playerL, topLeftPixel);
                }
            }



            _spriteBatch.End();
        }
    }
}

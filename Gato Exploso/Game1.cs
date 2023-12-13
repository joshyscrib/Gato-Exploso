using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Numerics;

namespace Gato_Exploso
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player gato;
        public static ContentManager GameContent;
        Level level1 = new Level();
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
            // TODO: Add your initialization logic here

            base.Initialize();
            level1.InitTiles();
        }

        // loads images for different classes
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gato.Load();
            // TODO: use this.Content to load your game content here
        }

        // main update function, gets called about every 30 milliseconds
        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            gato.Move(state);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // finding top-left pixel of the screen
            var topLeftPixel = new Microsoft.Xna.Framework.Vector2(gato.x - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2), gato.y - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2));
            _spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            level1.Draw(_spriteBatch, topLeftPixel, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, gato.x, gato.y);
            gato.Draw(_spriteBatch);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
            _spriteBatch.End();
        }
        public bool IsPointInRect(int x, int y, int width, int height, Vector2 point)
        {
            if(point.x >= x && point.x <= x + width && point.y >= y && point.y <= y){
                return true;
            }
            else{
            return false;
            }
        
        }
        public bool AreRectsInEachOther(int x1, int y1, int width1, int height1, int x2, int y2, int width2, int height2)
        {
        if()
        }
}

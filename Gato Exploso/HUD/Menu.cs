using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Gato_Exploso.HUD
{
    public class Menu
    {
        public delegate void PlayerActionHandler(object sender, string act);
        public event PlayerActionHandler PlayerAction;
        GraphicsDevice context;
        ContentManager Content;
        Texture2D texture;

        // splash screen image(game poster)
        Texture2D splashTexture;

        // spritefont to write text on the screen
        private SpriteFont font;
        private SpriteFont titleFont;
        private string menuType = "";
        public string coords = "";

        // regulates difficulty change speed
        int lastDiffChange = 0;

        // credits
        Texture2D credits;
        bool creditsPlaying = false;

        // time
        int menuTicks = 0;

        // width and height of the screen
        int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        int creditY = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        // gato's final point count
        int gatoPointsDead;
        bool hasGatoPoints = false;

        public Menu(ContentManager content)
        {
            Content = content;
        }
        public void Load(GraphicsDeviceManager mgr)
        {
            context = mgr.GraphicsDevice;
            texture = new Texture2D(context, 1, 1);
            texture.SetData(new Color[] { Color.White });
            font = Content.Load<SpriteFont>("Norm");
            titleFont = Content.Load<SpriteFont>("Title");
            splashTexture = Content.Load<Texture2D>("GatoPosterMenu");
            credits = Content.Load<Texture2D>("cReDiTsSs");
        }

        public void Draw(SpriteBatch spriteBatch, int ps)
        {
            menuTicks++;
            if (menuType == "splash")
            {
                switch (Game1.difficultyNumber)
                {
                    case 1:
                        spriteBatch.Draw(texture, new Rectangle(50, 600, 900, 900), Color.Lime);
                        spriteBatch.DrawString(titleFont, "Easy", new Vector2(370, 1000), Color.Black);
                        break;
                    case 2:
                        spriteBatch.Draw(texture, new Rectangle(50, 600, 900, 900), Color.Yellow);
                        spriteBatch.DrawString(titleFont, "Normal", new Vector2(340, 1000), Color.Black);
                        break;
                    case 3:
                        spriteBatch.Draw(texture, new Rectangle(50, 600, 900, 900), Color.Red);
                        spriteBatch.DrawString(titleFont, "Hard", new Vector2(370, 1000), Color.Black);
                        break;
                    case 4:
                        spriteBatch.Draw(texture, new Rectangle(50, 600, 900, 900), Color.Magenta);
                        spriteBatch.DrawString(titleFont, "Extreme", new Vector2(310, 1000), Color.Black);
                        break;
                }
                if (splashTexture != null)
                {
                    spriteBatch.Draw(splashTexture, new Rectangle(0, 0, width, height), Color.White);
                }
                spriteBatch.DrawString
                (
                font,
                coords,
                new Vector2(55, 25),
                Color.Black
                );
                if (credits != null && creditsPlaying)
                {
                    spriteBatch.Draw(credits, new Vector2(0, creditY), Color.White);
                    creditY -= 4;
                    if (creditY <= -7000)
                    {
                        creditsPlaying = false;
                        creditY = height;
                    }
                }
            }
            if (menuType == "pause")
            {
                // background of pause menu
                spriteBatch.Draw(texture, new Rectangle(150, 150, width - 300, height - 300), Color.Gray);
                // exit game button
                spriteBatch.Draw(texture, new Rectangle(width - 400, 170, 80, 80), Color.Lime);
                spriteBatch.DrawString(font, "Quit", new Vector2(width - 380, 200), Color.Black);
                spriteBatch.DrawString
                (
                font,
                "Paused",
                new Vector2(55, 25),
                Color.Black
                );
                DrawScoreboard(spriteBatch);
            }
            if (menuType == "dead")
            {
                if (!hasGatoPoints)
                {
                    gatoPointsDead = ps;
                }
                hasGatoPoints = true;
                // background of menu
                spriteBatch.Draw(texture, new Rectangle(150, 150, width - 300, height - 300), Color.Gray);
                spriteBatch.DrawString
                (
                titleFont,
                "Game Over",
                new Vector2((width / 2) - 200, 200),
                Color.Black
                );
                spriteBatch.DrawString
                (
                font,
                "score: " + ps,
                new Vector2((width / 2) - 200, 450),
                Color.Black
                );
                DrawScoreboard(spriteBatch);
                spriteBatch.Draw(texture, new Rectangle(width - 400, 170, 80, 80), Color.Lime);
                spriteBatch.DrawString(font, "Quit", new Vector2(width - 380, 200), Color.Black);
            }
        }
        public void Click(int x, int y)
        {
            // start button
            coords = x + " : " + y;
            if (x > 900 && x < 1670 && y > 870 && y < 1225 && menuType == "splash")
            {
                PlayerAction(this, "start");
            }

            // quit button
            if (x > 2160 && x < 2240 && y > 170 && y < 250 && (menuType == "pause" || menuType == "dead"))
            {
                PlayerAction(this, "start");
                Game1.Instance.PauseGame("splash");
            }


            // credits button
            if (x > 1915 && x < 2393 && y > 1197 && y < 1408 && menuType == "splash")
            {
                creditsPlaying = true;
            }

            // difficulty button
            if (x > 152 && y > 969 && x < 754 && y < 1146 && menuTicks - lastDiffChange > 20)
            {
                lastDiffChange = menuTicks;
                Game1.difficultyNumber++;
                if (Game1.difficultyNumber > 4)
                {
                    Game1.difficultyNumber = 1;
                }
            }
        }
        public void SetMenuType(string type)
        {
            menuType = type;
        }
        public void DrawScoreboard(SpriteBatch spriteBatch)
        {
            var info = Game1.Instance.GetGameInfo("gato", Game1.Instance.GetTime());
            List<Infos.PlayerInfo> playerList = info.PlayerInfos;
            int offY = 200;
            spriteBatch.DrawString
                (
                font,
                "Scoreboard",
                new Vector2(225, 160),
                Color.Black
                );
            for (int i = 0; i < playerList.Count; i++)
            {
                spriteBatch.DrawString
                (
                font,
                playerList[i].Name,
                new Vector2(205, offY),
                Color.Black
                );
                spriteBatch.DrawString
                (
                font,
                playerList[i].Points.ToString(),
                new Vector2(255, offY),
                Color.Black
                );
                offY += 20;
            }
        }
    }
}

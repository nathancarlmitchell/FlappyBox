using System;
using System.Collections.Generic;
using FlappyBox.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace FlappyBox.States
{
    public class GameOverState : State
    {
        private readonly List<Button> butttons;
        private readonly Menu menu;
        private readonly SpriteFont titleFont;

        private bool isHighScore = false;

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base()
        {
            Game1.Instance.IsMouseVisible = true;

            GameState.GamesPlayed++;

            Util.SaveGameData();
            Util.SaveSkinData();

            if (GameState.Score >= GameState.HighScore)
            {
                this.isHighScore = true;
            }

            Background.SetAlpha(0.33f);

            titleFont = Art.TitleFont;

            var newGameButton = new Button() { Text = "New Game" };
            newGameButton.Click += NewGameButton_Click;

            var mainMenuButton = new Button() { Text = "Main Menu" };
            mainMenuButton.Click += MainMenuButton_Click;

            var quitGameButton = new Button() { Text = "Quit" };
            quitGameButton.Click += QuitGameButton_Click;

            butttons = [newGameButton, mainMenuButton, quitGameButton];
            menu = new Menu(butttons);
        }

        int textCooldown = 0;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Background.Draw(gameTime, spriteBatch);

            // Draw HUD.
            Overlay.DrawHUD();

            int x;
            int y = 128;

            string text;
            Color color;

            if (isHighScore)
            {
                textCooldown++;
                text = "High Score";
                color = Color.Gold;
                if (textCooldown > 5)
                {
                    color = Color.AliceBlue;
                }
                if (textCooldown > 10)
                {
                    textCooldown = 0;
                }
            }
            else
            {
                text = "Game Over";
                color = Color.AliceBlue;
            }

            x = (int)(CenterWidth - (titleFont.MeasureString(text).X / 2));

            spriteBatch.DrawString(titleFont, text, new Vector2(x - 4, y + 4), Color.Black * 0.5f);
            spriteBatch.DrawString(titleFont, text, new Vector2(x, y), color);

            menu.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Input.NewGame();
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            Input.MainMenu();
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            game.Exit();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Background.Update(gameTime);

            GameState.CoinHUD.CoinTexture.UpdateFrame(elapsed);

            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (var button in butttons)
            {
                button.Update(gameTime, touchCollection);
            }
        }
    }
}

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

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base()
        {
            Game1.Instance.IsMouseVisible = true;

            GameState.GamesPlayed++;

            Util.SaveGameData();
            Util.SaveSkinData();

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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Background.Draw(gameTime, spriteBatch);

            // Draw HUD.
            Overlay.DrawHUD();

            int x = CenterWidth / 2;
            int y = 128;
            spriteBatch.DrawString(titleFont, "Game Over", new Vector2(x, y), Color.AliceBlue);

            menu.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Input.NewGame();
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            Game1.GameState = null;
            GameState.Score = 0;
            GameState.Coins = 0;
            game.ChangeState(new MenuState(game, graphicsDevice, content));
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

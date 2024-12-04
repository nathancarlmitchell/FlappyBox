using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using FlappyBox.Controls;

namespace FlappyBox.States
{
    public class GameOverState : State
    {
        private List<Button> _components;
        private Menu _menu;
        private SpriteFont titleFont,
            hudFont;

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base()
        {
            _game.IsMouseVisible = true;

            Util.SaveGameData(GameState.Score, GameState.Coins);
            Util.SaveSkinData();

            Background.SetAlpha(0.33f);

            titleFont = Art.TitleFont;
            hudFont = Art.HudFont;

            var newGameButton = new Button()
            {
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var mainMenuButton = new Button()
            {
                Text = "Main Menu",
            };

            mainMenuButton.Click += MainMenuButton_Click;

            var quitGameButton = new Button()
            {
                Text = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Button>() { newGameButton, mainMenuButton, quitGameButton };

            _menu = new Menu(_components);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Background.Draw(gameTime, spriteBatch);

            // Draw HUD.
            var color = Color.Black;
            if (GameState.Score >= GameState.HiScore)
            {
                color = Color.Yellow;
            }
            spriteBatch.DrawString(
                hudFont,
                "Score: " + GameState.Score,
                new Vector2(32, 64),
                color
            );
            spriteBatch.DrawString(
                hudFont,
                "Hi Score: " + GameState.HiScore,
                new Vector2(32, 92),
                color
            );
            spriteBatch.DrawString(
                hudFont,
                " x " + GameState.Coins,
                new Vector2(GameState.CoinHUD.X + 16, GameState.CoinHUD.Y - 8),
                Color.Black
            );
            GameState.CoinHUD.coinTexture.DrawFrame(
                spriteBatch,
                new Vector2(GameState.CoinHUD.X, GameState.CoinHUD.Y)
            );

            spriteBatch.DrawString(
                titleFont,
                "Game Over",
                new Vector2(CenterWidth / 2, 128),
                Color.AliceBlue
            );

            _menu.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            Game1.GameState = null;
            GameState.Score = 0;
            GameState.Coins = 0;
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Background.Update(gameTime);

            GameState.CoinHUD.coinTexture.UpdateFrame(elapsed);

            // Check touch input.
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (var component in _components)
            {
                component.Update(gameTime, touchCollection);
            }

            // Check player input.
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                NewGame();
            }
        }

        public void NewGame()
        {
            Game1.GameState = null;
            GameState.Score = 0;
            GameState.Coins = 0;
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }
    }
}

using System;
using System.Collections.Generic;
using FlappyBox.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Color = Microsoft.Xna.Framework.Color;

namespace FlappyBox.States
{
    public class PauseState : State
    {
        private readonly List<Button> buttons;
        private readonly Menu menu;

        public PauseState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base()
        {
            game.IsMouseVisible = true;

            Background.SetAlpha(0.5);

            var continueGameButton = new Button() { Text = "Continue" };
            continueGameButton.Click += ContinueGameButton_Click;

            var mainMenuButton = new Button() { Text = "Main Menu" };
            mainMenuButton.Click += MainMenuButton_Click;

            var quitGameButton = new Button() { Text = "Quit" };
            quitGameButton.Click += QuitGameButton_Click;

            buttons = [continueGameButton, mainMenuButton, quitGameButton];
            menu = new Menu(buttons);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Background.Draw(gameTime, spriteBatch);

            // Draw HUD.
            Overlay.DrawHUD();

            float x = CenterWidth - (Art.TitleFont.MeasureString("Paused").X / 2);
            float y = CenterHeight / 2;
            spriteBatch.DrawString(Art.TitleFont, "Paused", new Vector2(x, y), Color.White);

            GameState.Player.Draw(spriteBatch);

            menu.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void ContinueGameButton_Click(object sender, EventArgs e)
        {
            Input.ContinueGame();
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new MenuState(game, graphicsDevice, content));
            Game1.GameState = null;
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            game.Exit();
        }

        public override void PostUpdate(GameTime gameTime) { }

        public override void Update(GameTime gameTime)
        {
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (var button in buttons)
            {
                button.Update(gameTime, touchCollection);
            }
        }
    }
}

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
    public class PauseState : State
    {
        private List<Button> _components;
        private Menu _menu;
        public PauseState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base()
        {
            _game.IsMouseVisible = true;

            var continueGameButton = new Button()
            {
                Text = "Continue",
            };

            continueGameButton.Click += ContinueGameButton_Click;

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

            _components = new List<Button>()
            {
                continueGameButton,
                mainMenuButton,
                quitGameButton,
            };

            _menu = new Menu(_components);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(Art.HudFont, "Paused: " + GameState.Score, new Vector2(ControlWidthCenter, CenterHeight/2),
                Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);

            _menu.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void ContinueGame()
        {
            _game.ChangeState(Game1.GameState);
        }

        private void ContinueGameButton_Click(object sender, EventArgs e)
        {
            ContinueGame();
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            Game1.GameState = null;
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            // Check touch input.
            TouchCollection touchCollection = TouchPanel.GetState();

            foreach (var component in _components)
            {
                component.Update(gameTime, touchCollection);
            }

            // Check player input.
            if (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                ContinueGame();
            }
        }
    }
}
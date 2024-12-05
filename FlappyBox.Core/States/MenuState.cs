using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using FlappyBox.Controls;
using System.Xml.Linq;

namespace FlappyBox.States
{
    public class MenuState : State
    {
        private List<Button> buttons;
        private Menu menu;
        private SpriteFont titleFont;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base()
        {
            new GameState(_game, _graphicsDevice, _content);

            _game.IsMouseVisible = true;

            titleFont = Art.TitleFont;

            var newGameButton = new Button()
            {
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var loadSkinsButton = new Button()
            {
                Text = "Skins",
            };

            loadSkinsButton.Click += LoadSkinsButton_Click;

            var trophyButton = new Button()
            {
                Text = "Achievements",
            };

            trophyButton.Click += TrophyButton_Click;

            var quitGameButton = new Button()
            {
                Text = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            buttons = new List<Button>() { newGameButton, loadSkinsButton, trophyButton, quitGameButton };

            menu = new Menu(buttons);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw background.
            Background.Draw(gameTime, spriteBatch);

            // Draw menu.
            menu.Draw(gameTime, spriteBatch);

            // Draw title.
            Overlay.DrawTitle();

            spriteBatch.End();
        }

        private void TrophyButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new TrophyState(_game, _graphicsDevice, _content));
        }

        private void LoadSkinsButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new SkinsState(_game, _graphicsDevice, _content));
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }
        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            Background.Update(gameTime);

            // Check touch input.
            TouchCollection touchCollection = TouchPanel.GetState();

            foreach (var button in buttons)
            {
                button.Update(gameTime, touchCollection);
            }

            // Check player input.
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Enter))
            {
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            }
        }
    }
}

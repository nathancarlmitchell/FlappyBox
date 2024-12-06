using System;
using System.Collections.Generic;
using FlappyBox.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace FlappyBox.States
{
    public class MenuState : State
    {
        private readonly List<Button> buttons;
        private readonly Menu menu;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base()
        {
            Game1.Instance.IsMouseVisible = true;

            var newGameButton = new Button() { Text = "New Game" };
            newGameButton.Click += NewGameButton_Click;

            var loadSkinsButton = new Button() { Text = "Skins" };
            loadSkinsButton.Click += LoadSkinsButton_Click;

            var trophyButton = new Button() { Text = "Achievements" };
            trophyButton.Click += TrophyButton_Click;

            var quitGameButton = new Button() { Text = "Quit" };
            quitGameButton.Click += QuitGameButton_Click;

            buttons = [newGameButton, loadSkinsButton, trophyButton, quitGameButton];
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
            game.ChangeState(new TrophyState(game, graphicsDevice, content));
        }

        private void LoadSkinsButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new SkinsState(game, graphicsDevice, content));
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Input.NewGame();
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            game.Exit();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            Background.Update(gameTime);

            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (var button in buttons)
            {
                button.Update(gameTime, touchCollection);
            }
        }
    }
}

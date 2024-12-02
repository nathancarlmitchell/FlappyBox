﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using FlappyBox.Controls;

namespace FlappyBox.States
{
    public class MenuState : State
    {
        private List<Button> _components;
        public static SpriteFont titleFont;
        private Menu _mainMenu;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            new GameState(_game, _graphicsDevice, _content);

            _game.IsMouseVisible = true;

            titleFont = _content.Load<SpriteFont>("TitleFont");

            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("HudFont");

            MenuState.ControlWidthCenter =
                (_graphicsDevice.Viewport.Width / 2) - (buttonTexture.Width / 2);

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var loadSkinsButton = new Button(buttonTexture, buttonFont)
            {
                Text = "Skins",
            };

            loadSkinsButton.Click += LoadSkinsButton_Click;

            var trophyButton = new Button(buttonTexture, buttonFont)
            {
                Text = "Achievments",
            };

            trophyButton.Click += TrophyButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Text = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Button>() { newGameButton, loadSkinsButton, trophyButton, quitGameButton };

            _mainMenu = new Menu(_components);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw background.
            Background.Draw(gameTime, spriteBatch);

            // Draw menu.
            _mainMenu.Draw(gameTime, spriteBatch);

            // Draw title.
            spriteBatch.DrawString(
                titleFont,
                "Flappy Box",
                new Vector2(CenterWidth / 2, 128),
                Color.AliceBlue
            );

            spriteBatch.End();
        }

        public void onClick()
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
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

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            Background.Update(gameTime);

            // Check touch input.
            TouchCollection touchCollection = TouchPanel.GetState();

            foreach (var component in _components)
            {
                component.Update(gameTime, touchCollection);
            }

            // Check player input.
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Enter))
            {
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            }
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}

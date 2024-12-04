using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FlappyBox.States;
using System.Diagnostics;

namespace FlappyBox.Controls
{
    public class Menu
    {
        private List<Button> _buttons;
        private int _difference;
        private int _centerHeight;
        private int _centerWidth;
        private int _scale;
        private int x;

        public Menu(List<Button> components)
        {
            _buttons = components;
            _centerHeight = ((int)Game1.ScreenSize.Y / 2) + (components.Count * 20);
            _centerWidth = (Game1.Width / 2) - ((Art.ButtonTexture.Width * Game1.Scale) / 2);
            _scale = Game1.Scale;

            for (int i = 0; i < _buttons.Count; i++)
            {
                int totalComponents = _buttons.Count;
                int centerComponent = totalComponents / 2;
                _difference = i - centerComponent;
                _buttons[i].Position = new Vector2(
                    _centerWidth,
                    _centerHeight + _difference * 50 * _scale
                );
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].Draw(gameTime, spriteBatch);
            }
        }
    }
}

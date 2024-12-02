using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FlappyBox.States;

namespace FlappyBox.Controls
{
    public class Menu
    {
        private List<Button> _components;
        private int _difference;
        private int _centerHeight;
        private int _centerWidth;
        private int _scale;

        public Menu(List<Button> components)
        {
            _components = components;
            _centerHeight = MenuState.CenterHeight + (components.Count * 20);
            _centerWidth = MenuState.ControlWidthCenter;
            _scale = Game1.Scale;

            for (int i = 0; i < _components.Count; i++)
            {
                int totalComponents = _components.Count;
                int centerComponent = totalComponents / 2;
                _difference = i - centerComponent;
                _components[i].Position = new Vector2(
                    _centerWidth,
                    _centerHeight + _difference * 50 * _scale
                );
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Draw(gameTime, spriteBatch);
            }
        }
    }
}

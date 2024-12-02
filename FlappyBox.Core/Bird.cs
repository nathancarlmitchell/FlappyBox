using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FlappyBox.States;

namespace FlappyBox
{
    public class Bird : Object
    {
        protected ContentManager _content;
        public AnimatedTexture currentTexture;

        private const float rotation = 0;
        private const float scale = 1f;
        private const float depth = 0.5f;
        private const int frames = 2;
        private const int framesPerSec = 1;

        public Bird(ContentManager content)
        {
            _content = content;

            this.Height = 16;
            this.Width = 16;
            this.X = GameState.ScreenWidth;
            this.Y = GameState.CenterHeight;

            currentTexture = new AnimatedTexture(
                new Vector2(this.Height / 2, this.Width / 2),
                rotation,
                scale,
                depth
            );

            currentTexture.Load(_content, "bird", frames, framesPerSec);
        }
    }
}

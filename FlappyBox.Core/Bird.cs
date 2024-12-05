using System;
using System.Runtime.CompilerServices;
using FlappyBox.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBox
{
    public class Bird : Object
    {
        private readonly AnimatedTexture currentTexture;
        private readonly Random rand = new();

        private const float rotation = 0;
        private const float depth = 0.5f;
        private const int frames = 2;
        private const int framesPerSec = 1;

        private const float opacity = 0.75f;
        private readonly float scale = Game1.Scale;

        public Bird()
        {
            this.Height = 16;
            this.Width = 16;
            this.X = Game1.ScreenWidth;
            this.Y = Game1.ScreenHeight;

            currentTexture = new AnimatedTexture(
                new Vector2(this.Height / 2, this.Width / 2),
                rotation,
                scale,
                depth
            );

            currentTexture.Load(Game1.Instance.Content, "bird", frames, framesPerSec);
        }

        public void Update(float elapsed)
        {
            if (this.X < 0)
            {
                this.Reset();
            }
            this.currentTexture.UpdateFrame(elapsed);
            this.X -= 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.currentTexture.DrawFrame(spriteBatch, new Vector2(this.X, this.Y), opacity);
        }

        public void Reset()
        {
            int minHeight = 128;
            int maxHeight = Game1.ScreenHeight - 128;
            int height = (int)
                Math.Floor(rand.NextDouble() * (maxHeight - minHeight + 1) + minHeight);

            this.X = Game1.ScreenWidth + 128;
            this.Y = height;
        }
    }
}

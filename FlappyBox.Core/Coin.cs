using System;
using FlappyBox.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FlappyBox
{
    public class Coin : Object
    {
        public AnimatedTexture CoinTexture;

        private const float rotation = 0;
        private const float scale = 1f;
        private const float depth = 0.5f;
        private const int frames = 6;
        private const int framesPerSec = 12;

        private int hoverRange = 64;
        private int hover = 0;
        private bool direction = true;

        public Coin()
        {
            ContentManager content = Game1.Instance.Content;

            this.Height = 32;
            this.Width = 32;
            this.X = Game1.ScreenWidth;
            this.Y = Game1.ScreenHeight / 2;

            CoinTexture = new AnimatedTexture(
                new Vector2(this.Height / 2, this.Width / 2),
                rotation,
                scale,
                depth
            );

            CoinTexture.Load(content, "coin", frames, framesPerSec);
        }

        public void Move()
        {
            this.X--;
            this.Hover();
        }

        public void Hover()
        {
            if (direction)
            {
                this.hover += 1;
                this.Y += 1;
            }
            else
            {
                this.hover -= 1;
                this.Y -= 1;
            }

            if (Math.Abs(this.hover) > hoverRange)
            {
                this.direction = !this.direction;
            }
        }

        public void Update() { }
    }
}

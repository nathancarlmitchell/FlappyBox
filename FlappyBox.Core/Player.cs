using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FlappyBox.States;

namespace FlappyBox
{
    public class Player : Object
    {
        protected ContentManager _content;
        private AnimatedTexture playerIdleTexture,
            playerJumpTexture,
            wingLeft,
            wingRight;
        private AnimatedTexture currentTexture;

        private const float rotation = 0;
        private const float scale = 1f;
        private const float depth = 0.5f;

        public int Frames;
        public int FPS;

        private const int maxVelocity = 64;
        public int JumpVelocity { get; } = 14;
        public int Velocity { get; set; } = 14;
        public string SkinName { get; set; }

        public Player(ContentManager content)
        {
            _content = content;

            this.Height = 64;
            this.Width = 64;

            int wingSize = 16;

            playerIdleTexture = new AnimatedTexture(
                new Vector2(this.Height / 2, this.Width / 2),
                rotation,
                scale,
                depth
            );
            playerIdleTexture.Load(_content, "anim_idle_default", this.Frames, this.FPS);

            playerJumpTexture = new AnimatedTexture(
                new Vector2(this.Height / 2, this.Width / 2),
                rotation,
                scale,
                depth
            );
            playerJumpTexture.Load(_content, "anim_jump_default", this.Frames, this.FPS);

            wingLeft = new AnimatedTexture(
                new Vector2(wingSize / 2, wingSize / 2),
                rotation,
                scale,
                depth
            );
            wingLeft.Load(_content, "wing_left", 1, 0);

            wingRight = new AnimatedTexture(
                new Vector2(wingSize / 2, wingSize / 2),
                rotation,
                scale,
                depth
            );
            wingRight.Load(_content, "wing_right", 1, 0);

            currentTexture = playerIdleTexture;
        }

        public void ChangeVelocity(int change)
        {
            // Check terminal velocity.
            if (Math.Abs(this.Velocity) >= maxVelocity + 1)
            {
                return;
            }

            // Change velocity.
            this.Velocity += change;

            // Bounce bottom of screen.
            if (this.Y >= GameState.ScreenHeight - this.Height / 2)
            {
                if (this.Velocity == 0)
                {
                    this.Velocity = 2;
                    return;
                }
                this.Velocity = Math.Abs(this.Velocity) / 2;
            }
        }

        public void Jump(int _jumpVelocity)
        {
            if (this.Velocity > -2)
            {
                return;
            }
            this.Velocity = _jumpVelocity;
        }

        public void ChangeSkin(string skin, int frames, int fps)
        {
            this.Frames = frames;
            this.FPS = fps;

            string _name = skin.Split("_").Last();
            playerIdleTexture.Load(_content, "anim_idle_" + _name, frames, fps);
            playerJumpTexture.Load(_content, "anim_jump_" + _name, frames, fps);

            currentTexture = playerIdleTexture;
        }

        private int r = 0;

        public void Flap()
        {
            int rr = (int)((r * Math.PI * 2) * 1);
            this.wingLeft.Rotation = rr;
            this.wingRight.Rotation = -rr;

            r++;
        }

        public void UpdateTexture()
        {
            // Set Idle texture.
            if (this.Velocity < -2)
            {
                if (currentTexture != playerIdleTexture && this.Frames - 1 == currentTexture.Frame)
                {
                    currentTexture.Reset();
                    currentTexture = playerIdleTexture;
                }
            }

            // Set Jump texture.
            if (this.Velocity > -2)
            {
                this.Flap();
                if (currentTexture != playerJumpTexture)
                {
                    currentTexture.Reset();
                    currentTexture = playerJumpTexture;
                }
            }
        }

        private int velocityCooldown = 0;
        private int skip = 0;

        public void Update(float elapsed, GameTime gameTime)
        {
            // Update player animation.
            this.currentTexture.UpdateFrame(elapsed);

            // Update player velocity.
            this.Y -= this.Velocity / 2;

            velocityCooldown++;
            if (velocityCooldown >= 2)
            {
                skip++;
                if (skip >= 2)
                {
                    skip = 0;
                    velocityCooldown = 1;
                }
                else
                {
                    velocityCooldown = 0;
                }
                this.ChangeVelocity(-1);
            }

            this.UpdateTexture();
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            // Draw player texture.
            this.currentTexture.DrawFrame(_spriteBatch, new Vector2(this.X, this.Y));

            // Draw wings.
            this.wingLeft.DrawFrame(_spriteBatch, new Vector2(this.X - 32 - 4, this.Y + 16));
            this.wingRight.DrawFrame(
                _spriteBatch,
                new Vector2(this.X + this.Width - 32 + 4, this.Y + 16)
            );
        }
    }
}

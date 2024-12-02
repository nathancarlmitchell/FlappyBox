using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBox.States
{
    public static class Background
    {
        private static Texture2D backgroundTexture,
            cloudTexture;
        private static Bird bird;
        private static Object cloud;
        private static double alpha,
            targetAlpha;
        private static Random rand = new Random();

        static Background()
        {
            alpha = 0.0;
        }

        public static void SetAlpha(double _targetAlpha)
        {
            targetAlpha = _targetAlpha;
        }

        public static void UpdateAlpha()
        {
            if (alpha < targetAlpha)
            {
                alpha += 0.005;
                if (alpha > 1.0)
                {
                    alpha = 1.0;
                }
            }
            else
            {
                alpha -= 0.005;
                if (alpha < 0.0)
                {
                    alpha = 0;
                }
            }
        }

        public static void LoadContent(ContentManager content)
        {
            backgroundTexture = content.Load<Texture2D>("bg");
            cloudTexture = content.Load<Texture2D>("cloud");

            cloud = new Object { X = 250, Y = 200 };

            bird = new Bird(content);
            Reset(bird);
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw background.
            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White * (float)alpha);

            // Draw cloud.
            spriteBatch.Draw(
                cloudTexture,
                new Vector2(cloud.X, cloud.Y),
                Color.White * ((float)alpha - 0.25f)
            );

            // Draw bird.
            bird.currentTexture.DrawFrame(spriteBatch, new Vector2(bird.X, bird.Y), (float)0.75);
        }

        public static void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        private static int cloudCooldown = 0;

        public static void Update(GameTime gameTime)
        {
            UpdateAlpha();

            // Update cloud posistion.
            cloudCooldown++;
            if (cloudCooldown > 60)
            {
                cloudCooldown = 0;
                cloud.X -= 1;
            }
            if (cloud.X < 0)
            {
                Reset(cloud);
            }

            // Update bird animation.
            if (bird.X < 0)
            {
                Reset(bird);
            }
            bird.currentTexture.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            bird.X -= 1;
        }

        public static void Reset(Object obj)
        {
            int minHeight = 128;
            int maxHeight = GameState.ScreenHeight - 128;
            int height = (int)
                Math.Floor(rand.NextDouble() * (maxHeight - minHeight + 1) + minHeight);

            obj.X = GameState.ScreenWidth + 128;
            obj.Y = height;
        }
    }
}

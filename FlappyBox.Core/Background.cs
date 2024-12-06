using System;
using FlappyBox.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBox
{
    public static class Background
    {
        private static Texture2D backgroundTexture,
            cloudTexture;
        private static Bird bird;
        private static Object cloud;
        private static double alpha,
            targetAlpha;

        static Background()
        {
            alpha = 0.0;

            backgroundTexture = Art.BackgroundTexture;
            cloudTexture = Art.CloudTexture;

            cloud = new Object { X = 250, Y = 200 };

            bird = new Bird();
            bird.Reset();
        }

        public static void SetAlpha(double _alpha)
        {
            targetAlpha = _alpha;
        }

        public static float GetAlpha()
        {
            return (float)alpha;
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
            bird.Draw(spriteBatch);
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
            if (cloud.X < 0 - cloudTexture.Width)
            {
                cloud.X = Game1.ScreenWidth + cloudTexture.Width;
            }

            // Update bird animation.
            bird.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}

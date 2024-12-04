using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FlappyBox.States;

namespace FlappyBox
{
    public class Bird : Object
    {
        public AnimatedTexture currentTexture;

        private const float rotation = 0;
        private float scale = 1f;
        private const float depth = 0.5f;
        private const int frames = 2;
        private const int framesPerSec = 1;

        public Bird()
        {
            ContentManager content = Game1.Instance.Content;
            
            scale = Game1.Scale;

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

            currentTexture.Load(content, "bird", frames, framesPerSec);
        }
    }
}

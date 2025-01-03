using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBox
{
    /// <summary>
    /// A helper class for handling animated textures.
    /// </summary>
    public class AnimatedTexture
    {
        // Number of frames in the animation.
        private int frameCount;

        // The animation spritesheet.
        private Texture2D myTexture;

        // The number of frames to draw per second.
        private float timePerFrame;

        // The current frame being drawn.
        public int Frame;

        // Total amount of time the animation has been running.
        public float Elapsed;

        // Is the animation currently running?
        private bool isPaused;

        // The current rotation, scale and draw depth for the animation.
        public float Rotation, Scale, Depth;

        // The origin point of the animated texture.
        public Vector2 Origin;
        public int Width;

        public AnimatedTexture(Vector2 origin, float rotation, float scale, float depth)
        {
            this.Origin = origin;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
        }

        public void Load(ContentManager content, string asset, int frameCount, int framesPerSec)
        {
            this.frameCount = frameCount;
            myTexture = content.Load<Texture2D>(asset);
            timePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            Elapsed = 0;
            isPaused = false;

            Width = 0;
            if (frameCount > 0)
            {
                Width = myTexture.Width / frameCount;
            }
        }

        public void UpdateFrame(float elapsed)
        {
            if (isPaused)
            {
                return;
            }
            Elapsed += elapsed;
            if (Elapsed > timePerFrame)
            {
                Frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                Frame %= frameCount;
                Elapsed -= timePerFrame;
            }
        }

        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, Frame, screenPos);
        }

        public void DrawFrame(SpriteBatch batch, Vector2 screenPos, float alpha)
        {
            DrawFrame(batch, Frame, screenPos, alpha);
        }

        public void DrawFrame(SpriteBatch batch, Vector2 screenPos, Color color)
        {
            int FrameWidth = myTexture.Width / frameCount;
            Rectangle sourcerect = new Rectangle(FrameWidth * Frame, 0,
                FrameWidth, myTexture.Height);
            batch.Draw(myTexture, screenPos, sourcerect, color,
                Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            int FrameWidth = myTexture.Width / frameCount;
            Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0,
                FrameWidth, myTexture.Height);
            batch.Draw(myTexture, screenPos, sourcerect, Color.White,
                Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos, float alpha)
        {
            int FrameWidth = myTexture.Width / frameCount;
            Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0,
                FrameWidth, myTexture.Height);
            batch.Draw(myTexture, screenPos, sourcerect, Color.White * alpha,
                Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

        public bool IsPaused
        {
            get { return isPaused; }
        }

        public void Reset()
        {
            Frame = 0;
            Elapsed = 0f;
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Play()
        {
            isPaused = false;
        }

        public void Pause()
        {
            isPaused = true;
        }
    }
}

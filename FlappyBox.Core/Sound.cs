using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace FlappyBox
{
    static class Sound
    {
        public static SoundEffect Jump,
            Coin;

        private static readonly Random rand = new();

        public static void Load(ContentManager content)
        {
            Jump = content.Load<SoundEffect>("Sounds/jump");
            Coin = content.Load<SoundEffect>("Sounds/coin");
        }

        public static void Play(SoundEffect sound, float volume)
        {
            if (!Game1.Mute)
            {
                sound.Play(volume, 0.0f, 0.0f);
            }
        }

        public static void Play(SoundEffect sound, float volume, float pitchVariance)
        {
            if (!Game1.Mute)
            {
                float pitch = (float)(
                    rand.NextDouble() * (pitchVariance - -pitchVariance) + -pitchVariance
                );

                sound.Play(volume, pitch, 0.0f);
            }
        }
    }
}

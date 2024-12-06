using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBox
{
    static class Art
    {
        public static Texture2D ButtonTexture { get; private set; }
        public static Texture2D BoostTexture { get; private set; }
        public static Texture2D BackgroundTexture { get; private set; }
        public static Texture2D MuteTexture { get; private set; }
        public static Texture2D UnmuteTexture { get; private set; }

        //public static Texture2D WallTexture { get; private set; }
        public static Texture2D CloudTexture { get; private set; }
        public static AnimatedTexture TrophySprite { get; private set; }
        public static AnimatedTexture ArrowSprite { get; private set; }
        public static SpriteFont HudFont { get; private set; }
        public static SpriteFont TitleFont { get; private set; }
        public static SpriteFont DebugFont { get; private set; }

        public static void Load(ContentManager content)
        {
            ButtonTexture = content.Load<Texture2D>("Controls/Button");
            MuteTexture = content.Load<Texture2D>("Controls/mute");
            UnmuteTexture = content.Load<Texture2D>("Controls/unmute");

            BoostTexture = content.Load<Texture2D>("boost");
            BackgroundTexture = content.Load<Texture2D>("bg");
            CloudTexture = content.Load<Texture2D>("cloud");

            TrophySprite = new AnimatedTexture(new Vector2(0, 0), 0, 1f, 0.5f);
            TrophySprite.Load(content, "trophy", 1, 1);

            ArrowSprite = new AnimatedTexture(new Vector2(0, 0), 0, 1f, 0.5f);
            ArrowSprite.Load(content, "arrow", 4, 4);

            //WallTexture = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            //WallTexture.SetData(new[] { Color.White });

            DebugFont = content.Load<SpriteFont>("Fonts/DebugFont");

            if (OperatingSystem.IsAndroid())
            {
                HudFont = content.Load<SpriteFont>("Fonts/HudMobileFont");
                TitleFont = content.Load<SpriteFont>("Fonts/TitleMobileFont");
            }
            else
            {
                HudFont = content.Load<SpriteFont>("Fonts/HudFont");
                TitleFont = content.Load<SpriteFont>("Fonts/TitleFont");
            }
        }
    }
}

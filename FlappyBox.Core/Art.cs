using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBox
{
    static class Art
    {
        public static Texture2D ButtonTexture { get; private set; }
        public static Texture2D BackgroundTexture { get; private set; }
        public static Texture2D CloudTexture { get; private set; }
        public static AnimatedTexture ArrowSprite {  get; private set; }
        public static SpriteFont HudFont { get; private set; }
        public static SpriteFont TitleFont { get; private set; }
        public static SpriteFont DebugFont { get; private set; }

        public static void Load(ContentManager content)
        {
            ButtonTexture = content.Load<Texture2D>("Controls/Button");

            BackgroundTexture = content.Load<Texture2D>("bg");
            CloudTexture = content.Load<Texture2D>("cloud");

            ArrowSprite = new AnimatedTexture(new Vector2(0, 0), 0, 1f, 0.5f);
            ArrowSprite.Load(content, "arrow", 4, 4);

            //Pixel = new Texture2D(Player.GraphicsDevice, 1, 1);
            //Pixel.SetData(new[] { Color.White });

            DebugFont = content.Load<SpriteFont>("Fonts/DebugFont");
            TitleFont = content.Load<SpriteFont>("Fonts/TitleFont");

            if (OperatingSystem.IsAndroid())
            {
                HudFont = content.Load<SpriteFont>("Fonts/HudMobileFont");
            }
            else
            {
                HudFont = content.Load<SpriteFont>("Fonts/HudFont");
            }

        }

    }
}

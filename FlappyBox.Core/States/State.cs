using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBox.States
{
    public abstract class State
    {
        #region Fields
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Game1 _game;
        private static int _centerHeight;
        private static int _centerWidth;
        private static int _screenHeight;
        private static int _screenWidth;
        private static int _controlWidthCenter;

        #endregion

        #region Methods
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void PostUpdate(GameTime gameTime);

        public State()
        {
            _game = Game1.Instance;
            _graphicsDevice = Game1.Instance.GraphicsDevice;
            _content = Game1.Instance.Content;

            _centerWidth = (Game1.ScreenWidth / 2);
            _centerHeight = (Game1.ScreenHeight / 2);

            _screenWidth = Game1.ScreenWidth;
            _screenHeight = Game1.ScreenHeight;

            _controlWidthCenter = (Game1.ScreenWidth / 2) - ((Art.ButtonTexture.Width / 2) * Game1.Scale);
        }

        public static int ScreenHeight
        {
            get { return _screenHeight; }
        }

        public static int ScreenWidth
        {
            get { return _screenWidth; }
        }

        public static int CenterHeight
        {
            get { return _centerHeight; }
        }

        public static int CenterWidth
        {
            get { return _centerWidth; }
        }

        public static int ControlWidthCenter
        {
            get { return _controlWidthCenter; }
            set { _controlWidthCenter = value; }
        }

        public abstract void Update(GameTime gameTime);

        #endregion
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FlappyBox.States;

namespace FlappyBox
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics;
        private SpriteBatch spriteBatch;
        public static State GameState, SkinState, MenuState;
        private State _currentState, _nextState;

        // Helpful static properties.
        public static Game1 Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static int ScreenWidth { get { return (int)ScreenSize.X; } }
        public static int ScreenHeight { get { return (int)ScreenSize.Y; } }
        public static int Scale { get; set; }

        public Game1()
        {
            Instance = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            Window.Title = "Flappy Box";

            if (OperatingSystem.IsAndroid())
            {
                Graphics.IsFullScreen = true;
                Graphics.PreferredBackBufferWidth = 1280 * 4 / 3;
                Scale = 2;
            }
            else
            {
                Graphics.PreferredBackBufferWidth = 1280;
                Graphics.PreferredBackBufferHeight = 720;
                Scale = 1;
            }

            Graphics.ApplyChanges();
            Console.WriteLine("Screen Size: " + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + " x " + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Art.Load(Content);
            Background.SetAlpha(0.5);
            Util.CheckOS();
            Util.LoadGameData();
            Util.LoadSkinData(Content);
            Util.LoadTrophyData(Content);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState = new MenuState(this, Graphics.GraphicsDevice, Content);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                if (_nextState is GameState && GameState != null)
                {
                    _currentState = GameState;
                    Console.WriteLine("restoring GameState");
                }
                else if (_nextState is GameState)
                {
                    GameState = _nextState;
                    _currentState = _nextState;
                    Console.WriteLine("_nextState is GameState");
                }
                else if (_nextState is SkinsState && SkinState != null)
                {
                    _currentState = SkinState;
                    Console.WriteLine("restoring SkinSate");
                }
                else if (_nextState is SkinsState)
                {
                    SkinState = _nextState;
                    _currentState = _nextState;
                    Console.WriteLine("_nextState is SkinState");
                }
                else if (_nextState is MenuState && MenuState != null)
                {
                    _currentState = MenuState;
                    Console.WriteLine("restoring MenuState");
                }
                else if (_nextState is MenuState)
                {
                    MenuState = _nextState;
                    _currentState = _nextState;
                    Console.WriteLine("_nextState is MenuState");
                }
                else
                {
                    _currentState = _nextState;
                }

                _nextState = null;
            }

            _currentState.Update(gameTime);
            _currentState.PostUpdate(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _currentState.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
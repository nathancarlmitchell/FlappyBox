using System;
using System.Diagnostics;
using FlappyBox.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappyBox
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        private KeyboardState currentKeyboard,
            previousKeyboard;
        private State currentState,
            nextState;

        public static State GameState,
            SkinState,
            MenuState;

        // Helpful static properties.
        public static Game1 Instance { get; private set; }
        public static Viewport Viewport
        {
            get { return Instance.GraphicsDevice.Viewport; }
        }
        public static Vector2 ScreenSize
        {
            get { return new Vector2(Viewport.Width, Viewport.Height); }
        }
        public static int ScreenWidth
        {
            get { return (int)ScreenSize.X; }
        }
        public static int ScreenHeight
        {
            get { return (int)ScreenSize.Y; }
        }
        public static int Scale { get; set; }
        public static int Width { get; set; }
        public static bool Mute { get; set; }

        public Game1()
        {
            Instance = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void ChangeState(State state)
        {
            nextState = state;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            Mute = false;
            Window.Title = "Flappy Box";

            if (OperatingSystem.IsAndroid())
            {
                Graphics.IsFullScreen = true;
                Width = 1280 * 4 / 3;
                Graphics.PreferredBackBufferWidth = Width;
                Scale = 2;
            }
            else
            {
                Width = 1280;
                Graphics.PreferredBackBufferWidth = 1280;
                Graphics.PreferredBackBufferHeight = 720;
                Scale = 1;
            }

            Graphics.ApplyChanges();
            Debug.WriteLine(
                "Screen Size: "
                    + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width
                    + " x "
                    + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
            );

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

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            run();
        }

        protected void run()
        {
            currentState = new MenuState(this, Graphics.GraphicsDevice, Content);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (nextState != null)
            {
                if (nextState is GameState && GameState != null)
                {
                    currentState = GameState;
                    Console.WriteLine("restoring GameState");
                }
                else if (nextState is GameState)
                {
                    GameState = nextState;
                    currentState = nextState;
                    Console.WriteLine("nextState is GameState");
                }
                else if (nextState is SkinsState && SkinState != null)
                {
                    currentState = SkinState;
                    Console.WriteLine("restoring SkinSate");
                }
                else if (nextState is SkinsState)
                {
                    SkinState = nextState;
                    currentState = nextState;
                    Console.WriteLine("nextState is SkinState");
                }
                else if (nextState is MenuState && MenuState != null)
                {
                    currentState = MenuState;
                    Console.WriteLine("restoring MenuState");
                }
                else if (nextState is MenuState)
                {
                    MenuState = nextState;
                    currentState = nextState;
                    Console.WriteLine("nextState is MenuState");
                }
                else
                {
                    currentState = nextState;
                }

                nextState = null;
            }

            currentState.Update(gameTime);
            currentState.PostUpdate(gameTime);
            base.Update(gameTime);

            // Check keybord input.
            previousKeyboard = currentKeyboard;
            currentKeyboard = Keyboard.GetState();

            if (currentKeyboard.IsKeyDown(Keys.M) && !previousKeyboard.IsKeyDown(Keys.M))
            {
                Mute = !Mute;
                Overlay.ToggleAudio();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            currentState.Draw(gameTime, SpriteBatch);
            base.Draw(gameTime);
        }
    }
}

﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FlappyBox.States;

namespace FlappyBox
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public static State _gameState, _skinState, _menuState;
        private State _currentState, _nextState;
        public static int Scale { get; set; }
        public static SpriteFont HudFont { get; set; }

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            Window.Title = "Flappy Box";
            //this.IsFixedTimeStep = true;
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1 / 120.0);    // Update() is called every 30 times each second / 30 FPS
            //this.IsFixedTimeStep = false;
            if (OperatingSystem.IsAndroid())
            {
                graphics.IsFullScreen = true;
                graphics.PreferredBackBufferWidth = 1280 * 4 / 3;
                Scale = 2;
            }
            else
            {
                graphics.PreferredBackBufferWidth = 1280;
                graphics.PreferredBackBufferHeight = 720;
                Scale = 1;
            }
            //graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();
            Console.WriteLine("Screen Size: " + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + " x " + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {   if (OperatingSystem.IsAndroid())
            {
                HudFont = Content.Load<SpriteFont>("HudMobileFont");
            }
            else
            {
                HudFont = Content.Load<SpriteFont>("HudFont");
            }
            Util.CheckOS();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState = new MenuState(this, graphics.GraphicsDevice, Content);
            Background.LoadContent(Content);
            Background.SetAlpha(0.5);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                //Console.WriteLine(_nextState.GetType());
                //if (_gameState != null)
                //{
                //    Console.WriteLine("GameState is not null");
                //}

                if (_nextState is GameState && _gameState != null)
                {
                    _currentState = _gameState;
                    Console.WriteLine("restoring GameState");
                }
                else if (_nextState is GameState)
                {
                    _gameState = _nextState;
                    _currentState = _nextState;
                    Console.WriteLine("_nextState is GameState");
                }
                else if (_nextState is SkinsState && _skinState != null)
                {
                    _currentState = _skinState;
                    Console.WriteLine("restoring SkinSate");
                }
                else if (_nextState is SkinsState)
                {
                    _skinState = _nextState;
                    _currentState = _nextState;
                    Console.WriteLine("_nextState is SkinState");
                }
                else if (_nextState is MenuState && _menuState != null)
                {
                    _currentState = _menuState;
                    Console.WriteLine("restoring MenuState");
                }
                else if (_nextState is MenuState)
                {
                    _menuState = _nextState;
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _currentState.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
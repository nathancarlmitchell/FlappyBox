using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace FlappyBox.States
{
    public class GameState : State
    {
        private static SpriteFont hudFont,
            debugFont;
        private Texture2D wallTexture,
            boostTexture;
        public static Player Player;
        public static List<Skin> Skins { get; set; }
        public static Coin CoinHUD;
        private List<Wall> wallArray;
        private List<Coin> coinArray;
        private WallSpawner wallSpawner;
        private CoinSpawner coinSpawner;
        private bool _debug = false;

        public static int Score { get; set; }
        public static int HiScore { get; set; }
        public static int Coins { get; set; }

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base()
        {
            _game.IsMouseVisible = false;

            Util.LoadGameData();

            Background.SetAlpha(1.0);

            hudFont = Art.HudFont;
            debugFont = Art.DebugFont;

            wallTexture = new Texture2D(_graphicsDevice, 1, 1);
            wallTexture.SetData(new[] { Color.White });

            boostTexture = _content.Load<Texture2D>("boost");

            wallArray = new List<Wall>();
            coinArray = new List<Coin>();

            wallSpawner = new WallSpawner(_content);
            coinSpawner = new CoinSpawner();

            CoinHUD = new Coin(_content);
            CoinHUD.X = 32;
            CoinHUD.Y = 32;

            Player = new Player(_content);
            Player.X = ScreenWidth / 2;
            Player.Y = ScreenHeight / 2;
            Player.Height = 64;
            Player.Width = 64;

            // Set the selected player skin
            Util.LoadSkinData(_content);
            GameState.Skins = SkinsState.Skins;
            if (GameState.Skins is not null)
            {
                for (int i = 0; i < GameState.Skins.Count; i++)
                {
                    if (GameState.Skins[i].Selected)
                    {
                        Player.ChangeSkin(
                            GameState.Skins[i].Name,
                            GameState.Skins[i].Frames,
                            GameState.Skins[i].FPS
                        );
                        Player.SkinName = GameState.Skins[i].Name;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw background.
            Background.Draw(gameTime, spriteBatch);

            // Draw player.
            Player.Draw(spriteBatch);

            spriteBatch.End();

            // Draw walls.
            for (int i = 0; i < wallArray.Count; i++)
            {
                wallArray[i].Draw(spriteBatch);
            }

            spriteBatch.Begin();

            // Draw boost button on mobile.
            if (OperatingSystem.IsAndroid())
            {
                spriteBatch.Draw(boostTexture, new Vector2(0, ScreenHeight - 128), Color.White);
            }

            // Draw coins.
            for (int i = 0; i < coinArray.Count; i++)
            {
                coinArray[i]
                    .coinTexture.DrawFrame(
                        spriteBatch,
                        new Vector2(coinArray[i].X, coinArray[i].Y)
                    );
            }

            // Draw HUD.
            var color = Color.Black;
            if (Score >= HiScore)
            {
                color = Color.Yellow;
            }
            spriteBatch.DrawString(hudFont, "Score: " + Score, new Vector2(32, 64), color);
            spriteBatch.DrawString(hudFont, "Hi Score: " + HiScore, new Vector2(32, 92), color);
            spriteBatch.DrawString(
                hudFont,
                " x " + Coins,
                new Vector2(CoinHUD.X + 16, CoinHUD.Y - 8),
                Color.Black
            );
            CoinHUD.coinTexture.DrawFrame(spriteBatch, new Vector2(CoinHUD.X, CoinHUD.Y));

            // Draw boost button on mobile.
            if (OperatingSystem.IsAndroid())
            {
                spriteBatch.Draw(
                    boostTexture,
                    new Vector2(0, ScreenHeight - boostTexture.Height),
                    Color.White
                );
            }

            // Draw debug.
            if (_debug)
            {
                spriteBatch.DrawString(
                    debugFont,
                    "Coin Length: " + coinArray.Count,
                    new Vector2(64, ScreenHeight - 96),
                    Color.Black
                );
                spriteBatch.DrawString(
                    debugFont,
                    "Array Length: " + wallArray.Count,
                    new Vector2(64, ScreenHeight - 80),
                    Color.Black
                );

                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                spriteBatch.DrawString(
                    debugFont,
                    "elapsed: " + elapsed,
                    new Vector2(64, ScreenHeight - 64),
                    Color.Black
                );

                float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
                spriteBatch.DrawString(
                    debugFont,
                    "currentTime: " + currentTime,
                    new Vector2(64, ScreenHeight - 48),
                    Color.Black
                );
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime) { }

        private int wallCooldown = 0;
        private int coinCooldown = 0;

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;

            Score += 1;
            if (Score > HiScore)
            {
                HiScore = Score;
            }

            Background.Update(gameTime);

            // Rotate player.
            //player.currentTexture.Rotation = (float)(alpha * Math.PI * 2) * 4;

            // Update coin positions and check collisions.
            CoinHUD.coinTexture.UpdateFrame(elapsed);
            for (int i = 0; i < coinArray.Count; i++)
            {
                coinArray[i].coinTexture.UpdateFrame(elapsed);
                coinArray[i].X -= 1;
                coinArray[i].Hover();
                // Get coin.
                if (Player.CheckForCollisions(coinArray[i]))
                {
                    coinArray.RemoveAt(i);
                    Coins += 1;
                }

                if (coinArray[i].X + coinArray[i].Width < 0)
                {
                    coinArray.Remove(coinArray[i]);
                }
            }

            // Update wall positions and check collisions.
            for (int i = 0; i < wallArray.Count; i++)
            {
                // Check collisions.
                if (Player.CheckForCollisions(wallArray[i]))
                {
                    _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
                }

                wallArray[i].Move();

                // Remove walls off screen.
                if (wallArray[i].X + wallArray[i].Width < 0)
                {
                    wallArray.Remove(wallArray[i]);
                }
            }

            // Spawn a wall.
            wallCooldown++;
            if (wallCooldown >= 180)
            {
                wallCooldown = 0;
                wallArray.AddRange(wallSpawner.Spawn(_content));
            }

            // Spawn a coin.
            coinCooldown++;
            if (coinCooldown >= 360)
            {
                coinCooldown = 0;
                coinArray.Add(coinSpawner.Spawn(_content));
            }

            // Update player.
            Player.Update(elapsed, gameTime);

            // Check player input.
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Player.Jump(Player.JumpVelocity);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                Player.Jump(Player.JumpVelocity * 2);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _game.ChangeState(new PauseState(_game, _graphicsDevice, _content));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
            }

            // Check touch input.
            TouchCollection touchState = TouchPanel.GetState();
            if (touchState.AnyTouch())
            {
                int x = (int)touchState.GetPosition().X;
                int y = (int)touchState.GetPosition().Y;

                if (x < boostTexture.Width && y > GameState.ScreenHeight - boostTexture.Height)
                {
                    Player.Jump(Player.JumpVelocity * 2);
                    return;
                }
                Player.Jump(Player.JumpVelocity);
            }

            // Check player bounds
            if (Player.Y > ScreenHeight - Player.Height / 2)
            {
                Player.Y = ScreenHeight - Player.Height / 2;
            }
            else if (Player.Y < Player.Height / 2)
            {
                Player.Y = Player.Height / 2;
            }
        }
    }
}

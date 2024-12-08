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
        private List<Wall> wallArray;
        private List<Coin> coinArray;
        private WallSpawner wallSpawner;
        private CoinSpawner coinSpawner;
        private bool _debug = false;
        private static readonly Random rand = new();

        public static Player Player { get; set; }
        public static List<Skin> Skins { get; set; }
        public static Coin CoinHUD { get; set; }
        public static int Score { get; set; }
        public static int HighScore { get; set; }
        public static int TotalScore { get; set; }
        public static int Coins { get; set; }
        public static int TotalCoins { get; set; }
        public static int GamesPlayed { get; set; }

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base()
        {
            Game1.Instance.IsMouseVisible = false;

            Util.LoadGameData();

            Background.SetAlpha(1.0);

            wallArray = new List<Wall>();
            coinArray = new List<Coin>();

            wallSpawner = new WallSpawner();
            coinSpawner = new CoinSpawner();

            CoinHUD = new Coin() { X = 32, Y = 32 };

            Player = new Player()
            {
                X = Game1.ScreenWidth / 2,
                Y = Game1.ScreenHeight / 2,
                Height = 64,
                Width = 64,
            };

            // Set the selected player skin
            Util.LoadSkinData(content);
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

            // Draw coins.
            for (int i = 0; i < coinArray.Count; i++)
            {
                coinArray[i]
                    .CoinTexture.DrawFrame(
                        spriteBatch,
                        new Vector2(coinArray[i].X, coinArray[i].Y)
                    );
            }

            // Draw HUD.
            Overlay.DrawHUD();

            // Draw HUD on mobile.
            if (OperatingSystem.IsAndroid())
            {
                Overlay.DrawMobileHUD();
            }

            // Draw debug.
            if (_debug)
            {
                Overlay.DrawDebug(gameTime);
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime) { }

        private int wallCooldown = 0;

        public void UpdateWalls()
        {
            // Loop through walls.
            for (int i = 0; i < wallArray.Count; i++)
            {
                // Check collisions.
                if (Player.CheckForCollisions(wallArray[i]))
                {
                    game.ChangeState(new GameOverState(game, graphicsDevice, content));
                }

                // Update wall position.
                wallArray[i].Move();

                // Despawn offscreen walls.
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
                wallArray.AddRange(wallSpawner.Spawn());
            }
        }

        private int coinCooldown = 0;

        public void UpdateCoins(float elapsed)
        {
            // Loop through coins.
            for (int i = 0; i < coinArray.Count; i++)
            {
                // Update coin animation.
                coinArray[i].CoinTexture.UpdateFrame(elapsed);

                // Update coin position.
                coinArray[i].Move();

                if (Player.CheckForCollisions(coinArray[i]))
                {
                    // Get coin.
                    coinArray.RemoveAt(i);
                    Coins++;
                    TotalCoins++;

                    // Play coin sound
                    Sound.Play(Sound.Coin, 0.15f);
                }

                // Despawn offscreen coins.
                if (coinArray[i].X + coinArray[i].Width < 0)
                {
                    coinArray.Remove(coinArray[i]);
                }
            }

            // Spawn a coin.
            coinCooldown++;
            if (coinCooldown >= 360)
            {
                coinCooldown = 0;
                coinArray.Add(coinSpawner.Spawn());
            }

            // Update coin HUD animation.
            CoinHUD.CoinTexture.UpdateFrame(elapsed);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update score.
            Score++;
            if (Score > HighScore)
            {
                HighScore = Score;
            }

            // Update background.
            Background.Update(gameTime);

            UpdateCoins(elapsed);
            UpdateWalls();
            Player.Update(elapsed, gameTime);
        }
    }
}

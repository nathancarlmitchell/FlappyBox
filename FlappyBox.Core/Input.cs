using System;
using FlappyBox.States;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace FlappyBox
{
    public static class Input
    {
        private static readonly Game1 game = Game1.Instance;
        private static readonly GraphicsDevice graphicsDevice = Game1.Instance.GraphicsDevice;
        private static readonly ContentManager content = Game1.Instance.Content;

        private static KeyboardState keyboard,
            previousKeyboard;

        public static void Update(State currentState)
        {
            previousKeyboard = keyboard;
            keyboard = Keyboard.GetState();

            TouchCollection touchState = TouchPanel.GetState();

            // Universal input.
            //
            // Toggle Mute
            if (keyboard.IsKeyDown(Keys.M) && !previousKeyboard.IsKeyDown(Keys.M))
            {
                Game1.Mute = !Game1.Mute;
                Overlay.ToggleAudio();
            }

            // State specific input.
            if (currentState is MenuState)
            {
                if (keyboard.IsKeyDown(Keys.Enter) && !previousKeyboard.IsKeyDown(Keys.Enter))
                {
                    NewGame();
                }
            }

            if (currentState is GameState)
            {
                // Check keybord input.
                //
                // Jump.
                if (keyboard.IsKeyDown(Keys.Space))
                {
                    GameState.Player.Jump(GameState.Player.JumpVelocity);
                }

                // Boost.
                if (keyboard.IsKeyDown(Keys.LeftShift))
                {
                    GameState.Player.Jump(GameState.Player.JumpVelocity * 2);
                }

                // Pause.
                if (keyboard.IsKeyDown(Keys.Escape) || keyboard.IsKeyDown(Keys.P))
                {
                    game.ChangeState(new PauseState(game, graphicsDevice, content));
                }

                // Game over.
                if (keyboard.IsKeyDown(Keys.Q))
                {
                    game.ChangeState(new GameOverState(game, graphicsDevice, content));
                }

                // Check touch input.
                if (touchState.AnyTouch())
                {
                    int x = (int)touchState.GetPosition().X;
                    int y = (int)touchState.GetPosition().Y;

                    int xBoost = Art.BoostTexture.Width;
                    int yBoost = Game1.ScreenHeight - (Art.BoostTexture.Height * 2);

                    // Boost.
                    if (x < xBoost && y > yBoost)
                    {
                        GameState.Player.Jump(GameState.Player.JumpVelocity * 2);
                        return;
                    }

                    // Jump.
                    GameState.Player.Jump(GameState.Player.JumpVelocity);
                }
            }

            if (currentState is PauseState)
            {
                // Continue game.
                if (keyboard.IsKeyDown(Keys.Space) || keyboard.IsKeyDown(Keys.Enter))
                {
                    ContinueGame();
                }
            }

            if (currentState is GameOverState)
            {
                // New game.
                if (keyboard.IsKeyDown(Keys.Enter))
                {
                    NewGame();
                }
            }

            if (currentState is SkinsState)
            {
                // Left.
                if (keyboard.IsKeyDown(Keys.Left) && !previousKeyboard.IsKeyDown(Keys.Left))
                {
                    SkinsState.LeftArrowKey(-1);
                    Util.SaveSkinData();
                }

                // Right.
                if (keyboard.IsKeyDown(Keys.Right) && !previousKeyboard.IsKeyDown(Keys.Right))
                {
                    SkinsState.RightArrowKey();
                    Util.SaveSkinData();
                }

                // Enter.
                if (keyboard.IsKeyDown(Keys.Enter) && !previousKeyboard.IsKeyDown(Keys.Enter))
                {
                    MainMenu();
                }
            }

            if (currentState is TrophyState)
            {
                // Enter.
                if (keyboard.IsKeyDown(Keys.Enter) && !previousKeyboard.IsKeyUp(Keys.Enter))
                {
                    MainMenu();
                }
            }
        }

        public static void MainMenu()
        {
            game.ChangeState(new MenuState(game, graphicsDevice, content));
        }

        public static void NewGame()
        {
            Game1.GameState = null;
            GameState.Score = 0;
            GameState.Coins = 0;
            Game1.Instance.ChangeState(new GameState(game, graphicsDevice, content));
        }

        public static void ContinueGame()
        {
            Game1.Instance.ChangeState(Game1.GameState);
            Game1.Instance.IsMouseVisible = false;
            Background.SetAlpha(100);
        }
    }
}

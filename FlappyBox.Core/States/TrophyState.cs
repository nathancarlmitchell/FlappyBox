using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using FlappyBox.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace FlappyBox.States
{
    public class TrophyState : State
    {
        private KeyboardState _currentKeyboard,
            _previousKeyboard;
        private List<Button> _components;
        private Menu _menu;
        private AnimatedTexture trophySprite;
        private SpriteFont hudFont;

        public static List<Trophy> Trophys { get; set; }

        public TrophyState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base()
        {
            _game.IsMouseVisible = true;

            Background.SetAlpha(0.5f);

            hudFont = Art.HudFont;

            trophySprite = Art.TrophySprite;
            //_trophySprite = new AnimatedTexture(new Vector2(0, 0), 0, 1f, 0.5f);
            //_trophySprite.Load(_content, "trophy", 1, 1);

            var backButton = new Button()
            {
                Text = "Back",
            };

            backButton.Click += BackButton_Click;

            _components = new List<Button>() { backButton };

            _menu = new Menu(_components);

            Util.LoadTrophyData(content);

            UnlockTrophys();
            // Util.SaveTrophyData();

            for (int i = 0; i < Trophys.Count; i++)
            {
            //    Skins[i].Click += Skin_Click;

            //    if (Skins[i].Selected)
            //    {
            //        Skins[i].Activate();
            //    }
            }

            //Skins = Skins.OrderBy(o => o.Cost).ToList();
        }

        public static void UnlockTrophys()
        {
            for (int i = 0; i < Trophys.Count; i++)
            {
                switch (Trophys[i].Name)
                {
                    case "Tiny Wings":
                        if (GameState.HighScore >= 1000)
                            Trophys[i].Locked = false; break;

                    case "Angry Birds":
                        if (GameState.HighScore >= 5000)
                            Trophys[i].Locked = false; break;

                    case "Flight Simulator":
                        if (GameState.HighScore >= 10000)
                            Trophys[i].Locked = false; break;

                    case "Sunscreen":
                        Trophys[i].Locked = false;
                        for (int x = 0; x < SkinsState.Skins.Count; x++)
                        {
                            if (SkinsState.Skins[x].Locked)
                            {
                                Trophys[i].Locked = true;
                            }
                        } break;

                    case "Bezos":
                        if (GameState.TotalCoins >= 100)
                            Trophys[i].Locked = false; break;
                }
            }
            Util.SaveTrophyData();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw the background.
            Background.Draw(gameTime, spriteBatch);

            // Draw coins on HUD.
            //spriteBatch.DrawString(
            //    hudFont,
            //    " x " + GameState.Coins,
            //    new Vector2(GameState.CoinHUD.X + 16, GameState.CoinHUD.Y - 8),
            //    Color.Black
            //);
            //GameState.CoinHUD.coinTexture.DrawFrame(
            //    spriteBatch,
            //    new Vector2(GameState.CoinHUD.X, GameState.CoinHUD.Y)
            //);

            // Draw trophys.
            for (int i = 0; i < Trophys.Count; i++)
            {
                int _centerComponent = Trophys.Count / 2;
                int _difference = i - _centerComponent;
                int _x = MenuState.CenterWidth + _difference * 175 - (64 / Trophys.Count);
                int _y = MenuState.CenterHeight - 128 - 16;

                string name = "???";
                string description = "???";
                Color titleColor = Color.White;

                if (!Trophys[i].Locked)
                {
                    name = Trophys[i].Name;
                    description = Trophys[i].Description;
                    titleColor = Color.Gold;
                }

                // Draw trophy name.
                spriteBatch.DrawString(
                    hudFont,
                    name,
                    new Vector2(_x - (hudFont.MeasureString(name).X / 2) + (trophySprite.Width / 2), _y + 72),
                    titleColor
                );

                // Draw trophy description if hovering.
                if (Trophys[i].IsHovering)
                {
                    spriteBatch.DrawString(
                        hudFont,
                    description,
                        new Vector2(_x - (hudFont.MeasureString(description).X / 2) + (trophySprite.Width / 2), _y - 72),
                        Color.White
                    );
                }

                //if (Trophys[i].Selected)
                //{
                //    _trophySprite.DrawFrame(spriteBatch, new Vector2(_x, _y - 16 - 64));
                //    Trophys[i].Position = new Vector2(_x, _y - 16);
                //}
                //else
                //{
                //    Trophys[i].Position = new Vector2(_x, _y);
                //}

                Trophys[i].Position = new Vector2(_x, _y);

                Trophys[i].Draw(spriteBatch);
            }

            // Draw the button menu.
            _menu.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        //private void Skin_Click(object sender, EventArgs e)
        //{
        //    Skin skin = (Skin)sender;

        //    if (!skin.Locked)
        //    {
        //        for (int i = 0; i < Skins.Count; i++)
        //        {
        //            Skins[i].Deactivate();
        //        }

        //        skin.Activate();
        //    }
        //    else
        //    {
        //        if (GameState.Coins >= skin.Cost)
        //        {
        //            for (int i = 0; i < Skins.Count; i++)
        //            {
        //                Skins[i].Deactivate();
        //            }
        //            GameState.Coins -= skin.Cost;
        //            skin.Locked = false;
        //            skin.Activate();
        //            Console.WriteLine("Skin unlocked.");
        //        }
        //        Console.WriteLine("Skin Locked.");
        //    }
        //}

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainMenu();
        }

        //private void LeftArrowKey(int direction)
        //{
        //    int selected = 0;

        //    for (int i = 0; i < Skins.Count; i++)
        //    {
        //        if (Skins[i].Selected)
        //        {
        //            selected = i;
        //        }
        //    }

        //    Skins[selected].Deactivate();

        //    if (selected == 0)
        //    {
        //        Console.WriteLine("Reset");
        //        selected = Skins.Count;
        //    }

        //    bool skinLocked = true;
        //    while (skinLocked)
        //    {
        //        if (Skins[selected + direction].Locked)
        //        {
        //            selected += direction;
        //        }
        //        else
        //        {
        //            skinLocked = false;
        //        }
        //    }
        //    Skins[selected + direction].Activate();
        //}

        //private void RightArrowKey()
        //{
        //    int selected = 0;
        //    int unlockedCount = 0;
        //    for (int i = 0; i < Skins.Count; i++)
        //    {
        //        if (!Skins[i].Locked)
        //            unlockedCount += 1;
        //        if (Skins[i].Selected)
        //        {
        //            selected = i;
        //        }
        //    }

        //    Skins[selected].Deactivate();

        //    if (selected == Skins.Count)
        //    {
        //        selected = 0;
        //    }

        //    bool skinLocked = true;
        //    while (skinLocked)
        //    {
        //        if (selected + 1 == Skins.Count)
        //            selected = -1;

        //        if (Skins[selected + 1].Locked)
        //        {
        //            selected += 1;
        //        }
        //        else
        //        {
        //            skinLocked = false;
        //        }
        //    }
        //    Skins[selected + 1].Activate();
        //}

        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public void MainMenu()
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Background.Update(gameTime);

            // Check touch input.
            TouchCollection touchCollection = TouchPanel.GetState();

            foreach (var component in _components)
            {
                component.Update(gameTime, touchCollection);
            }

            TouchCollection touchState = TouchPanel.GetState();
            foreach (var trophy in Trophys)
            {
                trophy.Update(gameTime, touchState);
            }

            //_arrowSprite.UpdateFrame(elapsed);

            // Check player input.
            _previousKeyboard = _currentKeyboard;
            _currentKeyboard = Keyboard.GetState();
            // Left.
            //if (_currentKeyboard.IsKeyDown(Keys.Left) && !_previousKeyboard.IsKeyDown(Keys.Left))
            //{
            //    //this.LeftArrowKey(-1);
            //}
            // Right.
            //if (_currentKeyboard.IsKeyDown(Keys.Right) && !_previousKeyboard.IsKeyDown(Keys.Right))
            //{
            //    //this.RightArrowKey();
            //}
            // Enter.
            if (_currentKeyboard.IsKeyDown(Keys.Enter) && !_previousKeyboard.IsKeyUp(Keys.Enter))
            {
                MainMenu();
            }
        }
    }
}

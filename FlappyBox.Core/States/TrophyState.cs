using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using FlappyBox.Controls;

namespace FlappyBox.States
{
    public class TrophyState : State
    {
        private KeyboardState _currentKeyboard,
            _previousKeyboard;
        private List<Button> _components;
        private Menu _menu;
        private AnimatedTexture _trophySprite;

        public static List<Skin> Skins { get; set; }

        public TrophyState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = true;

            Background.SetAlpha(0.5f);

            _trophySprite = new AnimatedTexture(new Vector2(0, 0), 0, 1f, 0.5f);
            _trophySprite.Load(_content, "trophy", 1, 1);

            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("HudFont");

            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(CenterWidth, 250),
                Text = "Back",
            };

            backButton.Click += BackButton_Click;

            _components = new List<Button>() { backButton };

            _menu = new Menu(_components);

            //Util.LoadSkinData(content);

            //for (int i = 0; i < Skins.Count; i++)
            //{
            //    Skins[i].Click += Skin_Click;

            //    if (Skins[i].Selected)
            //    {
            //        Skins[i].Activate();
            //    }
            //}

            //Skins = Skins.OrderBy(o => o.Cost).ToList();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Background.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(
                GameState.hudFont,
                " x " + GameState.Coins,
                new Vector2(GameState.coinHUD.X + 16, GameState.coinHUD.Y - 8),
                Color.Black
            );
            GameState.coinHUD.coinTexture.DrawFrame(
                spriteBatch,
                new Vector2(GameState.coinHUD.X, GameState.coinHUD.Y)
            );


            int test_number = 10;
            for (int i = 0; i < test_number; i++)
            {
                int _centerComponent = test_number / 2;
                int _difference = i - _centerComponent;
                int _x = MenuState.CenterWidth + _difference * 100 - (64 / test_number);
                int _y = MenuState.CenterHeight - 128 - 16;

                //if (Skins[i].Locked)
                //{
                //    Skins[i].LoadTexture(_content, "locked", 1, 1);
                //    spriteBatch.DrawString(
                //        GameState.hudFont,
                //        " x " + Skins[i].Cost,
                //        new Vector2(_x, _y + 72),
                //        Color.Black
                //    );
                //}

                //if (Skins[i].Selected)
                //{
                //_trophySprite.DrawFrame(spriteBatch, new Vector2(_x, _y - 16 - 64));
                //    Skins[i].Position = new Vector2(_x, _y - 16);
                //}
                //else
                //{
                //    Skins[i].Position = new Vector2(_x, _y);
                //}
                _trophySprite.DrawFrame(spriteBatch, new Vector2(_x, _y));
                //Skins[i].Draw(spriteBatch);
            }

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

        //    Util.SaveGameData(GameState.Score, GameState.Coins);
        //    Util.SaveSkinData();
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

            GameState.coinHUD.coinTexture.UpdateFrame(elapsed);

            // Check touch input.
            TouchCollection touchCollection = TouchPanel.GetState();

            foreach (var component in _components)
            {
                component.Update(gameTime, touchCollection);
            }

            TouchCollection touchState = TouchPanel.GetState();
            //foreach (var skin in Skins)
            //{
            //    skin.Update(gameTime, touchState);
            //}

            //_arrowSprite.UpdateFrame(elapsed);

            // Check player input.
            _previousKeyboard = _currentKeyboard;
            _currentKeyboard = Keyboard.GetState();
            // Left.
            if (_currentKeyboard.IsKeyDown(Keys.Left) && !_previousKeyboard.IsKeyDown(Keys.Left))
            {
                //this.LeftArrowKey(-1);
                Util.SaveGameData(GameState.Score, GameState.Coins);
                Util.SaveSkinData();
            }
            // Right.
            if (_currentKeyboard.IsKeyDown(Keys.Right) && !_previousKeyboard.IsKeyDown(Keys.Right))
            {
                //this.RightArrowKey();
                Util.SaveGameData(GameState.Score, GameState.Coins);
                Util.SaveSkinData();
            }
            // Enter.
            if (_currentKeyboard.IsKeyDown(Keys.Enter) && !_previousKeyboard.IsKeyUp(Keys.Enter))
            {
                MainMenu();
            }
        }
    }
}

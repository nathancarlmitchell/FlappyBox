using System;
using System.Collections.Generic;
using FlappyBox.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace FlappyBox.States
{
    public class TrophyState : State
    {
        private List<Button> buttons;
        private Menu menu;
        private AnimatedTexture trophySprite;
        private SpriteFont hudFont;

        public static List<Trophy> Trophys { get; set; }

        public TrophyState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base()
        {
            Game1.Instance.IsMouseVisible = true;

            Background.SetAlpha(0.5f);

            hudFont = Art.HudFont;
            trophySprite = Art.TrophySprite;

            var backButton = new Button() { Text = "Back" };
            backButton.Click += BackButton_Click;

            buttons = new List<Button>() { backButton };
            menu = new Menu(buttons);

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
                            Trophys[i].Locked = false;
                        break;

                    case "Angry Birds":
                        if (GameState.HighScore >= 5000)
                            Trophys[i].Locked = false;
                        break;

                    case "Flight Simulator":
                        if (GameState.HighScore >= 10000)
                            Trophys[i].Locked = false;
                        break;

                    case "Sunscreen":
                        Trophys[i].Locked = false;
                        for (int x = 0; x < SkinsState.Skins.Count; x++)
                        {
                            if (SkinsState.Skins[x].Locked)
                            {
                                Trophys[i].Locked = true;
                            }
                        }
                        break;

                    case "Bezos":
                        if (GameState.TotalCoins >= 100)
                            Trophys[i].Locked = false;
                        break;
                }
            }
            Util.SaveTrophyData();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw the background.
            Background.Draw(gameTime, spriteBatch);

            // Draw trophys.
            for (int i = 0; i < Trophys.Count; i++)
            {
                int _centerComponent = Trophys.Count / 2;
                int _difference = i - _centerComponent;
                int _x =
                    MenuState.CenterWidth
                    + _difference * 175
                    - ((64 * Game1.Scale) / Trophys.Count);
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
                    new Vector2(
                        _x - (hudFont.MeasureString(name).X / 2) + (trophySprite.Width / 2),
                        _y + 72
                    ),
                    titleColor
                );

                // Draw trophy description if hovering.
                if (Trophys[i].IsHovering)
                {
                    spriteBatch.DrawString(
                        hudFont,
                        description,
                        new Vector2(
                            _x
                                - (hudFont.MeasureString(description).X / 2)
                                + (trophySprite.Width / 2),
                            _y - 72
                        ),
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
            menu.Draw(gameTime, spriteBatch);

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
            Input.MainMenu();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Background.Update(gameTime);

            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (var button in buttons)
            {
                button.Update(gameTime, touchCollection);
            }

            TouchCollection touchState = TouchPanel.GetState();
            foreach (var trophy in Trophys)
            {
                trophy.Update(gameTime, touchState);
            }

            //_arrowSprite.UpdateFrame(elapsed);
        }
    }
}

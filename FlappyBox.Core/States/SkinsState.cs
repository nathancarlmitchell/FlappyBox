using System;
using System.Collections.Generic;
using System.Linq;
using FlappyBox.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace FlappyBox.States
{
    public class SkinsState : State
    {
        private readonly List<Button> buttons;
        private readonly Menu menu;
        private readonly AnimatedTexture arrowSprite;
        private readonly SpriteFont hudFont;

        public static List<Skin> Skins { get; set; }

        public SkinsState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base()
        {
            game.IsMouseVisible = true;

            Background.SetAlpha(0.5f);

            arrowSprite = Art.ArrowSprite;
            hudFont = Art.HudFont;

            var backButton = new Button() { Text = "Back" };
            backButton.Click += BackButton_Click;

            buttons = new List<Button>() { backButton };
            menu = new Menu(buttons);

            Util.LoadSkinData(content);

            for (int i = 0; i < Skins.Count; i++)
            {
                Skins[i].Click += Skin_Click;

                if (Skins[i].Selected)
                {
                    Skins[i].Activate();
                }
            }

            Skins = Skins.OrderBy(o => o.Cost).ToList();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Background.Draw(gameTime, spriteBatch);

            Overlay.DrawHUD(false);

            for (int i = 0; i < Skins.Count; i++)
            {
                int _centerComponent = Skins.Count / 2;
                int _difference = i - _centerComponent;
                int _x = CenterWidth + _difference * 100 - (64 / Skins.Count);
                int _y = CenterHeight - 128 - 16;

                if (Skins[i].Locked)
                {
                    Skins[i].LoadTexture(content, "locked", 1, 1);
                    spriteBatch.DrawString(
                        hudFont,
                        " x " + Skins[i].Cost,
                        new Vector2(_x, _y + 72),
                        Color.Black
                    );
                }

                if (Skins[i].Selected)
                {
                    arrowSprite.DrawFrame(spriteBatch, new Vector2(_x, _y - 16 - 64));
                    Skins[i].Position = new Vector2(_x, _y - 16);
                }
                else
                {
                    Skins[i].Position = new Vector2(_x, _y);
                }

                Skins[i].Draw(spriteBatch);
            }

            menu.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void Skin_Click(object sender, EventArgs e)
        {
            Skin skin = (Skin)sender;

            if (!skin.Locked)
            {
                for (int i = 0; i < Skins.Count; i++)
                {
                    Skins[i].Deactivate();
                }
                Sound.Play(Sound.Blip, 0.75f);
                skin.Activate();
            }
            else
            {
                if (GameState.Coins >= skin.Cost)
                {
                    for (int i = 0; i < Skins.Count; i++)
                    {
                        Skins[i].Deactivate();
                    }
                    GameState.Coins -= skin.Cost;
                    skin.Locked = false;
                    skin.Activate();
                    Console.WriteLine("Skin unlocked.");
                    Sound.Play(Sound.Unlock, 0.35f);
                }
                Console.WriteLine("Skin Locked.");
                Sound.Play(Sound.Locked, 0.75f);
            }

            Util.SaveGameData();
            Util.SaveSkinData();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Input.MainMenu();
        }

        public static void LeftArrowKey(int direction)
        {
            Sound.Play(Sound.Blip, 0.75f);
            int selected = 0;

            for (int i = 0; i < Skins.Count; i++)
            {
                if (Skins[i].Selected)
                {
                    selected = i;
                }
            }

            Skins[selected].Deactivate();

            if (selected == 0)
            {
                Console.WriteLine("Reset");
                selected = Skins.Count;
            }

            bool skinLocked = true;
            while (skinLocked)
            {
                if (Skins[selected + direction].Locked)
                {
                    selected += direction;
                }
                else
                {
                    skinLocked = false;
                }
            }
            Skins[selected + direction].Activate();
        }

        public static void RightArrowKey()
        {
            Sound.Play(Sound.Blip, 0.75f);
            int selected = 0;
            int unlockedCount = 0;
            for (int i = 0; i < Skins.Count; i++)
            {
                if (!Skins[i].Locked)
                    unlockedCount += 1;
                if (Skins[i].Selected)
                {
                    selected = i;
                }
            }

            Skins[selected].Deactivate();

            if (selected == Skins.Count)
            {
                selected = 0;
            }

            bool skinLocked = true;
            while (skinLocked)
            {
                if (selected + 1 == Skins.Count)
                    selected = -1;

                if (Skins[selected + 1].Locked)
                {
                    selected += 1;
                }
                else
                {
                    skinLocked = false;
                }
            }
            Skins[selected + 1].Activate();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Background.Update(gameTime);

            GameState.CoinHUD.CoinTexture.UpdateFrame(elapsed);

            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (var button in buttons)
            {
                button.Update(gameTime, touchCollection);
            }

            foreach (var skin in Skins)
            {
                skin.Update(gameTime);
            }

            arrowSprite.UpdateFrame(elapsed);
        }
    }
}

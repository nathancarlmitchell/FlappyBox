using System;
using System.Collections.Generic;
using System.Linq;
using FlappyBox.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace FlappyBox
{
    public class Skin : Object
    {
        protected ContentManager content;
        private AnimatedTexture texture;
        private MouseState mouse,
            previousMouse;
        private bool isHovering;

        public int Frames = 2;
        public int FPS = 2;
        public string Name { get; set; }
        public bool Selected { get; set; }
        public bool Locked { get; set; }
        public int Cost { get; set; }
        public Vector2 Position { get; set; }
        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public new Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, 64, 64); }
        }

        public Skin(ContentManager _content, String name)
        {
            content = _content;

            this.Name = name;

            // Load the texture.
            texture = new AnimatedTexture(new Vector2(0, 0), 0, 1f, 0.5f);
            texture.Load(content, name, Frames, FPS);
        }

        public void LoadTexture(ContentManager content, String name)
        {
            texture = new AnimatedTexture(new Vector2(0, 0), 0, 1f, 0.5f);
            texture.Load(content, name, Frames, FPS);
        }

        public void LoadTexture(ContentManager content, String name, int frames, int fps)
        {
            texture = new AnimatedTexture(new Vector2(0, 0), 0, 1f, 0.5f);
            texture.Load(content, name, frames, fps);
        }

        public void Activate()
        {
            string name = this.Name.Split("_").Last();
            texture.Load(content, "anim_jump_" + name, Frames, FPS);
            this.Selected = true;
        }

        public void Deactivate()
        {
            string name = this.Name.Split("_").Last();
            texture.Load(content, "anim_idle_" + name, Frames, FPS);
            this.Selected = false;
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            previousMouse = mouse;
            mouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if (
                    mouse.LeftButton == ButtonState.Released
                    && previousMouse.LeftButton == ButtonState.Pressed
                )
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }

            // Check touch input.
            TouchCollection touchCollection = TouchPanel.GetState();
            if (touchCollection.AnyTouch())
            {
                foreach (TouchLocation tl in touchCollection)
                {
                    int x = (int)tl.Position.X;
                    int y = (int)tl.Position.Y;
                    var touchRectangle = new Rectangle(x, y, 1, 1);
                    if (touchRectangle.Intersects(Rectangle))
                    {
                        Click?.Invoke(this, new EventArgs());
                    }
                }
            }

            texture.UpdateFrame(elapsed);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            var color = Color.Gray;

            if (this.Selected)
            {
                color = Color.White;
            }

            if (isHovering)
            {
                color = Color.AntiqueWhite;
            }

            texture.DrawFrame(_spriteBatch, this.Position, color);
        }
    }
}

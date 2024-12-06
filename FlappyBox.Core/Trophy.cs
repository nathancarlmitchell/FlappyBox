using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace FlappyBox
{
    public class Trophy : Object
    {
        protected ContentManager content;
        private AnimatedTexture texture;
        private MouseState mouse,
            previousMouse;

        public bool IsHovering;

        public int Frames = 1;
        public int FPS = 1;
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
        public bool Locked { get; set; }
        public Vector2 Position { get; set; }
        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public new Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, 64, 64); }
        }

        public Trophy(ContentManager _content, String _texture)
        {
            this.content = _content;

            //this.Name = _texture;

            // Load the texture.
            this.texture = Art.TrophySprite;
        }

        public void Update(GameTime gameTime, TouchCollection touchCollection)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.IsHovering = false;
            this.Selected = false;

            previousMouse = mouse;
            mouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
            if (mouseRectangle.Intersects(Rectangle))
            {
                // This needs to be fixed.
                // Android flashes the desction text because mouseRectangle.
                if (!OperatingSystem.IsAndroid())
                {
                    this.IsHovering = true;
                }

                if (
                    mouse.LeftButton == ButtonState.Released
                    && previousMouse.LeftButton == ButtonState.Pressed
                )
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }

            // Check touch input.
            if (touchCollection.AnyTouch())
            {
                foreach (TouchLocation tl in touchCollection)
                {
                    int x = (int)tl.Position.X;
                    int y = (int)tl.Position.Y;
                    var touchRectangle = new Rectangle(x, y, 1, 1);
                    if (touchRectangle.Intersects(Rectangle))
                    {
                        //Click?.Invoke(this, new EventArgs());
                        IsHovering = true;
                    }
                }
            }

            texture.UpdateFrame(elapsed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.Black;

            if (!this.Locked)
            {
                color = Color.White;
            }

            if (!this.Locked && IsHovering)
            {
                color = Color.AntiqueWhite;
            }

            texture.DrawFrame(spriteBatch, this.Position, color);
        }
    }
}

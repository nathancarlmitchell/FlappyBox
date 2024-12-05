using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBox
{
    public class Wall : Object
    {
        // Tiling Texture
        private static Texture2D wallTexture;

        // How many tiles wide
        private int tileCountWidth = 1;

        // How many tiles high
        public int TileCountHeight { get; set; }

        // Rectangle to draw tiles in
        private Rectangle targetRectangle;

        public static void LoadTexture()
        {
            string textureName = "wall";
            //if (GameState.Player is not null)
            //{
            //    Console.WriteLine(GameState.Player.SkinName);
            //    if (GameState.Player.SkinName == "anim_idle_companion")
            //    {
            //        textureName = "ball";
            //    }
            //}

            // Load the texture to tile.
            //Console.WriteLine("Defualt Wall texture loaded.");
            wallTexture = Game1.Instance.Content.Load<Texture2D>(textureName);
        }

        public void Move()
        {
            this.X -= 2;
        }

        public void Update()
        {
            // Define a drawing rectangle based on the number of tiles wide and high, using the texture dimensions.
            targetRectangle = new Rectangle(
                0,
                0,
                wallTexture.Width * tileCountWidth,
                wallTexture.Height * TileCountHeight
            );
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                BlendState.AlphaBlend,
                SamplerState.LinearWrap,
                DepthStencilState.Default,
                RasterizerState.CullNone
            );

            _spriteBatch.Draw(
                wallTexture,
                new Vector2(this.X, this.Y),
                targetRectangle,
                Color.White
            );

            _spriteBatch.End();
        }
    }
}

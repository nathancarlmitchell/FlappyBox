using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FlappyBox.States;

namespace FlappyBox
{
    public class WallSpawner
    {
        private readonly Random rand = new();
        private int _screenHeight = Game1.ScreenHeight;
        private int _screenWidth = Game1.ScreenWidth;
        private int minHeight = 20;
        private int width = 64;
        private int minGap = 166;
        private int maxGap = 200;
        private List<Wall> walls;
        private Wall wall_1,
            wall_2;

        public WallSpawner()
        {
            // Changes the default wall texture if using skins.
            // Currently not used.
            Wall.LoadTexture();
        }

        public List<Wall> Spawn()
        {
            walls = new List<Wall>();
            wall_1 = new Wall();
            wall_2 = new Wall();

            int gap = (int)Math.Floor(rand.NextDouble() * (maxGap - minGap + 1) + minGap);
            int maxHeight = _screenHeight - gap - minGap;
            int height = (int)
                Math.Floor(rand.NextDouble() * (maxHeight - minHeight + 1) + minHeight);
            int tileHeight = (int)Math.Ceiling((double)height / 64);
            int heightDifference = (tileHeight * 64) - height;

            wall_1.X = _screenWidth;
            wall_1.Y = 0;
            wall_1.Width = width;
            wall_1.Height = height;
            wall_1.Y -= heightDifference;
            wall_1.Height += heightDifference;
            wall_1.TileCountHeight = tileHeight;

            wall_2.X = _screenWidth;
            wall_2.Y = height + gap;
            wall_2.Width = width;
            wall_2.Height = _screenHeight - height - gap;
            tileHeight = (int)Math.Ceiling((double)(_screenHeight - height - gap) / 64);
            wall_2.TileCountHeight = tileHeight;

            // Creates a new texture with the correct TileCountHeight.
            wall_1.Update();
            wall_2.Update();

            walls.Add(wall_1);
            walls.Add(wall_2);
            return walls;
        }
    }
}

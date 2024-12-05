using System;
using FlappyBox.States;

namespace FlappyBox
{
    public class CoinSpawner
    {
        private static readonly Random rand = new();
        private Coin coin;

        public Coin Spawn()
        {
            coin = new Coin();

            int heightBuffer = 64;
            int minHeight = heightBuffer + coin.Height;
            int maxHeight = Game1.ScreenHeight - (heightBuffer + coin.Height);

            int height = (int)
                Math.Floor(rand.NextDouble() * (maxHeight - minHeight + 1) + minHeight);

            coin.X = Game1.ScreenWidth;
            coin.Y = height;

            return coin;
        }
    }
}

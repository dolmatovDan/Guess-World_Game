using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake
{
    class Settings
    {
        public static int Width;
        public static int Height;
        public static int Speed;
        public static int Points;
        public static int Score;
        public static bool GameOver;
        public Settings()
        {
            Width = 20;
            Height = 20;
            Speed = 10;
            Score = 0;
            Points = 100;
            GameOver = false;
        }
        public static Direction direction;
    }
    public enum Direction
    {
        Up, 
        Down,
        Left,
        Right
    };
}

using System.Drawing;

namespace Snake.Engine
{
    public static class GameConfig
    {
        public const int BoardWidth = 20;
        public const int BoardHeight = 20;
        public const int TileWidth = 20;
        public const int TileHeight = 20;
        public const int SnakeMovementInterval = 300; // miliseconds

        public static bool Paused = true;
        public static readonly Color PausedBackground = Color.FromArgb(220, 220, 220);
        public static readonly Color UnpausedBackground = Color.White;
    }
}

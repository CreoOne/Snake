using System;

namespace Snake.Engine
{
    public static class StatisticsMonitor
    {
        public static void Reset()
        {
            Score = 0;
            Turns = 0;
            Tiles = 0;
        }

        public static double Fill { get; private set; }
        public static ulong Score { get; private set; }
        public static void IncrementScore()
        {
            Score++;
            Fill = (Score + 1) / (double)(GameConfig.BoardWidth * GameConfig.BoardHeight);
        }

        public static ulong Turns { get; private set; }
        public static void IncrementTurns()
        {
            Turns++;
        }

        public static ulong Tiles { get; private set; }
        public static void IncrementTiles()
        {
            Tiles++;
        }

        public static ulong Rating { get { return CalculateRating(); } }
        private static ulong CalculateRating()
        {
            return (ulong)(CalculateTurnUtilisation() * 500) + (ulong)(CalculateCompleteness() * 10);
        }

        private static double CalculateTurnUtilisation()
        {
            if (Score == 0)
                return 1;

            return Score / (Turns / 2d);
        }

        private static double CalculateCompleteness()
        {
            return Fill * Math.Sqrt(Math.Pow(GameConfig.BoardWidth, 2) + Math.Pow(GameConfig.BoardHeight, 2));
        }
    }
}

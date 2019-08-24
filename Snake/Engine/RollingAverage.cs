namespace Snake.Engine
{
    public static class RollingAverage
    {
        public static double Average(double average, double value, double length)
        {
            return (average * (length - 1) + value) / length;
        }
    }
}

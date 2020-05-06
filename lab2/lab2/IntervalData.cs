namespace lab2
{
    public class IntervalData
    {
        public double StartPoint { get; set; }
        public double EndPoint { get; set; }

        public double MidPoint => (StartPoint + EndPoint) / 2;

        public double PointsCount { get; set; }

        public bool IsIn(double x)
        {
            return StartPoint <= x && EndPoint > x;
        }
    }
}

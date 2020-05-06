using System;

namespace lab2
{
    public class GaussDen : ICalculated
    {
        public double Get(double x)
        {
            return (1 / Math.Sqrt(2 * Math.PI)) * (Math.Exp(-(Math.Pow(x, 2)) / 2));
        }
    }
}

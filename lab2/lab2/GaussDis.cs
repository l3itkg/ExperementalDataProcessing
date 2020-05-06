using System;

namespace lab2
{
    public class GaussDis : ICalculated
    {
        public double Get(double x)
        {
            double[] d = { 4986.7347e-5, 2114.1006e-5, 327.76263e-5, 38.0036e-6, 48.8906e-6, 53.83e-7 };
            return
                1 - 0.5 * Math.Pow((1 + d[0] * x + d[1] * Math.Pow(x, 2)
                                    + d[2] * Math.Pow(x, 3) + d[3] * Math.Pow(x, 4) + d[4]
                                    * Math.Pow(x, 5) + d[5] * Math.Pow(x, 6)), -16);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class XiSqr
    {
        private double Gauss(double alpha)
        {
            double[] c = { 2.515517, 0.8028538, 0.01032 };
            double[] d = { 1.432788, 0.189269, 0.001308 };
            var t = Math.Sqrt(Math.Log(Math.Pow(alpha, -2)));
            var res = t - ((c[0] + c[1] * t + c[2] * Math.Pow(t, 2)) / (1 + d[0] * t + d[1] * Math.Pow(t, 2) + d[2] * Math.Pow(t, 3)));
            return res;
        }

        public double Get(double q, int v)
        {
            var lambdaq = Gauss(q);
            var res = v * Math.Pow((1 - (2.0 / (9 * v)) + lambdaq * Math.Sqrt(2.0 / (9 * v))), 3);
            return res;
        }
    }
}

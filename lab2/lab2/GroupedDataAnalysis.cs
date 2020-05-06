using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class GroupedDataAnalysis
    {
        public IEnumerable<double> OrigData { get; private set; }
        public List<IntervalData> GroupedData { get; private set; }

        public GroupedDataAnalysis(IEnumerable<double> data)
        {
            OrigData = data;
            GroupData();
        }

        public void GroupData()
        {
            int cnt = (int)Math.Round(1 + 3.31 * Math.Log10(OrigData.Count()), 0);
            var min = OrigData.Min();
            var max = OrigData.Max();
            var step = (max - min) / cnt;
            // Формируем интервалы
            GroupedData = new List<IntervalData>();
            for (double k = min; k + step < max + step / 2; k += step)
            {
                GroupedData.Add(new IntervalData() { StartPoint = k, EndPoint = k + step, PointsCount = 0 });
            }
            //  Подсчитываем количество точек на интервал
            foreach (var p in OrigData)
            {
                var group = GroupedData.FirstOrDefault(x => x.IsIn(p));
                if (group == null)
                    group = GroupedData.Last();
                group.PointsCount += 1;
            }
        }

        public double[] GetInitMoments()
        {
            var initMoments = new double[4];
            for (int i = 0; i < 4; i++)
            {
                initMoments[i] =
                    GroupedData.Sum(x => Math.Pow(x.MidPoint, i + 1) * x.PointsCount) / GroupedData.Sum(x => x.PointsCount);
            }
            return initMoments;
        }

        public double[] GetDisplacedCentMoments()
        {
            var initMoments = GetInitMoments();
            var centMoments = new double[4];
            centMoments[0] =
                initMoments[0];
            centMoments[1] =
                initMoments[1]
                - initMoments[0] * initMoments[0];
            centMoments[2] =
                initMoments[2]
                - 3 * initMoments[0] * initMoments[1]
                + 2 * Math.Pow(initMoments[0], 3);
            centMoments[3] =
                initMoments[3]
                - 4 * initMoments[0] * initMoments[2]
                + 6 * Math.Pow(initMoments[0], 2) * initMoments[1]
                - 3 * Math.Pow(initMoments[0], 4);
            return centMoments;
        }

        public double[] GetCentMoments()
        {
            var centMoments = GetDisplacedCentMoments();
            var n = GroupedData.Sum(x => x.PointsCount);
            var newMoments = new double[4];
            newMoments[0] =
                centMoments[0];
            newMoments[1] =
                GroupedData.Sum(x => x.PointsCount * Math.Pow(x.MidPoint - newMoments[0], 2))
                / (n - 1);
            newMoments[2] =
                (n * n * centMoments[2]) / ((n - 1) * (n - 2));
            newMoments[3] = (
                (n * (n * n - 2 * n + 3) * centMoments[3]) -
                 3 * n * (2 * n - 3) * Math.Pow(centMoments[1], 2)
                 ) / ((n - 1) * (n - 2) * (n - 3));
            return newMoments;
        }

        public double GetMathExp()
        {
            return GetCentMoments()[0];
        }

        public double GetDisp()
        {
            return GetCentMoments()[1];
        }

        public double GetStdDev()
        {
            return Math.Sqrt(GetDisp());
        }

        public double GetAssymetryCoef()
        {
            return GetCentMoments()[2] / Math.Pow(GetStdDev(), 3);
        }

        public double GetExcessCoef()
        {
            var centMoments = GetCentMoments();
            return centMoments[3] / Math.Pow(centMoments[1], 2);
        }

        public List<double> GetStandartVars()
        {
            var mathExp = GetMathExp();
            var stdDev = GetStdDev();
            return GroupedData.Select(x => (x.MidPoint - mathExp) / stdDev).ToList();
        }

        public List<double> TheorFreqsFirst()
        {
            var n = GroupedData.Sum(x => x.PointsCount);
            var k = GroupedData.Count;
            var h = (OrigData.Max() - OrigData.Min()) / k;
            var matExp = GetMathExp();
            var stdDev = GetStdDev();
            var standartVars = GetStandartVars();

            var gauss = new GaussDen();
            return standartVars.Select(x => n * h * gauss.Get(x) / stdDev).ToList();
        }

        public List<double> TheorFreqsSecond()
        {
            var n = GroupedData.Sum(x => x.PointsCount);
            var k = GroupedData.Count;
            var h = (OrigData.Max() - OrigData.Min()) / k;
            var matExp = GetMathExp();
            var stdDev = GetStdDev();
            var standartVars = GetStandartVars();

            var gauss = new GaussDis();
            var theorFreqs = new List<double>();
            for (int i = 0; i < standartVars.Count; i++)
            {
                if (i == 0)
                {
                    theorFreqs.Add(n * (gauss.Get(standartVars[i + 1])));
                }
                else if (i == standartVars.Count - 1)
                {
                    theorFreqs.Add(n * (1 - gauss.Get(standartVars[i])));
                }
                else
                    theorFreqs.Add(n * (gauss.Get(standartVars[i + 1]) - gauss.Get(standartVars[i])));
            }
            return theorFreqs;
        }
    }
}

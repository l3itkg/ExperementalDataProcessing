using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            chartAbs.ChartAreas[0].AxisX.LabelStyle.Format = "{0:N2}";
            chartRel.ChartAreas[0].AxisX.LabelStyle.Format = "{0:N2}";
        }

        public double[] GetData()
        {
            var buffer = new List<double>();
            using (var fs = File.OpenRead("data.dat"))
            {
                using (var sr = new StreamReader(fs))
                {

                    while (!sr.EndOfStream)
                    {
                        buffer.Add(double.Parse(sr.ReadLine()));
                    }
                }
            }

            return buffer.ToArray();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var analyser = new GroupedDataAnalysis(GetData());
            var n = analyser.OrigData.Count();
            foreach (var group in analyser.GroupedData)
            {
                chartAbs.Series["polyAbs"].Points.AddXY(group.MidPoint, group.PointsCount / (group.EndPoint - group.StartPoint));
                chartRel.Series["polyAbs"].Points.AddXY(group.MidPoint, group.PointsCount / n / (group.EndPoint - group.StartPoint));
            }

            var init = analyser.GetInitMoments().Select(x => Math.Round(x, 3)).ToList();
            textBox1.AppendText($"Начальные моменты: {init[0]}   {init[1]}   {init[2]}   {init[3]}\n");
            var cent = analyser.GetCentMoments().Select(x => Math.Round(x, 3)).ToList();
            textBox1.AppendText($"Центральные моменты: {cent[0]}   {cent[1]}   {cent[2]}   {cent[3]}\n");
            var s = Math.Round(analyser.GetStdDev(), 3);
            var assym = Math.Round(analyser.GetAssymetryCoef(), 3);
            var excess = Math.Round(analyser.GetExcessCoef(), 3);
            textBox1.AppendText($"Среднеквадратическое откл.: {s}\n");
            textBox1.AppendText($"Коэффициент ассиметрии: {assym}\n");
            textBox1.AppendText($"Коэффициент эксцесса: {excess}\n");
            textBox1.AppendText("Значения и стандартизованные значения\n");
            var stand = analyser.GetStandartVars();
            for (int i = 0; i < stand.Count; i++)
            {
                textBox1.AppendText($"{Math.Round(analyser.GroupedData[i].MidPoint, 3)}\t{Math.Round(stand[i], 3)}\n");
            }


            var xi = new XiSqr().Get(0.05, analyser.GroupedData.Count - 1 - 2);
            var xi_t1 = analyser.TheorFreqsFirst();
            var xi1 = analyser.GroupedData.Select((x, i) => Math.Pow(x.PointsCount - xi_t1[i], 2) / xi_t1[i]).Sum();
            var xi_t2 = analyser.TheorFreqsSecond();
            var xi2 = analyser.GroupedData.Select((x, i) => Math.Pow(x.PointsCount - xi_t1[i], 2) / xi_t1[i]).Sum();
            textBox1.AppendText("Критерий хи-квадрат ( x2 < x2(q,v) )\n");
            textBox1.AppendText($"Способ первый: {xi1} < {xi} ? { (xi1 < xi ? "ок" : "miss")}\n");
            textBox1.AppendText($"Способ второй: {xi2} < {xi} ? { (xi1 < xi ? "ок" : "miss")}\n");
        }
    }
}
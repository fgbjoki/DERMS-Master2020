using CalculationEngine.Forecast.WeatherForecast.DataConverter.Regression.Functions;
using MathNet.Numerics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace CalculationEngine.Forecast.WeatherForecast.DataConverter.Regression
{
    public class RegressionParameters
    {
        public RegressionParameters(double[][] matrix, double[] values)
        {
            Matrix = matrix;
            Values = values;
        }

        public double[][] Matrix { get; set; }

        public double[] Values { get; set; }
    }

    public class RegressionFunctionCreator
    {
        public ReggressionFunction CreateFunction(Point[] samplePoints)
        {
            var parameters = CreateParameters(samplePoints);

            // function which gives slope(k) + intercept(n)
            // var lsq = new FloatLeastSquares(parameters.Matrix, parameters.Values, true);
            double[] lsq = Fit.MultiDim(parameters.Matrix, parameters.Values, intercept: true);


            //return new LinearReggressionFunction(lsq.X[1], lsq.X[0]);
            return new LinearReggressionFunction(float.Parse(lsq[1].ToString()), float.Parse(lsq[0].ToString()));
        }

        private RegressionParameters CreateParameters(Point[] samplePoints)
        {
            CultureInfo original = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            //FloatMatrix matrix = new FloatMatrix($"{samplePoints.Length}x1[" + string.Join(" ", samplePoints.Select(x => x.X)) + "]");
            double[][] matrix = new double[samplePoints.Length][];
            for(int i=0; i<samplePoints.Length; i++)
            {
                matrix[i] = new double[1];
            }

            int j = 0;
            foreach(Point p in samplePoints)
            {
                matrix[j][0] = p.X;
                j++;
            }
            //FloatVector values = new FloatVector($"[{string.Join(" ", samplePoints.Select(x => x.Y))}]");
            double[] values = samplePoints.Select(x => x.Y).ToArray<double>();

            Thread.CurrentThread.CurrentCulture = original;

            return new RegressionParameters(matrix, values);
        }
    }
}

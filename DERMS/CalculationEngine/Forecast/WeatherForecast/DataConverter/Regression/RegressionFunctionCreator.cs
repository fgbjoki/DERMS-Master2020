using CalculationEngine.Forecast.WeatherForecast.DataConverter.Regression.Functions;
using CenterSpace.NMath.Core;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace CalculationEngine.Forecast.WeatherForecast.DataConverter.Regression
{
    public class RegressionParameters
    {
        public RegressionParameters(FloatMatrix matrix, FloatVector values)
        {
            Matrix = matrix;
            Values = values;
        }

        public FloatMatrix Matrix { get; set; }

        public FloatVector Values { get; set; }
    }

    public class RegressionFunctionCreator
    {
        public ReggressionFunction CreateFunction(Point[] samplePoints)
        {
            var parameters = CreateParameters(samplePoints);

            var lsq = new FloatLeastSquares(parameters.Matrix, parameters.Values, true);

            return new LinearReggressionFunction(lsq.X[1], lsq.X[0]);
        }

        private RegressionParameters CreateParameters(Point[] samplePoints)
        {
            CultureInfo original = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            FloatMatrix matrix = new FloatMatrix($"{samplePoints.Length}x1[" + string.Join(" ", samplePoints.Select(x => x.X)) + "]");
            FloatVector values = new FloatVector($"[{string.Join(" ", samplePoints.Select(x => x.Y))}]");

            Thread.CurrentThread.CurrentCulture = original;

            return new RegressionParameters(matrix, values);
        }
    }
}

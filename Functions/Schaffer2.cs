using System;

namespace OpenPSO.Functions
{
    public class Schaffer2 : IFunction
    {
        public double Evaluate(double[] position)
        {
            if (position.Length != 2)
                throw new ArgumentException(
                    $"{nameof(Schaffer2)} function only works in 2D");

            double sSum = position[0] * position[0] + position[1] * position[1];
            double tmp1 = Math.Sin(Math.Sqrt(sSum));
            double tmp2 = 1 + 0.001 * sSum;

            return 0.5 + (tmp1 * tmp1 - 0.5) / (tmp2 * tmp2);
        }
    }
}

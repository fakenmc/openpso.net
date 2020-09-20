using System;

namespace OpenPSO.Functions
{
    public class Griewank : IFunction
    {
        public double Evaluate(double[] position)
        {
            double fit1 = 0.0;
            double fit2 = 1.0;

            for (int i = 0; i < position.Length; i++)
            {
                fit1 += position[i] * position[i];
                fit2 *= Math.Cos(position[i] / Math.Sqrt(i + 1.0));
            }
            return 1 + (fit1 / 4000.0) - fit2;
        }
    }
}

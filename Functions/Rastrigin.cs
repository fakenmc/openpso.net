using System;
using OpenPSO.Lib;

namespace OpenPSO.Functions
{
    public class Rastrigin : IFunction
    {
        public double Evaluate(double[] position)
        {
            double fitness = 0.0;

            for (int i = 0; i < position.Length; i++)
            {
                fitness += position[i] * position[i]
                    - 10 * Math.Cos(2.0 * Math.PI * position[i]) + 10;
            }
            return fitness;
        }
    }
}

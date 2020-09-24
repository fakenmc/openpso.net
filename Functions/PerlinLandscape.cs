using OpenPSO.Lib;
using System.Collections.Generic;

namespace OpenPSO.Functions
{
    public class PerlinLandscape : IFunction
    {
        public double Evaluate(IList<double> position)
        {
            if (position.Count != 2)
            {
                throw new System.NotImplementedException("PerlinLandscape function only defined for 2 dimensions!");
            }

            double fitness = 0.0;

            double x = position[0];
            double y = position[1];

            double amp = 20;
            double freq = 0.02;

            int octaves = 8;
            for (int i = 0; i < octaves; i++)
            {
                fitness += amp * Perlin2d.Evaluate(x * freq, y * freq);
                amp *= 0.5f;
                freq *= 2.0f;
            }

            return fitness;
        }
    }
}

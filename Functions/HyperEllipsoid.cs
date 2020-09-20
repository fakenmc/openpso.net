namespace OpenPSO.Functions
{
    public class HyperEllipsoid : IFunction
    {
        public double Evaluate(double[] position)
        {
            double fitness = 0.0;

            for (int i = 0; i < position.Length; i++)
            {
                fitness += i * position[i] * position[i];
            }
            return fitness;
        }
    }
}

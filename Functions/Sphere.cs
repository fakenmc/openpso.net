namespace OpenPSO.Functions
{
    public class Sphere : IFunction
    {
        public double Evaluate(double[] position)
        {
            double fitness = 0.0;

            for (int i = 0; i < position.Length; i++)
            {
                fitness += position[i] * position[i];
            }
            return fitness;
        }
    }
}

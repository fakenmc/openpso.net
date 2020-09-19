using System;

namespace PSO
{
    public class PSO
    {
        // Inertia weight
        private double w;

        // Acceleration coefficients, used to tune the relative influence of
        // each term of the formula
        private double c1, c2;

        private double xMax, vMax;

        private Random r;

        public PSO()
        {
            r = new Random();
        }

        private void UpdateParticle(Particle p)
        {
            for (int i = 0; i < p.NDim; i++)
            {
                // Update velocity
                p.Velocity[i] = w * p.Velocity[i]
                    + c1 * r.NextDouble() * (p.PBest(i) - p.Position[i])
                    + c2 * r.NextDouble() * (p.GBest(i) - p.Position[i]);

                // Keep velocity in bounds
                if (p.Velocity[i] > vMax) p.Velocity[i] = vMax;
                if (p.Velocity[i] < -vMax) p.Velocity[i] = -vMax;

                // Update position
                p.Position[i] = p.Position[i] + p.Velocity[i];

                // Keep position in bounds, stop particle if necessary
                if (p.Position[i] > xMax)
                {
                    p.Position[i] = xMax;
                    p.Velocity[i] = 0;
                }
                else if (p.Position[i] < -xMax)
                {
                    p.Position[i] = -xMax;
                    p.Velocity[i] = 0;
                }
            }
        }
    }
}

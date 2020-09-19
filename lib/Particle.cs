using System;
//using System.Collections.Generic;

namespace OpenPSO.Lib
{
    public class Particle
    {
        private Config cfg;

        //void* neigh_info;
        private int id;
        private double fitness;
        private double best_fitness_so_far;
        private double informants_best_fitness_so_far;
        private double[] informants_best_position_so_far;

        // Best position so far for this particle
        private double[] pBest;

        private double[] position;
        private double[] velocity;

        private Func<int, double> gBest;

        private int nDim;

        // // Best position so far for this particle
        // public double PBest(int i) => pBest[i];

        // // Best global/local position so far
        // public double GBest(int i) => gBest(i);

        // public IList<double> Position => position;
        // public IList<double> Velocity => velocity;


        public Particle(Config cfg)
        {
            this.cfg = cfg;
            nDim = position.Length;
        }


        private void Update()
        {
            for (int i = 0; i < nDim; i++)
            {
                // Update velocity
                velocity[i] = cfg.W * velocity[i]
                    + cfg.C1 * cfg.Rng.NextDouble() * (pBest[i] - position[i])
                    + cfg.C2 * cfg.Rng.NextDouble() * (gBest(i) - position[i]);

                // Keep velocity in bounds
                if (velocity[i] > cfg.VMax) velocity[i] = cfg.VMax;
                if (velocity[i] < -cfg.VMax) velocity[i] = -cfg.VMax;

                // Update position
                position[i] = position[i] + velocity[i];

                // Keep position in bounds, stop particle if necessary
                if (position[i] > cfg.XMax)
                {
                    position[i] = cfg.XMax;
                    velocity[i] = 0;
                }
                else if (position[i] < -cfg.XMax)
                {
                    position[i] = -cfg.XMax;
                    velocity[i] = 0;
                }
            }

            // Obtain particle fitness for new position
            fitness = cfg.function.Evaluate(position);

            // TODO Post-evaluation hooks, e.g. watershed
        }
    }
}

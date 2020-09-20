using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenPSO.Lib
{
    public class Particle
    {
        private Config cfg;

        //void* neigh_info;
        private double fitness;

        // Best fitness this particle ever had so far
        private double bestFitnessSoFar;
        // Best position so far for this particle
        private double[] bestPositionSoFar;

        // Best fitness ever known by neighbors
        private double neighsBestFitnessSoFar;

        private double[] neighsBestPositionSoFar;

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

        public readonly int id;

        public double Fitness =>  fitness;

        public ReadOnlyCollection<double> Position => Array.AsReadOnly(position);

        public IEnumerable<Particle> Neighbors => null; // TODO Connect this to ITopology

        public double BestFitnessSoFar => bestFitnessSoFar;

        public double NeighsBestFitnessSoFar => neighsBestFitnessSoFar;
        //public ReadOnlyCollection<double> NeighsBestPositionSoFar =>
        //    Array.AsReadOnly(neighsBestPositionSoFar);


        public Particle(int id, Config cfg)
        {
            this.id = id;
            this.cfg = cfg;
            nDim = position.Length;

            for (int i = 0; i < nDim; i++)
            {
                // TODO What if [xMin, xMax] is different for different dimensions?
                position[i] = cfg.Rng.NextDouble(cfg.InitXMin, cfg.InitXMax);
            }
        }

        public void UpdateBestNeighbor(Particle neighbor)
        {
            neighsBestFitnessSoFar = neighbor.BestFitnessSoFar;
            Array.Copy(
                neighbor.bestPositionSoFar, // Source
                neighsBestPositionSoFar,    // Destination
                nDim);
        }


        public void Update()
        {
            for (int i = 0; i < nDim; i++)
            {
                // Update velocity
                velocity[i] = cfg.W * velocity[i]
                    + cfg.C1 * cfg.Rng.NextDouble() * (bestPositionSoFar[i] - position[i])
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

        public void UpdateBestSoFar()
        {
            // Update knowledge of best fitness/position so far
            if (fitness < bestFitnessSoFar) // TODO Improve this for seeking max instead of min
            {
                bestFitnessSoFar = fitness;
                Array.Copy(position, bestPositionSoFar, nDim);
            }

            // Update knowledge of best neighbor so far if I am the best neighbor
            if (fitness < neighsBestFitnessSoFar) // TODO Improve this for seeking max instead of min
            {
                neighsBestFitnessSoFar = fitness;
                Array.Copy(position, neighsBestPositionSoFar, nDim);
            }
        }
    }
}

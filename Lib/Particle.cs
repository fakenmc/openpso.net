using System;
using System.Collections.Generic;

namespace OpenPSO.Lib
{
    public class Particle
    {

        private readonly int nDim;

        // Best position so far for this particle
        private readonly double[] bestPositionSoFar;

        private readonly double[] neighsBestPositionSoFar;

        private readonly double[] position;
        private readonly double[] velocity;

        public readonly int id;

        // Best position so far for this particle
        public IList<double> BestPositionSoFar =>  bestPositionSoFar;

        public IList<double> NeighsBestPositionSoFar => neighsBestPositionSoFar;

        public IList<double> Position => position;
        public IList<double> Velocity => velocity;

        // Current fitness
        public double Fitness { get; set; }

        // Best fitness this particle ever had so far
        public double BestFitnessSoFar { get; private set; }

        // Best fitness ever known by neighbors
        public double NeighsBestFitnessSoFar { get; private set; }


        public Particle(int id, PSO pso)
        {
            this.id = id;
            nDim = pso.NDims;

            position = new double[nDim];
            velocity = new double[nDim];
            bestPositionSoFar = new double[nDim];
            neighsBestPositionSoFar = new double[nDim];

            for (int i = 0; i < nDim; i++)
            {
                // Initialize position for current variable of current particle
                position[i] = pso.Rng.NextDouble(pso.InitXMin, pso.InitXMax); // TODO What if [xMin, xMax] is different for different dimensions?

                // Initialize velocity for current variable of current particle
                velocity[i] = pso.Rng.NextDouble(pso.XMin(pso), pso.XMax(pso))
                    * pso.Rng.NextDouble(-0.5, 0.5);
            }

            // Set best position so far as current position
            Array.Copy(position, bestPositionSoFar, nDim);

            // Set best neighbor position so far as myself
            Array.Copy(position, neighsBestPositionSoFar, nDim);

            // Determine fitness for initial position
            Fitness = pso.Function.Evaluate(position); // TODO Doesn't this count for PSO.TotalEvals?

            // TODO Hooks such as watershed

            // Set my own fitness as best fitness so far
            BestFitnessSoFar = Fitness;

            // Set me as the best neighbor so far
            NeighsBestFitnessSoFar = Fitness;
        }

        public void UpdateBestNeighbor(Particle neighbor)
        {
            NeighsBestFitnessSoFar = neighbor.BestFitnessSoFar;
            Array.Copy(
                neighbor.bestPositionSoFar, // Source
                neighsBestPositionSoFar,    // Destination
                nDim);
        }

        public void UpdateBestSoFar()
        {
            // Update knowledge of best fitness/position so far
            if (Fitness < BestFitnessSoFar) // TODO Improve this for seeking max instead of min
            {
                BestFitnessSoFar = Fitness;
                Array.Copy(position, bestPositionSoFar, nDim);
            }

            // Update knowledge of best neighbor so far if I am the best neighbor
            if (Fitness < NeighsBestFitnessSoFar) // TODO Improve this for seeking max instead of min
            {
                NeighsBestFitnessSoFar = Fitness;
                Array.Copy(position, neighsBestPositionSoFar, nDim);
            }
        }
    }
}

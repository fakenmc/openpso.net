using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenPSO.Lib
{
    public class Particle
    {

        private PSO pso;

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

        private Func<double> groupBest;

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

        public double BestFitnessSoFar => bestFitnessSoFar;

        public double NeighsBestFitnessSoFar => neighsBestFitnessSoFar;
        //public ReadOnlyCollection<double> NeighsBestPositionSoFar =>
        //    Array.AsReadOnly(neighsBestPositionSoFar);


        public Particle(int id, PSO pso)
        {
            this.id = id;
            this.pso = pso;
            nDim = pso.NDims;

            position = new double[nDim];
            velocity = new double[nDim];
            bestPositionSoFar = new double[nDim];
            neighsBestPositionSoFar = new double[nDim];

            switch (pso.GrpBest)
            {
                case GroupBest.Global:
                    groupBest = () => pso.BestSoFar.fitness;
                    break;
                case GroupBest.Local:
                    groupBest = () => neighsBestFitnessSoFar;
                    break;
            }

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
            fitness = pso.Function.Evaluate(position); // TODO Doesn't this count for PSO.TotalEvals?

            // TODO Hooks such as watershed

            // Set my own fitness as best fitness so far
            bestFitnessSoFar = fitness;

            // Set me as the best neighbor so far
            neighsBestFitnessSoFar = fitness;
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
                velocity[i] = pso.W(pso) * velocity[i]
                    + pso.C1(pso) * pso.Rng.NextDouble() * (bestPositionSoFar[i] - position[i])
                    + pso.C2(pso) * pso.Rng.NextDouble() * (groupBest() - position[i]);

                // Keep velocity in bounds
                if (velocity[i] > pso.VMax(pso)) velocity[i] = pso.VMax(pso);
                if (velocity[i] < -pso.VMax(pso)) velocity[i] = -pso.VMax(pso);

                // Update position
                position[i] = position[i] + velocity[i];

                // Keep position in bounds, stop particle if necessary
                if (position[i] > pso.XMax(pso))
                {
                    position[i] = pso.XMax(pso);
                    velocity[i] = 0;
                }
                else if (position[i] < pso.XMin(pso))
                {
                    position[i] = pso.XMin(pso);
                    velocity[i] = 0;
                }
            }

            // Obtain particle fitness for new position
            fitness = pso.Function.Evaluate(position);

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

using System;

namespace OpenPSO.Lib
{
    public class PSO
    {

        private int totalEvals = 0;
        private int bestSoFar;
        private Config cfg;
        private Particle[] particles;

        private Func<bool> updateStrategy;

        public event Action<PSO> EndOfIteration;

        public PSO(Config cfg)
        {
            totalEvals = 0;
            this.cfg = cfg;
        }

        public void UpdatePopData()
        {
            // TODO This method
        }

        /// <summary>
        /// Update position and velocity of all or some of the particles.
        /// </summary>
        /// <remarks>
        /// Client code will not usually call this function directly, unless
        /// more control is desired in the way the PSO algorithm advances.
        /// </remarks>
        /// <param name="iterations"></param>
        public void UpdateParticles(int iterations)
        {
            int evals = 0;

            // Cycle through particles
            foreach (Particle pCurr in particles)
            {
                // TODO Update or not to update according to SS-PSO

                // Cycle through neighbors
                foreach (Particle pNeigh in pCurr.Neighbors)
                {
                    // TODO If a neighbor particle is the worst particle, mark current particle for updating (SS-PSO only)

                    // Does the neighbor know of better fitness than current
                    // particle?
                    if (pNeigh.BestFitnessSoFar < pCurr.NeighsBestFitnessSoFar)
                    {
                        // If so, current particle will get that knowledge also
                        pCurr.UpdateBest(pNeigh);
                    }
                }

                // TODO Consider extra neighbors (Small World PSO)
                // In practice this should be a part of pCurr.Neighbors strategy

                // Update current particle?
                if (updateStrategy()) // TODO Connect this with SS-PSO
                {
                    pCurr.Update();
                    evals++;
                }

            }

            totalEvals += evals;
        }

        /// <summary>
        /// Execute a complete PSO run.
        /// </summary>
        public void Run()
        {
            int iterations = 0;
            int evaluations = 0;
            int critEvals = 0;

            // Keep going until maximum number of evaluations is reached
            do
            {

                // Update iteration count for current run
                iterations++;

                // Let particles know about best and worst fitness and determine
                // average fitness
                UpdatePopData();

                // Update all particles
                UpdateParticles(iterations);

                // Is the best so far below the stop criteria? If so did we
                // already saved the number of evaluations required to get below
                // the stop criteria?
                if (bestSoFar < cfg.criteria && critEvals == 0)
                {
                    // Keep the number of evaluations which attained the stop
                    // criteria
                    critEvals = evaluations;

                    // Stop current run if I'm not supposed to keep going
                    if (cfg.critKeepGoing) break;

                }

                // Call end-of-iteration events
                EndOfIteration?.Invoke(this);

            } while (evaluations < cfg.maxEvals);
        }

    }
}

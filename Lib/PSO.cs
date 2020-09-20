﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenPSO.Lib
{
    public class PSO
    {

        private Config cfg;
        private IList<Particle> particles;

        // Best fitness, position and particle ID so far
        private (double fitness, double[] position, Particle particle) bestSoFar;

        // Best fitness and particle ID at each iteration
        private (double fitness, Particle particle) bestCurr;
        // Worst fitness and particle ID at each iteration
        private (double fitness, Particle particle) worstCurr;

        // Average fitness at each iteration
        private double avgFitCurr;

        private Func<bool> updateStrategy;

        public event Action<PSO> PostIteration;
        public event Action<PSO> PostUpdatePopData;

        public (double fitness, ReadOnlyCollection<double> position)
            BestSoFar =>
                (bestSoFar.fitness, Array.AsReadOnly(bestSoFar.position));

        public int TotalEvals { get; private set; }

        public PSO(Config cfg)
        {
            TotalEvals = 0;
            this.cfg = cfg;

            // TODO A configurable update strategy
            updateStrategy = () => true; // For now, always update

            // Initialize list of particles
            particles = new List<Particle>(cfg.PopSize);

            // Initialize individual particles
            for (int i = 0; i < cfg.PopSize; i++)
            {
                Particle p = new Particle(i, cfg, n => bestSoFar.position[n]); // TODO This is the global best. Must also allow local best.
                particles.Add(p);
            }

            // TODO Topology
            cfg.topology.Init(particles);

            // Initialize bestSoFar as first particle
            bestSoFar = (particles[0].Fitness,
                particles[0].Position.ToArray(), particles[0]);

            // TODO This is probably not required here, since we're doing it
            // in UpdatePopData()
            // bestCurr = (particles[0].Fitness, particles[0]);
            // worstCurr = (particles[0].Fitness, particles[0]);

        }

        /// <summary>
        /// Update population data. Let particles know about particle with best
        /// and worst fitness in the population and calculate the average
        /// fitness in the population.
        /// </summary>
        /// <remarks>
        /// Client code will not usually call this function directly, unless
        /// more control is desired in the way the PSO algorithm advances.
        /// </remarks>
        public void UpdatePopData()
        {
            double sumFitness = 0;
            Particle pBest = particles[0];
            Particle pWorst = particles[0];

            foreach (Particle p in particles)
            {
                // Update worst in population
                if (p.Fitness > pWorst.Fitness) pWorst = p; // TODO Improve this for seeking max instead of min

                // Update best in population
                if (p.Fitness < pBest.Fitness) pBest = p; // TODO Improve this for seeking max instead of min

                // Ask particle to update knowledge of best fitnesses/positions
                // so far
                p.UpdateBestSoFar();

                // Update total fitness
                sumFitness += p.Fitness;
            }

            // Update worst fitness/particle for current iteration
            worstCurr = (pWorst.Fitness, pWorst);

            // Update best fitness/particle for current iteration
            bestCurr = (pBest.Fitness, pBest);

            // Updates best fitness/position so far in population (i.e. all
            // iterations so far)
            if (bestCurr.fitness < bestSoFar.fitness) // TODO Improve this for seeking max instead of min
            {
                bestSoFar.fitness = bestCurr.fitness;
                bestSoFar.position = bestCurr.particle.Position.ToArray();
                bestSoFar.particle = bestCurr.particle;
            }

            // Determine average fitness in the population
            avgFitCurr = sumFitness / particles.Count;

            // Call post-update population data events
            PostUpdatePopData?.Invoke(this);

        }

        /// <summary>
        /// Update position and velocity of all or some of the particles.
        /// </summary>
        /// <remarks>
        /// Client code will not usually call this function directly, unless
        /// more control is desired in the way the PSO algorithm advances.
        /// </remarks>
        public void UpdateParticles()
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
                    if (pNeigh.BestFitnessSoFar < pCurr.NeighsBestFitnessSoFar) // TODO Improve this for seeking max instead of min
                    {
                        // If so, current particle will get that knowledge also
                        pCurr.UpdateBestNeighbor(pNeigh);
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

            TotalEvals += evals;
        }

        /// <summary>
        /// Execute a complete PSO run.
        /// </summary>
        public void Run()
        {
            //int iterations = 0;
            int critEvals = 0; // TODO This should be instance variable
            TotalEvals = 0;

            // Keep going until maximum number of evaluations is reached
            do
            {
                // Update iteration count for current run
                //iterations++;

                // Let particles know about best and worst fitness and determine
                // average fitness
                UpdatePopData();

                // Update all particles
                UpdateParticles();

                // Is the best so far below the stop criteria? If so did we
                // already saved the number of evaluations required to get below
                // the stop criteria?
                if (bestSoFar.fitness < cfg.criteria && critEvals == 0) // TODO Improve this for seeking max instead of min
                {
                    // Keep the number of evaluations which attained the stop
                    // criteria
                    critEvals = TotalEvals;

                    // Stop current run if I'm not supposed to keep going
                    if (!cfg.critKeepGoing) break;

                }

                // Call end-of-iteration events
                PostIteration?.Invoke(this);

            } while (TotalEvals < cfg.maxEvals);
        }

    }
}
using System;
using System.Collections.Generic;

namespace PSO
{
    public class Particle
    {
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

        // Best position so far for this particle
        public double PBest(int i) => pBest[i];

        // Best global/local position so far
        public double GBest(int i) => gBest(i);

        public IList<double> Position => position;
        public IList<double> Velocity => velocity;
        public int NDim => position.Length;

    }
}

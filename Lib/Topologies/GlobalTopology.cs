using System;
using System.Linq;
using System.Collections.Generic;

namespace OpenPSO.Lib.Topologies
{
    public class GlobalTopology : ITopology
    {
        private int popSize;

        // TODO Probably topology properties will have to have an attribute
        // denoting they are a configurable parameter via reflection
        public int PopSize {
            get => popSize;
            private set
            {
                if (value < 2)
                    throw new ArgumentOutOfRangeException(
                        "Population size must be 2 or more");
                popSize = value;
            }
        }

        public IEnumerable<Particle> Particles { get; private set; }

        public GlobalTopology(int popSize)
        {
            PopSize = popSize;
        }

        /// <summary>
        /// Empty constructor. When used, topology properties should be setup
        /// via reflection.
        /// </summary>
        public GlobalTopology() {}

        public void Init(IEnumerable<Particle> particles)
        {
            if ((particles?.Count() ?? 0) != popSize)
                throw new ArgumentException("Invalid number of particles");
            Particles = particles;
        }

        public IEnumerable<Particle> GetNeighbors(int pid)
        {
            if (Particles is null)
                throw new InvalidOperationException("Topology not initialized");
            if (pid >= popSize)
                throw new ArgumentOutOfRangeException("Invalid particle ID");

            foreach (Particle p in Particles)
            {
                if (p.id != pid) yield return p;
            }
        }
    }
}

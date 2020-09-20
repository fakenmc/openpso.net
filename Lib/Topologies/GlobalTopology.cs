using System;
using System.Collections.Generic;

namespace OpenPSO.Lib.Topologies
{
    public class GlobalTopology : ITopology
    {
        private IEnumerable<Particle> particles;
        public int PopSize { get; } = 50;

        public void Init(IEnumerable<Particle> particles)
        {
            this.particles = particles;
        }

        public IEnumerable<Particle> GetNeighbors(int pid)
        {
            if (particles is null)
                throw new InvalidOperationException("Topology not initialized");

            foreach (Particle p in particles)
            {
                if (p.id != pid) yield return p;
            }
        }
    }
}

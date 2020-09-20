using System.Collections.Generic;

namespace OpenPSO.Lib
{
    public interface ITopology
    {
        int PopSize { get; }

        void Init(IEnumerable<Particle> particles);
        IEnumerable<Particle> GetNeighbors(int pid);

    }
}

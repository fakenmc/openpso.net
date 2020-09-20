using System.Collections.Generic;

namespace OpenPSO.Lib
{
    public interface ITopology
    {
        int PopSize { get; }
        IEnumerable<Particle> GetNeighbors(int pid);

    }
}

using System;
using OpenPSO.Functions;
using OpenPSO.Lib.Topologies;

namespace OpenPSO.Lib
{
    public class Config
    {
        // Inertia weight
        public double W { get; }

        // Acceleration coefficients, used to tune the relative influence of
        // each term of the formula
        public double C1 { get; }
        public double C2 { get; }

        // TODO Makes sense to also have XMin?
        public double XMax { get; }
        public double VMax { get; }

        public int PopSize => topology.PopSize;

        public Random Rng { get; }

        public double InitXMin { get; }
        public double InitXMax { get; }

        public readonly IFunction function;
        public readonly int nDims;

        public readonly int maxEvals;

        public readonly double criteria;

        public readonly bool critKeepGoing;

        public readonly ITopology topology;

        public Config()
        {
            Rng = new Random();

            W = 0.729844;
            C1 = 1.494;
            C2 = 1.494;

            XMax = 10;
            VMax = 10;
            InitXMin = 2.56;
            InitXMax = 5.12;

            function = new Rastrigin();
            nDims = 10;

            maxEvals = 980_000;
            criteria = 10;
            critKeepGoing = false;

            //topology = new VonNeumannGridTopology(7, 7);
            topology = new MooreGridTopology(7, 7);
            //topology = new GlobalTopology(50);
        }

    }
}

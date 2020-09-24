using System;
using OpenPSO.Lib;
using OpenPSO.Lib.Topologies;
using OpenPSO.Functions;

namespace OpenPSO.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 30; i++)
            {
                PSO pso = new PSO(
                    p => true,     // Update strategy
                    p => 0.729844, // inertia weight
                    p => 1.494,    // c1
                    p => 1.494,    // c2
                    p => -0.5,      // xMin
                    p => 0.5,       // xMax
                    p => 100,       // vMax
                    -0.5,          // Initial xMin
                    0.2,          // Initial xMax
                    new Weierstrass(),  // Function
                    10,            // Number of dimensions in function
                    980_000,       // Max. evaluations
                    0.01,            // Criterion
                    false,         // Keep going after criteria's been met?
                    new StaticRingTopology(49, 5) //new GlobalTopology(49)  // Topology
                );
                // new VonNeumannGridTopology(7, 7);
                // new MooreGridTopology(7, 7);
                //pso.PostIteration += PrintBestSoFar;
                pso.Run();
                Console.WriteLine("{0}\t{1}\t{2}", pso.TotalEvals, pso.BestSoFar.fitness, pso.AvgFitCurr);
            }
        }

        private static void PrintBestSoFar(PSO pso)
        {
            Console.Write(
                $"Evals={pso.TotalEvals} | BestFit={pso.BestSoFar.fitness:f4}");
            Console.Write($" | AvgFit={pso.AvgFitCurr:f4}");
            // Console.Write(" @ (");
            // for (int i = 0; i < pso.BestSoFar.position.Count; i++)
            //     Console.Write($"{pso.BestSoFar.position[i]:f2}, ");
            //Console.Write(")");
            Console.WriteLine();
        }
    }
}

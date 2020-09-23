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
            PSO pso = new PSO(
                p => true,     // Update strategy
                p => 0.729844, // inertia weight
                p => 1.494,    // c1
                p => 1.494,    // c2
                p => -10,      // xMin
                p => 10,       // xMax
                p => 10,       // vMax
                GroupBest.Global, // Local or global best?
                2.56,          // Initial xMin
                5.12,          // Initial xMax
                new Rastrigin(),  // Function
                10,            // Number of dimensions in function
                980,       // Max. evaluations
                10,            // Criterion
                false,         // Keep going after criteria's been met?
                new VonNeumannGridTopology(7, 7) //new GlobalTopology(50)  // Topology
            );
            // new VonNeumannGridTopology(7, 7);
            // new MooreGridTopology(7, 7);

            pso.PostIteration += PrintBestSoFar;
            pso.Run();
        }

        private static void PrintBestSoFar(PSO pso)
        {
            Console.Write(
                $"Evals={pso.TotalEvals} |Fitness={pso.BestSoFar.fitness} @ (");
            for (int i = 0; i < pso.BestSoFar.position.Count; i++)
                Console.Write($"{pso.BestSoFar.position[i]:f2}, ");
            Console.WriteLine(")");
        }
    }
}

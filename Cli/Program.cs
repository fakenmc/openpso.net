using System;
using OpenPSO.Lib;

namespace OpenPSO.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Config cfg = new Config();
            PSO pso = new PSO(cfg);
            pso.Run();
            Console.Write($"Fitness: {pso.BestSoFar.fitness} @ (");
            for (int i = 0; i < pso.BestSoFar.position.Count; i++)
                Console.Write($"{pso.BestSoFar.position[i]:f2}, ");
            Console.WriteLine(")");
        }
    }
}

using System;
using System.Collections.Generic;
using OpenPSO.Lib;

namespace OpenPSO.Functions
{
    /// <summary>
    /// Rastrigin function.
    /// </summary>
    /// <remarks>
    /// Characteristics:
    ///
    /// * $d$-dimensional.
    /// * Search domain: $-5.12 \leq x_i \leq 5.12,\: \forall i=1,\dots,d$
    /// * Minimum: $f(0,\dots,0)=0$
    ///
    /// Optimization setup suggestions:
    ///
    /// * Search domain: $-10 \leq x_i \leq 10,\: \forall i=1,\dots,d$
    /// * Initialization domain: $2.56 \leq x_i \leq 5.12,\: \forall i=1,\dots,d$
    /// * Stop criterion: 100
    ///
    /// References:
    ///
    /// * https://www.sfu.ca/~ssurjano/rastr.html
    /// * https://en.wikipedia.org/wiki/Rastrigin_function
    /// * https://peerj.com/articles/cs-202/
    /// </remarks>
    public class Rastrigin : IFunction
    {
        public double Evaluate(IList<double> position) => Function(position);

        public static double Function(IList<double> position)
        {
            double fitness = 0.0;

            for (int i = 0; i < position.Count; i++)
            {
                fitness += position[i] * position[i]
                    - 10 * Math.Cos(2.0 * Math.PI * position[i]) + 10;
            }
            return fitness;
        }

    }
}

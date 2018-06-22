using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineSharpLibrary;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            NeuralNetwork nn = new NeuralNetwork(2, 3, 1);
            var guess = nn.Predict(new double[] { 0.7, 0.3 });
            Console.WriteLine(guess);

        }
    }
}

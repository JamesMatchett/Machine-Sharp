using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSharpLibrary
{
    class TestProvisional
    {
        public static void RunTest()
        {
            //create neural net with 728 inputs, 2 hidden layers of 16 neurons each and output layer of 10 neurons
            int[] Hiddenlayers = new int[] { 16, 16 };
            ProvisionalNeuralNetwork PNN = new ProvisionalNeuralNetwork(2, Hiddenlayers, 728, 10);
            Console.ReadLine();
        }
    }
}

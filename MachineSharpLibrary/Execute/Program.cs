using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineSharpLibrary;

namespace Execute
{
    class Program
    {
        static void Main(string[] args)
        {
            var LMM = new LMMCNet(5, 2, new int[] { 5, 5 }, 5, true);

            foreach(double d in LMM.Net[0][0].WeightsOut)
            {
                Console.WriteLine(d);
            }
            LMM.RemoveNeuron(1,LMM.Net[1].Count-1);
            Console.WriteLine("----------");
            foreach (double d in LMM.Net[0][0].WeightsOut)
            {
                Console.WriteLine(d);
            }
            Console.WriteLine("Added");
            Console.ReadLine();
        }

        public class Helper
        {
            public static double[] GetInputs(int NeuronsPerLayer)
            {
                double[] ReturnArray = new double[NeuronsPerLayer];
                for (int i = 0; i < NeuronsPerLayer; i++)
                {
                    ReturnArray[i] = 1;
                }

                return ReturnArray;
            }

            public static int[] GetHLayerArray(int NHiddenLayers, int NeuronsPerLayer)
            {
                int[] ReturnArray = new int[NHiddenLayers];
                for (int j = 0; j < NHiddenLayers; j++)
                {
                    ReturnArray[j] = NeuronsPerLayer;
                }
                return ReturnArray;
            }

            public static int[] GetHLayerRand(int NHiddenLayers, Random random)
            {
                int[] ReturnArray = new int[NHiddenLayers];
                for (int j = 0; j < NHiddenLayers; j++)
                {
                    ReturnArray[j] = random.Next(1, 10);
                }
                return ReturnArray;
            }

            //A "true" neural net is one with varying neurons between layers and random number of layers & weights
            public static LMMCNet GetTrueNeuralNet()
            {
                Random random = new Random();
                int NHiddenLayers = random.Next(1, 5);
                int NInputs = random.Next(1, 10);
                int NOutputs = random.Next(1, 10);
                int[] NeuronsPerHiddenLayer = GetHLayerRand(NHiddenLayers, random);
                return new LMMCNet(NInputs, NHiddenLayers, NeuronsPerHiddenLayer, NOutputs, true);
            }

            public static double Squish(double input)
            {
                return (1 / (1 + Math.Exp(-input)));
            }
        }
    }
}

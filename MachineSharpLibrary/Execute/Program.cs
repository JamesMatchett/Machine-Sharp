using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineSharpLibrary;
using System.Net;
using System.IO;

namespace Execute
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Mnist> TrainingList= new List<Mnist>();

            StreamReader sw = new StreamReader(@"E:\Music\training.txt");
            
            int index = 0;
            string label = "";
            double[] arr = new double[28 * 28];
            while (!sw.EndOfStream)
            {
                if (index == 0)
                {
                    int tempint = (char)sw.Read() - 48;
                    label = tempint.ToString();
                    index++;
                }
                else
                {
                    arr[index - 1] = (char)sw.Read()-48;
                    index++;
                }

                if(index == 28 * 28)
                {
                    index = 0;
                    Mnist mn = new Mnist(arr, label);
                    TrainingList.Add(mn);
                    arr = new double[28 * 28];
                    label = "";
                }
            }


            Console.ReadLine();
            
            var LMM = new LMMCNet(5, 2, new int[] { 5, 5 }, 5, true);

            LMM.Train(Helper.GetInputs(LMM.NumberOfInputs), Helper.GetInputs(LMM.NumberOfOutputs));


            Console.ReadLine();
        }

        public class Mnist
        {
            double[] Data { get; set; }
            string Label { get; set; }
           
            public Mnist(double[] data, string label)
            {
                Data = data;
                Label = label;
            }
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

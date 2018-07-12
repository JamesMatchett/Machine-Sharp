using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineSharpLibrary;
using System.Net;
using System.IO;
using System.Drawing;

namespace Execute
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Parsing data -------------------");
            List<Mnist> TrainingList= new List<Mnist>();
            StreamReader sw = new StreamReader(@"E:\Music\training.txt");

            List<string> charstr = new List<string>();
            string build = "";
            int index = -1;
            int label = 0;
            double[] data = new double[28 * 28];
            while (!sw.EndOfStream)
            {
                int next = sw.Read() - 48;
                if(next == -4)
                {
                    if(index == -1)
                    {
                        label = Convert.ToInt32(build);
                        index++;
                    }
                    else
                    {
                        data[index] = Convert.ToInt32(build);
                        index++;
                    }

                    if(index == (28 * 28)-1)
                    {
                        TrainingList.Add(new Mnist(data, label));
                        index = -1;
                        data = new double[28 * 28];
                        build = "";
                        sw.Read();
                        sw.Read();
                    }

                    build = "";
                }
                else
                {
                    //check for line breaks & spaces
                    if (build.Contains(@"\"))
                    {
                        build = build.Remove(build.IndexOf(@"\"));
                    }
                    if (build.Contains(@"n"))
                    {
                        build = build.Remove(build.IndexOf(@"n"));
                    }
                    build += next;
                }

            }


            Random random = new Random();
            //choose random object
            for (int i = 0; i< 50; i++)
            {
               
                Mnist mn = TrainingList[random.Next(0, TrainingList.Count - 1)];
                Bitmap bm = new Bitmap(28, 28);

                index = 0;
                for(int x = 0; x<28; x++)
                {
                    for (int y = 0; y < 28; y++)
                    {
                        int bright = Convert.ToInt32(mn.Data[index]);
                        bm.SetPixel(y, x, Color.FromArgb(255, bright, bright, bright));
                        index++;
                    }
                }

                string filename = @"E:\Music\Imagetest" + i+" "+mn.Label+".png";
                bm.Save(filename);
                
            }

            Console.WriteLine("Files output");
            Console.ReadLine();
           

            int isn = 0;
            Console.WriteLine("Checking data -------------------");
            foreach(Mnist mn in TrainingList)
            {
                int value = Convert.ToInt32(mn.Label);
                if (value > 9 || value < 0)
                {
                    Console.WriteLine("error at {0}",isn);
                    
                    Console.ReadLine();
                }
                isn++;
            }

            var LMM = new LMMCNet(28 * 28, 1, new int[] { 5 }, 10, false);

            Console.WriteLine("Training-------------------");
            int count = 0;
            int total = TrainingList.Count;
            foreach(Mnist mn in TrainingList)
            {
                count++;
                Console.WriteLine("Executing {0} of {1}, cycle",count, total);
   

                var ExpectedOut = new double[10];
                for(int i =0; i < 10; i++)
                {
                    if (i == Convert.ToInt32(mn.Label)){
                        ExpectedOut[i] = 1;
                    }
                    else
                    {
                        ExpectedOut[i] = 0;
                    }
                }

                
                    LMM.Train(mn.Data, ExpectedOut);
                
            }

            int totalSuccesses = 0;
            int totalGuesses = 0;

            Console.WriteLine("Guessing -----------------------");
            foreach(Mnist mn in TrainingList)
            {
                totalGuesses++;
                Console.WriteLine("On guess {0}", totalGuesses);
                Console.WriteLine("{0} success from {1} guesses",totalSuccesses,totalGuesses);

                var output = LMM.Predict(mn.Data);
                int HighestIndex = 0;
                double HighestValue = 0;
                for(int i =0; i<5; i++)
                {
                    if (output[i] > HighestValue)
                    {
                        HighestIndex = i;
                        HighestValue = output[i];
                    }
                }

                if(HighestIndex == (mn.Label))
                {
                    totalSuccesses++;

                    if(mn.Label != 0)
                    {
                        Console.WriteLine("Not a 0");
                        Console.ReadLine();
                    }
                }

            }


            Console.ReadLine();
        }

        public class Mnist
        {
          public  double[] Data { get; set; }
           public int Label { get; set; }
           
            public Mnist(double[] data, int label)
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

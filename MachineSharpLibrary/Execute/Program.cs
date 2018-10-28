using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineSharpLibrary;
using System.Net;
using System.IO;
using System.Drawing;
using System.Threading;

namespace Execute
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
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

            sw.Close();


            /*
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
           


            var tempList = new List<Mnist>();
            int count2 = 0;
            foreach(Mnist nn in TrainingList)
            {
                tempList.Add(nn);
                if(count2 > 1000)
                {
                    break;
                }
                count2++;
            }

            TrainingList = tempList;

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



            var LMM = new LMMCNet(28 * 28, 1, new int[] { 20 }, 10, true);

            Console.WriteLine("Training-------------------");
            int count = 0;
            int total = TrainingList.Count;
            foreach(Mnist mn in TrainingList)
            {
                count++;
                Console.WriteLine("Executing {0} of {1}",count, total);
   

                var ExpectedOut = new double[10];
                ExpectedOut[mn.Label] = 1;

                for (int i = 0; i <= mn.Data.GetUpperBound(0); i++)
                {
                    mn.Data[i] = mn.Data[i] / 255;
                }
                
                

                LMM.Train(mn.Data,ExpectedOut);
                
            }

            int totalSuccesses = 0;
            int totalGuesses = 0;
            int secondGuesses = 0;
            Console.WriteLine("Guessing -----------------------");
            foreach(Mnist mn in TrainingList)
            {
                totalGuesses++;
                Console.WriteLine("On guess {0}", totalGuesses);
                Console.WriteLine("{0} success from {1} guesses",totalSuccesses,totalGuesses);
                Console.WriteLine("{0} first successes and {1} second guesses",totalSuccesses, secondGuesses);

                var output = LMM.Predict(mn.Data);
                
                Console.WriteLine("-----------------");
                int HighestIndex = 0;
                double HighestValue = 0;
                int secondIndex = 0;
                double secondValue = 0;
                for(int i =0; i<=output.GetUpperBound(0); i++)
                {
                    if (output[i] > HighestValue)
                    {
                        secondIndex = HighestIndex;
                        secondValue = HighestValue;
                        HighestIndex = i;
                        HighestValue = output[i];
                    }
                }

                Console.WriteLine("Array is");
                int coun = 0;
                foreach(double d in output)
                {
                    Console.WriteLine("{0} = {1}",coun,d);
                    coun++;
                }
                Console.WriteLine("");
                Console.WriteLine("Highest value is {0}",HighestValue);
                Console.WriteLine("Index of highest is {0}",HighestIndex);
                Console.WriteLine("Label is {0}",mn.Label);

                if(mn.Label == HighestIndex)
                {
                    Console.WriteLine("Is matching");
                } else if(mn.Label == secondIndex)
                {
                    secondGuesses++;
                }
                else
                {
                    Console.WriteLine("Is not matching");
                }

                
             

                if(HighestIndex == (mn.Label))
                {
                    totalSuccesses++;
                }

            }

        */
            //begin XOR problem
            //4 inputs of 1 or so
            // 0 0 = 0
            // 0 1 = 1
            // 1 0 = 1
            // 1 1 = 0

            int numberOfTests = 500000;
            var testList = new List<Xor>();
            Random rnd = new Random();
            for (int i = 0; i < numberOfTests;i++)
            {
                testList.Add(new Xor(rnd));
            }

            LMMCNet lMMCNet = new LMMCNet(2, 3, new int[] { 10, 10, 2 }, 1,  rnd);
            LMMCNet net = new LMMCNet(2, 3, new int[] { 10, 10, 2 }, 1,  rnd);

            foreach (Xor x in testList)
            {
                lMMCNet.Train(new double[] { x.a, x.b }, x.Expected);
            }

            int worked = 0;
            int failed = 0;
            double result = 0;
            Random random = new Random();
            int count = 0;
            while (count < 500000)
            {
                Xor x = new Xor(random);

                Console.WriteLine("Running");
                lMMCNet.Train(new double[] { x.a, x.b }, x.Expected);
                result = lMMCNet.Net[lMMCNet.Net.Count() - 1][0].OutValue;

                if (Math.Round(result) == x.Expected[0])
                {
                    worked++;
                }
                else
                {
                    failed++;
                }
                
                //Console.WriteLine("Result is {0}, Expected was {1}",result, x.Expected.First());
                //Console.WriteLine("{0} sucesses, {1} fails",worked,failed);


                count++;
                //Console.ReadLine();
            }


            Console.WriteLine("{0} sucesses, {1} fails", worked, failed);

            Console.WriteLine("Compiled successfully");
            Thread.Sleep(5000);
            Console.ReadLine();
        }



        public struct Xor
        {
            public int a;
            public int b;
            public double[] Expected;
            public Xor(Random random)
            {
                a = random.Next(0, 2);
                b = random.Next(0, 2);
                if(a == 0 && b == 1 || a==1 && b== 0)
                {
                    Expected = new double[] { 1 };
                }
                else
                {
                    Expected = new double[] { -1 };
                }
            }
        }


        /*
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

*/
    }
}

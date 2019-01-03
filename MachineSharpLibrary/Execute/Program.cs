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

        static void MNISTBinaryNet()
        {
            Console.WriteLine("Parsing data -------------------");
            List<Mnist> TrainingList = new List<Mnist>();
            StreamReader sw = new StreamReader(@"E:\Music\training.txt");

            List<string> charstr = new List<string>();
            string build = "";
            int index = -1;
            int label = 0;
            double[] data = new double[28 * 28];
            while (!sw.EndOfStream)
            {
                int next = sw.Read() - 48;
                if (next == -4)
                {
                    if (index == -1)
                    {
                        label = Convert.ToInt32(build);
                        index++;
                    }
                    else
                    {
                        data[index] = Convert.ToInt32(build);
                        index++;
                    }

                    if (index == (28 * 28) - 1)
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



            Random random = new Random();
            //choose random object
            for (int i = 0; i < 50; i++)
            {

                Mnist mn = TrainingList[random.Next(0, TrainingList.Count - 1)];
                Bitmap bm1 = new Bitmap(28, 28);

                index = 0;
                for (int x = 0; x < 28; x++)
                {
                    for (int y = 0; y < 28; y++)
                    {
                        int bright = Convert.ToInt32(mn.Data[index]);
                        bm1.SetPixel(y, x, Color.FromArgb(255, bright, bright, bright));
                        index++;
                    }
                }

                string filename = @"E:\Music\Imagetest" + i + " " + mn.Label + ".png";
                bm1.Save(filename);

            }


            Console.WriteLine("Files output | press enter to continue");
            //Console.ReadLine();



            var tempList = new List<Mnist>();
            int count2 = 0;
            foreach (Mnist nn in TrainingList)
            {
                tempList.Add(nn);
                if (count2 > 20000)
                {
                    break;
                }
                count2++;
            }

            TrainingList = tempList;

            int isn = 0;
            Console.WriteLine("Checking data -------------------");
            foreach (Mnist mn in TrainingList)
            {
                int value = Convert.ToInt32(mn.Label);
                if (value > 9 || value < 0)
                {
                    Console.WriteLine("error at {0}", isn);

                    Console.ReadLine();
                }
                isn++;
            }


            var fac = new BinaryNetFactory(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 28 * 28, new int[] { 10 }, new Random());
            

            Console.WriteLine("Training-------------------");
            int count = 0;
            int total = TrainingList.Count;
            foreach (Mnist mn in TrainingList)
            {
                count++;
                Console.WriteLine("Executing {0} of {1}", count, total);

                var ExpectedOut = new double[10];
                ExpectedOut[mn.Label] = 1;

                for (int i = 0; i <= mn.Data.GetUpperBound(0); i++)
                {
                    mn.Data[i] = mn.Data[i] / 255;
                }



                fac.Train(mn.Data, mn.Label);

            }

            //load chris' handwriting
            Bitmap bm = new Bitmap(@"E:\music\ChrisWriting.png");
            double[] dataArray = new double[28 * 28];
            int dataIndex = 0;
            for(int x = 0; x < 28; x++)
            {
                for(int y = 0; y < 28; y++)
                {
                    dataArray[dataIndex] = bm.GetPixel(x, y).GetBrightness() != 0 ? bm.GetPixel(x,y).GetBrightness() : 0;
                    dataIndex++;
                }
            }

          
            var result = fac.Predict(dataArray);
            Console.WriteLine("Result of Chris' handwriting is {0}",result[0]);
            Console.ReadLine();

            int totalSuccesses = 0;
            int totalGuesses = 0;
            int secondGuesses = 0;
            Console.WriteLine("Guessing -----------------------");
            foreach (Mnist mn in TrainingList)
            {
                totalGuesses++;
                Console.WriteLine("On guess {0}", totalGuesses);
                Console.WriteLine("{0} success from {1} guesses", totalSuccesses, totalGuesses);
                Console.WriteLine("{0} first successes and {1} second guesses", totalSuccesses, secondGuesses);

                var output = fac.Predict(mn.Data);

                Console.WriteLine("-----------------");
                

                if (mn.Label == output[0])
                {
                    Console.WriteLine("Is matching");
                    totalSuccesses++;
                }
              
                else
                {
                    Console.WriteLine("Is not matching");
                }
               
            }

            Console.WriteLine("{0} successes, {1} failures", totalSuccesses, totalGuesses - totalSuccesses);
            Console.ReadLine();

        }

        static void MNIST()
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

                sw.Close();


                
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


                Console.WriteLine("Files output | press enter to continue");
                Console.ReadLine();



                var tempList = new List<Mnist>();
                int count2 = 0;
                foreach(Mnist nn in TrainingList)
                {
                    tempList.Add(nn);
                    if(count2 > 15000)
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


           
                var LMM = new LMMCNet(28 * 28, 1, new int[] { 10 }, 10, new Random());

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
                    Hessian(LMM.Net);

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

            Console.WriteLine("{0} successes, {1} failures",totalSuccesses,totalGuesses-totalSuccesses);
            Console.ReadLine();

    


        }

        static void XOR() 
        {
            //begin XOR problem
            //4 inputs of 1 or so
            // 0 0 = 0
            // 0 1 = 1
            // 1 0 = 1
            // 1 1 = 0

            Console.WriteLine("Executing");

            int numberOfTests = 500000;
            var testList = new List<Xor>();
            Random rnd = new Random();
            for (int i = 0; i < numberOfTests; i++)
            {
                testList.Add(new Xor(rnd));
            }

            LMMCNet lMMCNet = new LMMCNet(2, 3, new int[] { 10, 10, 2 }, 1, rnd);
            LMMCNet net = new LMMCNet(2, 3, new int[] { 10, 10, 2 }, 1, rnd);

            foreach (Xor x in testList)
            {
                lMMCNet.Train(new double[] { x.a, x.b }, x.Expected);
            }

            int worked = 0;
            int failed = 0;
            double result = 0;
            Random random = new Random();
            int count = 0;
            while (count < 5000000)
            {
                Xor x = new Xor(random);
                Console.WriteLine(count);
                lMMCNet.Train(new double[] { x.a, x.b }, x.Expected);
                result = lMMCNet.Net[lMMCNet.Net.Count() - 1][0].OutValue;
                Console.WriteLine("result is {0}, expected is {1}", (result), x.Expected[0]);

                if (result.ToString() == "0.8" && x.Expected[0].ToString() == "0.8" || result.ToString() == "0.2" && x.Expected[0].ToString() == "0.2")
                {
                    worked++;
                }
                else
                {
                    failed++;
                }

                Hessian(lMMCNet.Net);
                count++;
            }

            Console.WriteLine("{0} sucesses, {1} fails", worked, failed);
            Console.WriteLine("Compiled successfully");
            Thread.Sleep(5000);
            Console.ReadLine();
        }

        static void BinaryNetXOR()
        {
            //begin XOR problem
            //4 inputs of 1 or so
            // 0 0 = 0
            // 0 1 = 1
            // 1 0 = 1
            // 1 1 = 0

            Console.WriteLine("Executing");

            int numberOfTests = 500000;
            var testList = new List<Xor>();
            Random rnd = new Random();
            for (int i = 0; i < numberOfTests; i++)
            {
                testList.Add(new Xor(rnd));
            }

            var factory = new BinaryNetFactory(new double[] { 0.1, 0.9 },
            2, new int[] { 10, 10, 2 }, rnd);

            foreach (Xor x in testList)
            {
                factory.Train(new double[] { x.a, x.b }, x.Expected[0]);
            }

            int worked = 0;
            int failed = 0;
            double result = 0;
            Random random = new Random();
            int count = 0;
            while (count < 500000)
            {
                Xor x = new Xor(random);
                Console.WriteLine(count);
                factory.Train(new double[] { x.a, x.b }, x.Expected[0]);
                result = factory.Predict(new double[] { x.a, x.b })[0];
                Console.WriteLine("result is {0}, expected is {1}", (result), x.Expected[0]);

                if (result.ToString() == x.Expected[0].ToString())
                {
                    worked++;
                }
                else
                {
                    failed++;
                }

                count++;
            }

            Console.WriteLine("{0} sucesses, {1} fails", worked, failed);
            Console.WriteLine("Compiled successfully");
            Thread.Sleep(5000);
            Console.ReadLine();
        }

        static void Hessian(Net net)
        {
            //40 px wide per layer (10 px actual neuron) 
            int Width = (net.NumberOfLayers * 40) + 40;

            //20 px height per neuron  (5px seperation each side, 10px actual neuron)
            int maxNeuronsInALayer = 0;
            for(int i = 0; i < net.NumberOfLayers; i++)
            {
                maxNeuronsInALayer = (net[i].Count > maxNeuronsInALayer) ? net[i].Count : maxNeuronsInALayer;
            }
            int Height = ((maxNeuronsInALayer) * 20) + 40;

            Bitmap bitmap = new Bitmap(Width, Height);
            setBackground(Width, Height, ref bitmap);

            //draw layer by layer
            int x = 0, y = 0;
            for(int i =0; i<net.NumberOfLayers; i++)
            {
                for(int j = 0; j < net[i].Count; j++)
                {
                    y += 5;
                    drawNeuron(x, y, ref bitmap, net[i, j].OutValue);
                    y += 10;
                    y += 5;
                }
            
                x += 40;
                y = 0;
            }

            //save to "E://music/BitmapTest@
            bitmap.Save(@"E:\music\Bitmaptest.png");
            //break point line for evalutation
            x = 0;
            

        }

        static void setBackground(int width, int height, ref Bitmap bitmap)
        {
            Color color = Color.FromArgb(0, 0, 0);
            for(int x=0; x<width; x++)
            {
                for(int y=0; y < height; y++)
                {
                    bitmap.SetPixel(x, y, color);
                }
            }
        }

        static void drawNeuron(int x, int y, ref Bitmap bitmap, double activation)
        {
            int colour = (int)Math.Round(activation * 255);
            colour += 20;

            colour = colour > 255 ? 255 : colour;

            Color color = Color.FromArgb(colour, colour, colour);
            for(int innerX = 0; innerX < 10; innerX++)
            {
                for(int innerY = 0; innerY < 10; innerY++)
                {
                    bitmap.SetPixel(innerX + x, innerY + y, color);
                }
            }     
            
            

            
        }

        static void Main(string[] args)
        {
            //MNISTBinaryNet();
            MNIST();
            //XOR();
            //BinaryNetXOR();
           
        }

        public class Mnist
        {
            public double[] Data { get; set; }
            public int Label { get; set; }

            public Mnist(double[] data, int label)
            {
                Data = data;
                Label = label;
            }
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
                if (a == 0 && b == 1 || a == 1 && b == 0)
                {
                    Expected = new double[] { 0.9 };
                }
                else
                {
                    Expected = new double[] { 0.1 };
                }
            }
        }
    }
}

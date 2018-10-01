using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MachineSharpLibrary;
using System.Collections.Generic;

namespace MachineSharpTestCases
{
    [TestClass]
    public class NetGenerationTests
    {
        [TestCategory("All 0 Net generation tests")]
        
        //Follow procedure Test_Inputs_ExpectedOutputs
        [TestMethod]
        public void NetGeneration_AllWeights0_AllOutputsEqualPointFive_NoHiddenLayers()
        {
            //create a neural net with all weights are 0's, all inputs are 1 expected output should be all 0.5's output.
            //test for number of neurons per layer 1~10 and 0 hidden layers
            for (int NeuronsPerLayer = 0; NeuronsPerLayer <= 10; NeuronsPerLayer++)
            {
                LMMCNet LMM = new LMMCNet(NeuronsPerLayer, 0, new int[] { }, NeuronsPerLayer, false);
                var output = LMM.Predict(Helper.GetInputs(NeuronsPerLayer));
                foreach (double d in output)
                {
                    Assert.AreEqual(d, 0.5);
                }   
            }
        }

        [TestMethod]
        public void NetGeneration_AllWeights0_AllOutputsEqualPointFive_1HiddenLayer()
        {
            for (int NeuronsPerLayer = 1; NeuronsPerLayer <= 10; NeuronsPerLayer++)
            {
                var LMM = new LMMCNet(NeuronsPerLayer, 1, new int[] {5}, NeuronsPerLayer, false);
                var output = LMM.Predict(Helper.GetInputs(NeuronsPerLayer));
                foreach (double d in output)
                {
                    Assert.AreEqual(d, 0.5);
                }
            }
        }

        [TestMethod]
        public void NetGeneration_AllWeights0_AllOutputsEqualPointFive_MultipleHiddenLayers()
        {
            for (int NHiddenLayers = 1; NHiddenLayers < 10; NHiddenLayers++)
            {

                for (int NeuronsPerLayer = 1; NeuronsPerLayer <= 10; NeuronsPerLayer++)
                {
                    var NeuronsPerHiddenLayer = Helper.GetHLayerArray(NHiddenLayers, NeuronsPerLayer);
                    var LMM = new LMMCNet(NeuronsPerLayer, NHiddenLayers, NeuronsPerHiddenLayer, NeuronsPerLayer, false);
                    var output = LMM.Predict(Helper.GetInputs(NeuronsPerLayer));
                    foreach (double d in output)
                    {
                        Assert.AreEqual(d, 0.5);
                    }
                }
            }
        }

        [TestCategory("Actual weight generation tests")]
        [TestMethod]
        //Put in random weights and inputs, check all outputs are between 1 and minus 1
        public void NetGeneration_RandomWeights_AllOutputsBetweenMinus1And1()
        {
            for (int NHiddenLayers = 0; NHiddenLayers <= 10; NHiddenLayers++)
            {
                for (int NeuronsPerLayer = 1; NeuronsPerLayer <= 10; NeuronsPerLayer++)
                {
                    var NeuronsPerHiddenLayer = Helper.GetHLayerArray(NHiddenLayers, NeuronsPerLayer);
                    var LMM = new LMMCNet(NeuronsPerLayer, NHiddenLayers, NeuronsPerHiddenLayer, NeuronsPerLayer, true);
                    var output = LMM.Predict(Helper.GetInputs(NeuronsPerLayer));
                    Assert.AreEqual(output.GetUpperBound(0) + 1, NeuronsPerLayer);
                    foreach (double d in output)
                    {
                        Assert.IsTrue(d <= 1 && d >= -1);
                    }
                }

            }
        }

        //final test, varying number of neurons per layer between each layer
        [TestMethod]
        public void NetGeneration_RandomWeights_VaryingNeuronLayers()
        {
            var LMM = Helper.GetTrueNeuralNet();
            var output = LMM.Predict(Helper.GetInputs(LMM.Net[0].Count));
            //Assert that the number of outputs is equal to the number of neurons in the output layer
            Assert.AreEqual(output.GetUpperBound(0) + 1, LMM.Net[LMM.Net.Count-1].Count);
            foreach (double d in output)
            {
                Assert.IsTrue(d <= 1 && d >= -1);
            }
        }
    }

    [TestClass]
    public class InternalMechanismTests
    {
        [TestCategory("Internal mechanism tests")]
        
       
        [TestMethod]
        public void InternalMechanism_CheckSumMethod_NoHiddenLayer()
        {
            Random random = new Random();
            int NeuronsPerLayer = random.Next(5, 10);
            //make a net with varying neurons per layer
            var LMM = new LMMCNet(NeuronsPerLayer, 0, new int[] { }, NeuronsPerLayer, true);
            var output = LMM.Predict(Helper.GetInputs(NeuronsPerLayer));
            int index = 0;
            foreach (double d in output)
            {
                double sum = 0;
                foreach (Neuron N in LMM.Net[0])
                {
                    sum += ((N.OutValue * N.WeightsOut[index])+N.Bias);
                }
                sum = Helper.Squish(sum);
                Assert.AreEqual(sum, d);
                index++;
            }
        }

        [TestMethod]
        public void InternalMechanism_CheckSumMethod_OneHiddenLayer()
        {
            //check sum between input and hidden
            //then sum between hidden and output
            Random random = new Random();
            int NeuronsPerLayer = random.Next(5, 10);
            var LMM = new LMMCNet(NeuronsPerLayer, 1, new int[] {random.Next(5,10) }, NeuronsPerLayer, true);
            var output = LMM.Predict(Helper.GetInputs(NeuronsPerLayer));
            int indexer = 0;

            foreach (Neuron N in LMM.Net[1])
            {
                double sum = 0;
                foreach (Neuron OuterN in LMM.Net[0])
                {
                    sum += ((OuterN.OutValue * OuterN.WeightsOut[indexer]) + OuterN.Bias);
                }
                indexer++;
                sum = Helper.Squish(sum);
                Assert.AreEqual(N.OutValue, sum);
            }
        }

        [TestMethod]
        public void InternalMechanism_CheckSumMethod_MultipleHiddenLayers()
        {
            Random random = new Random();
            var LMM = Helper.GetTrueNeuralNet();
            var output = LMM.Predict(Helper.GetInputs(LMM.NumberOfInputs));


            for(int i = 1; i < LMM.NumberOfHiddenLayers; i++)
            {
                int indexer = 0;
                foreach (Neuron N in LMM.Net[i])
                {
                    double sum = 0;
                    foreach (Neuron OuterN in LMM.Net[i-1])
                    {
                        sum += ((OuterN.OutValue * OuterN.WeightsOut[indexer]) + OuterN.Bias);
                    }
                    indexer++;
                    sum = Helper.Squish(sum);
                    Assert.AreEqual(N.OutValue, sum);
                }
            }
        }

        [TestMethod]
        public void InternalMechanism_CorrectSettingOfInputNeurons()
        {
            var LMM = Helper.GetTrueNeuralNet();
            var Inputs = Helper.GetInputs(LMM.NumberOfInputs);
            LMM.Predict(Inputs);
            int index = 0;
            foreach(double inputValue in Inputs)
            {
                Assert.AreEqual(inputValue, LMM.Net[0][index].OutValue);
                index++;
            }
        }

        //Just raw copying the code from PNN instead of making it public as it's unlikely to be changed
        //I know with all likelyhood this will pass but just interesting to see any edge cases
        [TestMethod]
        public void InternalMechanism_SquashCheck()
        {
            Random random = new Random();
            for(int i = 0; i < 10; i++)
            {
                double result = (1 / (1 + Math.Exp(-random.Next())));
                Assert.IsTrue(result >= 0 && result <=1);
            }
        }
    }

    [TestClass]
    public class NetManipulationTests
    {
        [TestClass]
        public class AddNeurons
        {
            [TestMethod]
            public void NetManipulationTests_AddNeuronToInput()
            {
                var LMM = Helper.GetTrueNeuralNet();
                int InputsStart = LMM.Net[0].Count;
                LMM.AddNeuron(0);
                //check neuron is added
                Assert.AreEqual((InputsStart + 1), LMM.Net[0].Count);
                //check neuron has correct weightsOut (next layer)
                Assert.AreEqual(LMM.Net[0][LMM.Net[0].Count - 1].WeightsOut.GetUpperBound(0) + 1, LMM.Net[1].Count);

                var output = LMM.Predict(Helper.GetInputs(InputsStart + 1));
                //make sure nothing has gone wrong with the outputs
                Assert.AreEqual(output.GetUpperBound(0) + 1, LMM.NumberOfOutputs);

                //verify the "NumberOfInputs" variable has been internally incrimented and is correct
                Assert.AreEqual(LMM.NumberOfInputs, InputsStart + 1);
                Assert.AreEqual(LMM.NumberOfInputs, LMM.Net[0].Count);
            }

            [TestMethod]
            public void NetManipulationTests_AddNeuronToFirstHidden()
            {
                var LMM = Helper.GetTrueNeuralNet();
                var HiddenNeuronsStart = LMM.Net[1].Count;
                LMM.AddNeuron(1);

                //check neuron is added successfully
                Assert.AreEqual(HiddenNeuronsStart + 1, LMM.Net[1].Count);

                //check WeightsOut from previous layer are adjusted correctly
                foreach (Neuron N in LMM.Net[0])
                {
                    Assert.AreEqual(LMM.Net[1].Count, N.WeightsOut.GetUpperBound(0) + 1);
                }

                //check outputs are congruent
                var output = LMM.Predict(Helper.GetInputs(LMM.NumberOfInputs));
                Assert.AreEqual(output.GetUpperBound(0) + 1, LMM.NumberOfOutputs);
            }

            [TestMethod]
            public void NetManipulationTests_AddNeuronToMiddleHidden()
            {
                var LMM = Helper.GetTrueNeuralNet();
                var HiddenNeuronsStart = LMM.Net[2].Count;
                LMM.AddNeuron(2);

                //check neuron is added successfully
                Assert.AreEqual(HiddenNeuronsStart + 1, LMM.Net[2].Count);

                //check WeightsOut from previous layer are adjusted correctly
                foreach (Neuron N in LMM.Net[1])
                {
                    Assert.AreEqual(LMM.Net[2].Count, N.WeightsOut.GetUpperBound(0) + 1);
                }

                //check outputs are congruent
                var output = LMM.Predict(Helper.GetInputs(LMM.NumberOfInputs));
                Assert.AreEqual(output.GetUpperBound(0) + 1, LMM.NumberOfOutputs);
            }

            [TestMethod]
            public void NetManipulationTests_AddNeuronToLastHidden()
            {
                var LMM = Helper.GetTrueNeuralNet();
                var IndexOfLastHiddenLayer = (LMM.Net.Count - 2);
                var HiddenNeuronsStart = LMM.Net[IndexOfLastHiddenLayer].Count;
                LMM.AddNeuron(IndexOfLastHiddenLayer);

                //check neuron is added successfully
                Assert.AreEqual(HiddenNeuronsStart + 1, LMM.Net[IndexOfLastHiddenLayer].Count);

                //check WeightsOut from previous layer are adjusted correctly
                foreach (Neuron N in LMM.Net[IndexOfLastHiddenLayer - 1])
                {
                    Assert.AreEqual(LMM.Net[IndexOfLastHiddenLayer].Count, N.WeightsOut.GetUpperBound(0) + 1);
                }

                //check outputs are congruent
                var output = LMM.Predict(Helper.GetInputs(LMM.NumberOfInputs));
                Assert.AreEqual(output.GetUpperBound(0) + 1, LMM.NumberOfOutputs);
            }

            [TestMethod]
            public void NetManipulationTests_AddNeuronToOutputLayer()
            {
                var LMM = Helper.GetTrueNeuralNet();
                var IndexOfOutput = (LMM.Net.Count - 1);
                var OutputNeuronsStart = LMM.Net[IndexOfOutput].Count;
                LMM.AddNeuron(IndexOfOutput);

                //check neuron is added successfully
                Assert.AreEqual(OutputNeuronsStart + 1, LMM.Net[IndexOfOutput].Count);

                //check WeightsOut from previous layer are adjusted correctly
                foreach (Neuron N in LMM.Net[IndexOfOutput - 1])
                {
                    Assert.AreEqual(LMM.Net[IndexOfOutput].Count, N.WeightsOut.GetUpperBound(0) + 1);
                }

                //check outputs are congruent
                var output = LMM.Predict(Helper.GetInputs(LMM.NumberOfInputs));
                Assert.AreEqual(output.GetUpperBound(0) + 1, LMM.NumberOfOutputs);

                //verify the "NumberOfInputs" variable has been internally de-incrimented and is correct
                Assert.AreEqual(LMM.NumberOfOutputs, OutputNeuronsStart + 1);
                Assert.AreEqual(LMM.NumberOfOutputs, LMM.Net[IndexOfOutput].Count);
            }
        }

        [TestClass]
        public class RemoveNeurons
        {
            [TestMethod]
            public void RemoveNeuronFromInput()
            {
                var LMM = Helper.GetTrueNeuralNet();
                int NumberOfInitialInputs = LMM.NumberOfInputs;
                LMM.RemoveNeuron(0, 0);

                //Check Neuron is removed successfully
                Assert.AreEqual(LMM.NumberOfInputs, NumberOfInitialInputs - 1);

                //check outputs are congruent
                var output = LMM.Predict(Helper.GetInputs(LMM.NumberOfInputs));
                Assert.AreEqual(output.GetUpperBound(0) + 1, LMM.NumberOfOutputs);

                //verify the "NumberOfInputs" variable has been internally de-incrimented and is correct
                Assert.AreEqual(LMM.NumberOfInputs, LMM.Net[0].Count);

            }

            [TestMethod]
            public void RemoveNeuronFromFirstHidden()
            {
                var LMM = Helper.GetTrueNeuralNet();
                int RandToRemove = Helper.GetRandomToRemove(LMM, 1);
                int NumberOfInitialNeurons = LMM.Net[1].Count;

                List<double[]> WeightsOutBefore = new List<double[]>();
                foreach(Neuron N in LMM.Net[0])
                {
                    WeightsOutBefore.Add(N.WeightsOut);
                }
                LMM.RemoveNeuron(1, RandToRemove);

                //Make sure Neuron is removed successfully
                Assert.AreEqual(LMM.Net[1].Count, NumberOfInitialNeurons - 1);

                //check Weights out for each neuron in the previous layer have been correctly adjusted
                foreach(Neuron N in LMM.Net[0])
                {
                    Assert.AreEqual(N.WeightsOut.GetUpperBound(0) + 1, LMM.Net[1].Count);
                }

                //check weights still line up e.g. Wout[0] => 0 etc etc
                int indexer = 0;
                int Neuron = 0;
                bool BreakDiscovered = false;
                foreach (double[] DD in WeightsOutBefore)
                {
                    BreakDiscovered = false;
                    foreach(double D in DD)
                    {
                        //simply skip comparison if index == one we removed, allows on second loop, index falls relatively 1 back
                        //compared to initial array, compare as normal
                        if((indexer != RandToRemove) || BreakDiscovered)
                        {
                            Assert.AreEqual(D, LMM.Net[0][Neuron].WeightsOut[indexer]);
                            indexer++;
                        }
                        else
                        {
                            BreakDiscovered = true;
                        } 
                    }
                    indexer = 0;
                    Neuron++;
                }

            }

            [TestMethod]
            public void RemoveNeuronFromMiddleHidden()
            {
                var LMM = Helper.GetTrueNeuralNet();
                int RandToRemove = Helper.GetRandomToRemove(LMM, 2);
                int NumberOfInitialNeurons = LMM.Net[2].Count;

                List<double[]> WeightsOutBefore = new List<double[]>();
                foreach (Neuron N in LMM.Net[1])
                {
                    WeightsOutBefore.Add(N.WeightsOut);
                }
                LMM.RemoveNeuron(2, RandToRemove);

                //Make sure Neuron is removed successfully
                Assert.AreEqual(LMM.Net[2].Count, NumberOfInitialNeurons - 1);

                //check Weights out for each neuron in the previous layer have been correctly adjusted
                foreach (Neuron N in LMM.Net[1])
                {
                    Assert.AreEqual(N.WeightsOut.GetUpperBound(0) + 1, LMM.Net[2].Count);
                }

                //check weights still line up e.g. Wout[0] => 0 etc etc
                int indexer = 0;
                int Neuron = 0;
                bool BreakDiscovered = false;
                foreach (double[] DD in WeightsOutBefore)
                {
                    BreakDiscovered = false;
                    foreach (double D in DD)
                    {
                        //simply skip comparison if index == one we removed, allows on second loop, index falls relatively 1 back
                        //compared to initial array, compare as normal
                        if ((indexer != RandToRemove) || BreakDiscovered)
                        {
                            Assert.AreEqual(D, LMM.Net[1][Neuron].WeightsOut[indexer]);
                            indexer++;
                        }
                        else
                        {
                            BreakDiscovered = true;
                        }
                    }
                    indexer = 0;
                    Neuron++;
                }
            }

            [TestMethod]
            public void RemoveNeuronFromLastHidden()
            {
                var LMM = Helper.GetTrueNeuralNet();
                var IndexOfLast = LMM.Net.Count - 2;
                int RandToRemove = Helper.GetRandomToRemove(LMM, IndexOfLast);
                int NumberOfInitialNeurons = LMM.Net[IndexOfLast].Count;

                List<double[]> WeightsOutBefore = new List<double[]>();
                foreach (Neuron N in LMM.Net[IndexOfLast-1])
                {
                    WeightsOutBefore.Add(N.WeightsOut);
                }
                LMM.RemoveNeuron(IndexOfLast, RandToRemove);

                //Make sure Neuron is removed successfully
                Assert.AreEqual(LMM.Net[IndexOfLast].Count, NumberOfInitialNeurons - 1);

                //check Weights out for each neuron in the previous layer have been correctly adjusted
                foreach (Neuron N in LMM.Net[IndexOfLast-1])
                {
                    Assert.AreEqual(N.WeightsOut.GetUpperBound(0) + 1, LMM.Net[IndexOfLast].Count);
                }

                //check weights still line up e.g. Wout[0] => 0 etc etc
                int indexer = 0;
                int Neuron = 0;
                bool BreakDiscovered = false;
                foreach (double[] DD in WeightsOutBefore)
                {
                    BreakDiscovered = false;
                    foreach (double D in DD)
                    {
                        //simply skip comparison if index == one we removed, allows on second loop, index falls relatively 1 back
                        //compared to initial array, compare as normal
                        if ((indexer != RandToRemove) || BreakDiscovered)
                        {
                            Assert.AreEqual(D, LMM.Net[IndexOfLast-1][Neuron].WeightsOut[indexer]);
                            indexer++;
                        }
                        else
                        {
                            BreakDiscovered = true;
                        }
                    }
                    indexer = 0;
                    Neuron++;
                }
            }

            [TestMethod]
            public void RemoveNeuronFromOutput()
            {
                var LMM = Helper.GetTrueNeuralNet();
                var IndexOfLast = LMM.Net.Count - 1;
                int RandToRemove = Helper.GetRandomToRemove(LMM, IndexOfLast);
                int NumberOfInitialNeurons = LMM.Net[IndexOfLast].Count;

                List<double[]> WeightsOutBefore = new List<double[]>();
                foreach (Neuron N in LMM.Net[IndexOfLast - 1])
                {
                    WeightsOutBefore.Add(N.WeightsOut);
                }
                LMM.RemoveNeuron(IndexOfLast, RandToRemove);

                //Assert number of outputs interally altered correctly
                Assert.AreEqual(LMM.NumberOfOutputs, LMM.Net[IndexOfLast].Count);

                //Make sure Neuron is removed successfully
                Assert.AreEqual(LMM.Net[IndexOfLast].Count, NumberOfInitialNeurons - 1);

                //check Weights out for each neuron in the previous layer have been correctly adjusted
                foreach (Neuron N in LMM.Net[IndexOfLast - 1])
                {
                    Assert.AreEqual(N.WeightsOut.GetUpperBound(0) + 1, LMM.Net[IndexOfLast].Count);
                }

                //check weights still line up e.g. Wout[0] => 0 etc etc
                int indexer = 0;
                int Neuron = 0;
                bool BreakDiscovered = false;
                foreach (double[] DD in WeightsOutBefore)
                {
                    BreakDiscovered = false;
                    foreach (double D in DD)
                    {
                        //simply skip comparison if index == one we removed, allows on second loop, index falls relatively 1 back
                        //compared to initial array, compare as normal
                        if ((indexer != RandToRemove) || BreakDiscovered)
                        {
                            Assert.AreEqual(D, LMM.Net[IndexOfLast - 1][Neuron].WeightsOut[indexer]);
                            
                            indexer++;
                        }
                        else
                        {
                            BreakDiscovered = true;
                        }
                    }
                    indexer = 0;
                    Neuron++;
                }
            }
        }

        [TestClass]
        public class AddRemoveLayers
        {
            //Add Layer
            [TestMethod]
            public void AddLayer()
            {
                var LMM = Helper.GetTrueNeuralNet();
                //insert a layer at positon 1, will become first hidden layer
                int InitialLayers = LMM.Net.Count;
                LMM.AddLayer(1, 5);

                //check layer has been successfully added
                Assert.AreEqual(LMM.Net.Count, InitialLayers + 1);

                //check each neuron in new layer has correct number of weights out
                foreach(Neuron N in LMM.Net[1])
                {
                    Assert.AreEqual(N.WeightsOut.GetUpperBound(0) + 1, LMM.Net[2].Count);
                }

                //check each neuron in the previous layer has the correct number of weights out to the new layer
                foreach (Neuron N in LMM.Net[0])
                {
                    Assert.AreEqual(N.WeightsOut.GetUpperBound(0) + 1, LMM.Net[1].Count);
                }

                //make sure outputs still work
               var outputs = LMM.Predict(Helper.GetInputs(LMM.NumberOfInputs));
                Assert.AreEqual(outputs.GetUpperBound(0) + 1, LMM.NumberOfOutputs);
            }

            //add a layer to a net that currently has no hidden layers
            [TestMethod]
            public void AddLayerToNoHiddens()
            {
                var LMM = new LMMCNet(5, 0, new int[] { }, 5, true);
                int InitialLayers = LMM.Net.Count;
                LMM.AddLayer(1, 5);
                //check layer has been successfully added
                Assert.AreEqual(LMM.Net.Count, InitialLayers + 1);

                //check each neuron in new layer has correct number of weights out
                foreach (Neuron N in LMM.Net[1])
                {
                    Assert.AreEqual(N.WeightsOut.GetUpperBound(0) + 1, LMM.Net[2].Count);
                }

                //check each neuron in the previous layer has the correct number of weights out to the new layer
                foreach (Neuron N in LMM.Net[0])
                {
                    Assert.AreEqual(N.WeightsOut.GetUpperBound(0) + 1, LMM.Net[1].Count);
                }

                //make sure outputs still work
                var outputs = LMM.Predict(Helper.GetInputs(LMM.NumberOfInputs));
                Assert.AreEqual(outputs.GetUpperBound(0) + 1, LMM.NumberOfOutputs);

            }


            //Remove layer
            [TestMethod]
            public void RemoveLayer()
            {
                var LMM = Helper.GetTrueNeuralNet();
                var InitialLayers = LMM.Net.Count;
                //remove layer at position 1
                LMM.RemoveLayer(1);

                //check layer has been successfully removed
                Assert.AreEqual(InitialLayers - 1, LMM.Net.Count);

                //check each neuron in the layer before has correct weights out e.g. now 0 => 1
                foreach(Neuron N in LMM.Net[0])
                {
                    Assert.AreEqual(N.WeightsOut.GetUpperBound(0) + 1, LMM.Net[1].Count);
                }

                //make sure outputs still work
                var outputs = LMM.Predict(Helper.GetInputs(LMM.NumberOfInputs));
                Assert.AreEqual(outputs.GetUpperBound(0) + 1, LMM.NumberOfOutputs);
            }
            

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

        public static int[]  GetHLayerArray(int NHiddenLayers, int NeuronsPerLayer)
        {
            int[] ReturnArray = new int[NHiddenLayers];
            for (int j = 0; j < NHiddenLayers; j++)
            {
                ReturnArray[j] = NeuronsPerLayer;
            }
            return ReturnArray;
        }

        public static int[] GetHLayerRand(int NHiddenLayers,Random random)
        {
            int[] ReturnArray = new int[NHiddenLayers];
            for (int j = 0; j < NHiddenLayers; j++)
            {
                ReturnArray[j] = random.Next(2,10);
            }
            return ReturnArray;
        }

        //A "true" neural net is one with varying neurons between layers and random number of layers & weights
        public static LMMCNet GetTrueNeuralNet()
        {
            Random random = new Random();
            int NHiddenLayers = random.Next(2, 5);
            int NInputs = random.Next(2, 10);
            int NOutputs = random.Next(2, 10);
            int[] NeuronsPerHiddenLayer = GetHLayerRand(NHiddenLayers, random);
            return new LMMCNet(NInputs, NHiddenLayers, NeuronsPerHiddenLayer, NOutputs, true);
        }

        public static int GetRandomToRemove(LMMCNet LMM, int LayerNumber)
        {
            Random random = new Random();
            return (random.Next(0, LMM.Net[LayerNumber].Count));
        }

        public static double Squish(double input)
        {
            return  (1 / (1 + Math.Exp(-input)));
        }
    }
}

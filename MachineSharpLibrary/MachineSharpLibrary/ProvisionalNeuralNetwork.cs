using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSharpLibrary
{
    class ProvisionalNeuralNetwork
    {
        List<Neuron> InputLayer = new List<Neuron>();
        List<Neuron> OutputLayer = new List<Neuron>();
        List<List<Neuron>> HiddenLayers = new List<List<Neuron>>();

        //parameterless constructor to stop the compiler from complaining
        public ProvisionalNeuralNetwork()
        { 
        }

        /// <summary>
        /// Create a neural network
        /// </summary>
        /// <param name="NumberOfHiddenLayers">How many Hidden layers to create</param>
        /// <param name="NeuronsPerHiddenLayer">Number of neurons per hidden layer, e.g. first index = number of neurons in first layer etc etc</param>
        /// <param name="NumberOfInputs">Number of input neurons</param>
        /// <param name="NumberOfOutputs">Number of output neurons</param>
        public ProvisionalNeuralNetwork(int NumberOfHiddenLayers, int[] NeuronsPerHiddenLayer, int NumberOfInputs, int NumberOfOutputs, bool UseRandom)
        {
            //catch for any mismatch errors
            if(NumberOfHiddenLayers != NeuronsPerHiddenLayer.Count())
            {
                //if there are more or less neuron counts for the layers requested then throw an error
                throw new Exception("Difference between number of hidden layers and neurons supplied for each layer");
            }

            if (!UseRandom)
            {
                FillInputLayer(NumberOfInputs, NeuronsPerHiddenLayer, NumberOfOutputs);

                FillHiddenLayers(NumberOfHiddenLayers, NeuronsPerHiddenLayer, NumberOfOutputs);

                FillOutputLayer(NumberOfOutputs);
            }
            else
            {
                Random rand = new Random();

                FillRandInputLayer(NumberOfInputs, NeuronsPerHiddenLayer, NumberOfOutputs, rand);

                FillRandHiddenLayers(NumberOfHiddenLayers, NeuronsPerHiddenLayer, NumberOfOutputs, rand);

                FillRandOutputLayer(NumberOfOutputs);
            }

        }

        private void FillInputLayer(int NumberOfInputs, int[] NeuronsPerHiddenLayer, int NumberOfOutputs)
        {
            //fill input layer with neurons
            for (int i = 0; i < NumberOfInputs; i++)
            {
                //if hidden layer exists, number of outputs for each neuron in input layer = number of neurons in first hidden layer
                if (NeuronsPerHiddenLayer.Count() > 0)
                {
                    InputLayer.Add(new Neuron(NeuronsPerHiddenLayer[0]));
                }
                else
                {
                    //if no hidden layers exist then number of outputs for each neuron in input layer = number of neurons in the output layer
                    InputLayer.Add(new Neuron(NumberOfOutputs));
                }
            }
        }

        private void FillHiddenLayers(int NumberOfHiddenLayers, int[] NeuronsPerHiddenLayer, int NumberOfOutputs)
        {
            //fill hidden layer(s) with neurons if any exist
            if (NumberOfHiddenLayers > 0)
            {
                //if only 1 hidden layer then number of outputs from hidden layer = number of neurons in final output layer
                if (NumberOfHiddenLayers == 1)
                {
                    List<Neuron> FirstHiddenLayer = new List<Neuron>();
                    for (int i = 0; i < NeuronsPerHiddenLayer[0]; i++)
                    {
                        FirstHiddenLayer.Add(new Neuron(NumberOfOutputs));
                    }
                    HiddenLayers.Add(FirstHiddenLayer);
                }
                else
                {
                    //more than 1 hidden layer, succesive number of outputs = number of neurons for next layer
                    int LayerCount = 0;
                    for (LayerCount = 0; LayerCount < NumberOfHiddenLayers - 1; LayerCount++)
                    {
                        List<Neuron> NextLayer = new List<Neuron>();
                        for (int i = 0; i < NeuronsPerHiddenLayer[LayerCount]; i++)
                        {
                            NextLayer.Add(new Neuron(NeuronsPerHiddenLayer[LayerCount + 1]));
                        }
                        HiddenLayers.Add(NextLayer);
                    }

                    //for last layer, number of outputs from hidden layer = number of neurons in final output layer
                    List<Neuron> FinalHiddenLayer = new List<Neuron>();
                    for (int i = 0; i < NeuronsPerHiddenLayer[NumberOfHiddenLayers - 1]; i++)
                    {
                        FinalHiddenLayer.Add(new Neuron(NumberOfOutputs));
                    }
                    HiddenLayers.Add(FinalHiddenLayer);
                }
            }
        }

        private void FillOutputLayer(int NumberOfOutputs)
        {
            for (int i = 0; i < NumberOfOutputs; i++)
            {
                //1 output
                OutputLayer.Add(new Neuron(1));
            }
        }

        private void FillRandInputLayer(int NumberOfInputs, int[] NeuronsPerHiddenLayer, int NumberOfOutputs, Random rand)
        {
            //fill input layer with neurons
            for (int i = 0; i < NumberOfInputs; i++)
            {
                //if hidden layer exists, number of outputs for each neuron in input layer = number of neurons in first hidden layer
                if (NeuronsPerHiddenLayer.Count() > 0)
                {
                    InputLayer.Add(new Neuron(NeuronsPerHiddenLayer[0],rand));
                }
                else
                {
                    //if no hidden layers exist then number of outputs for each neuron in input layer = number of neurons in the output layer
                    InputLayer.Add(new Neuron(NumberOfOutputs,rand));
                }
            }
        }

        private void FillRandHiddenLayers(int NumberOfHiddenLayers, int[] NeuronsPerHiddenLayer, int NumberOfOutputs, Random rand)
        {
            //fill hidden layer(s) with neurons if any exist
            if (NumberOfHiddenLayers > 0)
            {
                //if only 1 hidden layer then number of outputs from hidden layer = number of neurons in final output layer
                if (NumberOfHiddenLayers == 1)
                {
                    List<Neuron> FirstHiddenLayer = new List<Neuron>();
                    for (int i = 0; i < NeuronsPerHiddenLayer[0]; i++)
                    {
                        FirstHiddenLayer.Add(new Neuron(NumberOfOutputs, rand));
                    }
                    HiddenLayers.Add(FirstHiddenLayer);
                }
                else
                {
                    //more than 1 hidden layer, succesive number of outputs = number of neurons for next layer
                    int LayerCount = 0;
                    for (LayerCount = 0; LayerCount < NumberOfHiddenLayers - 1; LayerCount++)
                    {
                        List<Neuron> NextLayer = new List<Neuron>();
                        for (int i = 0; i < NeuronsPerHiddenLayer[LayerCount]; i++)
                        {
                            NextLayer.Add(new Neuron(NeuronsPerHiddenLayer[LayerCount + 1], rand));
                        }
                        HiddenLayers.Add(NextLayer);
                    }

                    //for last layer, number of outputs from hidden layer = number of neurons in final output layer
                    List<Neuron> FinalHiddenLayer = new List<Neuron>();
                    for (int i = 0; i < NeuronsPerHiddenLayer[NumberOfHiddenLayers - 1]; i++)
                    {
                        FinalHiddenLayer.Add(new Neuron(NumberOfOutputs, rand));
                    }
                    HiddenLayers.Add(FinalHiddenLayer);
                }
            }
        }

        private void FillRandOutputLayer(int NumberOfOutputs)
        {
            for (int i = 0; i < NumberOfOutputs; i++)
            {
                //1 output
                OutputLayer.Add(new Neuron(1));
            }
        }

        public Double[] Predict(Double[] InputArray)
        {
            //go through each layer, start with first, then every hidden, landing on output
            if(InputArray.Count() != InputLayer.Count())
            {
                throw new Exception("Invalid number of inputs");
            }
            else
            {
                //input layer
                for(int i = 0; i < InputLayer.Count(); i++)
                {
                    InputLayer[i].Activation = InputArray[i];
                }

                //hidden layers
                if(HiddenLayers.Count() > 0)
                {
                    int layerNumber = 1;

                    //if 1 hidden layer
                    if (HiddenLayers.Count() == 1)
                    {
                        
                        int iterator = 0;
                        foreach(Neuron N in HiddenLayers.First())
                        {
                            //sum & squash of all activations * weights from previous layers
                            N.Activation = Sum(layerNumber, iterator);
                            iterator++;
                        }
                    }
                    else
                    {
                        //if more than 1 hidden layer
                        foreach(List<Neuron> LN in HiddenLayers)
                        {
                            int iterator = 0;
                            foreach (Neuron N in LN)
                            {
                                N.Activation = Sum(layerNumber, iterator);
                                iterator++;
                            }
                            layerNumber++;
                        }
                    }
                }

                //output layers
                //if no hidden layers
                if(HiddenLayers.Count == 0)
                {
                    int iterator = 0;
                    foreach (Neuron N in OutputLayer)
                    {
                        //sum & squash of all activations * weights from previous layers
                        N.Activation = Sum(0, iterator);
                        iterator++;
                    }
                }
                else if(HiddenLayers.Count == 1)
                {
                    int iterator = 0;
                    foreach (Neuron N in OutputLayer)
                    {
                        //sum & squash of all activations * weights from previous layers
                        N.Activation = Sum(1, iterator);
                        iterator++;
                    }
                }
                //multiple hidden layers, take from last
                else
                {
                    int iterator = 0;
                    foreach (Neuron N in OutputLayer)
                    {
                        //sum & squash of all activations * weights from previous layers
                        N.Activation = Sum(HiddenLayers.Count, iterator);
                        iterator++;
                    }
                }
            }
            double[] ReturnArray = new double[OutputLayer.Count()];
            int OutIterator = 0;
            foreach(Neuron D in OutputLayer)
            {
                ReturnArray[OutIterator] = D.Activation;
                OutIterator++;
            }
            return ReturnArray;

        }

        //input = layer 0, first hidden layer = layer 1 etc etc
        private double Sum(int layerNumber, int neuronNumber)
        {
            double sum = 0;
            //if first hidden layer, take activations & weights from input layer
            if(layerNumber == 0)
            {
                foreach (Neuron N in InputLayer)
                {
                    sum += (N.WeightsOut[neuronNumber] * N.Activation) + N.Bias;
                }
            }
            else
            {
                foreach (Neuron N in HiddenLayers[layerNumber-2])
                {
                    sum += (N.WeightsOut[neuronNumber] * N.Activation) + N.Bias;
                }
            }

            sum = Squash(sum);
            return sum;
        }

        private double Squash(double input)
        {
            return 1 / (1 + Math.Exp(-input));
        }
    }
}

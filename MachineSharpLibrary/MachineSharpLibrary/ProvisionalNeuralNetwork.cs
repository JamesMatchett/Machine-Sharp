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
        public ProvisionalNeuralNetwork(int NumberOfHiddenLayers, int[] NeuronsPerHiddenLayer, int NumberOfInputs, int NumberOfOutputs)
        {
            //catch for any mismatch errors
            if(NumberOfHiddenLayers != NeuronsPerHiddenLayer.Count())
            {
                //if there are more or less neuron counts for the layers requested then throw an error
                throw new Exception("Difference between number of hidden layers and neurons supplied for each layer");
            }

            //fill input layer with neurons
            for(int i = 0; i < NumberOfInputs; i++)
            {
                //if hidden layer exists, number of outputs for each neuron in input layer = number of neurons in first hidden layer
                if(NeuronsPerHiddenLayer.Count() > 0)
                {
                    InputLayer.Add(new Neuron(NeuronsPerHiddenLayer[0]));
                }
                else
                {
                    //if no hidden layers exist then number of outputs for each neuron in input layer = number of neurons in the output layer
                    InputLayer.Add(new Neuron(NumberOfOutputs));
                }
            }


            //fill hidden layer(s) with neurons if any exist
            if (NumberOfHiddenLayers > 0)
            {
                //if only 1 hidden layer then number of outputs from hidden layer = number of neurons in final output layer
                if(NumberOfHiddenLayers == 1)
                {
                    List<Neuron> FirstHiddenLayer = new List<Neuron>();
                    for(int i = 0; i<NeuronsPerHiddenLayer[0]; i++)
                    {
                        FirstHiddenLayer.Add(new Neuron(NumberOfOutputs));
                    }
                    HiddenLayers.Add(FirstHiddenLayer);
                }
                else
                {
                    //more than 1 hidden layer, succesive number of outputs = number of neurons for next layer
                    int LayerCount = 0;
                    for(LayerCount = 0; LayerCount < NumberOfHiddenLayers -1 ; LayerCount++)
                    {
                        List<Neuron> NextLayer = new List<Neuron>();
                        for(int i =0; i<NeuronsPerHiddenLayer[LayerCount]; i++)
                        {
                            NextLayer.Add(new Neuron(NeuronsPerHiddenLayer[i + 1]));
                        }
                        HiddenLayers.Add(NextLayer);
                    }

                    //for last layer, number of outputs from hidden layer = number of neurons in final output layer
                    List<Neuron> FinalHiddenLayer = new List<Neuron>();
                    for (int i = 0; i < NeuronsPerHiddenLayer[NumberOfHiddenLayers-1]; i++)
                    {
                        FinalHiddenLayer.Add(new Neuron(NumberOfOutputs));
                    }
                    HiddenLayers.Add(FinalHiddenLayer);
                }
            }

            //fill output layer with neurons
            //output layer will always have 1 output per neuron the "result"
            for(int i =0; i < NumberOfOutputs; i++)
            {
                OutputLayer.Add(new Neuron(1));
            }
        }
    }
}

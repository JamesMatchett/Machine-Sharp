using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSharpLibrary
{
   public class LMMCNet : INetBase
    {
        public List<List<Neuron>> Net;
        protected override Activations ActivationsFunction { get; set; }
        public int NumberOfInputs { get; private set; }
        public int NumberOfHiddenLayers { get; private set; }
        public int[] NeuronsPerHiddenLayer { get; private set; }
        public int NumberOfOutputs { get; private set; }
        private Random _localRandom { get; set; }

        protected override void InitNet()
        {
           
        }

        

        public LMMCNet(int numberOfInputs, int numberOfHiddenLayers, int[] neuronsPerHiddenLayer, int numberOfOutputs, bool MakeRandom)
        : this(numberOfInputs, numberOfHiddenLayers,  neuronsPerHiddenLayer,  numberOfOutputs,  MakeRandom, Activations.Sigmoid, .7)
        {
            //constructor with no activation function specified & default learning rate
        }

        public LMMCNet(int numberOfInputs, int numberOfHiddenLayers, int[] neuronsPerHiddenLayer, int numberOfOutputs, bool MakeRandom, 
            Activations activations, double InitLearningRate)
        {
            NumberOfInputs = numberOfInputs;
            NumberOfHiddenLayers = numberOfHiddenLayers;
            NeuronsPerHiddenLayer = neuronsPerHiddenLayer;
            NumberOfOutputs = numberOfOutputs;
            Net = new List<List<Neuron>>();
            ActivationsFunction = activations;
            LearningRate = InitLearningRate;

            //TOOD:  Add exception handling for valid funcs

            if (MakeRandom)
            {
                _localRandom = new Random();
            }
            
            Exception_CheckValidState();

            
            if (numberOfHiddenLayers == 0)
            {
                Net.Add(MakeLayer(NumberOfInputs, numberOfInputs, MakeRandom));
            }
            else
            {
                //Input layer
                Net.Add(MakeLayer(NumberOfInputs, NeuronsPerHiddenLayer[0], MakeRandom));
                //HiddenLayers
                for (int i = 0; i < numberOfHiddenLayers - 1; i++)
                {
                    Net.Add(MakeLayer(neuronsPerHiddenLayer[i], neuronsPerHiddenLayer[i + 1], MakeRandom));
                }
                Net.Add(MakeLayer(neuronsPerHiddenLayer.Last(), numberOfOutputs, MakeRandom));
            }
                //OutputLayer
            Net.Add(MakeLayer(numberOfOutputs, 0, MakeRandom));
            
        }

        public List<Neuron> MakeLayer(int NeuronsInThisLayer, int NeuronsInNextLayer, bool MakeRandom)
        {
            var ReturnList = new List<Neuron>();
            if (!MakeRandom)
            {
                for (int i = 0; i < NeuronsInThisLayer; i++)
                {
                    ReturnList.Add(new Neuron(NeuronsInNextLayer));
                }
            }
            else
            {
                for (int i = 0; i < NeuronsInThisLayer; i++)
                {
                    ReturnList.Add(new Neuron(NeuronsInNextLayer, _localRandom));
                }
            }
            return ReturnList;
        }


        public override double[] Predict (double[] Inputs)
        {
            //check valid input count
            Exception_Predict(Inputs.Count(), this.NumberOfInputs);
            //Set inputs
            for (int i = 0; i < NumberOfInputs; i++)
            {
                Net[0][i].OutValue = Inputs[i];
            }

            for (int LayerNumber = 1; LayerNumber < Net.Count(); LayerNumber++)
            {
                for (int Neuron = 0; Neuron < Net[LayerNumber].Count(); Neuron++)
                {
                    double sum = 0;
                    foreach(Neuron N in Net[LayerNumber - 1])
                    {
                        sum += ((N.OutValue * N.WeightsOut[Neuron]) + N.Bias);
                    }
                    Net[LayerNumber][Neuron].OutValue = Activation(sum,Activations.Sigmoid);
                }
            }

            double[] Outputs = new double[NumberOfOutputs];
            for(int N = 0; N < NumberOfOutputs; N++)
            {
                Outputs[N] = Net[Net.Count - 1][N].OutValue;
            }

            return Outputs;
            
        }

        /// <summary>
        /// Adds a neuron to the end of a layer
        /// </summary>
        /// <param name="LayerNumber">Which layer to add the neuron to, 0 indexed, layer 0 = input layer, Net.Count - 1 = output layer </param>  
        public void AddNeuron(int LayerNumber)
        {
            if(LayerNumber >= Net.Count || LayerNumber < 0)
            {
                throw new InvalidOperationException(("Layer " + LayerNumber + " Does not exist, largest index of this net is " + (Net.Count - 1)));
            }

            //Add to input layer
            if (LayerNumber == 0)
            {
                Net[0].Add(new Neuron(Net[1].Count));
                NumberOfInputs++;
            }

            else { 
            //Add to Output layer
                 if (LayerNumber == Net.Count - 1)
                {
                    Net[LayerNumber].Add(new Neuron(0));
                    NumberOfOutputs++;
                }
                else
                {
                    Net[LayerNumber].Add(new Neuron(Net[LayerNumber + 1].Count));
                }

                //adjust weights of previous layer
                BackAdjustWeights(LayerNumber - 1, Net[LayerNumber].Count - 1);
            }

           
        }

        /// <summary>
        /// Remove a neuron from a layer
        /// </summary>
        /// <param name="LayerNumber">Which layer to remove the neuron from, 0 indexed </param>
        /// <param name="NeuronNumber"></param>
        public void RemoveNeuron(int LayerNumber, int NeuronNumber)
        {
            if (LayerNumber >= Net.Count || LayerNumber < 0)
            {
                throw new InvalidOperationException(("Layer " + LayerNumber + " Does not exist, largest index of this net is " + (Net.Count - 1)));
            }

            if (Net[LayerNumber].Count <= NeuronNumber || NeuronNumber < 0)
            {
                throw new InvalidOperationException("Layer has " + Net[LayerNumber].Count + " Neurons, you asked to remove " + NeuronNumber);
            }

            if (Net[LayerNumber].Count == 1)
            {
                throw new InvalidOperationException("Layer has " + Net[LayerNumber].Count + " Neuron, In order to remove the layer, please call RemoveLayer");
            }

            var newLayer = new List<Neuron>();
            int indexer = 0;
            foreach (Neuron N in Net[LayerNumber])
            {
                if (indexer != NeuronNumber)
                {
                    newLayer.Add(N);
                }
                indexer++;
            }
            Net[LayerNumber] = newLayer;

            //adjust weightsout array of previous layer
            if (LayerNumber != 0)
            {
                BackAdjustWeights(LayerNumber - 1, NeuronNumber);
            }

            if (LayerNumber == 0)
            {
                NumberOfInputs--;
            }

            if (LayerNumber == Net.Count - 1)
            {
                NumberOfOutputs--;
            }
        }

       /// <summary>
       /// Add a layer to the net with a set number of Neurons
       /// </summary>
       /// <param name="LayerNumber">Index the new layer will occupy, e.g. "1" will mean the layer added will be the 
       /// first hidden layer, every other layer is moved 1 up, cannot be 0 or the same as the output layer
       /// </param>
       /// <param name="NeuronsInNewLayer">How many neurons the new layer should occupy</param>
        public void AddLayer(int LayerNumber, int NeuronsInNewLayer)
        {
            Exception_AddLayer(LayerNumber, NeuronsInNewLayer);
            List<Neuron> newLayer = MakeLayer(NeuronsInNewLayer, Net[LayerNumber].Count, true);
            Net.Insert(LayerNumber, newLayer);
            int NeuronNo = 0;
            foreach(Neuron N in Net[LayerNumber - 1])
            {
                N.WeightsOut = new double[Net[LayerNumber].Count()];
                while(N.WeightsOut.GetUpperBound(0)+1 != Net[LayerNumber].Count())
                {
                    BackAdjustWeights(LayerNumber - 1, NeuronNo);
                }
                NeuronNo++;
            }
        }

        /// <summary>
        /// Removes a layer from the net
        /// </summary>
        /// <param name="LayerNumber">Which layer to remove, cannot be 0 or the same as the output layer</param>
        public void RemoveLayer(int LayerNumber)
        {
            //Checklist
            //Exception, layer number less than 1 or greater than output-1 D 
            //Remove layer D 
            //set weights out from layer before to match neurons in layer after

            Exception_Remove(LayerNumber);

            Net.RemoveAt(LayerNumber);

            int NeuronNo = 0;
            foreach(Neuron N in Net[LayerNumber - 1])
            {
                N.WeightsOut = new double[Net[LayerNumber].Count];
                while(N.WeightsOut.GetUpperBound(0)+1 != Net[LayerNumber].Count)
                {
                    BackAdjustWeights(LayerNumber - 1, NeuronNo);
                }
                NeuronNo++;
            }
        }

        public override void Train(double[] Inputs, double[] ExpectedOutputs)
        {

            //Check input number of elements correct
            Exception_Train(Inputs, ExpectedOutputs);

            //calculate actual outputs
            double[] ActualOutputs = Predict(Inputs);

            //evaluate cost for overall network
            var costval = Cost(ActualOutputs, ExpectedOutputs).Sum();

            //dw (break apart) ->  (deriviate of error with respect to outputs) * (deriviative of the output with respect to the weights)

            //Gradients = outputs
            List<double> gradients = new List<double>();
            //last layer number = Net.Count()-1;
            //neuron = Net[lastLayer].Count();
            int Lastlayer = Net.Count() - 1;
            int NeuronCount = Net[Lastlayer].Count();

            for (int i = 0; i<NeuronCount; i++)
            {
                gradients.Add(Activation(Activations.DSigmoid,(Net[Lastlayer][i].OutValue)));
            }

            //cross multiply by errors (not that kind of cross multiply! (element wise))
            double[] CostArray = Cost(ActualOutputs, ExpectedOutputs);
            for (int i = 0; i < NeuronCount; i++)
            {
                gradients[i] *= CostArray[i];
            }

            //learning rate
            for (int i = 0; i < NeuronCount; i++)
            {
                gradients[i] *= LearningRate;
            }

            //add grads + biases
            for(int i = 0; i < NeuronCount; i++)
            {
                Net[Lastlayer][i].Bias += gradients[i];
            }


            //dot product
            //Weights out, gradients
            //1D list = gradients, weights out is a 2D list 
            //2xn matrix by 1xn list 

            double[,] weightDeltas = new double[gradients.Count(), Net[Lastlayer - 1].Count()];
            double tempSum = 0;
            for (int i = 0; i < gradients.Count(); i++)
            {
                for (int k = 0; k < Net[Lastlayer - 1].Count(); k++)
                {
                    tempSum = 0;
                    for (int j = 0; j < gradients.Count(); j++)
                    {
                        tempSum += gradients[i] * Net[Lastlayer - 1][j].OutValue;
                    }
                    weightDeltas[i, k] = tempSum;
                }
            }
            
            //MEGALOOP ™

            for (int i = 0; i < weightDeltas.GetLength(0); i++)
            {
                for (int j = 0; j < weightDeltas.GetLength(1); j++)
                {
                    for (int k = 0; k < Net[Lastlayer - 1].Count()-1; k++)
                    {
                        for (int l = 0; l < Net[Lastlayer - 1][k].WeightsOut.Count()-1; l++)
                        {
                            Net[Lastlayer - 1][k].WeightsOut[l] += weightDeltas[k, l];
                        }
                    }
                }
            }



        }

        private void Exception_AddLayer(int LayerNumber, int NeuronsInNewLayer)
        {
            if (LayerNumber < 1 || LayerNumber > Net.Count - 1)
            {
                throw new InvalidOperationException("Layer number has to be larger than 0 and less than the index of the output layer which" +
                    " will be incrimented when a layer is successfully added");
            }

            if (NeuronsInNewLayer <= 0)
            {
                throw new InvalidOperationException("New layer must have more than 0 neurons");
            }
        }

        private void Exception_Train(double[] SuppInputs, double[] ExpOutputs)
        {
            if (ExpOutputs.GetUpperBound(0) + 1 != this.NumberOfOutputs)
            {
                throw new Exception("Number of expected outputs not equal to number of actual outputs");
            }

            if (SuppInputs.GetUpperBound(0) + 1 != this.NumberOfInputs)
            {
                throw new Exception("Number of inputs supplied not equal to number of expected inputs");
            }
        }

        private void Exception_Remove(int LayerNumber)
        {
            if (LayerNumber < 1 || LayerNumber >= Net.Count - 1)
            {
                throw new InvalidOperationException("Layer cannot be smaller than 1 or the same as or larger than the output index");
            }
        }

        public void Exception_CheckValidState()
        {
            //Number of hidden vs neurons per hidden 
            if (this.NumberOfHiddenLayers != this.NeuronsPerHiddenLayer.Count())
            {
                throw new InvalidOperationException("Number of neurons per hidden layer does not match number of hidden layers");
            }
        }

        private void Exception_Predict(int SuppliedInputs, int ExpectedInputs)
        {
            if (SuppliedInputs != ExpectedInputs)
            {
                throw new InvalidOperationException("Number of inputs supplied is not equal to the number the net can take (" + ExpectedInputs + ")");
            }
        }

       
        protected override double[] Cost(double [] ActualOutput, double [] ExpectedOutput)
        {
            double[] ReturnArray = new double[ActualOutput.Count()];
            for (int i = 0; i <= ActualOutput.GetUpperBound(0); i++)
            {
                ReturnArray[i] = (ExpectedOutput[i]- ActualOutput[i]);
            }
            return ReturnArray;

        }


        //Activation function?
        protected override double Activation(double ValueIn, Activations activation)
        {
            switch (activation)
            {
                case (Activations.Sigmoid):
                    return (1 / (1 + Math.Exp(-ValueIn)));

                default:
                    return (1 / (1 + Math.Exp(-ValueIn)));
            }
        }
        

      
        private void BackAdjustWeights(int LayerToBeAdjusted, int NeuronNumber)
        {

            if (Net[LayerToBeAdjusted][0].WeightsOut.Count() > Net[LayerToBeAdjusted + 1].Count())
            {
                //Neuron removed so we must remove the relevant weightout
                foreach (Neuron N in Net[LayerToBeAdjusted])
                {
                    var NewWeightsOut = new double[Net[LayerToBeAdjusted + 1].Count];
                    int indexer = 0;
                    for (int i = 0; i < N.WeightsOut.Count(); i++)
                    {
                        if (i != NeuronNumber)
                        {
                            NewWeightsOut[indexer] = N.WeightsOut[i];
                            indexer++;
                        }
                    }
                    N.WeightsOut = NewWeightsOut;
                }
            }
            else
            {
                //Neuron added so we must add a weight out
                
                foreach (Neuron N in Net[LayerToBeAdjusted])
                {
                    var NewWeightsOut = new double[Net[LayerToBeAdjusted + 1].Count];

                    int indexor = 0;
                    foreach (double d in N.WeightsOut)
                    {
                        NewWeightsOut[indexor] = d;
                        indexor++;
                    }
                    N.WeightsOut = NewWeightsOut;
                }
            }
        }
    }
        

}

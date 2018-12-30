using System;
using System.Collections.Generic;

namespace MachineSharpLibrary
{
    public class Net 
    {
        private List<List<Neuron>> _net;

        public int NumberOfInputs
        {
            get
            {
                return _net[0].Count;
            }
        }

        public int NumberOfOutputs
        {
            get
            {
                return _net[_net.Count - 1].Count;
            }
        }

        public int[] NumberOfNeuronsPerHiddenLayer()
        {
            var l = new List<int>();
            foreach (List<Neuron> n in _net)
            {
                l.Add(n.Count);
            }
            return l.ToArray();
        }

        //indexer to return layer when indexed
        public List<Neuron> this[int index]
        {
            get
            {
                return _net[index];
            }
            set
            {
                _net[index] = value;
            }
        }

        //indexer to return neuron when reqeusted
        public Neuron this[int layerNumber, int NeuronNumber]
        {
            get
            {
                return _net[layerNumber][NeuronNumber];
            }

            set
            {
                _net[layerNumber][NeuronNumber] = value;
            }
        }

        //include a default constructor to allow for future constructors
        //to be added if needed.
        public Net()
        {
            _net = new List<List<Neuron>>();
        }

        public void Add(List<Neuron> layerToAdd)
        {
            _net.Add(layerToAdd);
        }

        //returns the number of layers present in the net
        public int Count()
        {
            return (_net == null) ? -1 : _net.Count;
        }

        public void Insert(List<Neuron> NewLayer, int IndexToInsert, bool backAdjustNeeded)
        {
            _net.Insert(IndexToInsert, NewLayer);
            if (backAdjustNeeded)
            {
                BackAdjustWeights(IndexToInsert - 1);
            }
        }

        public void RemoveLayer(int IndexToRemoveFrom, bool backAdjustNeeded)
        {
            _net.RemoveAt(IndexToRemoveFrom);
            if (backAdjustNeeded)
            {
                BackAdjustWeights(IndexToRemoveFrom - 1);
            }
        }

        //public BackAdjust that backadjusts a whole layer when called
        public void BackAdjustWeights(int LayerNumber)
        {
            //todo assert layernumber provided is valid
            int NeuronNo = 0;
            foreach (Neuron N in _net[LayerNumber - 1])
            {
                N.WeightsOut = new double[_net[LayerNumber].Count];
                while (N.WeightsOut.GetUpperBound(0) + 1 != _net[LayerNumber].Count)
                {
                    _backAdjustWeights(LayerNumber - 1, NeuronNo);
                }
                NeuronNo++;
            }
        }

        //internal backadjustment that works on a specific neuron in a specific layer
        private void _backAdjustWeights(int LayerToBeAdjusted, int NeuronNumber)
        {
            if (_net[LayerToBeAdjusted][0].WeightsOutCount > _net[LayerToBeAdjusted + 1].Count)
            {
                //Neuron removed so we must remove the relevant weightout
                foreach (Neuron N in _net[LayerToBeAdjusted])
                {
                    var NewWeightsOut = new double[_net[LayerToBeAdjusted + 1].Count];
                    int indexer = 0;
                    for (int i = 0; i < N.WeightsOutCount; i++)
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

                foreach (Neuron N in _net[LayerToBeAdjusted])
                {
                    var NewWeightsOut = new double[_net[LayerToBeAdjusted + 1].Count];
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


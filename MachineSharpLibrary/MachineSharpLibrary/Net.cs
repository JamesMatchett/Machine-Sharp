using System;
using System.Collections.Generic;

namespace MachineSharpLibrary
{
    public class Net
    {
        private List<List<Neuron>> _net;

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

        public void AddLayer(List<Neuron> NewLayer, int IndexToInsert, bool backAdjustNeeded)
        {
            _net.Insert(IndexToInsert, NewLayer);
            if (backAdjustNeeded)
            {
                //todo
                throw new NotImplementedException();
            }
        }

        public void RemoveLayer(int IndexToRemoveFrom, bool backAdjustNeeded)
        {
            _net.RemoveAt(IndexToRemoveFrom);
            if (backAdjustNeeded)
            {
                backAdjustWeights();
            }
        }

        public void BackAdjustWeights()
        {
            //todo
            throw new NotImplementedException();
        }
    }
}

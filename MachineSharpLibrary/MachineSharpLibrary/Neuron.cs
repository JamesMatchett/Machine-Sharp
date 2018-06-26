using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSharpLibrary
{
    class Neuron
    {
       //first index = weight from neuron to first neuron in next layer
       public double[] WeightsOut { get; set; }
       public double Bias { get; set; }
       public double Activation = 0;
       
        /// <summary>
        /// Create a neuron with empty weights out, bias default to 0
        /// </summary>
        /// <param name="NumberOfLayersOut"></param>
        public Neuron(int NumberOfLayersOut)
        {
            WeightsOut = new Double[NumberOfLayersOut];
            for(int i = 0; i < NumberOfLayersOut; i++)
            {
                WeightsOut[i] = 0;
            }
            Bias = 0;
        }

        /// <summary>
        /// Create a neuron with preset weights out and bias default to 0
        /// </summary>
        /// <param name="PresetWeightsOut"></param>
        public Neuron(Double[] PresetWeightsOut)
        {
            WeightsOut = PresetWeightsOut;
        }


        /// <summary>
        /// Create a neuron with preset weights and bias
        /// </summary>
        /// <param name="PresetWeightsOut"></param>
        /// <param name="_Bias"></param>
        public Neuron(Double[] PresetWeightsOut, double _Bias)
        {
            WeightsOut = PresetWeightsOut;
            Bias = _Bias;
        }

        public Neuron(Double[] PresetWeightsOut, double _Bias, Random random)
        {
            for(int i = 0; i<PresetWeightsOut.Count();i++)
            {
                double Rand = random.Next(-100000, 100000);
                PresetWeightsOut[i] = (Rand / 100000);
            }
            WeightsOut = PresetWeightsOut;
            Bias = _Bias;
        }
    }
}

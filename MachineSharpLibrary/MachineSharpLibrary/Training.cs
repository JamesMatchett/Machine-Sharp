using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSharpLibrary
{
    class Training
    {
        //This class inherits neural network and has the ability to correctively alter weightings and biases depending on the set
        //of expected outputs and actual outputs

        /// <summary>
        /// Constructs the Training class through the neural net to be trained
        /// </summary>
        /// <param name="_NeuralNetwork">Neural network to be trained</param>
        private NeuralNetwork neuralNetwork;
        public Training(NeuralNetwork _NeuralNetwork)
        {
            neuralNetwork = _NeuralNetwork;
        }




        /// <summary>
        /// Trains the neural net by simulating an input and comparing the expected output with the actual output
        /// </summary>
        /// <param name="InputExpectedOutputList">Pairs of input data and their respective expected outputs</param>
        /// <returns></returns>
        public Double Train(List<InputExpectedOutputPair> InputExpectedOutputList)
        {
            //Calculate total cost of the network over all examples given
            double CumulativeCost = 0;
            foreach (InputExpectedOutputPair IEO in InputExpectedOutputList)
            {
                //Run number through neural net
                //Compare output with expected output
                CumulativeCost += CalculateCost(IEO);
            }
            Double AverageCost = (CumulativeCost / InputExpectedOutputList.Count);

            //Now focus on ONE training example
            //nudge each neuron in approparite direciton
            //magnitude of nudge is proportional to distance from ideal value
            Double[] Difference = CalculateDifferencePerNeuron(InputExpectedOutputList.First());
            
            //now we can nudge the neuron by either changing bias, changing weighting, change activation from previous layer
            //Focus on neurons from previous layers with stronger activations as they are more relevant to this single example
            //compute this for all outputs, even if they are outputs we do not want as we can aim to reduce them to 0
            //TODO Nudge function

        }

        /// <summary>
        /// Calculates the cost of one training example for all the whole network
        /// </summary>
        /// <param name="IEO"></param>
        /// <returns></returns>
        private double CalculateCost(InputExpectedOutputPair IEO)
        {
            Double[] Result = neuralNetwork.Predict(IEO.InputArray);
            //square of the difference between the expected and actual results
            int iterator = 0;
            double difference = 0;
            foreach(double D in IEO.ExpectedOutput)
            {
                difference += Math.Pow(D - IEO.ExpectedOutput[iterator], 2);
                iterator++;
            }
            return difference;
        }



        /// <summary>
        /// Returns the difference for each neuron bewteen the expected values and actual values
        /// </summary>
        /// <param name="IEO"></param>
        /// <returns></returns>
        private double[] CalculateDifferencePerNeuron(InputExpectedOutputPair IEO)
        {
            Double[] Result = neuralNetwork.Predict(IEO.InputArray);
            int iterator = 0;
            foreach (double D in IEO.ExpectedOutput)
            {
                Result[iterator] = D - Result[iterator];
                iterator++;
            }
            return Result;
        }
    }

    public class InputExpectedOutputPair
    {
        public double[] InputArray;
        public double[] ExpectedOutput;

        public InputExpectedOutputPair(double[] _InputArray, double[] _ExpectedOutput)
        {
            InputArray = _InputArray;
            ExpectedOutput = _ExpectedOutput;
        }

    }
}

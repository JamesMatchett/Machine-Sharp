using System;
using System.Collections.Generic;
namespace MachineSharpLibrary
{
    public class BinaryNetFactory 
    {
        private List<BinaryNet> NetList = new List<BinaryNet>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MachineSharpLibrary.BinaryNetFactory"/> class.
        /// </summary>
        /// <param name="ExpetedAnswersIn">Double array of the Expeted answers of each net.</param>
        /// <param name="NumberOfInputs">Number of inputs each net should take</param>
        /// <param name="NeuronsPerHiddenLayer">Int array of Neurons per hidden layer.</param>
        /// <param name="random">Nullable rand, include a random to randomly set weights</param>
        public BinaryNetFactory(double[] ExpetedAnswersIn, int NumberOfInputs,
        int[] NeuronsPerHiddenLayer, Random random = null)
        {
            for (int i = 0; i <= ExpetedAnswersIn.GetUpperBound(0); i++)
            {
                LMMCNet lMMC = new LMMCNet(NumberOfInputs, NeuronsPerHiddenLayer.GetUpperBound(0) + 1,
                NeuronsPerHiddenLayer, 1, random);
                NetList.Add(new BinaryNet(lMMC, ExpetedAnswersIn[i]));
            }
        }

        public double[] Predict(double[] Inputs)
        {
            double Answer = 0;
            double Strength = 0;
            //todo Parallel.foreach
            foreach (BinaryNet B in NetList)
            {
                var result = B.Net.Predict(Inputs);
                if (result[0] > Strength)
                {
                    Strength = result[0];
                    Answer = B.SoughtAnswer;
                }
            }
            return new double[] { Answer };

        }

        public void Train(double[] Inputs, double ExpectedOutput)
        {
            double[] ExpectedOutputs = new double[] {0 };
            //todo Parallel.foreach
            foreach (BinaryNet B in NetList)
            {
                if (ExpectedOutput.ToString() == B.SoughtAnswer.ToString())
                {
                    ExpectedOutputs[0] = 1;
                }
                else
                {
                    ExpectedOutputs[0] = 0;
                }

                B.Net.Train(Inputs, ExpectedOutputs);
            }
        }
    }
}

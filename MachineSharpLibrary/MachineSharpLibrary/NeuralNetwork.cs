using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSharpLibrary
{
    public class NeuralNetwork
    {
        public Matrix ihWeights { get; set; }
        public Matrix hoWeights { get; set; }
        private int NoOfInputs { get; set; }
        private int NoOfHidden { get; set; }
        private int NoOfOutputs { get; set; }

        public NeuralNetwork(int inputs, int hidden, int outputs)
        {
            Random random = new Random();
            ihWeights = new Matrix(hidden, inputs, random);
            hoWeights = new Matrix(outputs, hidden, random);
            NoOfInputs = inputs;
            NoOfHidden = hidden;
            NoOfOutputs = outputs;
        }

        /// <summary>
        /// Makes a prediction from the given inputs
        /// </summary>
        /// <returns></returns>
        public double[] Predict(double[] inputArray)
        {
            Matrix inputs = Matrix.GetFromArray(inputArray);
            Matrix hidden_outputs = new Matrix(NoOfHidden, 1);
            Matrix outputs = new Matrix(NoOfOutputs, 1);
            
            //Sumation
            double sum = 0;
            for(int i = 0; i < ihWeights.Rows; i++)
            {
                sum = 0;
                for (int j = 0; j < ihWeights.Cols; j++)
                {
                    sum += inputs.Data[j, 0] * ihWeights.Data[i, j];
                }
                hidden_outputs.Data[i, 0] = sum;
            }

            for (int i = 0; i < hoWeights.Rows; i++)
            {
                sum = 0;
                for (int j = 0; j < hoWeights.Cols; j++)
                {
                    sum += hidden_outputs.Data[j, 0] * hoWeights.Data[i, j];
                }
                //Sigmoid 'Squishification' function
                outputs.Data[i, 0] = 1/(1 + Math.Exp(-sum));
            }
            return Matrix.ConvertToArray(outputs);
        }

        public void Train(double[] inputs, double answer)
        {

        }
    }
}

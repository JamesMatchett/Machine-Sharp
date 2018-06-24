using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSharpLibrary
{
    class Training : NeuralNetwork
    {
        //This class inherits neural network and has the ability to correctively alter weightings and biases depending on the set
        //of expected outputs and actual outputs

            /// <summary>
            /// Constructs the Training class and through inheritance the neural net to be trained
            /// </summary>
            /// <param name="_Inputs"></param>
            /// <param name="_hidden"></param>
            /// <param name="_outputs"></param>
        public Training(int _Inputs, int _hidden, int _outputs) : base(_Inputs, _hidden, _outputs)
        {
            
        }

        //Allow an already existing neural net to be trained
        public Training(NeuralNetwork _NeuralNetwork) : base(_NeuralNetwork)
        {

        }

        /// <summary>
        /// Trains the neural net by simulating an input and comparing the expected output with the actual output
        /// </summary>
        /// <param name="InputArray">The input data to to the neural net</param>
        /// <param name="ExpectedOutput">The expected output from the neural net</param>
        /// <returns></returns>
        public Double Train(double[] InputArray, double[] ExpectedOutput)
        {
            //Run number through neural net
            //Compare output with expected output
            Double[] Result = this.Predict(InputArray);
            //find best weighting to adjust

            //find direction to adjust (either up or down)

            //recursively adjust until local mininum found

            //apply adjustment to neural net
        }

        

        


    }
}

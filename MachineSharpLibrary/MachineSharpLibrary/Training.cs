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

        


    }
}

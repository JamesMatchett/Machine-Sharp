using System;
namespace MachineSharpLibrary
{
    //static class that contains a list of static methods that take a ref net and
    //parameters to work out if an operation is valid
    //e.g. adding/removing a layer etc
    public static class NetExceptionsFactory
    {
        public static void Exception_AddLayer(ref Net net, int LayerNumber, int NeuronsInNewLayer)
        {
            if (LayerNumber < 1 || LayerNumber > net.Count() - 1)
            {
                throw new InvalidOperationException("Layer number has to be larger than 0 and less than the index of the output layer which" +
                    " will be incrimented when a layer is successfully added");
            }

            if (NeuronsInNewLayer <= 0)
            {
                throw new InvalidOperationException("New layer must have more than 0 neurons");
            }
        }

        public static void Exception_Train(ref Net net, double[] SuppInputs, double[] ExpOutputs)
        {
            if (ExpOutputs.GetUpperBound(0) + 1 != net.NumberOfOutputs)
            {
                throw new Exception("Number of expected outputs not equal to number of actual outputs");
            }

            if (SuppInputs.GetUpperBound(0) + 1 != net.NumberOfInputs)
            {
                throw new Exception("Number of inputs supplied not equal to number of expected inputs");
            }
        }

        public static void Remove(ref Net net, int LayerNumber)
        {
            if (LayerNumber < 1 || LayerNumber >= net.Count() - 1)
            {
                throw new InvalidOperationException("Layer cannot be smaller than 1 or the same as or larger than the output index");
            }
        }


        public static void Predict(ref Net net, int SuppliedInputs, int ExpectedInputs)
        {
            if (SuppliedInputs != ExpectedInputs)
            {
                throw new InvalidOperationException("Number of inputs supplied is not equal to the number the net can take (" + ExpectedInputs + ")");
            }
        }
    }
}

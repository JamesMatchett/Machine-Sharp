using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSharpLibrary
{
    interface INet
    {
        //public facing methods
        double[] Predict(double[] Inputs);
        void Train(double[] Inputs, double[] ExpectedOutputs);
    }

    public abstract class INetBase : INet
    {
        public abstract Activations ActivationsFunction { get; set; }

        public abstract double[] Predict(double[] Inputs);
        public abstract void Train(double[] Inputs, double[] ExpectedOutputs);
        protected abstract double Activation(double ValueIn, Activations activation);
        protected abstract double Cost(double[] ActualOutput, double[] ExpectedOutput);
    }

    public class Enums
    {
        public enum Activations { Sigmoid, DSigmoid }
        public enum CostFunctions { }
    }

}

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
        protected abstract Activations ActivationsFunction { get; set; }
        private double _LearningRate { get; set; }

        protected double LearningRate
        {
            get { return _LearningRate; }
            set { _LearningRate = (value >= 0) ? _LearningRate = value : _LearningRate = _LearningRate; }
        }


        public abstract double[] Predict(double[] Inputs);
        public abstract void Train(double[] Inputs, double[] ExpectedOutputs = null);
        protected abstract double Activation(double ValueIn, Activations activation);
        protected abstract double[] Cost(double[] ActualOutput, double[] ExpectedOutput = null);
        protected abstract void InitNet();



        public enum Activations { Sigmoid, DSigmoid }

        public double Activation(Activations activations, double ValueIn)
        {
            switch (activations)
            {
                case (Activations.Sigmoid):
                    return (1 / (1 + Math.Exp(-ValueIn)));
                    break;

                case (Activations.DSigmoid):
                    return (ValueIn * (1 - ValueIn));
                    break;

                default:
                    throw new Exception("Invalid activation");

            }

        }

    }
    public enum CostFunctions { }



}

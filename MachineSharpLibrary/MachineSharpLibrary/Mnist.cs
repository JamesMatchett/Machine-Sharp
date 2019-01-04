using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSharpLibrary
{
    public class Mnist
    {
        public double[] Data { get; set; }
        public int Label { get; set; }

        public Mnist(double[] data, int label)
        {
            Data = data;
            Label = label;
        }
    }
}

using System;
namespace MachineSharpLibrary
{
    public class BinaryNet
    {
        public BinaryNet(Net _Net, double _SoughtAnswer)
        {
            Net = _Net;
            SoughtAnswer = _SoughtAnswer;
        }

        public Net Net { get; set; }
        public double SoughtAnswer { get; set; }
    }
}

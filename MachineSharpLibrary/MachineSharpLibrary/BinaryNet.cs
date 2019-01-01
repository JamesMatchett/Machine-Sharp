using System;

namespace MachineSharpLibrary
{
    public class BinaryNet 
    {
        public BinaryNet(INet _Net, double _SoughtAnswer)
        {
            Net = _Net;
            SoughtAnswer = _SoughtAnswer;
        }

        public INet Net { get; set; }
        
        public double SoughtAnswer { get; set; }

       
    }
}

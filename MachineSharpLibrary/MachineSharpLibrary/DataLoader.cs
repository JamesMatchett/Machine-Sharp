using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSharpLibrary
{
    class DataLoader
    {
        private string _path { get; set; }
        private StreamReader _sr { get; set; }
        private Mnist _internalNext {get;set;}
        private Mnist _internalSecond { get; set; }
        private Mnist _internalTemp { get; set; }

        public DataLoader(string path)
        {
            _path = path;
            _sr = new StreamReader(_path);
            _internalNext = LoadNext(1);
        }

        //Deconstructor to close streams and save memory
        ~DataLoader()
        {
            _path = null;
            _sr.Close();
        }

        public Mnist Next()
        {
            _internalTemp = _internalNext;
             BumpQueue();
            return _internalTemp;
        }

       private void BumpQueue() 
       {
            Task.Run(() =>
            {
                _internalNext = _internalSecond;
                _internalSecond = LoadNext(1);
            }
            );
       }


        private Mnist LoadNext(int NumberToLoad)
        {
            int i = 0;
            StringBuilder build = new StringBuilder();
            int index = -1;
            int label = 0;
            double[] data = new double[28 * 28];
            while (!_sr.EndOfStream && i < NumberToLoad)
            {
                int next = _sr.Read() - 48;
                if (next == -4)
                {
                    if (index == -1)
                    {
                        label = Convert.ToInt32(build);
                        index++;
                    }
                    else
                    {
                        data[index] = Convert.ToInt32(build);
                        index++;
                    }

                    if (index == (28 * 28) - 1)
                    {
                        index = -1;
                        data = new double[28 * 28];
                        build.Clear();
                        _sr.Read();
                        _sr.Read();
                        return (new Mnist(data, label));
                    }

                    build.Clear();
                }
                else
                {
                    //check for line breaks & spaces
                    string testString = build.ToString();
                    if (testString.Contains(@"\"))
                    {
                        build = build.Clear();
                        build.Append(testString.Remove(testString.IndexOf(@"\")));
                    }
                    if (testString.Contains(@"n"))
                    {
                        build.Clear();
                        build.Append(testString.Remove(testString.IndexOf(@"n")));
                    }
                    build.Append(next);
                }
                i++;
            }

            //should only reach here when there is an eofstream exception
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSharpLibrary
{
    public class Matrix
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public double[,] Data { get; set; }
        private Random random;

        public Matrix(int rows, int cols, bool randomize = true)
        {
            Rows = rows;
            Cols = cols;
            Data = new double[Rows, Cols];
            random = new Random();
            if (randomize)
            {
                RandomizeValues();
            }
        }

        public void RandomizeValues()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i, j] = (float)random.Next(0, 10);
                }
            }
        }

        public void Add(int x)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i, j] += x;
                }
            }
        }

        public void Multiply(float x)
        {
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Cols; j++)
                {
                    Data[i, j] *= x;
                }
            }
        }

        public static Matrix Multiply(Matrix a, Matrix b)
        {
            if(a.Rows != b.Cols)
            {
                throw new ArgumentException("Rows of A must Equal Rows of B");
            }
            Matrix c = new Matrix(a.Rows, b.Cols);
            double tempSum = 0;
            for (int i = 0; i < a.Rows; i++)
            {
                for (int k = 0; k < b.Cols; k++)
                {
                    tempSum = 0;
                    for (int j = 0; j < a.Cols; j++)
                    {
                        tempSum += a.Data[i, j] * b.Data[j, i];
                    }
                    c.Data[i, k] = tempSum;
                }
            }
            return c;
        }

        public Matrix Transpose()
        {
            Matrix b = new Matrix(Cols, Rows);
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Cols; j++)
                {
                    b.Data[j, i] = Data[i, j];
                }
            }
            return b;
        }
    }
}

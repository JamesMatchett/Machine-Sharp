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

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Data = new double[Rows, Cols];
        }

        //called when a matrix should be filled with random numbers
        public Matrix(int rows, int cols, Random random)
        {
            Rows = rows;
            Cols = cols;
            Data = new double[Rows, Cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i, j] = random.Next(0, 10);
                }
            }
        }


        //Add a single scalar value to all values in matrix
        public void Add(double x)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i, j] += x;
                }
            }
        }

        //Add element by element of matrix
        public void Add(Matrix x, Matrix y)
        {
            Matrix z= new Matrix(x.Rows, x.Cols);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i, j] = x.Data[i, j] + y.Data[i, j];
                }
            }
        }

        //Multiply each item in the matrix by a scalar value
        public void Multiply(double x)
        {
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Cols; j++)
                {
                    Data[i, j] *= x;
                }
            }
        }

        //Dot product of Matricies
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

        //Change all rows of matrix to columns and all columns to rows
        //and return as new matrix
        public static Matrix Transpose(Matrix a)
        {
            Matrix b = new Matrix(a.Cols, a.Rows);
            for(int i = 0; i < a.Rows; i++)
            {
                for(int j = 0; j < a.Cols; j++)
                {
                    b.Data[j, i] = a.Data[i, j];
                }
            }
            return b;
        }
    }
}

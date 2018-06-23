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
        public double LearningRate { get; set; }
        public double[,] Data { get; set; }

        /// <summary>
        /// Create an empty matrix
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="cols">Number of columns</param>
        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Data = new double[Rows, Cols];
        }

        /// <summary>
        /// Create a new matrix with random numbers
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="cols">Number of columns</param>
        /// <param name="random">Random to generate random numbers, when creating many 
        /// random matricies use the same Random variable!</param>
        public Matrix(int rows, int cols, Random random)
        {
            Rows = rows;
            Cols = cols;
            Data = new double[Rows, Cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i, j] = random.NextDouble();
                }
            }
        }


        /// <summary>
        /// Add a single scalar value to all values in matrix
        /// </summary>
        /// <param name="x">Scalar value to be added</param>
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

        /// <summary>
        /// Add element by element of matrix
        /// </summary>
        /// <remarks>
        /// Matrix x multiplied by Matrix y is NOT equal to Matrix y 
        /// multiplied by Matrix x
        /// </remarks>
        /// <param name="x">First matrix</param>
        /// <param name="y">Second matrix</param>
        public Matrix Add(Matrix x, Matrix y)
        {
            if(x.Cols != y.Cols || x.Rows != y.Rows)
            {
                throw new ArgumentException("Matrix dimensions must be equal to add!");
            }
            Matrix z = new Matrix(x.Rows, x.Cols);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    z.Data[i, j] = x.Data[i, j] + y.Data[i, j];
                }
            }
            return z;
        }

        /// <summary>
        /// Creates a Matrix from a single dimensional double array
        /// </summary>
        /// <param name="values"></param>
        /// <returns>Matrix from double array</returns>
        public static Matrix GetFromArray(double[] array)
        {
            Matrix m = new Matrix(array.Length, 1);
            for(int i = 0; i < array.Length; i++)
            {
                m.Data[i, 0] = array[i];
            }
            return m;
        }

        public static double[] ConvertToArray(Matrix matrix)
        {
            double[] values = new double[matrix.Cols * matrix.Rows];
            for(int i = 1; i <= matrix.Rows; i++)
            {
                for (int j = 1; j <= matrix.Cols; j++)
                {
                    values[(i * j) - 1] = matrix.Data[i-1, j-1];
                }
            }
            return values;
        }

        /// <summary>
        /// Multiply each item in the matrix by a scalar value
        /// </summary>
        /// <param name="x">Scalar value</param>
        public void Multiply(double x)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i, j] *= x;
                }
            }
        }

        /// <summary>
        /// //Dot product of Matricies
        /// </summary>
        /// <param name="a">First matrix</param>
        /// <param name="b">Second matrix</param>
        /// <returns></returns>
        public static Matrix Multiply(Matrix a, Matrix b)
        {
            if (a.Rows != b.Cols)
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

        /// <summary>
        /// Change all rows of matrix to columns and all columns to rows
        //  and return as a new matrix
        /// </summary>
        /// <param name="a">Input matrix</param>
        /// <returns>Matrix as a row</returns>
        public static Matrix Transpose(Matrix a)
        {
            Matrix b = new Matrix(a.Cols, a.Rows);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    b.Data[j, i] = a.Data[i, j];
                }
            }
            return b;
        }
    }
}

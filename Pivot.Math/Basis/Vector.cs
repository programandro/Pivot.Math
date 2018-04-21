using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pivot.Math.Basis
{
    public class Vector : ICloneable
    {
        private int _length;
        private double[] _values;

        public Vector(int length)
        {
            if (length <= 0)
                throw new Exception("Length less than or iqual zero!");

            _values = Enumerable.Repeat<double>(0, _length).ToArray();
        }

        public Vector(params double[] values)
        {
            _length = values.Length;
            _values = new double[_length];
            for (var i = 0; i < _length; i++)
                _values[i] = values[i];
        }

        public static Vector Parse(string vector)
        {
            if ((vector[0] != '(') || (vector[vector.Length - 1] != ')')) throw new ArgumentException("Bad string parsing!");
            string tvector = vector.Substring(1, vector.Length - 2);
            string[] v = tvector.Split(',');
            int n = v.Length;
            Vector result = new Vector(n);
            for (int i = 0; i < n; i++)
                result[i] = double.Parse(v[i]);
            return result;
        }

        public override string ToString()
        {
            string result = "(";
            for (int i = 0; i < _length - 1; i++)
                result = result + _values[i].ToString() + ", ";
            result = result + _values[_length - 1].ToString() + ")";
            return result;
        }

        public static Vector NullVector(int length)
            => new Vector(length);

        public virtual double this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

        public virtual int Length => _length;

        public static Vector CanonicVector(int index, int length)
        {
            var result = NullVector(length);
            result[index] = 1;
            return result;
        }

        public static Vector operator *(Vector vector, double constant)
        {
            Vector mult = new Vector(vector.Length);
            for (int i = 0; i < vector.Length; i++)
                mult[i] = vector[i] * constant;
            return mult;
        }

        public static Vector operator *(double constant, Vector vector)
        {
            Vector mult = new Vector(vector.Length);
            for (int i = 0; i < vector.Length; i++)
                mult[i] = vector[i] * constant;
            return mult;
        }

        public static Vector operator -(Vector vector1, Vector vector2)
        {
            if (vector1.Length != vector2.Length)
                throw new Exception("Can't sum different size vectors");
            var result = new Vector(vector1.Length);
            for (int i = 0; i < vector1.Length; i++)
                result[i] = vector1[i] - vector2[i];
            return result;
        }

        public static Vector operator -(Vector vector)
            => vector * (-1);

        public virtual Vector PlusMult(double constant, Vector vector)
            => this + (vector * constant);

        public static double operator *(Vector vector1, Vector vector2)
        {
            double sum = 0;
            for (int i = 0; i < vector1.Length; i++)
                sum += vector1[i] * vector2[i];
            return sum;
        }

        public virtual Matrix MultToMatrix(Vector vector)
        {
            var result = new Matrix(_length, vector.Length);
            for (int i = 0; i < _length; i++)
                for (int j = 0; j < vector.Length; j++)
                    result[i, j] = this[i] * vector[j];
            return result;
        }

        public virtual Matrix ToMatrix(bool horizontal)
        {
            Matrix M;
            if (horizontal)
                M = new Matrix(1, _length);
            else M = new Matrix(_length, 1);
            for (int i = 0; i < _length; i++)
                if (horizontal)
                    M[0, i] = this[i];
                else M[i, 0] = this[i];
            return M;
        }

        public static Vector operator *(Vector vector, Matrix matrix)
        {
            if (matrix.Rows != vector.Length)
                throw new Exception("Can't multiply!");
            Vector result = new Vector(matrix.Columns);
            for (int i = 0; i < matrix.Columns; i++)
            {
                double sum = 0;
                for (int j = 0; j < vector.Length; j++)
                    sum += vector[j] * matrix[j, i];
                result[i] = sum;
            }
            return result;
        }

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            if (vector1.Length != vector2.Length)
                throw new Exception("Cant`n sum two vectors whit diferent dimensions");
            Vector sum = new Vector(vector1.Length);
            for (int i = 0; i < vector1.Length; i++)
                sum[i] = vector1[i] + vector2[i];
            return sum;
        }

        public static Vector operator /(Vector vector, double constant)
            => vector * (1 / constant);

        public virtual Vector Concat(Vector vector)
        {
            Vector result = new Vector(_length + vector.Length);
            for (int i = 0; i < _length; i++)
                result[i] = _values[i];
            for (int i = _length; i < vector.Length; i++)
                result[i] = vector[i - _length];
            return result;
        }

        public virtual Vector SubVector(int from, int to)
        {
            Vector result = new Vector(to - from + 1);
            for (int i = from; i <= to; i++)
                result[i - from] = _values[i];
            return result;
        }

        #region ICloneable Members

        public object Clone()
        {
            var vector = new Vector(_length);
            for (int i = 0; i < _length; i++)
                vector[i] = _values[i];
            return vector;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Pivot.Math.Basis
{
    public class Matrix : ICloneable
    {
        protected double[,] _values;
        protected int _rows;
        protected int _columns;

        public Matrix(int rows, int columns)
        {
            _values = new double[rows, columns];
            _rows = rows;
            _columns = columns;
        }

        public Matrix(int size) : this(size, size) { }

        public static Matrix NullMatrix(int rows, int columns)
        {
            var matrix = new Matrix(rows, columns);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    matrix[i, i] = 0;
            return matrix;
        }

        public static Matrix Identity(int rows, int columns)
        {
            var identity = new Matrix(rows, columns);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    if (i == j) identity[i, j] = 1;
                    else identity[i, j] = 0;
            return identity;
        }

        public static Matrix IdentityMatrix(int size)
            => Identity(size, size);

        public virtual double this[int rowIndex, int columnIndex]
        {
            get => _values[rowIndex, columnIndex];
            set => _values[rowIndex, columnIndex] = value;
        }

        public virtual int Rows => _rows;

        public virtual int Columns => _columns;

        public static Matrix operator /(Matrix matrix, double constant)
            => matrix * (1 / constant);

        public static Matrix operator -(Matrix matrix)
            => matrix * (-1);

        #region ICloneable Members

        public virtual object Clone()
        {
            var matrix = new Matrix(_rows, _columns);
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _columns; j++)
                    matrix[i, j] = this[i, j];
            return matrix;
        }

        #endregion

        public virtual void InterchangeRows(int index1, int index2)
        {
            double temp;
            for (int i = 0; i < this._columns; i++)
            {
                temp = this[index1, i];
                this[index1, i] = this[index2, i];
                this[index2, i] = temp;
            }
        }

        public virtual void InterchangeColumns(int index1, int index2)
        {
            double temp;
            for (int i = 0; i < _rows; i++)
            {
                temp = this[i, index1];
                this[i, index1] = this[i, index2];
                this[i, index2] = temp;
            }
        }

        public virtual bool IsSquare => Rows == Columns;

        public virtual Matrix Transpose()
        {
            var matrix = new Matrix(_columns, _rows);
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _columns; j++)
                    matrix[i, j] = this[j, i];
            return matrix;
        }

        public static Matrix operator *(Matrix matrix, double constant)
        {
            int col = matrix.Columns, r = matrix.Rows;
            var result = new Matrix(col, r);
            for (int i = 0; i < r; i++)
                for (int j = 0; j < col; j++)
                    result[i, j] = matrix[i, j] * constant;
            return result;
        }

        public static Matrix operator *(double constant, Matrix matrix)
            => matrix * constant;

        public static Vector operator *(Matrix matrix, Vector vector)
        {
            if (vector.Length != matrix.Columns)
                throw new Exception("Can't multiply!");
            Vector result = new Vector(matrix.Columns);
            for (int i = 0; i < matrix.Rows; i++)
            {
                double s = 0;
                for (int j = 0; j < matrix.Columns; j++)
                    s += matrix[i, j] * vector[j];
                result[i] = s;
            }
            return result;
        }

        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            int constant = matrix1.Columns, r = matrix1.Rows;
            if ((constant != matrix2.Columns) || (r != matrix2.Rows))
                throw new Exception("Can't sum!");
            Matrix sum = new Matrix(constant, r);
            for (int i = 0; i < r; i++)
                for (int j = 0; j < constant; j++)
                    sum[i, j] = matrix1[i, j] + matrix2[i, j];
            return sum;
        }

        public static Matrix operator +(Matrix matrix, Vector vector)
        {
            int constant = matrix.Columns, r = matrix.Rows;
            if (((constant > 1) && (r > 1)) || ((r != vector.Length) && (constant != vector.Length)))
                throw new Exception("Can't sum");
            Matrix result = new Matrix(r, constant);
            if (constant == 1)
            {
                for (int i = 0; i < vector.Length; i++)
                    result[i, 0] = matrix[i, 0] + vector[i];
            }
            else
            {
                for (int i = 0; i < vector.Length; i++)
                    result[0, i] = matrix[0, i] + vector[i];
            }
            return result;
        }

        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            int constant = matrix1.Columns, r = matrix1.Rows;
            if ((constant != matrix2.Columns) || (r != matrix2.Rows))
                throw new Exception("Can't rest!");
            Matrix rest = new Matrix(r, constant);
            for (int i = 0; i < r; i++)
                for (int j = 0; j < constant; j++)
                    rest[i, j] = matrix1[i, j] - matrix2[i, j];
            return rest;
        }

        public static Matrix operator -(Matrix matrix, Vector vector)
        {
            int constant = matrix.Columns, r = matrix.Rows;
            if (((constant > 1) && (r > 1)) || ((r != vector.Length) && (constant != vector.Length)))
                throw new Exception("Can't sum");
            Matrix result = new Matrix(r, constant);
            if (constant == 1)
            {
                for (int i = 0; i < vector.Length; i++)
                    result[i, 0] = matrix[i, 0] - vector[i];
            }
            else
            {
                for (int i = 0; i < vector.Length; i++)
                    result[0, i] = matrix[0, i] - vector[i];
            }
            return result;
        }


        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            int c1 = matrix1.Columns, r1 = matrix1.Rows, c2 = matrix2.Columns, r2 = matrix2.Rows;
            if (c1 != r2)
                throw new Exception("Can't multilply!");
            Matrix mult = new Matrix(r1, c2);
            for (int i = 0; i < r1; i++)
                for (int k = 0; k < c2; k++)
                {
                    double s = 0;
                    for (int j = 0; j < c1; j++)
                        s += matrix1[i, j] * matrix2[j, k];
                    mult[i, k] = s;
                }
            return mult;
        }

        public virtual Vector RowToVector(int index)
        {
            Vector result = new Vector(_columns);
            for (int i = 0; i < _columns; i++)
                result[i] = this[index, i];
            return result;
        }

        public virtual Vector ColumnToVector(int index)
        {
            Vector result = new Vector(_rows);
            for (int i = 0; i < _rows; i++)
                result[i] = this[i, index];
            return result;
        }

        public virtual void ChangeRow(int index, Vector vector)
        {
            if (vector.Length != _columns)
                throw new Exception("Invalid vector");
            for (int i = 0; i < _columns; i++)
                this[index, i] = vector[i];
        }

        public virtual void ChangeColumn(int index, Vector vector)
        {
            if (vector.Length != _rows)
                throw new Exception("Invalid vector!");
            for (int i = 0; i < _rows; i++)
                this[i, index] = vector[i];
        }

        /// <summary>
        /// Multiply a source row by a constant and adds the result to a destination row
        /// </summary>
        public virtual void ModifyRows(int destIndex, int sourceIndex, double constant)
        {
            for (int i = 0; i < _columns; i++)
                this[destIndex, i] = this[sourceIndex, i] * constant;
        }

        /// <summary>
        /// Multiply a source column by a constant and adds the result to a destination column
        /// </summary>
        public virtual void ModifyColumns(int destIndex, int sourceIndex, double constant)
        {
            for (int i = 0; i < _rows; i++)
                this[i, destIndex] = this[i, sourceIndex] * constant;
        }

        public virtual void SumRows(int sourceIndex, int destIndex)
        {
            ModifyRows(destIndex, sourceIndex, 1);
        }

        public virtual void RestRows(int sourceIndex, int destIndex)
        {
            ModifyRows(destIndex, sourceIndex, -1);
        }

        public virtual void SumColumns(int sourceIndex, int destIndex)
        {
            ModifyColumns(destIndex, sourceIndex, 1);
        }

        public virtual void RestColumns(int sourceIndex, int destIndex)
        {
            ModifyColumns(destIndex, sourceIndex, -1);
        }

        public virtual Matrix Minor(int excludeRow, int excludeColumn)
        {
            Matrix matrix = new Matrix(_rows - 1, _columns - 1);
            int ki = 0, kj = 0;
            for (int i = 0; i < _rows - 1; i++)
            {
                if (i == excludeRow) ki = 1;
                for (int j = 0; j < _columns - 1; j++)
                {
                    if (j >= excludeColumn) kj = 1; else kj = 0;
                    matrix[i, j] = this[i + ki, j + kj];
                }
            }
            return matrix;
        }

        public virtual Matrix PrincipalMinor(int size)
        {
            if ((size > _columns) || (size > _rows))
                throw new Exception("Invalid minor!");
            Matrix matrix = new Matrix(size, size);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    matrix[i, j] = this[i, j];
            return matrix;
        }

        public override string ToString()
        {
            string s = "( ";
            for (int i = 0; i < _rows - 1; i++)
                s += RowToVector(i).ToString() + " , ";
            s += RowToVector(_rows - 1).ToString() + " )";
            return s;
        }

        public virtual Matrix PermuteRows(params int[] permutationIndexs)
        {
            Matrix matrix = new Matrix(_rows, _columns);
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    matrix[i, j] = this[permutationIndexs[i], j];
                }
            }
            return matrix;
        }

        public virtual Matrix PermuteColumns(params int[] permutationIndexs)
        {
            Matrix matrix = new Matrix(_rows, Columns);
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    matrix[i, j] = this[i, permutationIndexs[j]];
                }
            }
            return matrix;
        }
    }
}

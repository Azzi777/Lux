using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lux.Framework
{
    /// <summary>
    /// Data structure representing a 4 by 4 matrix.
    /// </summary>
    public struct Matrix4
    {
        internal OpenTK.Matrix4d OpenTKEquivalent
        {
            get
            {
                return new OpenTK.Matrix4d(
                        this[0, 0], this[0, 1], this[0, 2], this[0, 3],
                        this[1, 0], this[1, 1], this[1, 2], this[1, 3],
                        this[2, 0], this[2, 1], this[2, 2], this[2, 3],
                        this[3, 0], this[3, 1], this[3, 2], this[3, 3]);
            }
        }

        /// <summary>
        /// The 16-cell data array of the matrix. Each index ranges from 0 to 3.
        /// </summary>
        public double[] Data { get; set; }

        /// <summary>
        /// An accessor used to reach the data array of the matrix.
        /// </summary>
        /// <param name="i">The index, ranging from 0 to 15.</param>
        /// <returns>The value in the cell given by index i.</returns>
        public double this[int i]
        {
            get
            {
                if (Data == null)
                {
                    Data = new double[16];
                }
                return Data[i];
            }
            set
            {
                if (Data == null)
                {
                    Data = new double[16];
                }
                Data[i] = value;
            }
        }

        /// <summary>
        /// An accessor used to reach the data array of the matrix.
        /// </summary>
        /// <param name="i">The horizontal index, ranging from 0 to 3.</param>
        /// <param name="j">The vertical index, ranging from 0 to 3.</param>
        /// <returns>The value in the cell given by indices i and j.</returns>
        public double this[int i, int j]
        {
            get
            {
                if (Data == null)
                {
                    Data = new double[16];
                }
                return Data[i + j * 4];
            }
            set
            {
                if (Data == null)
                {
                    Data = new double[16];
                }
                Data[i + j * 4] = value;
            }
        }

        /// <summary>
        /// Creates a new identity matrix.
        /// </summary>
        static public Matrix4 Identity
        {
            get
            {
                return new Matrix4(OpenTK.Matrix4d.Identity);
            }
        }

        public Matrix4(double m11, double m12, double m13, double m14, double m21, double m22, double m23, double m24, double m31, double m32, double m33, double m34, double m41, double m42, double m43, double m44)
            : this()
        {
            Data = new double[16] 
			{
				m11, m12, m13, m14, 
				m21, m22, m23, m24, 
				m31, m32, m33, m34,
				m41, m42, m43, m44
			};
        }

        internal Matrix4(OpenTK.Matrix4d inMat)
            : this()
        {
            Data = new double[16] 
			{
				inMat.M11, inMat.M12, inMat.M13, inMat.M14, 
				inMat.M21, inMat.M22, inMat.M23, inMat.M24, 
				inMat.M31, inMat.M32, inMat.M33, inMat.M34, 
				inMat.M41, inMat.M42, inMat.M43, inMat.M44, 
			};
        }

        internal void FromOpenTKEquivalent(OpenTK.Matrix4d inMat)
        {
            Data = new double[16] 
			{
				inMat.M11, inMat.M12, inMat.M13, inMat.M14, 
				inMat.M21, inMat.M22, inMat.M23, inMat.M24, 
				inMat.M31, inMat.M32, inMat.M33, inMat.M34, 
				inMat.M41, inMat.M42, inMat.M43, inMat.M44, 
			};
        }

        /// <summary>
        /// Multiplies to matrices to produce a new matrix.
        /// </summary>
        /// <param name="mat1">The first factor.</param>
        /// <param name="mat2">The second factor.</param>
        /// <returns>A new matrix.</returns>
        static public Matrix4 operator *(Matrix4 mat1, Matrix4 mat2)
        {
            return new Matrix4(mat1.OpenTKEquivalent * mat2.OpenTKEquivalent);
        }

        /// <summary>
        /// Transforms a vector using the matrix.
        /// </summary>
        /// <param name="vector">The vector to transform.</param>
        /// <returns>A transformed vector.</returns>
        public Vector3 Transform(Vector3 vector)
        {
            return new Vector3(OpenTK.Vector3d.Transform(vector.OpenTKEquivalent, OpenTKEquivalent));

        }

        /// <summary>
        /// Creates a translation matrix from a position vector
        /// </summary>
        /// <param name="vector">The position vector</param>
        /// <returns>A transformation matrix</returns>
        public static Matrix4 CreateTranslation(Vector3 vector)
        {
            return new Matrix4(OpenTK.Matrix4d.CreateTranslation(vector.OpenTKEquivalent));
        }

        /// <summary>
        /// Rotates a matrix by using a quaternion
        /// </summary>
        /// <param name="orientation">The orientation quaternion</param>
        /// <returns>A rotated matrix</returns>
        public void Rotate(Quaternion orientation)
        {
            FromOpenTKEquivalent(OpenTKEquivalent * OpenTK.Matrix4d.Rotate(orientation.OpenTKEquivalent));
        }

        /// <summary>
        /// Rotates a matrix by using a quaternion
        /// </summary>
        /// <param name="orientation">The orientation quaternion</param>
        /// <returns>A rotated matrix</returns>
        public static Matrix4 Rotate(Matrix4 matrix, Quaternion orientation)
        {
            return new Matrix4(matrix.OpenTKEquivalent * OpenTK.Matrix4d.Rotate(orientation.OpenTKEquivalent));
        }
    }
}

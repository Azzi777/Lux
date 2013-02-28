using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace Lux.Framework
{
	/// <summary>
	/// Data structure representing a 4 by 4 matrix.
	/// </summary>
	public struct Matrix4
	{
		internal Matrix4d OpenTKEquivalent { get; set; }

		/// <summary>
		/// The 16-cell data array of the matrix. Each index ranges from 0 to 3.
		/// </summary>
		public double[] Data
		{
			get
			{
				return new double[16] 
				{
					OpenTKEquivalent.M11, OpenTKEquivalent.M12, OpenTKEquivalent.M13, OpenTKEquivalent.M14, 
					OpenTKEquivalent.M21, OpenTKEquivalent.M22, OpenTKEquivalent.M23, OpenTKEquivalent.M24, 
					OpenTKEquivalent.M31, OpenTKEquivalent.M32, OpenTKEquivalent.M33, OpenTKEquivalent.M34, 
					OpenTKEquivalent.M41, OpenTKEquivalent.M42, OpenTKEquivalent.M43, OpenTKEquivalent.M44, 
				};
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
			OpenTKEquivalent = new Matrix4d(m11, m12, m13, m14,
											m21, m22, m23, m24,
											m31, m32, m33, m34,
											m41, m42, m43, m44);
		}

		internal Matrix4(OpenTK.Matrix4d inMat)
			: this()
		{
			OpenTKEquivalent = inMat;
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
			return new Matrix4(Matrix4d.CreateTranslation(vector.OpenTKEquivalent));
		}

		/// <summary>
		/// Rotates a matrix using a quaternion
		/// </summary>
		/// <param name="orientation">The orientation quaternion</param>
		/// <returns>A rotated matrix</returns>
		public void Rotate(Quaternion orientation)
		{
			OpenTKEquivalent = OpenTKEquivalent * Matrix4d.Rotate(orientation.OpenTKEquivalent);
		}

		/// <summary>
		/// Creates a rotation matrix from a quaternion
		/// </summary>
		/// <param name="orientation">The orientation quaternion</param>
		/// <returns>A rotation matrix</returns>
		static public Matrix4 CreateRotation(Quaternion orientation)
		{
			return new Matrix4(Matrix4d.Rotate(orientation.OpenTKEquivalent));
		}
	}
}

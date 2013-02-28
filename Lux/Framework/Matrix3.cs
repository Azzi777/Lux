using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lux.Framework
{
	public struct Matrix3
	{
		#region - Data -
		public double[,] Data;

		public double this[int i]
		{
			get
			{
				if (Data == null) Data = new double[3, 3];
				return Data[i % 3, i / 3];
			}
			set
			{
				if (Data == null) Data = new double[3, 3];
				Data[i % 3, i / 3] = value;
			}
		}

		public double this[int i, int j]
		{
			get
			{
				if (Data == null) Data = new double[3, 3];
				return Data[i, j];
			}
			set
			{
				if (Data == null) Data = new double[3, 3];
				Data[i, j] = value;
			}
		}
		#endregion

		#region - Static Properties -
		static public Matrix3 Identity
		{
			get
			{
				Matrix3 returnValue = new Matrix3();
				returnValue[0, 0] = 1.0D;
				returnValue[1, 1] = 1.0D;
				returnValue[2, 2] = 1.0D;

				return returnValue;
			}
		}
		#endregion

		#region - Constructors -
		public Matrix3(double[,] data)
		{
			if (data.GetLength(0) != 3 || data.GetLength(1) != 3)
			{
				throw new ArgumentException("Argument must be an array of size 3x3!");
			}

			Data = data;
		}

		public Matrix3(double d11, double d12, double d13, double d21, double d22, double d23, double d31, double d32, double d33)
		{
			Data = new double[3, 3];
			Data[0, 0] = d11; Data[0, 1] = d12; Data[0, 2] = d13;
			Data[1, 0] = d21; Data[1, 1] = d22; Data[1, 2] = d23;
			Data[2, 0] = d31; Data[2, 1] = d32; Data[2, 2] = d33;
		}
		#endregion

		#region - Methods -

		#endregion

		#region - Operators -
		static public Matrix3 operator -(Matrix3 mat)
		{
			return new Matrix3(-mat[0, 0], -mat[0, 1], -mat[0, 2], -mat[1, 0], -mat[1, 1], -mat[1, 2], -mat[2, 0], -mat[2, 1], -mat[2, 2]);
		}

		static public Matrix3 operator -(Matrix3 a, Matrix3 b)
		{
			return a + (-b);
		}

		static public Matrix3 operator +(Matrix3 a, Matrix3 b)
		{
			return new Matrix3(
				a[0, 0] + b[0, 0], a[0, 1] + b[0, 1], a[0, 2] + b[0, 2],
				a[1, 0] + b[1, 0], a[1, 1] + b[1, 1], a[1, 2] + b[1, 2],
				a[2, 0] + b[2, 0], a[2, 1] + b[2, 1], a[2, 2] + b[2, 2]
				);
		}

		static public Vector3 operator *(Matrix3 mat, Vector3 vec)
		{
			return new Vector3(
					mat[0, 0] * vec.X + mat[0, 1] * vec.Y + mat[0, 2] * vec.Z,
					mat[1, 0] * vec.X + mat[1, 1] * vec.Y + mat[1, 2] * vec.Z,
					mat[2, 0] * vec.X + mat[2, 1] * vec.Y + mat[2, 2] * vec.Z
					);
		}

		static public Matrix3 operator *(Matrix3 a, Matrix3 b)
		{
			return new Matrix3(
				a[0, 0] * b[0, 0] + a[0, 1] * b[1, 0] + a[0, 2] * b[2, 0], a[0, 0] * b[0, 1] + a[0, 1] * b[1, 1] + a[0, 2] * b[2, 1], a[0, 0] * b[0, 2] + a[0, 1] * b[1, 2] + a[0, 2] * b[2, 2],
				a[1, 0] * b[0, 0] + a[1, 1] * b[1, 0] + a[1, 2] * b[2, 0], a[1, 0] * b[0, 1] + a[1, 1] * b[1, 1] + a[1, 2] * b[2, 1], a[1, 0] * b[0, 2] + a[1, 1] * b[1, 2] + a[1, 2] * b[2, 2],
				a[2, 0] * b[0, 0] + a[2, 1] * b[1, 0] + a[2, 2] * b[2, 0], a[2, 0] * b[0, 1] + a[2, 1] * b[1, 1] + a[2, 2] * b[2, 1], a[2, 0] * b[0, 2] + a[2, 1] * b[1, 2] + a[2, 2] * b[2, 2]
				);
		}

		static public Matrix3 operator *(Matrix3 mat, double sca)
		{
			return new Matrix3(sca * mat[0, 0], sca * mat[0, 1], sca * mat[0, 2], sca * mat[1, 0], sca * mat[1, 1], sca * mat[1, 2], sca * mat[2, 0], sca * mat[2, 1], sca * mat[2, 2]);
		}

		static public Matrix3 operator *(double sca, Matrix3 mat)
		{
			return mat * sca;
		}

		static public Matrix3 operator /(Matrix3 mat, double sca)
		{
			return new Matrix3(sca / mat[0, 0], sca / mat[0, 1], sca / mat[0, 2], sca / mat[1, 0], sca / mat[1, 1], sca / mat[1, 2], sca / mat[2, 0], sca / mat[2, 1], sca / mat[2, 2]);
		}
		#endregion

		#region - Creators -
		static public Matrix3 CreateRotationX(double angle)
		{
			return new Matrix3(1, 0, 0, 0, Math.Cos(angle), -Math.Sin(angle), 0, Math.Sin(angle), Math.Cos(angle));
		}

		static public Matrix3 CreateRotationY(double angle)
		{
			return new Matrix3(Math.Cos(angle), 0, Math.Sin(angle), 0, 1, 0, -Math.Sin(angle), 0, Math.Cos(angle));
		}

		static public Matrix3 CreateRotationZ(double angle)
		{
			return new Matrix3(Math.Cos(angle), -Math.Sin(angle), 0, Math.Sin(angle), Math.Cos(angle), 0, 0, 0, 1);
		}

		static public Matrix3 CreateFromAxisAngle(Vector3 axis, double angle)
		{
			double x = axis.X;
			double y = axis.Y;
			double z = axis.Z;
			double ca = Math.Cos(angle);
			double sa = Math.Sin(angle);

			return new Matrix3(
				ca + x * x * (1 - ca), x * y * (1 - ca) - z * sa, x * z * (1 - ca) + y * sa,
				y * x * (1 - ca) + z * sa, ca + y * y * (1 - ca), y * z * (1 - ca) - x * sa,
				z * x * (1 - ca) - y * sa, z * y * (1 - ca) + x * sa, ca + z * z * (1 - ca)
				);
		}
		#endregion
	}
}

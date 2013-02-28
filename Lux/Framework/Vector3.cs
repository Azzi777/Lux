using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lux.Framework
{
	/// <summary>
	/// Represents a three-dimensional vector
	/// </summary>
	public struct Vector3
	{
		internal OpenTK.Vector3d OpenTKEquivalent { get { return new OpenTK.Vector3d(X, Y, Z); } }

		/// <summary>
		/// The X-compontent of the vector
		/// </summary>
		public double X { get; set; }

		/// <summary>
		/// The Y-compontent of the vector
		/// </summary>
		public double Y { get; set; }

		/// <summary>
		/// The Z-compontent of the vector
		/// </summary>
		public double Z { get; set; }


		/// <summary>
		/// The squared length of the vector. This operation is much faster than the (unsquared) length.
		/// </summary>
		public double LengthSquared { get { return X * X + Y * Y + Z * Z; } }

		/// <summary>
		/// The length of the vector. This operation might be slow, consider using LengthSquared.
		/// </summary>
		public double Length { get { return Math.Sqrt(LengthSquared); } }

		/// <summary>
		/// Returns a normalized vector
		/// </summary>
		public Vector3 Normalized { get { if (this.Length != 0) { return (this / this.Length); } else { return this; } } }

		/// <summary>
		/// A new vector [ 0, 0, 0 ]
		/// </summary>
		static public Vector3 Zero { get { return new Vector3(0, 0, 0); } }

		/// <summary>
		/// A new vector [ 1, 1, 1 ]
		/// </summary>
		static public Vector3 One { get { return new Vector3(1, 1, 1); } }

		/// <summary>
		/// A new vector [ 1, 0, 0 ]
		/// </summary>
		static public Vector3 PosX { get { return new Vector3(1, 0, 0); } }

		/// <summary>
		/// A new vector [ -1, 0, 0 ]
		/// </summary>
		static public Vector3 NegX { get { return new Vector3(-1, 0, 0); } }

		/// <summary>
		/// A new vector [ 0, 1, 0 ]
		/// </summary>
		static public Vector3 PosY { get { return new Vector3(0, 1, 0); } }

		/// <summary>
		/// A new vector [ 0, -1, 0 ]
		/// </summary>
		static public Vector3 NegY { get { return new Vector3(0, -1, 0); } }

		/// <summary>
		/// A new vector [ 0, 0, 1 ]
		/// </summary>
		static public Vector3 PosZ { get { return new Vector3(0, 0, 1); } }

		/// <summary>
		/// A new vector [ 0, 0, -1 ]
		/// </summary>
		static public Vector3 NegZ { get { return new Vector3(0, 0, -1); } }


		/// <summary>
		/// A new vector pointing right in a right-handed co-ordinate system
		/// </summary>
		static public Vector3 Right { get { return Vector3.PosX; } }

		/// <summary>
		/// A new vector pointing left in a right-handed co-ordinate system
		/// </summary>
		static public Vector3 Left { get { return Vector3.NegX; } }

		/// <summary>
		/// A new vector pointing up in a right-handed co-ordinate system
		/// </summary>
		static public Vector3 Up { get { return Vector3.PosY; } }

		/// <summary>
		/// A new vector pointing down in a right-handed co-ordinate system
		/// </summary>
		static public Vector3 Down { get { return Vector3.NegY; } }

		/// <summary>
		/// A new vector pointing backwards in a right-handed co-ordinate system
		/// </summary>
		static public Vector3 Backwards { get { return Vector3.PosZ; } }

		/// <summary>
		/// A new vector pointing forwards in a right-handed co-ordinate system
		/// </summary>
		static public Vector3 Forwards { get { return Vector3.NegZ; } }

		/// <summary>
		/// Creates a new three-dimensional vector from a set of double-precision floating-point numbers.
		/// </summary>
		/// <param name="x">The X-component of the vector.</param>
		/// <param name="y">The Y-component of the vector.</param>
		/// <param name="z">The Z-component of the vector.</param>
		public Vector3(double x, double y, double z)
			: this()
		{
			X = x;
			Y = y;
			Z = z;
		}


		/// <summary>
		/// Creates a new three-dimensional vector from a set of integers.
		/// </summary>
		/// <param name="x">The X-component of the vector.</param>
		/// <param name="y">The Y-component of the vector.</param>
		/// <param name="z">The Z-component of the vector.</param>
		public Vector3(int x, int y, int z)
			: this()
		{
			X = x;
			Y = y;
			Z = z;
		}

		internal Vector3(OpenTK.Vector3d vec)
			: this()
		{
			X = vec.X;
			Y = vec.Y;
			Z = vec.Z;
		}

		/// <summary>
		/// Adds to vectors together.
		/// </summary>
		/// <param name="in1">The first addend.</param>
		/// <param name="in2">The second addend.</param>
		/// <returns>The sum of two vectors.</returns>
		static public Vector3 operator +(Vector3 in1, Vector3 in2)
		{
			return new Vector3(in1.X + in2.X, in1.Y + in2.Y, in1.Z + in2.Z);
		}

		/// <summary>
		/// Subracts one vector from another.
		/// </summary>
		/// <param name="in1">The minuend.</param>
		/// <param name="in2">The subtrahend.</param>
		/// <returns>The difference between two vectors.</returns>
		static public Vector3 operator -(Vector3 in1, Vector3 in2)
		{
			return new Vector3(in1.X - in2.X, in1.Y - in2.Y, in1.Z - in2.Z);
		}

		/// <summary>
		/// Component-wise multiplication of two vectors. Use DotProduct() or CrossProduct() for other multiplication operations.
		/// </summary>
		/// <param name="in1">The first factor.</param>
		/// <param name="in2">The second factor.</param>
		/// <returns>The component-wise multiplication of two vectors.</returns>
		static public Vector3 operator *(Vector3 in1, Vector3 in2)
		{
			return new Vector3(in1.X * in2.X, in1.Y * in2.Y, in1.Z * in2.Z);
		}

		/// <summary>
		/// Component-wise division of two vectors.
		/// </summary>
		/// <param name="in1">The dividend.</param>
		/// <param name="in2">The divisor</param>
		/// <returns>The component-wise division of two vectors.</returns>
		static public Vector3 operator /(Vector3 in1, Vector3 in2)
		{
			return new Vector3(in1.X / in2.X, in1.Y / in2.Y, in1.Z / in2.Z);
		}

		/// <summary>
		/// Component-wise modulo of two vectors.
		/// </summary>
		/// <param name="in1">The dividend.</param>
		/// <param name="in2">The divisor</param>
		/// <returns>The component-wise modulo of two vectors.</returns>
		static public Vector3 operator %(Vector3 in1, Vector3 in2)
		{
			return new Vector3(in1.X % in2.X, in1.Y % in2.Y, in1.Z % in2.Z);
		}

		/// <summary>
		/// Multiplication of a vector and a scalar
		/// </summary>
		/// <param name="in1">The vector.</param>
		/// <param name="in2">The scalar.</param>
		/// <returns>A scaled vector.</returns>
		static public Vector3 operator *(Vector3 in1, double in2)
		{
			return new Vector3(in1.X * in2, in1.Y * in2, in1.Z * in2);
		}

		/// <summary>
		/// Divison of a vector and a scalar
		/// </summary>
		/// <param name="in1">The vector.</param>
		/// <param name="in2">The scalar.</param>
		/// <returns>A scaled vector.</returns>
		static public Vector3 operator /(Vector3 in1, double in2)
		{
			return in1 * (1 / in2);
		}

		/// <summary>
		/// Performs the cross product operation on two vectors.
		/// </summary>
		/// <param name="in1">The first vector.</param>
		/// <param name="in2">The second vector.</param>
		/// <returns>A new vector which is perpendicular to both the input vectors.</returns>
		static public Vector3 Cross(Vector3 in1, Vector3 in2)
		{
			return new Vector3(
				in1.Y * in2.Z - in1.Z * in2.Y,
				in1.Z * in2.X - in1.X * in2.Z,
				in1.X * in2.Y - in1.Y * in2.X);
		}

		/// <summary>
		/// Performs the cross product operation on two vectors.
		/// </summary>
		/// <param name="vec">The second vector.</param>
		/// <returns>A new vector which is perpendicular to both the input vectors.</returns>
		public Vector3 Cross(Vector3 vec)
		{
			return Vector3.Cross(this, vec);
		}

		/// <summary>
		/// Performs the dot product operation on two vectors.
		/// </summary>
		/// <param name="vec1">The first vector.</param>
		/// <param name="vec2">The second vector.</param>
		/// <returns>A double-precision floating-point number which is the dot product of the two vectors.</returns>
		static public double Dot(Vector3 vec1, Vector3 vec2)
		{
			return vec1.X * vec2.X + vec1.Y * vec2.Y + vec1.Z * vec2.Z;
		}

		/// <summary>
		/// Performs the dot product operation on two vectors.
		/// </summary>
		/// <param name="vec">The second vector.</param>
		/// <returns>A double-precision floating-point number which is the dot product of the two vectors.</returns>
		public double Dot(Vector3 vec)
		{
			return Vector3.Dot(this, vec);
		}


		/// <summary>
		/// Returns a nicely formatted string form the vector.
		/// </summary>
		/// <returns>A nicely formatte string.</returns>
		public override string ToString()
		{
			return "[ " + X + ", " + Y + ", " + Z + " ]";
		}
	}
}

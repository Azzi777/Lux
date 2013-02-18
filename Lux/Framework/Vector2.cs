using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lux.Framework
{
	/// <summary>
	/// Represents a two-dimensional vector
	/// </summary>
	public struct Vector2
	{
		/// <summary>
		/// The X-compontent of the vector
		/// </summary>
		public double X { get; set; }

		/// <summary>
		/// The Y-compontent of the vector
		/// </summary>
		public double Y { get; set; }


		/// <summary>
		/// The squared length of the vector. This operation is much faster than the (unsquared) length.
		/// </summary>
		public double LengthSquared { get { return X * X + Y * Y; } }

		/// <summary>
		/// The length of the vector. This operation might be slow, consider using LengthSquared.
		/// </summary>
		public double Length { get { return Math.Sqrt(LengthSquared); } }

		/// <summary>
		/// A new vector [ 0, 0 ]
		/// </summary>
		static public Vector2 Zero { get { return new Vector2(0, 0); } }

		/// <summary>
		/// A new vector [ 1, 1 ]
		/// </summary>
		static public Vector2 One { get { return new Vector2(1, 1); } }

		/// <summary>
		/// A new vector [ 1, 0 ]
		/// </summary>
		static public Vector2 PosX { get { return new Vector2(1, 0); } }

		/// <summary>
		/// A new vector [ -1, 0 ]
		/// </summary>
		static public Vector2 NegX { get { return new Vector2(-1, 0); } }

		/// <summary>
		/// A new vector [ 0, 1 ]
		/// </summary>
		static public Vector2 PosY { get { return new Vector2(0, 1); } }

		/// <summary>
		/// A new vector [ 0, -1 ]
		/// </summary>
		static public Vector2 NegY { get { return new Vector2(0, -1); } }


		/// <summary>
		/// A new vector pointing right
		/// </summary>
		static public Vector2 Right { get { return Vector2.PosX; } }

		/// <summary>
		/// A new vector pointing left
		/// </summary>
		static public Vector2 Left { get { return Vector2.NegX; } }

		/// <summary>
		/// A new vector pointing up
		/// </summary>
		static public Vector2 Up { get { return Vector2.PosY; } }

		/// <summary>
		/// A new vector pointing down
		/// </summary>
		static public Vector2 Down { get { return Vector2.NegY; } }

		/// <summary>
		/// Creates a new two-dimensional vector from a set of double-precision floating-point numbers.
		/// </summary>
		/// <param name="x">The X-component of the vector.</param>
		/// <param name="y">The Y-component of the vector.</param>
		public Vector2(double x, double y)
			: this()
		{
			X = x;
			Y = y;
		}


		/// <summary>
		/// Creates a new two-dimensional vector from a set of integers.
		/// </summary>
		/// <param name="x">The X-component of the vector.</param>
		/// <param name="y">The Y-component of the vector.</param>
		public Vector2(int x, int y)
			: this()
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Adds to vectors together.
		/// </summary>
		/// <param name="in1">The first addend.</param>
		/// <param name="in2">The second addend.</param>
		/// <returns>The sum of two vectors.</returns>
		static public Vector2 operator +(Vector2 in1, Vector2 in2)
		{
			return new Vector2(in1.X + in2.X, in1.Y + in2.Y);
		}

		/// <summary>
		/// Subracts one vector from another.
		/// </summary>
		/// <param name="in1">The minuend.</param>
		/// <param name="in2">The subtrahend.</param>
		/// <returns>The difference between two vectors.</returns>
		static public Vector2 operator -(Vector2 in1, Vector2 in2)
		{
			return new Vector2(in1.X - in2.X, in1.Y - in2.Y);
		}

		/// <summary>
		/// Component-wise multiplication of two vectors. Use DotProduct() for other multiplication operations.
		/// </summary>
		/// <param name="in1">The first factor.</param>
		/// <param name="in2">The second factor.</param>
		/// <returns>The component-wise multiplication of two vectors.</returns>
		static public Vector2 operator *(Vector2 in1, Vector2 in2)
		{
			return new Vector2(in1.X * in2.X, in1.Y * in2.Y);
		}

		/// <summary>
		/// Component-wise division of two vectors.
		/// </summary>
		/// <param name="in1">The dividend.</param>
		/// <param name="in2">The divisor</param>
		/// <returns>The component-wise division of two vectors.</returns>
		static public Vector2 operator /(Vector2 in1, Vector2 in2)
		{
			return new Vector2(in1.X / in2.X, in1.Y / in2.Y);
		}

		/// <summary>
		/// Component-wise modulo of two vectors.
		/// </summary>
		/// <param name="in1">The dividend.</param>
		/// <param name="in2">The divisor</param>
		/// <returns>The component-wise modulo of two vectors.</returns>
		static public Vector2 operator %(Vector2 in1, Vector2 in2)
		{
			return new Vector2(in1.X % in2.X, in1.Y % in2.Y);
		}

		/// <summary>
		/// Multiplication of a vector and a scalar
		/// </summary>
		/// <param name="in1">The vector.</param>
		/// <param name="in2">The scalar.</param>
		/// <returns>A scaled vector.</returns>
		static public Vector2 operator *(Vector2 in1, double in2)
		{
			return new Vector2(in1.X * in2, in1.Y * in2);
		}

		/// <summary>
		/// Divison of a vector and a scalar
		/// </summary>
		/// <param name="in1">The vector.</param>
		/// <param name="in2">The scalar.</param>
		/// <returns>A scaled vector.</returns>
		static public Vector2 operator /(Vector2 in1, double in2)
		{
			return in1 * (1 / in2);
		}

		/// <summary>
		/// Performs the dot product operation on two vectors.
		/// </summary>
		/// <param name="vec1">The first vector.</param>
		/// <param name="vec2">The second vector.</param>
		/// <returns>A double-precision floating-point number which is the dot product of the two vectors.</returns>
		static public double Dot(Vector2 vec1, Vector2 vec2)
		{
			return vec1.X * vec2.X + vec1.Y * vec2.Y;
		}

		/// <summary>
		/// Performs the dot product operation on two vectors.
		/// </summary>
		/// <param name="vec">The second vector.</param>
		/// <returns>A double-precision floating-point number which is the dot product of the two vectors.</returns>
		public double Dot(Vector2 vec)
		{
			return Vector2.Dot(this, vec);
		}

		/// <summary>
		/// Returns a nicely formatted string form the vector.
		/// </summary>
		/// <returns>A nicely formatte string.</returns>
		public override string ToString()
		{
			return "[ " + X + ", " + Y + " ]";
		}
	}
}

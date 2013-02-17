using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace Lux.Framework
{
    /// <summary>
    /// A quaternion data structure, often representing orientation or rotation.
    /// </summary>
    public struct Quaternion
    {
        /// <summary>
        /// The real component of the quaternion
        /// </summary>
        public double R { get { return OpenTKEquivalent.W; } set { OpenTKEquivalent.W = value; } }

        /// <summary>
        /// The I-component of the quaternion
        /// </summary>
        public double I { get { return OpenTKEquivalent.X; } set { OpenTKEquivalent.X = value; } }

        /// <summary>
        /// The J-component of the quaternion
        /// </summary>
        public double J { get { return OpenTKEquivalent.Y; } set { OpenTKEquivalent.Y = value; } }

        /// <summary>
        /// The K-component of the quaternion
        /// </summary>
        public double K { get { return OpenTKEquivalent.Z; } set { OpenTKEquivalent.Z = value; } }

        /// <summary>
        /// A new quaternion [ 1, 0, 0, 0 ]
        /// </summary>
        static public Quaternion Identity { get { return new Quaternion(1, 0, 0, 0); } }

        /// <summary>
        /// The axis component of the quaternion.
        /// </summary>
        public Vector3 Axis { get { return new Vector3(I, J, K); } }

        /// <summary>
        /// The angle component of the quaternion.
        /// </summary>
        public double Angle { get { return R; } }

        internal Quaterniond OpenTKEquivalent;

        /// <summary>
        /// Creates a new quaternion from a scaled vector, where the magnitude is the angle and the direction is the axis.
        /// </summary>
        /// <param name="scaledAxis">The scaled vector.</param>
        public Quaternion(Vector3 scaledAxis)
            : this()
        {
            if(scaledAxis.LengthSquared == 0)
            {
                OpenTKEquivalent = Quaterniond.Identity;
            }
            else
            {
                OpenTKEquivalent = new Quaterniond(scaledAxis.Normalized.OpenTKEquivalent, scaledAxis.Length);
            }
        }

        /// <summary>
        /// Creates a new quaternion from a vector representing an axis, and an angle around this axis.
        /// </summary>
        /// <param name="axis">The vector axis around which to turn.</param>
        /// <param name="angle">The angle to turn.</param>
        public Quaternion(double angle, Vector3 axis)
            : this()
        {
            OpenTKEquivalent = Quaterniond.FromAxisAngle(axis.OpenTKEquivalent, angle);
        }

        /// <summary>
        /// Creates a new quaternion from an axis, denoted by i, j, k, and an angle r.
        /// </summary>
        /// <param name="i">The x-component of the axis.</param>
        /// <param name="j">The y-component of the axis.</param>
        /// <param name="k">The z-component of the axis.</param>
        /// <param name="r">The angle.</param>
        public Quaternion(double r, double i, double j, double k)
            : this()
        {
            OpenTKEquivalent = new Quaterniond(i, j, k, r);
        }

        internal Quaternion(Quaterniond openTKEquivalent)
            : this()
        {
            OpenTKEquivalent = openTKEquivalent;
        }

        /// <summary>
        /// Normalizes the quaternion.
        /// </summary>
        public void Normalize()
        {
            OpenTKEquivalent.Normalize();
        }

        /// <summary>
        /// The multiplicative operator, in practise, used for adding orientations/rotations.
        /// </summary>
        /// <param name="quat1">The first quaternion.</param>
        /// <param name="quat2">The second quaternion.</param>
        /// <returns>A new quaternion representing the combined rotation.</returns>
        static public Quaternion operator *(Quaternion quat1, Quaternion quat2)
        {
            return new Quaternion(quat1.OpenTKEquivalent * quat2.OpenTKEquivalent);
        }

        /// <summary>
        /// The multiplicative operator, in practise, used for scaling orientations/rotations.
        /// </summary>
        /// <param name="quat1">The quaternion.</param>
        /// <param name="scale">The scale factor.</param>
        /// <returns>A new quaternion representing the combined rotation.</returns>
        static public Quaternion operator *(Quaternion quat1, double scale)
        {
            return new Quaternion(quat1.OpenTKEquivalent * scale);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lux.Framework
{
    struct Matrix3
    {
        public double[,] Data;

        public double this[int i]
        {
            get
            {
                if (Data == null)
                {
                    Data = new double[3, 3];
                }
                return Data[i % 3, (int)Math.Floor((double)i / 3.0D)];
            }
            set
            {
                if (Data == null)
                {
                    Data = new double[3, 3];
                }
                Data[i % 3, (int)Math.Floor((double)i / 3.0D)] = value;
            }
        }

        public double this[int i, int j]
        {
            get
            {
                if (Data == null)
                {
                    Data = new double[3, 3];
                }
                return Data[i, j];
            }
            set
            {
                if (Data == null)
                {
                    Data = new double[3, 3];
                }
                Data[i, j] = value;
            }
        }

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

        public Vector3 Transform(Vector3 vector)
        {
            if (Data == null)
            {
                Data = new double[3, 3];
            }
            return new Vector3(this[0, 0] * vector.X + this[0, 1] * vector.Y + this[0, 2] * vector.Z,
                this[1, 0] * vector.X + this[1, 1] * vector.Y + this[1, 2] * vector.Z,
                this[2, 0] * vector.X + this[2, 1] * vector.Y + this[2, 2] * vector.Z);

        }
    }
}

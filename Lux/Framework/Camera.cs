using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lux.Framework
{
	public class Camera
	{
		public Vector3 Position { get; set; }
		public double Yaw { get; set; }
		public double Pitch { get; set; }
		public double Roll { get; set; }

		public Vector3 GetForwards
		{
			get
			{
				Matrix3 rot = Matrix3.CreateRotationY(Yaw) * Matrix3.CreateRotationX(Pitch) * Matrix3.CreateRotationZ(Roll);

				return rot * Vector3.Forwards;
			}
		}

		internal OpenTK.Matrix4d OpenTKViewMatrix
		{
			get
			{
				Matrix3 rot =  Matrix3.CreateRotationY(Yaw) * Matrix3.CreateRotationX(Pitch) * Matrix3.CreateRotationZ(Roll);

				Vector3 dir = rot * Vector3.Forwards;
				Vector3 up = rot * Vector3.Up;
				return OpenTK.Matrix4d.LookAt(Position.OpenTKEquivalent, Position.OpenTKEquivalent + dir.OpenTKEquivalent, up.OpenTKEquivalent);
			}
		}

		public Camera(double y, double p, double r)
		{
			Yaw = y;
			Pitch = p;
			Roll = r;
		}
	}
}

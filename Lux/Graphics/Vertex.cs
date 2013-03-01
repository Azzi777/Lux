using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lux.Graphics
{
	internal struct MeshVertex
	{
		public MeshPosition Position;
		public MeshNormal Normal;
		public MeshTexCoord TexCoord;
		public MeshTangent Tangent;

		public MeshVertex(MeshPosition pos, MeshNormal norm, MeshTexCoord texcoord, MeshTangent tangent)
		{
			Position = pos;
			Normal = norm;
			TexCoord = texcoord;
			Tangent = tangent;
		}

		static public int GetSize()
		{
			return MeshPosition.GetSize() + MeshNormal.GetSize() + MeshTexCoord.GetSize() + MeshTangent.GetSize();
		}
	}

	internal struct MeshPosition
	{
		public float X;
		public float Y;
		public float Z;

		public MeshPosition(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		static public int GetSize()
		{
			return 3 * sizeof(float);
		}
	}

	internal struct MeshNormal
	{
		public float X;
		public float Y;
		public float Z;

		public MeshNormal(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		static public int GetSize()
		{
			return 3 * sizeof(float);
		}
	}

	internal struct MeshTangent
	{
		public float X;
		public float Y;
		public float Z;

		public MeshTangent(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		static public int GetSize()
		{
			return 3 * sizeof(float);
		}
	}

	internal struct MeshTexCoord
	{
		public float X;
		public float Y;

		public MeshTexCoord(float x, float y)
		{
			X = x;
			Y = y;
		}

		static public int GetSize()
		{
			return 2 * sizeof(float);
		}
	}
}

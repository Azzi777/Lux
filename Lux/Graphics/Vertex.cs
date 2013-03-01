using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lux.Graphics
{
	internal struct MeshVertex
	{
		MeshPosition Position;
		MeshNormal Normal;
		MeshTexCoord TexCoord;

		public MeshVertex(MeshPosition pos, MeshNormal norm, MeshTexCoord texcoord)
		{
			Position = pos;
			Normal = norm;
			TexCoord = texcoord;
		}

		static public int GetSize()
		{
			return MeshPosition.GetSize() + MeshNormal.GetSize() + MeshTexCoord.GetSize();
		}
	}

	internal struct MeshPosition
	{
		float X;
		float Y;
		float Z;

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
		float X;
		float Y;
		float Z;

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

	internal struct MeshTexCoord
	{
		float X;
		float Y;

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

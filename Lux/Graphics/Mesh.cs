using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lux.Graphics
{
	internal class Mesh
	{
		Face[] Faces;

		uint VertexBufferID;
		uint NormalBufferID;
		uint TextureBufferID;

		public Mesh(MeshVertex[] vertices, MeshNormal[] normals, MeshTexCoord[] texCoords, Face[] faces)
		{
			GL.GenBuffers(1, out VertexBufferID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * MeshVertex.GetSize()), vertices, BufferUsageHint.StaticDraw);

			GL.GenBuffers(1, out NormalBufferID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferID);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normals.Length * MeshNormal.GetSize()), normals, BufferUsageHint.StaticDraw);

			GL.GenBuffers(1, out TextureBufferID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, TextureBufferID);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texCoords.Length * MeshTexCoord.GetSize()), texCoords, BufferUsageHint.StaticDraw);

			Faces = faces;
		}

		public void Render()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.VertexPointer(3, VertexPointerType.Float, MeshVertex.GetSize(), 0);

			GL.EnableClientState(ArrayCap.ColorArray);
			GL.ColorPointer(3, ColorPointerType.Float, MeshVertex.GetSize(), 12);

			GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferID);
			GL.EnableClientState(ArrayCap.NormalArray);
			GL.NormalPointer(NormalPointerType.Float, MeshNormal.GetSize(), 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, TextureBufferID);
			GL.EnableClientState(ArrayCap.TextureCoordArray);
			GL.TexCoordPointer(2, TexCoordPointerType.Float, MeshTexCoord.GetSize(), 0);


			foreach (Face face in Faces)
			{
				face.Render();
			}
			
			GL.DisableClientState(ArrayCap.VertexArray);
			GL.DisableClientState(ArrayCap.ColorArray);
			GL.DisableClientState(ArrayCap.NormalArray);
			GL.DisableClientState(ArrayCap.TextureCoordArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
	}

	internal struct MeshVertex
	{
		float X;
		float Y;
		float Z;
		float R;
		float G;
		float B;

		public MeshVertex(float x, float y, float z, float r, float g, float b)
		{
			X = x;
			Y = y;
			Z = z;
			R = r;
			G = g;
			B = b;
		}

		public MeshVertex(float x, float y, float z, Color c)
		{
			X = x;
			Y = y;
			Z = z;
			R = (float)c.R / 255;
			G = (float)c.G / 255;
			B = (float)c.B / 255;
		}

		static public int GetSize()
		{
			return 6 * sizeof(float);
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

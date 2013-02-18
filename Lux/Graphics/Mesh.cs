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
		uint VertexBufferID;
		uint IndexBufferID;
		int TriangleCount;

		static public Mesh UnitCube
		{
			get
			{
				return new Mesh(
					new MeshVertex[]{
                    new MeshVertex(-1, -1, -1, 0, 0, 0), 
                    new MeshVertex(-1, -1, +1, 0, 0, 1),
                    new MeshVertex(-1, +1, -1, 0, 1, 0),
                    new MeshVertex(-1, +1, +1, 0, 1, 1),
                    new MeshVertex(+1, -1, -1, 1, 0, 0),
                    new MeshVertex(+1, -1, +1, 1, 0, 1),
                    new MeshVertex(+1, +1, -1, 1, 1, 0),
                    new MeshVertex(+1, +1, +1, 1, 1, 1)},
					new uint[] { 
                        0, 2, 6,   6, 4, 0, 
                        4, 6, 7,   7, 5, 4, 
                        5, 7, 3,   3, 1, 5, 
                        1, 3, 2,   2, 0, 1, 
                        3, 7, 6,   6, 2, 3, 
                        4, 5, 1,   1, 0, 4 });
			}
		}

		static public Mesh UnitIcosahedron
		{
			get
			{
				float p = 2.0F / ((float)Math.Sqrt(5.0F) + 1.0F);

				return new Mesh(
					new MeshVertex[]{
                    new MeshVertex(-1, +p, 0, Color.Red), 
                    new MeshVertex(+1, +p, 0, Color.Blue),
                    new MeshVertex(-1, -p, 0, Color.Black),
                    new MeshVertex(+1, -p, 0, Color.White),
                    new MeshVertex(-p, 0, +1, Color.Green),
                    new MeshVertex(+p, 0, +1, Color.Yellow),
                    new MeshVertex(-p, 0, -1, Color.Brown),
                    new MeshVertex(+p, 0, -1, Color.Teal),
                    new MeshVertex(0, +1, +p, Color.Orange),
                    new MeshVertex(0, +1, -p, Color.Magenta),
                    new MeshVertex(0, -1, +p, Color.Cyan),
                    new MeshVertex(0, -1, -p, Color.Chocolate)},
					new uint[] { 9, 1, 7, 9, 7, 6, 9, 6, 0, 9, 0, 8, 9, 8, 1, 6, 7, 11, 7, 3, 11, 7, 1, 3, 1, 5, 3, 1, 8, 5, 8, 4, 5, 8, 0, 4, 0, 2, 4, 0, 6, 2, 6, 11, 2, 10, 2, 11, 10, 11, 3, 10, 3, 5, 10, 5, 4, 10, 4, 2 });
			}
		}

		public Mesh(MeshVertex[] vertices, uint[] indices)
		{
			GL.GenBuffers(1, out VertexBufferID);
			GL.GenBuffers(1, out IndexBufferID);

			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferID);

			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * (vertices.Length == 0 ? 0 : MeshVertex.GetSize())), vertices, BufferUsageHint.StaticDraw);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)), indices.ToArray(), BufferUsageHint.StaticDraw);

			TriangleCount = indices.Length / 3;
		}

		//private MeshVertex[] Vertices;
		//private uint[] Indices;
		//public Mesh(MeshVertex[] vertices, uint[] indices)
		//{
		//    Vertices = vertices;
		//    Indices = indices;
		//}


		//public void Setup()
		//{
		//    GL.GenBuffers(1, out VertexBufferID);
		//    GL.GenBuffers(1, out IndexBufferID);

		//    GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
		//    GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferID);

		//    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vertices.Length * (Vertices.Length == 0 ? 0 : MeshVertex.GetSize())), Vertices, BufferUsageHint.StaticDraw);
		//    GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(Indices.Length * sizeof(uint)), Indices.ToArray(), BufferUsageHint.StaticDraw);

		//    TriangleCount = Indices.Length / 3;
		//}

		public void Render()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferID);

			GL.EnableClientState(ArrayCap.VertexArray);
			GL.VertexPointer(3, VertexPointerType.Float, 24, 0);

			GL.EnableClientState(ArrayCap.ColorArray);
			GL.ColorPointer(3, ColorPointerType.Float, 24, 12);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			GL.DrawElements(BeginMode.Triangles, 3 * TriangleCount, DrawElementsType.UnsignedInt, 0);

			GL.DisableClientState(ArrayCap.VertexArray);
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
}

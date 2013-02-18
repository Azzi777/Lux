using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lux.Graphics
{
	internal class Face
	{
		int VertexCount;
		uint IndexBufferID;


		public Face(uint[] indices)
		{
			GL.GenBuffers(1, out IndexBufferID);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferID);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)), indices, BufferUsageHint.StaticDraw);

			VertexCount = indices.Length;
		}


		public void Render()
		{
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferID);
			GL.EnableClientState(ArrayCap.IndexArray);
			GL.IndexPointer(IndexPointerType.Int, sizeof(uint), 0);

			if (VertexCount == 3)
			{
				GL.DrawElements(BeginMode.Triangles, VertexCount, DrawElementsType.UnsignedInt, 0);
			}
			else if (VertexCount == 4)
			{
				GL.DrawElements(BeginMode.Quads, VertexCount, DrawElementsType.UnsignedInt, 0);
			}

			GL.DisableClientState(ArrayCap.IndexArray);
		}
	}
}

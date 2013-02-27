using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using Lux.Framework;
using Lux.Resources;

namespace Lux.Graphics
{
	internal class Model
	{
		private bool IsFinished { get; set; }

		Mesh[] Meshes;

		uint VertexBufferID;
		uint NormalBufferID;
		uint TextureBufferID;

		MeshVertex[] TempVertices;
		MeshNormal[] TempNormals;
		MeshTexCoord[] TempTexCoords;

		public Model(MeshVertex[] vertices, MeshNormal[] normals, MeshTexCoord[] texCoords, Mesh[] meshes)
		{
			TempVertices = vertices;
			TempNormals = normals;
			TempTexCoords = texCoords;

			Meshes = meshes;

			IsFinished = false;
		}

		private void Finish()
		{
			if (IsFinished)
			{
				return;
			}

			GL.GenBuffers(1, out VertexBufferID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(TempVertices.Length * MeshVertex.GetSize()), TempVertices, BufferUsageHint.StaticDraw);

			GL.GenBuffers(1, out NormalBufferID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferID);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(TempNormals.Length * MeshNormal.GetSize()), TempNormals, BufferUsageHint.StaticDraw);

			GL.GenBuffers(1, out TextureBufferID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, TextureBufferID);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(TempTexCoords.Length * MeshTexCoord.GetSize()), TempTexCoords, BufferUsageHint.StaticDraw);


			TempVertices = null;
			TempNormals = null;
			TempTexCoords = null;

			foreach (Mesh m in Meshes)
			{
				m.Finish();
			}

			IsFinished = true;
		}

		static public Model LoadFromFile(string path)
		{
			return ObjLoader.LoadFromFile(path);
		}

		public void Render(Entity entity)
		{
			Finish();
			OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
			OpenTK.Graphics.OpenGL.GL.MultMatrix(entity.TransformMatrix.Data);

			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.VertexPointer(3, VertexPointerType.Float, MeshVertex.GetSize(), 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferID);
			GL.EnableClientState(ArrayCap.NormalArray);
			GL.NormalPointer(NormalPointerType.Float, MeshNormal.GetSize(), 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, TextureBufferID);
			GL.EnableClientState(ArrayCap.TextureCoordArray);
			GL.TexCoordPointer(2, TexCoordPointerType.Float, MeshTexCoord.GetSize(), 0);

			foreach (Mesh m in Meshes)
			{
				m.Render();
			}

			GL.DisableClientState(ArrayCap.VertexArray);
			GL.DisableClientState(ArrayCap.NormalArray);
			GL.DisableClientState(ArrayCap.TextureCoordArray);
		}
	}
}

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

		MeshVertex[] TempVertices;
		Texture[] TempTextures;
		uint TexturesBufferID;

		public Model(MeshVertex[] vertices, Mesh[] meshes, Texture[] textures)
		{
			Meshes = meshes;
			TempVertices = vertices;
			TempTextures = textures;

			IsFinished = false;
		}

		private void Finish(ShaderProgram textureShader)
		{
			if (IsFinished)
			{
				return;
			}

			int ArrayID;
			GL.GenVertexArrays(1, out ArrayID);
			GL.BindVertexArray(ArrayID);
			GL.GenBuffers(1, out VertexBufferID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);

			GL.BufferData<MeshVertex>(BufferTarget.ArrayBuffer, (IntPtr)(TempVertices.Length * MeshVertex.GetSize()), TempVertices, BufferUsageHint.StaticDraw);

			textureShader.SetVertexFormat();
			
			TempVertices = null;

			TexturesBufferID = Texture.CreateTexture2DArray(TempTextures);
			TempTextures = null;

			foreach (Mesh m in Meshes)
			{
				m.Finish();
			}

			IsFinished = true;
		}

		static public Model LoadFromFile(string path)
		{
			return ObjLoader2.LoadFromFile(path);
		}

		public void Render(Entity entity, ShaderProgram textureShader)
		{
			Finish(textureShader);
			textureShader.SetMatrix4("mat_world", entity.TransformMatrix);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2DArray, TexturesBufferID);

			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
			//GL.EnableClientState(ArrayCap.VertexArray);
			//GL.VertexPointer(3, VertexPointerType.Float, MeshVertex.GetSize(), 0);

			foreach (Mesh m in Meshes)
			{
				m.Render(textureShader);
			}

			//GL.DisableClientState(ArrayCap.VertexArray);
		}
	}
}

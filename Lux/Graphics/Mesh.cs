using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Lux.Graphics
{
	internal class Mesh
	{
		int VertexCount;
		uint IndexBufferID;

		public Color4 AmbientColor;
		public Color4 DiffuseColor;
		public Color4 EmissiveColor;
		public Color4 SpecularColor;
		public float SpecularCoefficient;
		public float Transparency;
		public float ReflectionIndex;

		public Texture AmbientTexture;
		public Texture DiffuseTexture;
		public Texture AlphaTexture;
		public Texture BumpMapTexture;

		public Mesh(uint[] indices)
		{
			GL.GenBuffers(1, out IndexBufferID);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferID);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)), indices, BufferUsageHint.StaticDraw);

			VertexCount = indices.Length;

			AmbientColor = Color4.White;
			DiffuseColor = Color4.White;
			EmissiveColor = Color4.White;
			SpecularColor = Color4.White;
			Transparency = 0.0F;
			SpecularCoefficient = 10.0F;
			ReflectionIndex = 1.5F;
		}

		public void Render()
		{
			GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, AmbientColor);
			GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, DiffuseColor);
			GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, EmissiveColor);
			GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, SpecularColor);
			GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, SpecularCoefficient);

			if (AmbientTexture != null)
			{
				GL.Enable(EnableCap.Texture2D);
				GL.BindTexture(TextureTarget.Texture2D, AmbientTexture.TextureID);
			}

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferID);
			GL.EnableClientState(ArrayCap.IndexArray);
			GL.IndexPointer(IndexPointerType.Int, sizeof(uint), 0);

			GL.DrawElements(BeginMode.Triangles, VertexCount, DrawElementsType.UnsignedInt, 0);

			GL.DisableClientState(ArrayCap.IndexArray);

			GL.Disable(EnableCap.Texture2D);
		}

		public void LoadAmbientTexture(string path)
		{
			AmbientTexture = new Texture(path);
		}

		public void LoadDiffuseTexture(string path)
		{
			DiffuseTexture = new Texture(path);
		}

		public void LoadAlphaTexture(string path)
		{
			AlphaTexture = new Texture(path);
		}

		public void LoadBumpMapTexture(string path)
		{
			BumpMapTexture = new Texture(path);
		}
	}

	internal struct MeshVertex
	{
		float X;
		float Y;
		float Z;

		public MeshVertex(float x, float y, float z)
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

﻿using System;
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

		uint[] TempIndices;

		public Color4 AmbientColor;
		public Color4 DiffuseColor;
		public Color4 EmissiveColor;
		public Color4 SpecularColor;
		public float SpecularCoefficient;
		public float Transparency;
		public float ReflectionIndex;

		public Texture AmbientTexture { get; set; }
		public Texture DiffuseTexture { get; set; }
		public Texture AlphaTexture { get; set; }
		public Texture BumpMapTexture { get; set; }
		public Texture SpecularHighlightTexture { get; set; }
		public Texture SpecularTexture { get; set; }
		public Texture StencilDecal { get; set; }

		public Mesh(uint[] indices)
		{
			TempIndices = indices;

			AmbientColor = Color4.Black;
			DiffuseColor = Color4.Black;
			EmissiveColor = Color4.Black;
			SpecularColor = Color4.Black;
			Transparency = 0.0F;
			SpecularCoefficient = 10.0F;
			ReflectionIndex = 1.5F;
		}


		public void Finish()
		{
			GL.GenBuffers(1, out IndexBufferID);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferID);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(TempIndices.Length * sizeof(uint)), TempIndices, BufferUsageHint.StaticDraw);

			VertexCount = TempIndices.Length;

			TempIndices = null;
		}

		public void Render(ShaderProgram shaderProgram)
		{
			if (AmbientTexture != null)
			{
				shaderProgram.SetInteger("textureID", AmbientTexture.TextureID);
			}
			else if (DiffuseTexture != null)
			{
				shaderProgram.SetInteger("textureID", DiffuseTexture.TextureID);
			}


			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferID);
			GL.EnableClientState(ArrayCap.IndexArray);
			GL.IndexPointer(IndexPointerType.Int, sizeof(uint), 0);

			GL.DrawElements(BeginMode.Triangles, VertexCount, DrawElementsType.UnsignedInt, 0);

			GL.DisableClientState(ArrayCap.IndexArray);
		}
	}

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

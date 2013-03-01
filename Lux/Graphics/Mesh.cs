using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Lux.Framework;

namespace Lux.Graphics
{
	internal class Mesh
	{
		int VertexCount;
		uint IndexBufferID;

		uint[] TempIndices;

		public Color AmbientColor;
		public Color DiffuseColor;
		public Color EmissiveColor;
		public Color SpecularColor;
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

			AmbientColor = Color.Black;
			DiffuseColor = Color.Black;
			EmissiveColor = Color.Black;
			SpecularColor = Color.Black;
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
			TextureDefine(shaderProgram);

			GL.Uniform4(GL.GetUniformLocation(shaderProgram.ID, "mat_ambient"), AmbientColor.GetOpenTKEquivalent());
			GL.Uniform4(GL.GetUniformLocation(shaderProgram.ID, "mat_diffuse"), DiffuseColor.GetOpenTKEquivalent());
			GL.Uniform4(GL.GetUniformLocation(shaderProgram.ID, "mat_specular"), /*SpecularColor*/DiffuseColor.GetOpenTKEquivalent());
			GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "mat_shininess"), SpecularCoefficient);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferID);

			GL.DrawElements(BeginMode.Triangles, VertexCount, DrawElementsType.UnsignedInt, 0);


			TextureUndefine(shaderProgram);
		}

		private void TextureDefine(ShaderProgram shaderProgram)
		{
			if (AmbientTexture != null)
			{
				GL.ActiveTexture(TextureUnit.Texture0);
				GL.BindTexture(TextureTarget.Texture2D, AmbientTexture.TextureID);
				GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "tex_ambient"), 0);
			}

			if (DiffuseTexture != null)
			{
				GL.ActiveTexture(TextureUnit.Texture1);
				GL.BindTexture(TextureTarget.Texture2D, DiffuseTexture.TextureID);
				GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "tex_diffuse"), 1);
			}

			if (AlphaTexture != null)
			{
				GL.ActiveTexture(TextureUnit.Texture2);
				GL.BindTexture(TextureTarget.Texture2D, AlphaTexture.TextureID);
				GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "tex_alpha"), 2);
			}

			if (BumpMapTexture != null)
			{
				GL.ActiveTexture(TextureUnit.Texture3);
				GL.BindTexture(TextureTarget.Texture2D, BumpMapTexture.TextureID);
				GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "tex_bump"), 3);
			}

			if (SpecularHighlightTexture != null)
			{
				GL.ActiveTexture(TextureUnit.Texture4);
				GL.BindTexture(TextureTarget.Texture2D, SpecularHighlightTexture.TextureID);
				GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "tex_specular_highlight"), 4);
			}

			if (SpecularTexture != null)
			{
				GL.ActiveTexture(TextureUnit.Texture5);
				GL.BindTexture(TextureTarget.Texture2D, SpecularTexture.TextureID);
				GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "tex_specular"), 5);
			}

			if (StencilDecal != null)
			{
				GL.ActiveTexture(TextureUnit.Texture6);
				GL.BindTexture(TextureTarget.Texture2D, StencilDecal.TextureID);
				GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "tex_stencil_decal"), 6);
			}
		}

		private void TextureUndefine(ShaderProgram shaderProgram)
		{
				GL.ActiveTexture(TextureUnit.Texture0);
				GL.BindTexture(TextureTarget.Texture2D, 0);
				GL.ActiveTexture(TextureUnit.Texture1);
				GL.BindTexture(TextureTarget.Texture2D, 0);
				GL.ActiveTexture(TextureUnit.Texture2);
				GL.BindTexture(TextureTarget.Texture2D, 0);
				GL.ActiveTexture(TextureUnit.Texture3);
				GL.BindTexture(TextureTarget.Texture2D, 0);
				GL.ActiveTexture(TextureUnit.Texture4);
				GL.BindTexture(TextureTarget.Texture2D, 0);
				GL.ActiveTexture(TextureUnit.Texture5);
				GL.BindTexture(TextureTarget.Texture2D, 0);
				GL.ActiveTexture(TextureUnit.Texture6);
				GL.BindTexture(TextureTarget.Texture2D, 0);
		}
	}
}

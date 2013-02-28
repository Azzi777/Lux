using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Lux.Graphics
{
	internal class ShaderProgram
	{
		public int ID { get; private set; }

		public ShaderProgram(string vertexShader, string fragmentShader)
		{
			ID = GL.CreateProgram();

			int vID = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vID, vertexShader);
			GL.CompileShader(vID);

			int compileResult;
			GL.GetShader(vID, ShaderParameter.CompileStatus, out compileResult);
			if (compileResult != 1)
			{
				string error = GL.GetShaderInfoLog(vID);
				throw new Exception("Error while compiling the vertex shader. \n\nError message: \"" + error + "\"");
			}

			GL.AttachShader(ID, vID);

			int fID = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fID, fragmentShader);
			GL.CompileShader(fID);



			GL.GetShader(fID, ShaderParameter.CompileStatus, out compileResult);
			if (compileResult != 1)
			{
				string error = GL.GetShaderInfoLog(fID);
				throw new Exception("Error while compiling the fragment shader. \n\nError message: \"" + error + "\"");
			}

			GL.AttachShader(ID, fID);

			GL.LinkProgram(ID);
		}

		public void SetVector3(string varName, Lux.Framework.Vector3 vec3)
		{
			int id = GL.GetUniformLocation(ID, varName);
			GL.Uniform3(id, (Vector3)vec3.OpenTKEquivalent);
		}

		public void SetVector2(string varName, Lux.Framework.Vector2 vec2)
		{
			int id = GL.GetUniformLocation(ID, varName);
			GL.Uniform2(id, (Vector2)vec2.OpenTKEquivalent);
		}

		public void SetInteger(string varName, uint integer)
		{
			int id = GL.GetUniformLocation(ID, varName);
			GL.Uniform2(id, (float)integer, 0.0F);
		}

		public void SetMatrix4(string varName, Lux.Framework.Matrix4 mat4)
		{
			int id = GL.GetUniformLocation(ID, varName);
			float[] temp_mat4 = new float[16] {
				(float)mat4.OpenTKEquivalent.M11, (float)mat4.OpenTKEquivalent.M12, (float)mat4.OpenTKEquivalent.M13, (float)mat4.OpenTKEquivalent.M14,
				(float)mat4.OpenTKEquivalent.M21, (float)mat4.OpenTKEquivalent.M22, (float)mat4.OpenTKEquivalent.M23, (float)mat4.OpenTKEquivalent.M24,
				(float)mat4.OpenTKEquivalent.M31, (float)mat4.OpenTKEquivalent.M32, (float)mat4.OpenTKEquivalent.M33, (float)mat4.OpenTKEquivalent.M34,
				(float)mat4.OpenTKEquivalent.M41, (float)mat4.OpenTKEquivalent.M42, (float)mat4.OpenTKEquivalent.M43, (float)mat4.OpenTKEquivalent.M44
			};

			GL.UniformMatrix4(id, 1, false, temp_mat4);
		}

		public void SetVertexFormat()
		{
			int posAttrib = GL.GetAttribLocation(ID, "inPosition");
			GL.VertexAttribPointer(posAttrib, 3, VertexAttribPointerType.Float, false, 32, 0);
			GL.EnableVertexAttribArray(posAttrib);

			int normAttrib = GL.GetAttribLocation(ID, "inNormal");
			GL.VertexAttribPointer(normAttrib, 3, VertexAttribPointerType.Float, false, 32, 12);
			GL.EnableVertexAttribArray(normAttrib);

			int texAttrib = GL.GetAttribLocation(ID, "inTexcoord");
			GL.VertexAttribPointer(texAttrib, 2, VertexAttribPointerType.Float, false, 32, 24);
			GL.EnableVertexAttribArray(texAttrib);
		}

		static internal string TextureVertexShaderSource = @"
#version 150
attribute vec3 inPosition;
attribute vec3 inNormal;
attribute vec2 inTexcoord;

out vec3 Normal;
out vec2 Texcoord;

uniform mat4 mat_world;
uniform mat4 mat_view;
uniform mat4 mat_proj;

void main()
{
	Normal = inNormal;
	Texcoord = inTexcoord;

	gl_Position = mat_proj * mat_view * mat_world * vec4(inPosition, 1.0);
}";

		static internal string TextureFragmentShaderSource = @"
#version 150
#extension GL_EXT_gpu_shader4 : enable
in vec3 Normal;
in vec2 Texcoord;

out vec4 outColor;

uniform sampler2D tex_ambient;
uniform sampler2D tex_diffuse;
uniform sampler2D tex_alpha;
uniform sampler2D tex_bump;
uniform sampler2D tex_specular_highlight;
uniform sampler2D tex_specular;
uniform sampler2D tex_stencil_decal;

void main()
{
	outColor = vec4(texture2D(tex_ambient, Texcoord).rgb, 1.0) * vec4(vec3(abs(dot(Normal, normalize(vec3(-1.0, -3.0, -1.0))))), 1.0);
	if(texture2D(tex_alpha, Texcoord).r == 0 && texture2D(tex_alpha, Texcoord).a == 1)
	{
		discard;
	}
}";
	}
}

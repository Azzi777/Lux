using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Lux.Graphics
{
	internal class ShaderProgram
	{
		public int ID { get; private set; }

		public ShaderProgram(string fragmentShader)
		{
			ID = GL.CreateProgram();

			int fID = CompileShader(fragmentShader, ShaderType.FragmentShader);
			GL.AttachShader(ID, fID);

			GL.LinkProgram(ID);

			string info = GL.GetProgramInfoLog(ID);
			System.Console.WriteLine(info);

			if (fID != 0)
			{
				GL.DeleteShader(fID);
			}
		}

		public ShaderProgram(string vertexShader, string fragmentShader)
		{
			ID = GL.CreateProgram();

			int vID = CompileShader(vertexShader, ShaderType.VertexShader);
			GL.AttachShader(ID, vID);

			int fID = CompileShader(fragmentShader, ShaderType.FragmentShader);
			GL.AttachShader(ID, fID);

			GL.LinkProgram(ID);

			string info = GL.GetProgramInfoLog(ID);
			System.Console.WriteLine(info);

			if (vID != 0)
			{
				GL.DeleteShader(vID);
			}

			if (fID != 0)
			{
				GL.DeleteShader(fID);
			}
		}

		int CompileShader(string source, ShaderType type)
		{
			int id = GL.CreateShader(type);
			GL.ShaderSource(id, source);
			GL.CompileShader(id);

			int compileResult;
			GL.GetShader(id, ShaderParameter.CompileStatus, out compileResult);
			if (compileResult != 1)
			{
				string error = GL.GetShaderInfoLog(id);
				throw new Exception("Error while compiling the vertex shader. \n\nError message: \"" + error + "\"");
			}

			return id;
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

		static internal string ScreenFragmentShaderSource = @"
#version 150

out vec4 outColor;

uniform sampler2D colorTexture;

void main()
{
	vec2 texcoord = gl_FragCoord.xy / vec2(1024.0, 768.0);

	vec3 color = texture2D( colorTexture, texcoord );
	
	outColor = vec4(color, 1.0);
}";
	}
}

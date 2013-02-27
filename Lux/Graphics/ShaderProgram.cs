using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Lux.Graphics
{
	class ShaderProgram
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

		public void Use()
		{
			GL.UseProgram(ID);
		}

		static internal string TextureVertexShaderSource = @"
#version 150
in vec3 inPosition;
in vec2 inTexcoord;
in vec3 inNormal;

out vec2 Texcoord;
out vec3 Normal;

uniform mat4 worldMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

void main()
{
	Texcoord = inTexcoord;
	Normal = inNormal;

	gl_Position = worldMatrix * viewMatrix * projectionMatrix * vec4(inPosition, 1.0);
}";

		static internal string TextureFragmentShaderSource = @"
#version 150
in vec2 Texcoord;
in vec3 Normal;
in int Texture;

out vec4 outColor;

uniform sampler2DArray textureArray;

void main()
{
	outColor = texture2DArray(textureArray, vec3(Texcoord, Texture));
}";
	}
}

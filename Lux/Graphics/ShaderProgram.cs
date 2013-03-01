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

		public void SetMatrix4(string varName, Lux.Framework.Matrix4 mat4)
		{
			float[] temp_mat4 = new float[16] {
				(float)mat4.OpenTKEquivalent.M11, (float)mat4.OpenTKEquivalent.M12, (float)mat4.OpenTKEquivalent.M13, (float)mat4.OpenTKEquivalent.M14,
				(float)mat4.OpenTKEquivalent.M21, (float)mat4.OpenTKEquivalent.M22, (float)mat4.OpenTKEquivalent.M23, (float)mat4.OpenTKEquivalent.M24,
				(float)mat4.OpenTKEquivalent.M31, (float)mat4.OpenTKEquivalent.M32, (float)mat4.OpenTKEquivalent.M33, (float)mat4.OpenTKEquivalent.M34,
				(float)mat4.OpenTKEquivalent.M41, (float)mat4.OpenTKEquivalent.M42, (float)mat4.OpenTKEquivalent.M43, (float)mat4.OpenTKEquivalent.M44
			};

			GL.UniformMatrix4(GL.GetUniformLocation(ID, varName), 1, false, temp_mat4);
		}

		public void SetVertexFormat()
		{
			int posAttrib = GL.GetAttribLocation(ID, "inPosition");
			GL.VertexAttribPointer(posAttrib, 3, VertexAttribPointerType.Float, false, 32, 0);
			GL.EnableVertexAttribArray(posAttrib);

			int normAttrib = GL.GetAttribLocation(ID, "inNormal");
			GL.VertexAttribPointer(normAttrib, 3, VertexAttribPointerType.Float, false, 32, 12);
			GL.EnableVertexAttribArray(normAttrib);

			int texAttrib = GL.GetAttribLocation(ID, "inTexCoord");
			GL.VertexAttribPointer(texAttrib, 2, VertexAttribPointerType.Float, false, 32, 24);
			GL.EnableVertexAttribArray(texAttrib);
		}

		static internal string TextureVertexShaderSource = @"
#version 150
in vec3 inPosition;
in vec3 inNormal;
in vec2 inTexCoord;

out vec3 normal;
out vec2 texCoord;
out vec3 light_dir;
out vec3 eye_dir;

uniform mat4 mat_world;
uniform mat4 mat_view;
uniform mat4 mat_proj;
uniform vec3 light_pos;
uniform vec3 eye_pos;

void main()
{
	normal = inNormal;
	texCoord = inTexCoord;

	light_dir = normalize(vec3(2.0, 3.0, 1.0));//normalize(inPosition - light_pos);
	eye_dir = normalize(inPosition - eye_pos);

	gl_Position = mat_proj * mat_view * mat_world * vec4(inPosition, 1.0);
}";

		static internal string TextureFragmentShaderSource = @"
#version 150
in vec3 normal;
in vec2 texCoord;

out vec4 outColor;

uniform sampler2D tex_ambient;
uniform sampler2D tex_diffuse;
uniform sampler2D tex_alpha;
uniform sampler2D tex_bump;
uniform sampler2D tex_specular_highlight;
uniform sampler2D tex_specular;
uniform sampler2D tex_stencil_decal;

uniform vec4 light_ambient;
uniform vec4 light_diffuse;
uniform vec4 light_specular;
in vec3 light_dir;
in vec3 eye_dir;


uniform vec4 mat_ambient;
uniform vec4 mat_diffuse;
uniform vec4 mat_specular;
uniform float mat_shininess;

void main()
{
//	vec3 tangent;
//	vec3 binormal;
//
//	vec3 c1 = cross(normal, vec3(0.0, 0.0, 1.0));
//	vec3 c2 = cross(normal, vec3(0.0, 1.0, 0.0));
//
//	if (length(c1) > length(c2))
//	{
//		tangent = normalize(c1);;
//	}
//	else
//	{
//		tangent = normalize(c2);;
//	}
//
//	binormal = cross(normal, tangent);
//	binormal = normalize(binormal);
//
//	outColor = vec4(texture2D(tex_ambient, texCoord).rgb * max(dot(normal, normalize(eye_dir + light_dir)), 0.0), 1.0);
//	return;
	vec3 normalBump = normalize(texture2D(tex_bump, texCoord).rgb * 2.0 - 1.0);

	vec3 halfVec = (eye_dir + light_dir) / length(eye_dir + light_dir);
	vec3 tangent = cross(normalBump, vec3(1.0));
	vec3 biNormal = cross(normalBump, tangent);

	float diffuseCoef = max(dot(light_dir, normalBump), 0.0);
	float specularCoef = pow(max(dot(halfVec, normalBump), 0.0), mat_shininess);

	vec4 ambient = texture2D(tex_ambient, texCoord) * light_ambient * mat_ambient;
	vec4 diffuse = texture2D(tex_diffuse, texCoord) * light_diffuse * mat_diffuse * diffuseCoef;
	vec4 specular = texture2D(tex_specular, texCoord) * light_specular * mat_specular * specularCoef;
	
	outColor.rgb = (ambient + diffuse + specular).rgb * (max(dot(light_dir, normal), 0.0) + 0.3) * 1.5;
	outColor.a = 1.0;

	if(texture2D(tex_alpha, texCoord).r == 0 && texture2D(tex_alpha, texCoord).a == 1)
	{
		discard;
	}
}";

		static internal string ScreenFragmentShaderSource = @"
#version 150

out vec4 outColor;

uniform sampler2DMS colorTexture;
uniform int samples;

void main()
{
	vec3 color = vec3(0.0);
	for (int i = 0; i < samples; i++)
	{
		color += texelFetch(colorTexture, ivec2(gl_FragCoord.xy), i).rgb;
	}
	
	outColor = vec4(color / samples, 1.0);
}";
	}
}

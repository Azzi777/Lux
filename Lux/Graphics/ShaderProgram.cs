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
			GL.VertexAttribPointer(posAttrib, 3, VertexAttribPointerType.Float, false, 44, 0);
			GL.EnableVertexAttribArray(posAttrib);

			int normAttrib = GL.GetAttribLocation(ID, "inNormal");
			GL.VertexAttribPointer(normAttrib, 3, VertexAttribPointerType.Float, false, 44, 12);
			GL.EnableVertexAttribArray(normAttrib);

			int texAttrib = GL.GetAttribLocation(ID, "inTexCoord");
			GL.VertexAttribPointer(texAttrib, 2, VertexAttribPointerType.Float, false, 44, 24);
			GL.EnableVertexAttribArray(texAttrib);

			int tangAttrib = GL.GetAttribLocation(ID, "inTangent");
			GL.VertexAttribPointer(tangAttrib, 3, VertexAttribPointerType.Float, false, 44, 32);
			GL.EnableVertexAttribArray(tangAttrib);
		}

		static internal string TextureVertexShaderSource = @"
#version 420
in vec3 inPosition;
in vec3 inNormal;
in vec2 inTexCoord;
in vec3 inTangent;

smooth out vec2 texCoord;
smooth out vec3 normal;
smooth out vec3 light_dir;
smooth out vec3 eye_dir;
smooth out vec3 tangent;
smooth out vec3 binormal;

uniform mat4 mat_world;
uniform mat4 mat_view;
uniform mat4 mat_proj;
uniform vec3 light_pos;
uniform vec3 eye_pos;

void main()
{
	normal = inNormal;
	texCoord = inTexCoord;
	tangent = inTangent;
	binormal = normalize(cross(normal, tangent));

	light_dir = normalize(light_pos - inPosition);
	eye_dir = normalize(inPosition - eye_pos);

	gl_Position = mat_proj * mat_view * mat_world * vec4(inPosition, 1.0);
}";

		static internal string TextureFragmentShaderSource = @"
#version 420
smooth in vec3 normal;
smooth in vec3 tangent;
smooth in vec3 binormal;
smooth in vec2 texCoord;


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
smooth in vec3 light_dir;
smooth in vec3 eye_dir;


uniform vec4 mat_ambient;
uniform vec4 mat_diffuse;
uniform vec4 mat_specular;
uniform float mat_shininess;

out vec4 colorOut;
out vec4 normalOut;

void main()
{
	
	mat3 rotation = mat3(tangent, binormal, normal);

	vec3 normalBump = normal;
	
	if(texture2D(tex_bump, texCoord).a != 0.0)
	{
		vec3 bump = texture2D(tex_bump, texCoord).rgb * 2.0 - vec3(1.0);
		normalBump += rotation * bump * 2;
	}
	normalBump = normalBump;

	normalOut = vec4(0.0);
	normalOut.a = 1.0;
	normalOut.rgb = (normalize(normalBump) / 2 + vec3(0.5));

	vec3 halfVec = normalize(eye_dir + light_dir);
	
	float diffuseCoef = max(dot(normalBump, light_dir), 0.0);
	float specularCoef = pow(clamp(dot(normalBump, halfVec), 0.0, 1.0), mat_shininess);
	
	vec4 ambient = texture2D(tex_ambient, texCoord) * light_ambient * mat_ambient;
	vec4 diffuse = texture2D(tex_diffuse, texCoord) * light_diffuse * mat_diffuse * diffuseCoef;
	vec4 specular = texture2D(tex_specular, texCoord) * light_specular * mat_specular * specularCoef;

	colorOut.rgb = (ambient + diffuse + specular).rgb;
	colorOut.a = 1.0;

	if(texture2D(tex_alpha, texCoord).a == 1)
	{
		//outColor.a = texture2D(tex_alpha, texCoord).r;

		if(texture2D(tex_alpha, texCoord).r <= 0.5)
		{
			discard;
		}
	}
}
";

		static internal string ScreenFragmentShaderSource = @"
#version 420

out vec4 outColor;

uniform sampler2DMS colorTexture;
uniform sampler2DMS normalTexture;
uniform sampler2DMS depthTexture;

uniform int samples;
uniform vec2 cameraRange;

vec4 readMSPixel( sampler2DMS texture, in ivec2 coord ) 
{
	vec4 pixel = vec4(0.0);
	for (int i = 0; i < samples; i++)
	{
		pixel += texelFetch(texture, coord, i) / samples;
	}
	return pixel;
}

float readDepth( in ivec2 coord ) 
{
	return (2.0 * cameraRange.x) / (cameraRange.y + cameraRange.x - readMSPixel(depthTexture, coord).r * (cameraRange.y - cameraRange.x));	
}
 
float compareDepths( in float depth1, in float depth2 ) {
	float aoCap = 1.0;
	float aoMultiplier=10000.0;
	float depthTolerance=0.000;
	float aorange = 100.0;// units in space the AO effect extends to (this gets divided by the camera far range
	float diff = sqrt( clamp(1.0-(depth1-depth2) / (aorange/(10000.0-10.0)),0.0,1.0) );
	float ao = min(aoCap,max(0.0,depth1-depth2-depthTolerance) * aoMultiplier) * diff;
	return ao;
}
 
void main(void)
{	
	float depth = readDepth( ivec2(gl_FragCoord.xy) );
	float d;
 
	float pw = 1;
	float ph = 1;
 
	float aoCap = 1.0;
 
	float ao = 0.0;
 
	float aoMultiplier=10000.0;
 
	float depthTolerance = 0.001;
 
	float aoscale=1.0;
	

	int passes = 4;
	for(int i = 0; i < passes; i++)
	{
		d = readDepth( ivec2(gl_FragCoord.xy) + ivec2(pw, ph));
		ao += compareDepths(depth, d) / aoscale;
 
		d = readDepth( ivec2(gl_FragCoord.xy) + ivec2(-pw, ph));
		ao += compareDepths(depth, d) / aoscale;
 
		d = readDepth( ivec2(gl_FragCoord.xy) + ivec2(pw, -ph));
		ao += compareDepths(depth, d) / aoscale;
 
		d = readDepth( ivec2(gl_FragCoord.xy) + ivec2(-pw, -ph));
		ao += compareDepths(depth, d) / aoscale;
 
		pw *= 2.0;
		ph *= 2.0;
		aoMultiplier /= 2.0;
		aoscale *= 1.2;
	}
	ao /= passes * 4.0;
	ao = clamp(ao, 0.25, 0.5);

	outColor = vec4(1.0 - ao);
	//outColor.rgb *= readMSPixel(colorTexture, ivec2(gl_FragCoord.xy)).rgb;
	outColor.rgb = readMSPixel(colorTexture, ivec2(gl_FragCoord.xy)).rgb;
}";
	}
}

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
			GL.VertexAttribPointer(posAttrib, 3, VertexAttribPointerType.Float, false, 48, 0);
			GL.EnableVertexAttribArray(posAttrib);

			int normAttrib = GL.GetAttribLocation(ID, "inNormal");
			GL.VertexAttribPointer(normAttrib, 3, VertexAttribPointerType.Float, false, 48, 12);
			GL.EnableVertexAttribArray(normAttrib);

			int texAttrib = GL.GetAttribLocation(ID, "inTexCoord");
			GL.VertexAttribPointer(texAttrib, 2, VertexAttribPointerType.Float, false, 48, 24);
			GL.EnableVertexAttribArray(texAttrib);

			int tangAttrib = GL.GetAttribLocation(ID, "inTangent");
			GL.VertexAttribPointer(tangAttrib, 4, VertexAttribPointerType.Float, false, 48, 32);
			GL.EnableVertexAttribArray(tangAttrib);
		}

		static internal string TextureVertexShaderSource = @"
#version 420
in vec3 inPosition;
in vec3 inNormal;
in vec2 inTexCoord;
in vec4 inTangent;

smooth out vec3 normal;
smooth out vec3 light_dir;
smooth out vec3 eye_dir;
out vec2 texCoord;
out vec3 tangent;
out vec3 binormal;

uniform mat4 mat_world;
uniform mat4 mat_view;
uniform mat4 mat_proj;
uniform vec3 light_pos;
uniform vec3 eye_pos;

void main()
{
	normal = inNormal;
	texCoord = inTexCoord;
	tangent = inTangent.xyz;
	binormal = normalize(cross(normal, tangent));

	if(inTangent.w == -1.0)
	{
		binormal *= -1;
	}

	vec3 vertex_pos = (mat_world * vec4(inPosition, 1.0)).xyz;

	light_dir = normalize(vertex_pos - light_pos);
	eye_dir = normalize(vertex_pos - eye_pos);

	gl_Position = mat_proj * mat_view * mat_world * vec4(inPosition, 1.0);
}";

		static internal string TextureFragmentShaderSource = @"
#version 420
smooth in vec3 normal;
in vec2 texCoord;
in vec3 tangent;
in vec3 binormal;


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
out vec4 tangentOut;

void main()
{
	mat3 rotation = mat3(tangent, binormal, normal);

	vec3 normalBump = normal;
	
	if(texture2D(tex_bump, texCoord).a != 0.0)
	{
		vec3 bump = texture2D(tex_bump, texCoord).rgb * 2.0 - vec3(1.0);
		normalBump += rotation * bump;
	}
	normalOut.a = 1.0;
	normalOut.rgb = (normalize(normalBump) / 2 + vec3(0.5));

	tangentOut.a = 1.0;
	tangentOut.rgb = (normalize(tangent) / 2 + vec3(0.5));
	
	float lambertTerm = max(dot(-light_dir, normalBump), 0.0);
	float specularCoef = pow(max(dot(reflect(-light_dir, normalBump), eye_dir), 0.0), mat_shininess);
	
	float attenuation = 0.7;
	
	vec4 col_ambient = texture2D(tex_ambient, texCoord);
	vec4 col_diffuse = texture2D(tex_diffuse, texCoord);
	vec4 col_specular = texture2D(tex_specular, texCoord);

	col_specular = (col_specular.a != 0.0) ? col_specular : vec4(vec3(0.5), 1.0);
	
	vec3 ambient = (col_ambient * light_ambient * mat_ambient).rgb;
	vec3 diffuse = (col_diffuse * light_diffuse * mat_diffuse).rgb * lambertTerm * attenuation;
	vec3 specular = (col_specular * light_specular * mat_specular).rgb * specularCoef * attenuation * 0.5;

	colorOut.rgb = ambient + diffuse + specular;
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

uniform vec3 eye_pos;

uniform sampler2DMS colorTexture;
uniform sampler2DMS normalTexture;
uniform sampler2DMS tangentTexture;
uniform sampler2DMS depthTexture;

uniform int samples;
uniform int bufferType;
uniform vec2 cameraRange;

uniform mat4 mat_world;
uniform mat4 mat_view;
uniform mat4 mat_proj;

vec4 readMSPixel(sampler2DMS texture, ivec2 coord) 
{
	vec4 pixel = vec4(0.0);
	for (int i = 0; i < samples; i++)
	{
		pixel += texelFetch(texture, coord, i) / samples;
	}
	return pixel;
}

float translateDepth(float depth) 
{
	vec3 nearFarSettings = vec3(cameraRange.y, cameraRange.y - cameraRange.x, cameraRange.y * cameraRange.x);
	return nearFarSettings.z / (nearFarSettings.x - depth * nearFarSettings.y);
}

float readDepth(ivec2 coord) 
{
	return translateDepth(readMSPixel(depthTexture, coord).r);
}

float readDepth(ivec2 coord, int sampleIndex) 
{
	return translateDepth(texelFetch(depthTexture, coord, sampleIndex).r);
}

vec3 WorldPositionFromDepth(vec2 coord, int sampleIndex)
{
	float depth = texelFetch(depthTexture, ivec2(coord), sampleIndex).r;
	vec2 normScrCoord = (coord / vec2(1440, 900)); // 0 to 1 range

	vec4 v_screen = vec4(normScrCoord, depth, 1.0 );
	vec4 v_homo = inverse(mat_proj) * 2.0 * (v_screen - vec4(0.5));
	vec3 v_eye = v_homo.xyz / v_homo.w; //transfer from homogeneous coordinates

	return (inverse(mat_view) * vec4(v_eye, 1.0)).xyz;
}

float random(float start, float end, vec2 seed)
{
	//seed *= -abs(dot( seed, (mat_view * inverse(mat_proj) * vec4(seed, eye_pos.zy)).xy));
	uint n = floatBitsToUint(seed.y * 214013.0 + seed.x * 2531011.0);
	n = n * (n * n * 15731u + 789221u);
	n = (n >> 9u) | 0x3F800000u;

	float value = 2.0 - uintBitsToFloat(n);

	value = start + value * (end - start);
    return value;
}

#define kernelSize 16

vec3[kernelSize] generateKernel(vec2 coords) 
{
	vec3[kernelSize] kernel;
	for (int i = 0; i < kernelSize; i++) 
	{
		kernel[i] = normalize(vec3(random(-1.0, 1.0, coords * 1.03 * (i + 1)), random(-1.0, 1.0, coords * 1.05 * (i + 1)), random(0.0, 1.0, coords * 1.07 * (i + 1))));
		float scale = float(i) / float(kernelSize);
		kernel[i] *= (0.1 + scale * scale * 0.9);
	}
	return kernel;
}

float calculateOcclusionFactor(vec2 screenCoords, int sampleIndex) 
{
	vec3 position = WorldPositionFromDepth(screenCoords, sampleIndex);
	vec3 normal = texelFetch(normalTexture, ivec2(screenCoords), sampleIndex).xyz * 2.0 - vec3(1.0);
	vec3 tangent = texelFetch(tangentTexture, ivec2(screenCoords), sampleIndex).xyz * 2.0 - vec3(1.0);
	vec3 binormal = normalize(cross(normal, tangent));

	mat3 rotation = mat3(tangent, binormal, normal);
	
	vec3[kernelSize] kernel = generateKernel(screenCoords * 1.01 * sampleIndex);

	float radius = 40;
	float occlusion = 0.0;
	
	int validCycles = 1;
	for(int i = 0; i < kernelSize; i++)
	{
		vec3 samplePos = (rotation * kernel[i]) * radius + position;

		vec4 offset =  mat_proj * mat_view * mat_world * vec4(samplePos, 1.0);
		offset.xy /= offset.w;
		offset.xy = (offset.xy * 0.5 + 0.5) * vec2(1440, 900);

		if(offset.x < 0.0 || offset.y < 0.0 || offset.x > 1440.0 || offset.y > 900.0)
		{
			continue;
		}

		float sampleDepth = offset.w;
		float fragmentDepth = readDepth(ivec2(offset.xy), sampleIndex);
		
		if(fragmentDepth < sampleDepth && abs(fragmentDepth - sampleDepth) < 40.0)
		{
			occlusion += abs(fragmentDepth / sampleDepth);//1.0;
		}
		validCycles++;
	}
	float pixelDepth = readDepth(ivec2(screenCoords), sampleIndex);
	if(pixelDepth / cameraRange.y > 0.8) return 1.0;
	return 1.0 - sqrt(occlusion / float(validCycles));
}


float SSAO(vec2 screenCoords) 
{
	float occlusion = 0.0;
	for (int i = 0; i < samples; i++)
	{
		occlusion += calculateOcclusionFactor(screenCoords, i) / samples;
	}
	return occlusion;
}

 
void main(void)
{	
	switch(bufferType)
	{
		case 0:
		{
			outColor.rgb = readMSPixel(colorTexture, ivec2(gl_FragCoord.xy * 2)).rgb;
			break;
		}

		case 1:
		{
			outColor.rgb = readMSPixel(normalTexture, ivec2(gl_FragCoord.xy * 2 - vec2(1440, 0))).rgb;
			break;
		}

		case 2:
		{
			outColor.rgb = vec3(SSAO(gl_FragCoord.xy * 2 - vec2(0, 900)));
			break;
		}

		case 3:
		{
			outColor.rgb = readMSPixel(tangentTexture, ivec2(gl_FragCoord.xy * 2 - vec2(1440, 900))).rgb;
			//outColor.rgb = vec3(random(0.5, 1.0, gl_FragCoord.xy));
			//outColor.rgb = readMSPixel(colorTexture, ivec2(gl_FragCoord.xy * 2 - vec2(1440, 900))).rgb * vec3(SSAO(gl_FragCoord.xy * 2 - vec2(1440, 900)));
			break;
		}
	}
}";
	}
}

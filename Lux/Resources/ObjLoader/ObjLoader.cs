using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using OpenTK.Graphics;

using Lux.Graphics;
using Lux.Resources.ObjLoader;

namespace Lux.Resources.ObjLoader
{
	static internal class ObjLoader
	{
		internal static Model LoadFromFile(string path)
		{
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;			
			ObjModel result = ObjModel.Load(@"models\sponza.obj");
						
			List<MeshVertex> meshVertices = new List<MeshVertex>();

			Dictionary<string, Texture> textures = LoadMTLTextures(result.Materials.Values.ToList());

			List<Mesh> meshes = new List<Mesh>();

			foreach (var group in result.Groups)
			{
				List<uint> meshIndices = new List<uint>();

				foreach (var face in group.Faces)
				{
					for (int i = 0; i < face.Vertices.Count; i++)
					{
						int vertexPointer = face.Vertices[i].VertexIndex - 1;
						int texturePointer = face.Vertices[i].TextureIndex - 1;
						int normalPointer = face.Vertices[i].NormalIndex - 1;

						meshIndices.Add((uint)meshVertices.Count);

						meshVertices.Add(new MeshVertex(
							result.Vertices[vertexPointer],
							result.Normals[normalPointer],
							result.Textures[texturePointer]));
					}

					if (face.Vertices.Count == 4)
					{
						uint vertexPointer1 = meshIndices[meshIndices.Count - 4];
						uint vertexPointer2 = meshIndices[meshIndices.Count - 2];

						meshIndices.Insert(meshIndices.Count - 1, vertexPointer1);
						meshIndices.Insert(meshIndices.Count - 1, vertexPointer2);
					}
				}
				Mesh currentMesh = new Mesh(meshIndices.ToArray());

				ApplyMaterial(currentMesh, group.Material, textures);

				meshes.Add(currentMesh);
			}

			return new Model(meshVertices.ToArray(), meshes.ToArray(), textures.Values.ToArray());
		}

		static private Dictionary<string, Texture> LoadMTLTextures(List<Material> mtl)
		{
			Dictionary<string, Texture> returnValue = new Dictionary<string, Texture>();

			foreach (Material material in mtl)
			{
				if(material.AlphaTextureMap != null)
				{
					if (!returnValue.ContainsKey(material.AlphaTextureMap) && File.Exists(material.AlphaTextureMap))
					{
						returnValue.Add(material.AlphaTextureMap, new Texture(material.AlphaTextureMap));
					}
				}
				if (material.AmbientTextureMap != null)
				{
					if (!returnValue.ContainsKey(material.AmbientTextureMap) && File.Exists(material.AmbientTextureMap))
					{
						returnValue.Add(material.AmbientTextureMap, new Texture(material.AmbientTextureMap));
					}
				}
				if (material.BumpMap != null)
				{
					if (!returnValue.ContainsKey(material.BumpMap) && File.Exists(material.BumpMap))
					{
						returnValue.Add(material.BumpMap, new Texture(material.BumpMap));
					}
				}
				if (material.DiffuseTextureMap != null)
				{
					if (!returnValue.ContainsKey(material.DiffuseTextureMap) && File.Exists(material.DiffuseTextureMap))
					{
						returnValue.Add(material.DiffuseTextureMap, new Texture(material.DiffuseTextureMap));
					}
				}
				if (material.SpecularHighlightTextureMap != null)
				{
					if (!returnValue.ContainsKey(material.SpecularHighlightTextureMap) && File.Exists(material.SpecularHighlightTextureMap))
					{
						returnValue.Add(material.SpecularHighlightTextureMap, new Texture(material.SpecularHighlightTextureMap));
					}
				}
				if (material.SpecularTextureMap != null)
				{
					if (!returnValue.ContainsKey(material.SpecularTextureMap) && File.Exists(material.SpecularTextureMap))
					{
						returnValue.Add(material.SpecularTextureMap, new Texture(material.SpecularTextureMap));
					}
				}
				if (material.StencilDecalMap != null)
				{
					if (!returnValue.ContainsKey(material.StencilDecalMap) && File.Exists(material.StencilDecalMap))
					{
						returnValue.Add(material.StencilDecalMap, new Texture(material.StencilDecalMap));
					}
				}
			}

			return returnValue;
		}

		static private void ApplyMaterial(Mesh mesh, Material material, Dictionary<string, Texture> textures)
		{
			if (material.AlphaTextureMap != null && textures.ContainsKey(material.AlphaTextureMap))
			{
				mesh.AlphaTexture = textures[material.AlphaTextureMap];
			}
			if (material.AmbientTextureMap != null && textures.ContainsKey(material.AmbientTextureMap))
			{
				mesh.AmbientTexture = textures[material.AmbientTextureMap];
			}
			if (material.BumpMap != null && textures.ContainsKey(material.BumpMap))
			{
				mesh.BumpMapTexture = textures[material.BumpMap];
			}
			if (material.DiffuseTextureMap != null && textures.ContainsKey(material.DiffuseTextureMap))
			{
				mesh.DiffuseTexture = textures[material.DiffuseTextureMap];
			}
			if (material.SpecularHighlightTextureMap != null && textures.ContainsKey(material.SpecularHighlightTextureMap))
			{
				mesh.SpecularHighlightTexture = textures[material.SpecularHighlightTextureMap];
			}
			if (material.SpecularTextureMap != null && textures.ContainsKey(material.SpecularTextureMap))
			{
				mesh.SpecularTexture = textures[material.SpecularTextureMap];
			}
			if (material.StencilDecalMap != null && textures.ContainsKey(material.StencilDecalMap))
			{
				mesh.StencilDecal = textures[material.StencilDecalMap];
			}

			mesh.AmbientColor = material.AmbientColor;
			mesh.DiffuseColor = material.DiffuseColor;
			mesh.SpecularColor = material.SpecularColor;
			mesh.SpecularCoefficient = material.SpecularCoefficient;
		}
	}
}

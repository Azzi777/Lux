using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using OpenTK.Graphics;

using ObjLoader.Loader.Loaders;
 
using Lux.Graphics;

namespace Lux.Resources
{
	static internal class ObjLoader2
	{
		internal static Model LoadFromFile(string path)
		{
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			Directory.SetCurrentDirectory("models");
			var objLoaderFactory = new ObjLoaderFactory();
			var objLoader = objLoaderFactory.Create(new MaterialStreamProvider());

			var result = objLoader.Load(new FileStream("sponza.obj", FileMode.Open));
						
			List<MeshVertex> meshVertices = new List<MeshVertex>();

			Dictionary<string, Texture> textures = LoadMTLTextures(result.Materials);

			List<Mesh> meshes = new List<Mesh>();

			foreach (var group in result.Groups)
			{
				List<uint> meshIndices = new List<uint>();

				foreach (var face in group.Faces)
				{
					for (int i = 0; i < face.Count; i++)
					{
						int vertexPointer = face[i].VertexIndex - 1;
						int texturePointer = face[i].TextureIndex - 1;
						int normalPointer = face[i].NormalIndex - 1;

						meshIndices.Add((uint)meshVertices.Count);

						meshVertices.Add(new MeshVertex(
							new MeshPosition(result.Vertices[vertexPointer].X, result.Vertices[vertexPointer].Y, result.Vertices[vertexPointer].Z),
							new MeshNormal(result.Normals[normalPointer].X, result.Normals[normalPointer].Y, result.Normals[normalPointer].Z),
							new MeshTexCoord(result.Textures[texturePointer].X, result.Textures[texturePointer].Y)));
					}

					if (face.Count == 4)
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

		static private Dictionary<string, Texture> LoadMTLTextures(IList<ObjLoader.Loader.Data.Material> mtl)
		{
			Dictionary<string, Texture> returnValue = new Dictionary<string, Texture>();

			foreach (var material in mtl)
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

		static private void ApplyMaterial(Mesh mesh, ObjLoader.Loader.Data.Material material, Dictionary<string, Texture> textures)
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

			mesh.AmbientColor = new Color4(material.AmbientColor.X, material.AmbientColor.Y, material.AmbientColor.Z, 1.0F);
			mesh.DiffuseColor = new Color4(material.DiffuseColor.X, material.DiffuseColor.Y, material.DiffuseColor.Z, 1.0F);
			mesh.SpecularColor = new Color4(material.SpecularColor.X, material.SpecularColor.Y, material.SpecularColor.Z, 1.0F);
			mesh.SpecularCoefficient = material.SpecularCoefficient;
		}
	}
}

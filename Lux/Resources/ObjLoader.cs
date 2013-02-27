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
						
			MeshVertex[] meshVertices = new MeshVertex[result.Vertices.Count];
			MeshTexCoord[] meshTexCoords = new MeshTexCoord[result.Vertices.Count];
			MeshNormal[] meshNormals = new MeshNormal[result.Vertices.Count];

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

						meshIndices.Add((uint)vertexPointer);

						meshVertices[vertexPointer] = new MeshVertex(result.Vertices[vertexPointer].X, result.Vertices[vertexPointer].Y, result.Vertices[vertexPointer].Z);
						meshTexCoords[vertexPointer] = new MeshTexCoord(result.Textures[texturePointer].X, result.Textures[texturePointer].Y);
						meshNormals[vertexPointer] = new MeshNormal(result.Normals[normalPointer].X, result.Normals[normalPointer].Y, result.Normals[normalPointer].Z);
					}

					if (face.Count == 4)
					{
						int vertexPointer1 = face[0].VertexIndex - 1;
						int vertexPointer2 = face[2].VertexIndex - 1;

						meshIndices.Insert(meshIndices.Count - 1, (uint)vertexPointer1);
						meshIndices.Insert(meshIndices.Count - 1, (uint)vertexPointer2);
					}
				}
				Mesh currentMesh = new Mesh(meshIndices.ToArray());

				ApplyMaterial(currentMesh, group.Material, textures);

				meshes.Add(currentMesh);
			}

			return new Model(meshVertices, meshNormals, meshTexCoords, meshes.ToArray());
		}

		static private Dictionary<string, Texture> LoadMTLTextures(IList<ObjLoader.Loader.Data.Material> mtl)
		{
			Dictionary<string, Texture> returnValue = new Dictionary<string, Texture>();

			foreach (var material in mtl)
			{
				if(material.AlphaTextureMap != null)
				{
					if (!returnValue.ContainsKey(material.AlphaTextureMap))
					{
						returnValue.Add(material.AlphaTextureMap, new Texture(material.AlphaTextureMap));
					}
				}
				if (material.AmbientTextureMap != null)
				{
					if (!returnValue.ContainsKey(material.AmbientTextureMap))
					{
						returnValue.Add(material.AmbientTextureMap, new Texture(material.AmbientTextureMap));
					}
				}
				if (material.BumpMap != null)
				{
					if (!returnValue.ContainsKey(material.BumpMap))
					{
						returnValue.Add(material.BumpMap, new Texture(material.BumpMap));
					}
				}
				if (material.DiffuseTextureMap != null)
				{
					if (!returnValue.ContainsKey(material.DiffuseTextureMap))
					{
						returnValue.Add(material.DiffuseTextureMap, new Texture(material.DiffuseTextureMap));
					}
				}
				if (material.SpecularHighlightTextureMap != null)
				{
					if (!returnValue.ContainsKey(material.SpecularHighlightTextureMap))
					{
						returnValue.Add(material.SpecularHighlightTextureMap, new Texture(material.SpecularHighlightTextureMap));
					}
				}
				if (material.SpecularTextureMap != null)
				{
					if (!returnValue.ContainsKey(material.SpecularTextureMap))
					{
						returnValue.Add(material.SpecularTextureMap, new Texture(material.SpecularTextureMap));
					}
				}
				if (material.StencilDecalMap != null)
				{
					if (!returnValue.ContainsKey(material.StencilDecalMap))
					{
						returnValue.Add(material.StencilDecalMap, new Texture(material.StencilDecalMap));
					}
				}
			}

			return returnValue;
		}

		static private void ApplyMaterial(Mesh mesh, ObjLoader.Loader.Data.Material material, Dictionary<string, Texture> textures)
		{
			if (material.AlphaTextureMap != null)
			{
				mesh.AlphaTexture = textures[material.AlphaTextureMap];
			}
			if (material.AlphaTextureMap != null)
			{
				mesh.AmbientTexture = textures[material.AlphaTextureMap];
			}
			if (material.BumpMap != null)
			{
				mesh.BumpMapTexture = textures[material.BumpMap];
			}
			if (material.DiffuseTextureMap != null)
			{
				mesh.DiffuseTexture = textures[material.DiffuseTextureMap];
			}
			if (material.SpecularHighlightTextureMap != null)
			{
				mesh.SpecularHighlightTexture = textures[material.SpecularHighlightTextureMap];
			}
			if (material.SpecularTextureMap != null)
			{
				mesh.SpecularTexture = textures[material.SpecularTextureMap];
			}
			if (material.StencilDecalMap != null)
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

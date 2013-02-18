using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
 
using Lux.Graphics;

namespace Lux.Resources
{
	static internal class ObjLoader
	{
		internal static Model LoadFromFile(string path)
		{
			string directory = path.Substring(0, path.LastIndexOf('\\'));



			List<MeshVertex> tempMeshVertices = new List<MeshVertex>();
			List<MeshTexCoord> tempMeshTexCoords = new List<MeshTexCoord>();
			List<MeshNormal> tempMeshNormals = new List<MeshNormal>();

			Dictionary<KeyValuePair<string, string>, List<string>> meshDefinitions = new Dictionary<KeyValuePair<string, string>, List<string>>();

			string[] lines = File.ReadAllLines(path);
			string mtldefinitions = "";

			string lastObject = "";

			foreach (string line in lines)
			{
				if (line.Length == 0 || line[0] == '#')
				{
					continue;
				}

				string[] parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
				switch (parts[0])
				{
					case "mtllib":
					{
						mtldefinitions = File.ReadAllText(directory + @"\" + parts[1]);
						break;
					}

					case "v":
					{
						tempMeshVertices.Add(new MeshVertex(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
						break;
					}

					case "vt":
					{
						tempMeshTexCoords.Add(new MeshTexCoord(float.Parse(parts[1]), float.Parse(parts[2])));
						break;
					}

					case "vn":
					{
						tempMeshNormals.Add(new MeshNormal(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
						break;
					}

					case "g":
					{
						lastObject = parts[1];
						break;
					}

					case "usemtl":
					{
						if (lastObject == "")
						{
							meshDefinitions.Add(new KeyValuePair<string, string>(parts[1], ""), new List<string>());
						}
						else
						{
							meshDefinitions.Add(new KeyValuePair<string, string>(lastObject, parts[1]), new List<string>());
						}
						break;
					}

					case "f":
					{
						meshDefinitions.Last().Value.Add(line);
						break;
					}
				}
			}
			
			MeshVertex[] meshVertices = new MeshVertex[tempMeshVertices.Count];
			MeshTexCoord[] meshTexCoords = new MeshTexCoord[tempMeshVertices.Count];
			MeshNormal[] meshNormals = new MeshNormal[tempMeshVertices.Count];

			Dictionary<string, Texture> textures = LoadMTLTextures(mtldefinitions, directory);

			List<Mesh> meshes = new List<Mesh>();

			foreach (KeyValuePair<KeyValuePair<string, string>, List<string>> mesh in meshDefinitions)
			{
				List<uint> meshIndices = new List<uint>();
				foreach (string face in mesh.Value)
				{
					string[] parts = face.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

					for (int i = 1; i < parts.Length; i++)
					{
						int vertexPointer = int.Parse(parts[i].Substring(0, parts[i].IndexOf('/'))) - 1;
						int texturePointer = int.Parse(parts[i].Substring(parts[i].IndexOf('/') + 1, parts[i].LastIndexOf('/') - (parts[i].IndexOf('/') + 1))) - 1;
						int normalPointer = int.Parse(parts[i].Substring(parts[i].LastIndexOf('/') + 1)) - 1;

						meshIndices.Add((uint)vertexPointer);

						meshVertices[vertexPointer] = tempMeshVertices[vertexPointer];
						meshTexCoords[vertexPointer] = tempMeshTexCoords[texturePointer];
						meshNormals[vertexPointer] = tempMeshNormals[normalPointer];
					}

					if (parts.Length == 5)
					{
						int vertexPointer1 = int.Parse(parts[1].Substring(0, parts[1].IndexOf('/'))) - 1;
						int vertexPointer2 = int.Parse(parts[3].Substring(0, parts[3].IndexOf('/'))) - 1;

						meshIndices.Insert(meshIndices.Count - 1, (uint)vertexPointer1);
						meshIndices.Insert(meshIndices.Count - 1, (uint)vertexPointer2);
					}
				}
				Mesh currentMesh = new Mesh(meshIndices.ToArray());

				int mtlid = mtldefinitions.IndexOf("newmtl " + mesh.Key.Value);
				string mtldefinition = "";

				if (mtldefinitions.IndexOf("newmtl", mtlid + 1) > 0)
				{
					mtldefinition = mtldefinitions.Substring(mtlid, mtldefinitions.IndexOf("newmtl", mtlid + 1) - mtlid);
				}
				else
				{
					mtldefinition = mtldefinitions.Substring(mtlid);
				}

				ParseMTL(mtldefinition, currentMesh, directory, textures);

				meshes.Add(currentMesh);
			}

			return new Model(meshVertices, meshNormals, meshTexCoords, meshes.ToArray());
		}

		static private Dictionary<string, Texture> LoadMTLTextures(string mtl, string directory)
		{
			mtl = mtl.Replace('\t', ' ');
			Dictionary<string, Texture> returnValue = new Dictionary<string, Texture>();

			foreach (string line in mtl.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
			{
				string[] parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

				if (parts[0] == "map_Ka" || parts[0] == "map_Kd" || parts[0] == "map_d" || parts[0] == "map_bump" || parts[0] == "bump")
				{
					if (!returnValue.ContainsKey(parts[1]))
					{
						returnValue.Add(parts[1], new Texture(directory + @"\" + parts[1]));
					}
				}
			}

			return returnValue;
		}

		static private void ParseMTL(string mtl, Mesh mesh, string directory, Dictionary<string, Texture> textures)
		{
			mtl = mtl.Replace('\t', ' ');

			string[] lines = mtl.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			foreach (string line in lines)
			{
				string[] parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

				switch (parts[0])
				{
					case "newmtl":
					{
						break;
					}

					case "Ns":
					{
						mesh.SpecularCoefficient = float.Parse(parts[1]);
						break;
					}

					case "Ni":
					{
						mesh.ReflectionIndex = float.Parse(parts[1]);
						break;
					}

					case "d":
					{
						mesh.Transparency = float.Parse(parts[1]);
						break;
					}

					case "Tr":
					{
						break;
					}

					case "Tf":
					{
						break;
					}

					case "illum":
					{
						break;
					}

					case "Ka":
					{
						mesh.AmbientColor = new Color4(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]), 1.000F);
						break;
					}

					case "Kd":
					{
						mesh.DiffuseColor = new Color4(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]), 1.000F);
						break;
					}

					case "Ks":
					{
						mesh.SpecularColor = new Color4(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]), 1.000F);
						break;
					}

					case "Ke":
					{
						mesh.EmissiveColor = new Color4(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]), 1.000F);
						break;
					}

					case "map_Ka":
					{
						mesh.AmbientTexture = textures[parts[1]];
						break;
					}

					case "map_Kd":
					{
						mesh.DiffuseTexture = textures[parts[1]];
						break;
					}

					case "map_d":
					{
						mesh.AlphaTexture = textures[parts[1]];
						break;
					}

					case "map_bump":
					{
						mesh.BumpMapTexture = textures[parts[1]];
						break;
					}

					case "bump":
					{
						mesh.BumpMapTexture = textures[parts[1]];
						break;
					}

					default:
					{
						throw new Exception("Unknown attribute in MTL-file!");
					}
				}
			}
		}
	}
}

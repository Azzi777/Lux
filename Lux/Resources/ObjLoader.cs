using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lux.Graphics;

namespace Lux.Resources
{
	static internal class ObjLoader
	{
		//internal static Model LoadFromFile(string path)
		//{
		//	List<Mesh> meshes = new List<Mesh>();
		//	List<MeshVertex> meshVertices = new List<MeshVertex>();
		//	List<uint[]> meshIndices = new List<uint[]>();

		//	string[] lines = File.ReadAllLines(path);

		//	foreach (string line in lines)
		//	{
		//		if (line.Length == 0 || line[0] == '#')
		//		{
		//			continue;
		//		}

		//		string[] parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
		//		switch (parts[0])
		//		{
		//			case "mtllib":
		//			{
		//				break;
		//			}

		//			case "v":
		//			{
		//				meshVertices.Add(new MeshVertex(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]), 200, 200, 200));
		//				break;
		//			}

		//			case "f":
		//			{
		//				List<uint> faceIndices = new List<uint>();
		//				for (int i = 1; i < parts.Length; i++)
		//				{
		//					faceIndices.Add(uint.Parse(parts[i].Substring(0, parts[i].IndexOf('/'))));
		//				}
		//				meshIndices.Add(faceIndices.ToArray());
		//				break;
		//			}

		//			default:
		//			{
		//				break;
		//			}
		//		}
		//	}

		//	meshes.Add(new Mesh(meshVertices.ToArray(), meshIndices));

		//	return new Model(meshes.ToArray());
		//}

		internal static Model LoadFromFile(string path)
		{
			List<MeshVertex> tempMeshVertices = new List<MeshVertex>();
			List<MeshTexCoord> tempMeshTexCoords = new List<MeshTexCoord>();
			List<MeshNormal> tempMeshNormals = new List<MeshNormal>();

			List<string> faceDefinitions = new List<string>();

			string[] lines = File.ReadAllLines(path);

			foreach (string line in lines)
			{
				if (line.Length == 0 || line[0] == '#')
				{
					continue;
				}

				string[] parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
				switch (parts[0])
				{
					case "v":
					{
						tempMeshVertices.Add(new MeshVertex(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]), 200, 200, 200));
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

					case "f":
					{
						faceDefinitions.Add(line);
						break;
					}
				}
			}

			List<Face> meshFaces = new List<Face>();

			MeshVertex[] meshVertices = new MeshVertex[tempMeshVertices.Count];
			MeshTexCoord[] meshTexCoords = new MeshTexCoord[tempMeshVertices.Count];
			MeshNormal[] meshNormals = new MeshNormal[tempMeshVertices.Count];

			foreach (string face in faceDefinitions)
			{
				string[] parts = face.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

				List<uint> meshIndices = new List<uint>();
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

				meshFaces.Add(new Face(meshIndices.ToArray()));
			}

			return new Model(new Mesh[] { new Mesh(meshVertices, meshNormals, meshTexCoords, meshFaces.ToArray()) });
		}
	}
}

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
						tempMeshVertices.Add(new MeshVertex(ParseFloat(parts[1]), ParseFloat(parts[2]), ParseFloat(parts[3]), 200, 200, 200));
						break;
					}

					case "vt":
					{
						tempMeshTexCoords.Add(new MeshTexCoord(ParseFloat(parts[1]), ParseFloat(parts[2])));
						break;
					}

					case "vn":
					{
						tempMeshNormals.Add(new MeshNormal(ParseFloat(parts[1]), ParseFloat(parts[2]), ParseFloat(parts[3])));
						break;
					}

					case "f":
					{
						faceDefinitions.Add(line);
						break;
					}
				}
			}
			
			MeshVertex[] meshVertices = new MeshVertex[tempMeshVertices.Count];
			MeshTexCoord[] meshTexCoords = new MeshTexCoord[tempMeshVertices.Count];
			MeshNormal[] meshNormals = new MeshNormal[tempMeshVertices.Count];
			List<uint> meshIndices = new List<uint>();

			foreach (string face in faceDefinitions)
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

			return new Model(new Mesh[] { new Mesh(meshVertices, meshNormals, meshTexCoords, meshIndices.ToArray()) });
		}

		static internal float ParseFloat(string data)
		{
			return float.Parse(data, System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}

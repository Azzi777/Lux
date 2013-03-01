using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lux.Graphics;
using Lux.Framework;

namespace Lux.Resources.ObjLoader
{
	internal class ObjModel
	{
		public Dictionary<string, Material> Materials { get; set; }
		public List<Group> Groups { get; set; }
		public List<MeshPosition> Vertices { get; set; }
		public List<MeshTexCoord> Textures { get; set; }
		public List<MeshNormal> Normals { get; set; }

		private ObjModel()
		{
			Materials = new Dictionary<string, Material>();
			Groups = new List<Group>();
			Vertices = new List<MeshPosition>();
			Textures = new List<MeshTexCoord>();
			Normals = new List<MeshNormal>();
		}

		static internal ObjModel Load(string path)
		{
			if (path.Contains(@"\"))
			{
				Directory.SetCurrentDirectory(path.Substring(0, path.LastIndexOf(@"\")));
				path = path.Substring(path.LastIndexOf(@"\") + 1);
			}
			ObjModel returnValue = new ObjModel();

			StreamReader stream = new StreamReader(path);

			while (!stream.EndOfStream)
			{
				ParseLine(stream.ReadLine(), ref returnValue);
			}

			return returnValue;
		}

		static private void ParseLine(string line, ref ObjModel returnValue)
		{
			if (line.Length == 0 || line[0] == '#')
			{
				return;
			}

			string[] parts = line.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

			switch (parts[0])
			{
				case "mtllib":
				{
					LoadMTLLib(parts[1], ref returnValue);
					break;
				}

				case "newmtl":
				{
					returnValue.Materials.Add(parts[1], new Material(parts[1]));
					break;
				}

				case "Ka":
				{
					returnValue.Materials.Last().Value.AmbientColor = new Color(double.Parse(parts[1]), double.Parse(parts[2]), double.Parse(parts[3]));
					break;
				}

				case "Kd":
				{
					returnValue.Materials.Last().Value.DiffuseColor = new Color(double.Parse(parts[1]), double.Parse(parts[2]), double.Parse(parts[3]));
					break;
				}

				case "Ks":
				{
					returnValue.Materials.Last().Value.SpecularColor = new Color(double.Parse(parts[1]), double.Parse(parts[2]), double.Parse(parts[3]));
					break;
				}

				case "Ns":
				{
					returnValue.Materials.Last().Value.SpecularCoefficient = float.Parse(parts[1]);
					break;
				}

				case "d":
				{
					returnValue.Materials.Last().Value.Transparency = float.Parse(parts[1]);
					break;
				}

				case "Tr":
				{
					returnValue.Materials.Last().Value.Transparency = float.Parse(parts[1]);
					break;
				}

				case "illum":
				{
					returnValue.Materials.Last().Value.IlluminationModel = int.Parse(parts[1]);
					break;
				}

				case "map_Ka":
				{
					returnValue.Materials.Last().Value.AmbientTextureMap = parts.Last();
					break;
				}

				case "map_Kd":
				{
					returnValue.Materials.Last().Value.DiffuseTextureMap = parts.Last();
					break;
				}

				case "map_Ks":
				{
					returnValue.Materials.Last().Value.SpecularTextureMap = parts.Last();
					break;
				}

				case "map_Ns":
				{
					returnValue.Materials.Last().Value.SpecularHighlightTextureMap = parts.Last();
					break;
				}

				case "map_d":
				{
					returnValue.Materials.Last().Value.AlphaTextureMap = parts.Last();
					break;
				}

				case "map_bump":
				{
					returnValue.Materials.Last().Value.BumpMap = parts.Last();
					break;
				}

				case "bump":
				{
					returnValue.Materials.Last().Value.BumpMap = parts.Last();
					break;
				}

				case "disp":
				{
					returnValue.Materials.Last().Value.DisplacementMap = parts.Last();
					break;
				}

				case "decal":
				{
					returnValue.Materials.Last().Value.StencilDecalMap = parts.Last();
					break;
				}

				case "v":
				{
					returnValue.Vertices.Add(new MeshPosition(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
					break;
				}

				case "vn":
				{
					returnValue.Normals.Add(new MeshNormal(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
					break;
				}

				case "vt":
				{
					returnValue.Textures.Add(new MeshTexCoord(float.Parse(parts[1]), -float.Parse(parts[2])));
					break;
				}

				case "g":
				{
					Material lastMaterial = null;
					if (returnValue.Groups.Count > 0)
					{
						lastMaterial = returnValue.Groups.Last().Material;
					}
					returnValue.Groups.Add(new Group(parts[1]));
					returnValue.Groups.Last().Material = lastMaterial;
					break;
				}

				case "usemtl":
				{
					if (returnValue.Groups.Last().Material != null)
					{
						returnValue.Groups.Add(new Group(parts[1]));
					}
					returnValue.Groups.Last().Material = returnValue.Materials[parts[1]];
					break;
				}

				case "f":
				{
					Face f = new Face();
					for (int i = 1; i < parts.Length; i++)
					{
						f.Vertices.Add(new FaceVertex(int.Parse(parts[i].Split('/')[0]), int.Parse(parts[i].Split('/')[1]), int.Parse(parts[i].Split('/')[2])));
					}
					returnValue.Groups.Last().Faces.Add(f);
					break;
				}
			}
		}

		static private void LoadMTLLib(string path, ref ObjModel returnValue)
		{
			StreamReader stream = new StreamReader(path);

			while (!stream.EndOfStream)
			{
				ParseLine(stream.ReadLine(), ref returnValue);
			}
		}
	}
}

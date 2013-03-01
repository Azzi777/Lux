using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lux.Graphics;

namespace Lux.Resources.ObjLoader
{
	internal class Face
	{
		public readonly List<FaceVertex> Vertices = new List<FaceVertex>();
	}

	internal struct FaceVertex
	{
		internal FaceVertex(int vertexIndex, int textureIndex, int normalIndex)
			: this()
		{
			VertexIndex = vertexIndex;
			TextureIndex = textureIndex;
			NormalIndex = normalIndex;
		}

		public int VertexIndex { get; set; }
		public int TextureIndex { get; set; }
		public int NormalIndex { get; set; }
	}
}

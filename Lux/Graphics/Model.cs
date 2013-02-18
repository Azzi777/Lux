using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using Lux.Framework;
using Lux.Resources;

namespace Lux.Graphics
{
	internal class Model
	{
		Mesh[] Meshes;

		//static public Model UnitCube
		//{
		//	get
		//	{
		//		return new Model(new Mesh[] { Mesh.UnitCube });
		//	}
		//}

		//static public Model UnitIcosahedron
		//{
		//	get
		//	{
		//		return new Model(new Mesh[] { Mesh.UnitIcosahedron });
		//	}
		//}

		public Model(Mesh[] meshes)
		{
			Meshes = meshes;
		}

		static public Model LoadFromFile(string path)
		{
			return ObjLoader.LoadFromFile(path);
		}

		public void Render(Entity entity)
		{
			OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
			OpenTK.Graphics.OpenGL.GL.MultMatrix(entity.TransformMatrix.Data);

			foreach (Mesh m in Meshes)
			{
				m.Render();
			}
		}
	}
}

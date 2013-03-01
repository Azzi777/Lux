using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lux.Resources.ObjLoader
{
	internal class Group
	{
		public List<Face> Faces = new List<Face>();
		public string Name { get; private set; }
		public Material Material { get; set; }

		public Group(string name)
		{
			Name = name;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lux.Framework;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Engine.LoadDependencies();

			Engine engine = new Engine();

			Entity ent1 = engine.CreateEntity("ent.obj", "ent.phys");
			Entity ent2 = engine.CreateEntity("ent.obj", "ent.phys");

			engine.Physics.AddForceGenerator(new Force1(), ent1);
			engine.Physics.AddForceGenerator(new Force2(), ent2);
			engine.Run();

			while (true) ;
		}
	}
}

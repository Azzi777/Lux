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

			engine.CreateEntity("sponza.obj", "ent.phys");

			engine.Run();

			engine.CameraPosition = new Vector3(500, 100, 0);

			double d = 0.0D;
			while (true)
			{
				engine.CameraLookat = engine.CameraPosition + new Vector3(Math.Sin(d), 0, Math.Cos(d));

				d += 0.00000005D;
			}
		}
	}
}

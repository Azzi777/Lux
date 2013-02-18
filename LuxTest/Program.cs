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

			engine.CreateEntity(@"models\sponza.obj", "ent.phys");

			engine.Run();

			engine.CameraPosition = new Vector3(500, 500, 0);

			double d = 0.0D;
			while (true)
			{
				engine.CameraPosition = new Vector3(Math.Sin(d) * 1000, 500, 0);
				engine.CameraLookat = engine.CameraPosition + new Vector3(Math.Sin(10 * d), 0, Math.Cos(10 * d));

				d += 0.000000003D;
			}
		}
	}
}

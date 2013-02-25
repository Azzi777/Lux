using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lux.Framework;
using Lux.Input;
using Lux.Physics;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Engine.LoadDependencies();

			Engine engine = new Engine();

			engine.Run();

			engine.CreateEntity(@"models\sponza.obj", "ent.phys");

			engine.CameraPosition = new Vector3(500, 500, 0);

			Vector3 LookDir = new Vector3(0, 0, 0);

			engine.BindKey(Key.W, () => { engine.CameraPosition += LookDir; });

			double d = 0.0D;
			while (true)
			{
				LookDir = new Vector3(Math.Sin(10 * d), 0, Math.Cos(10 * d));
				engine.CameraLookat = LookDir + engine.CameraPosition;


				d += 0.000000003D;
			}
		}
	}
}

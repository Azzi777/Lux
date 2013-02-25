using System;
using System.Collections.Generic;
using System.Diagnostics;
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

			engine.Input.BindKeyHold(Key.W, () => { engine.CameraPosition += LookDir * 10; });
			engine.Input.BindKeyHold(Key.D, () => { engine.CameraPosition += LookDir.Cross(Vector3.Up) * 10; });
			engine.Input.BindKeyHold(Key.A, () => { engine.CameraPosition -= LookDir.Cross(Vector3.Up) * 10; });
			engine.Input.BindKeyHold(Key.S, () => { engine.CameraPosition -= LookDir * 10; });
			engine.Input.BindKeyHold(Key.Space, () => { engine.CameraPosition += Vector3.Up * 10; });
			engine.Input.BindKeyHold(Key.LeftShift, () => { engine.CameraPosition -= Vector3.Up * 10; });


			Stopwatch timer = new Stopwatch();
			double d = 0;

			while (true)
			{
				timer.Restart();
				LookDir = new Vector3(Math.Sin(d), 0, Math.Cos(d));
				engine.CameraLookat = LookDir + engine.CameraPosition;

				d += 0.001;

				while (timer.Elapsed.TotalMilliseconds < 10) ;
			}
		}
	}
}

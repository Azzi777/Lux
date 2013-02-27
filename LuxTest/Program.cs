using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Lux.Framework;
using Lux.Input;
using Lux.Physics;

namespace Test
{
	class Program
	{
		static Engine engine;
		static Vector3 LookDir;
		static void Main(string[] args)
		{
			Engine.LoadDependencies();

			engine = new Engine();

			engine.Run();

			Console.WriteLine("Loading model...");
			Entity sponza = engine.CreateEntity(@"models\sponza.obj", "ent.phys");
			Console.WriteLine("Done!");

			engine.CameraPosition = new Vector3(500, 100, 0);

			LookDir = new Vector3(0, 0, 0);

			engine.Input.BindKeyHold(Key.W, () => { engine.CameraPosition += Vector3.Forwards * 10; });
			engine.Input.BindKeyHold(Key.D, () => { engine.CameraPosition += Vector3.Right * 10; });
			engine.Input.BindKeyHold(Key.A, () => { engine.CameraPosition += Vector3.Left * 10; });
			engine.Input.BindKeyHold(Key.S, () => { engine.CameraPosition += Vector3.Backwards * 10; });
			engine.Input.BindKeyHold(Key.Space, () => { engine.CameraPosition += Vector3.Up * 10; });
			engine.Input.BindKeyHold(Key.LeftShift, () => { engine.CameraPosition -= Vector3.Up * 10; });


			Stopwatch timer = new Stopwatch();
			double d = 0;


			while (engine.IsRunning)
			{
				timer.Restart();
				LookDir = new Vector3(Math.Sin(d), 0, Math.Cos(d));
				engine.CameraLookat = LookDir + engine.CameraPosition;

				d += 0.0004;

				while (timer.Elapsed.TotalMilliseconds < 1) ;
			}
		}
	}
}

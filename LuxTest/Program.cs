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

		static void Main(string[] args)
		{
			Engine.LoadDependencies();

			engine = new Engine();

			engine.Run();

			Console.WriteLine("Loading model...");
			Entity sponza = engine.CreateEntity(@"models\sponza.obj", "ent.phys");
			Console.WriteLine("Done!");

			engine.CameraPosition = new Vector3(500, 500, 0);

			Vector3 LookDir = new Vector3(0, 0, 0);

			engine.Input.BindKeyHold(Key.W, () => { engine.CameraPosition += LookDir * 10; });
			engine.Input.BindKeyHold(Key.D, () => { engine.CameraPosition += LookDir.Cross(Vector3.Up) * 10; });
			engine.Input.BindKeyHold(Key.A, () => { engine.CameraPosition -= LookDir.Cross(Vector3.Up) * 10; });
			engine.Input.BindKeyHold(Key.S, () => { engine.CameraPosition -= LookDir * 10; });
			engine.Input.BindKeyHold(Key.Space, () => { engine.CameraPosition += Vector3.Up * 10; });
			engine.Input.BindKeyHold(Key.LeftShift, () => { engine.CameraPosition -= Vector3.Up * 10; });
			engine.Input.BindKeyHold(Key.Escape, () => { engine.Stop(); });
			engine.Input.BindMouseEvent(MouseEvent.Move, (MouseEventArguments e) => { Console.WriteLine(e.X + ", " + e.Y); });
			engine.Input.BindMouseEvent(MouseEvent.WheelMove, (MouseEventArguments e) => { Console.WriteLine(e.WheelPosition); });

			Stopwatch timer = new Stopwatch();
			double d = 0;


			while (engine.IsRunning)
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

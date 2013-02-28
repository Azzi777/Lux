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

			engine.Camera = new Camera(0.0, 0.0, 0.0);
			engine.Camera.Position = new Vector3(500, 500, 0);

			engine.Input.BindKeyHold(Key.W, () => { engine.Camera.Position += engine.Camera.GetForwards * 10.0; });
			//engine.Input.BindKeyHold(Key.S, () => { engine.Camera.Position -= engine.CameraLookDir * 10.0; });
			//engine.Input.BindKeyHold(Key.A, () => { engine.Camera.Position -= engine.CameraLookDir.Cross(Vector3.Up) * 10.0; });
			//engine.Input.BindKeyHold(Key.D, () => { engine.Camera.Position += engine.CameraLookDir.Cross(Vector3.Up) * 10.0; });
			//engine.Input.BindKeyHold(Key.Space, () => { engine.CameraPosition += Vector3.PosY * 10.0; });
			//engine.Input.BindKeyHold(Key.LeftShift, () => { engine.CameraPosition += Vector3.NegY * 10.0; });

			engine.Input.BindKeyHold(Key.A, () => { engine.Camera.Yaw += 0.03; if (engine.Camera.Yaw < 0) engine.Camera.Yaw += Math.PI * 2; });
			engine.Input.BindKeyHold(Key.D, () => { engine.Camera.Yaw -= 0.03; if (engine.Camera.Yaw >= Math.PI * 2) engine.Camera.Yaw -= Math.PI * 2; });
			engine.Input.BindKeyHold(Key.W, () => { engine.Camera.Pitch += 0.03; if (engine.Camera.Pitch > Math.PI / 2) engine.Camera.Pitch = Math.PI / 2; });
			engine.Input.BindKeyHold(Key.S, () => { engine.Camera.Pitch -= 0.03; if (engine.Camera.Pitch < -Math.PI / 2) engine.Camera.Pitch = -Math.PI / 2; });

			engine.Input.BindKeyHold(Key.Escape, () => { engine.Stop(); });
			engine.Input.BindMouseEvent(MouseEvent.Move, (MouseEventArguments e) => { Console.WriteLine(e.X + ", " + e.Y); });
			engine.Input.BindMouseEvent(MouseEvent.WheelMove, (MouseEventArguments e) => { Console.WriteLine(e.WheelPosition); });

			Stopwatch timer = new Stopwatch();
			double d = 0;


			while (engine.IsRunning)
			{
				timer.Restart();
				d += 0.00501;

				while (timer.Elapsed.TotalMilliseconds < 1) ;
			}
		}
	}
}

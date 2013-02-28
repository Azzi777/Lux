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
			engine.CameraLookDir = Vector3.PosX;

			double pitch = 0.0;
			double yaw = 0.0;

			engine.Input.BindKeyHold(Key.W, () => { engine.CameraPosition += engine.CameraLookDir * 10.0; });
			engine.Input.BindKeyHold(Key.S, () => { engine.CameraPosition -= engine.CameraLookDir * 10.0; });
			engine.Input.BindKeyHold(Key.A, () => { engine.CameraPosition -= engine.CameraLookDir.Cross(Vector3.Up) * 10.0; });
			engine.Input.BindKeyHold(Key.D, () => { engine.CameraPosition += engine.CameraLookDir.Cross(Vector3.Up) * 10.0; });
			//engine.Input.BindKeyHold(Key.Space, () => { engine.CameraPosition += Vector3.PosY * 10.0; });
			//engine.Input.BindKeyHold(Key.LeftShift, () => { engine.CameraPosition += Vector3.NegY * 10.0; });

			engine.Input.BindKeyHold(Key.Q, () => { yaw -= 0.03; if (yaw < 0) yaw += Math.PI * 2; });
			engine.Input.BindKeyHold(Key.E, () => { yaw += 0.03; if (yaw >= Math.PI * 2) yaw -= Math.PI * 2; });
			engine.Input.BindKeyHold(Key.R, () => { pitch += 0.03; if (pitch > Math.PI / 2 - 0.00001) pitch = Math.PI / 2 - 0.00001; });
			engine.Input.BindKeyHold(Key.F, () => { pitch -= 0.03; if (pitch < -Math.PI / 2 + 0.00001) pitch = -Math.PI / 2 + 0.00001; });

			engine.Input.BindKeyHold(Key.Escape, () => { engine.Stop(); });
			engine.Input.BindMouseEvent(MouseEvent.Move, (MouseEventArguments e) => { Console.WriteLine(e.X + ", " + e.Y); });
			engine.Input.BindMouseEvent(MouseEvent.WheelMove, (MouseEventArguments e) => { Console.WriteLine(e.WheelPosition); });

			Stopwatch timer = new Stopwatch();
			double d = 0;


			while (engine.IsRunning)
			{
				engine.CameraLookDir = new Vector3(Math.Cos(yaw) * Math.Cos(pitch), Math.Sin(pitch), Math.Sin(yaw) * Math.Cos(pitch));

				timer.Restart();
				d += 0.00501;

				while (timer.Elapsed.TotalMilliseconds < 1) ;
			}
		}
	}
}

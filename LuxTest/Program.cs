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

			engine.Input.BindKeyHold(Key.W, () => { engine.Camera.Position += engine.Camera.GetRotationMatrix * Vector3.Forwards * 10.0; });
			engine.Input.BindKeyHold(Key.S, () => { engine.Camera.Position += engine.Camera.GetRotationMatrix * Vector3.Backwards * 10.0; });
			engine.Input.BindKeyHold(Key.A, () => { engine.Camera.Position += engine.Camera.GetRotationMatrix * Vector3.Left * 10.0; });
			engine.Input.BindKeyHold(Key.D, () => { engine.Camera.Position += engine.Camera.GetRotationMatrix * Vector3.Right * 10.0; });
			engine.Input.BindKeyHold(Key.Space, () => { engine.Camera.Position += Vector3.Up * 10.0; });
			engine.Input.BindKeyHold(Key.LeftShift, () => { engine.Camera.Position += Vector3.Down * 10.0; });

			engine.Input.BindKeyHold(Key.Q, () => { engine.Camera.Yaw += 0.03; if (engine.Camera.Yaw < 0) engine.Camera.Yaw += Math.PI * 2; });
			engine.Input.BindKeyHold(Key.E, () => { engine.Camera.Yaw -= 0.03; if (engine.Camera.Yaw >= Math.PI * 2) engine.Camera.Yaw -= Math.PI * 2; });
			engine.Input.BindKeyHold(Key.R, () => { engine.Camera.Pitch += 0.03; if (engine.Camera.Pitch > Math.PI / 2) engine.Camera.Pitch = Math.PI / 2; });
			engine.Input.BindKeyHold(Key.F, () => { engine.Camera.Pitch -= 0.03; if (engine.Camera.Pitch < -Math.PI / 2) engine.Camera.Pitch = -Math.PI / 2; });

			engine.Input.BindKeyHold(Key.Escape, () => { engine.Stop(); });
			engine.Input.BindMouseEvent(MouseEvent.Move, (MouseEventArguments e) => 
			{
				engine.Camera.Yaw -= e.DeltaX * 0.03; 
				if (engine.Camera.Yaw < 0) engine.Camera.Yaw += Math.PI * 2;
				if (engine.Camera.Yaw >= Math.PI * 2) engine.Camera.Yaw -= Math.PI * 2;

				engine.Camera.Pitch -= e.DeltaY  * 0.03;
				if (engine.Camera.Pitch > Math.PI / 2) engine.Camera.Pitch = Math.PI / 2;
				if (engine.Camera.Pitch < -Math.PI / 2) engine.Camera.Pitch = -Math.PI / 2;
			});
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

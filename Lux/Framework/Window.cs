using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;

using Lux.Resources;
using Lux.Graphics;
using Lux.Physics;
using Lux.Input;

namespace Lux.Framework
{
	internal class Window : GameWindow
	{
		private Engine Parent;

		public Window(Engine parent) : base(1024, 768, new GraphicsMode(32, 24, 8, 16))
		{
			Parent = parent;
		}

		protected override void OnLoad(EventArgs e)
		{
			Parent.Graphics.SetupRender();

			base.OnLoad(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			Parent.Physics.Update(e.Time);
			Parent.Input.Update();
		}

		uint frames = 0;
		Stopwatch framerate;
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			Parent.Graphics.Render(e.Time);

			frames++;


			if (framerate == null)
			{
				framerate = new Stopwatch();
				framerate.Start();
			}
			if (framerate.ElapsedMilliseconds > 1000)
			{
				Console.WriteLine("FPS: " + ((double)frames / framerate.ElapsedMilliseconds * 1000));
				frames = 0;
				framerate.Restart();
			}
		}
	}
}

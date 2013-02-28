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

		public Window(Engine parent) : base(1024, 768, new GraphicsMode(32, 24, 8, 0))
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
			Parent.Update(e.Time);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			Parent.Render(e.Time);
		}
	}
}

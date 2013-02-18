using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Lux.Framework;

namespace Lux.Graphics
{
	public class GraphicsEngine
	{
		private Engine Parent;
		private OpenTK.Matrix4 View;
		private OpenTK.Matrix4 Projection;

		private IGraphicsContext ResourceContext;
		private INativeWindow ResourceWindow;
		private GraphicsContext RenderContext;

		internal GraphicsEngine(Engine parent)
		{
			Parent = parent;

			GraphicsContext.ShareContexts = true;
			ResourceWindow = new NativeWindow();
			ResourceContext = new GraphicsContext(GraphicsMode.Default, ResourceWindow.WindowInfo);
			ResourceContext.MakeCurrent(ResourceWindow.WindowInfo);
			(ResourceContext as IGraphicsContextInternal).LoadAll();
			GraphicsContext.Assert();
		}

		internal void SetupRender()
		{
			RenderContext = new GraphicsContext(GraphicsMode.Default, Parent.Window.WindowInfo, 1, 0, GraphicsContextFlags.Default);
			RenderContext.MakeCurrent(Parent.Window.WindowInfo);
			Parent.Window.Visible = true;

			GL.Enable(EnableCap.DepthTest);
			GL.ClearColor(Color.CornflowerBlue);

			View = OpenTK.Matrix4.LookAt(OpenTK.Vector3.One * 3, OpenTK.Vector3.Zero, OpenTK.Vector3.UnitY);
			Projection = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver3, (float)Parent.Window.Width / Parent.Window.Height, 0.1F, 1000.0F);

			GraphicsContext.Assert();
			//foreach (Entity entity in Parent.Entities)
			//{
			//    entity.Model.Setup();
			//}
		}

		internal void Render(double deltaTime)
		{
			GraphicsContext.Assert();
			View = OpenTK.Matrix4.LookAt(OpenTK.Vector3.UnitZ * 30, OpenTK.Vector3.Zero, OpenTK.Vector3.UnitY);
			Projection = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver3, (float)Parent.Window.Width / Parent.Window.Height, 0.1F, 1000.0F);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


			foreach (Entity entity in Parent.Entities)
			{
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadMatrix(ref Projection);
				GL.MatrixMode(MatrixMode.Modelview);
				GL.LoadMatrix(ref View);

				entity.Model.Render(entity);
			}

			GraphicsContext.CurrentContext.SwapBuffers();
		}
	}
}

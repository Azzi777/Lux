using System;
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
		private Matrix4d View;
		private Matrix4d Projection;

		internal GraphicsEngine(Engine parent)
		{
			Parent = parent;
		}

		internal void SetupRender()
		{
			Parent.Window.Visible = true;

			GL.Enable(EnableCap.Multisample);
			GL.Enable(EnableCap.DepthTest);
			GL.ClearColor(Color4.CornflowerBlue);

			View = OpenTK.Matrix4d.LookAt(Parent.CameraPosition.OpenTKEquivalent, OpenTK.Vector3d.Zero, OpenTK.Vector3d.UnitY);
			Projection = Matrix4d.CreatePerspectiveFieldOfView(MathHelper.PiOver3, (float)Parent.Window.Width / Parent.Window.Height, 0.1F, 100000.0F);

			GraphicsContext.CurrentContext.VSync = false;
		}

		internal void Render(double deltaTime)
		{
			View = OpenTK.Matrix4d.LookAt(Parent.CameraPosition.OpenTKEquivalent, Parent.CameraLookat.OpenTKEquivalent, OpenTK.Vector3d.UnitY);
			Projection = OpenTK.Matrix4d.CreatePerspectiveFieldOfView(MathHelper.PiOver3, (float)Parent.Window.Width / Parent.Window.Height, 0.1F, 100000.0F);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			

			lock(Parent.Entities)
			{
				foreach (Entity entity in Parent.Entities)
				{
					GL.MatrixMode(MatrixMode.Projection);
					GL.LoadMatrix(ref Projection);
					GL.MatrixMode(MatrixMode.Modelview);
					GL.LoadMatrix(ref View);

					entity.Model.Render(entity);
				}
			}

			GraphicsContext.CurrentContext.SwapBuffers();
		}
	}
}

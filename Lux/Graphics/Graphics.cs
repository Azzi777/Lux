﻿using System;
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

			GL.Light(LightName.Light0, LightParameter.Position, new Vector4((OpenTK.Vector3)Parent.CameraPosition.OpenTKEquivalent, 1.0F));
			GL.Light(LightName.Light0, LightParameter.Specular, Color4.White);
			GL.Light(LightName.Light0, LightParameter.Ambient, Color4.Black);
			GL.Light(LightName.Light0, LightParameter.Diffuse, Color4.White);

			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.Enable(EnableCap.Lighting);
			GL.Enable(EnableCap.Light0);

			foreach (Entity entity in Parent.Entities)
			{
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadMatrix(ref Projection);
				GL.MatrixMode(MatrixMode.Modelview);
				GL.LoadMatrix(ref View);

				entity.Model.Render(entity);
			}

			GL.Disable(EnableCap.Light0);
			GL.Disable(EnableCap.Lighting);
			GL.Disable(EnableCap.CullFace);

			GraphicsContext.CurrentContext.SwapBuffers();
		}
	}
}

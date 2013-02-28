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

			View = OpenTK.Matrix4d.LookAt(Parent.ameraPosition.OpenTKEquivalent, OpenTK.Vector3d.Zero, OpenTK.Vector3d.UnitY);
			Projection = Matrix4d.CreatePerspectiveFieldOfView(MathHelper.PiOver3, (float)Parent.Window.Width / Parent.Window.Height, 0.1F, 10000.0F);

			GraphicsContext.CurrentContext.VSync = false;
		}

		internal void Render(double deltaTime)
		{
			View = OpenTK.Matrix4d.LookAt(Parent.CaeraPosition.OpenTKEquivalent, ParVector3CameraLookatProjection = OpenTK.Matrix4d.CreatePerspectiveFieldOfView(MathHelper.PiOver3, (float)Parent.Window.Width / Parent.Window.Height, 0.1F, 10000.0F);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref Projection);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.L
			

			(refView);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Light(LightName.Light0, LightParameter.Position, new Vector4((OpenTK.Vector3)Parent.CameraPosition.OpenTKEquivalent, 1.0F));
			GL.Light(LightName.Light0, LightParameter.Specular, Color4.White);
			GL.Light(LightName.Light0, LightParameter.Ambient, Color4.Black);
			GL.Light(LightName.Light0, LightParameter.Diffuse, Color4.White);


			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.Enable(EnableCap.Lighting);
			GL.Enable(EnableCap.Light0);

			lock (Parent.Entities)
			{
				foreach (Entity entity in Parent.Entities)
				{
					entity.Model.Render(entity);
				}
			}

			GL.BindTexture(TextureTarget.Texture2D, 27);
			GL.Enable(EnableCap.Texture2D);

			GL.Begin(BeginMode.Quads);
			{
				GL.Vertex3(0, 100, -1000);
				GL.TexCoord2(0, 0);
				GL.Vertex3(0, 1100, -1000);
				GL.TexCoord2(0, 1);
				GL.Vertex3(0, 1100, 1000);
				GL.TexCoord2(1, 1);
				GL.Vertex3(0, 100, 1000);
				GL.TexCoord2(1, 0);
			}
			GL.End();

			GL.Disable(EnableCap.Light0);
			GL.Disable(EnableCap.Lighting);
			GL.Disable(EnableCap.CullFace);

			GraphicsContext.CurrentContext.SwapBuffers();
		}
	}
}

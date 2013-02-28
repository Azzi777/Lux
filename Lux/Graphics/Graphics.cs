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

		internal ShaderProgram TextureShader;
		internal ShaderProgram ScreenShader;

		Framebuffer ColorFramebuffer;

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

			GraphicsContext.CurrentContext.VSync = false;

			TextureShader = new ShaderProgram(ShaderProgram.TextureVertexShaderSource, ShaderProgram.TextureFragmentShaderSource);
			ScreenShader = new ShaderProgram(ShaderProgram.ScreenFragmentShaderSource);

			ColorFramebuffer = new Framebuffer(Parent.Window.Width, Parent.Window.Height);
		}

		internal void Render(double deltaTime)
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, ColorFramebuffer.ID);

			GL.ClearColor(Color.CornflowerBlue.GetSystemEquivalent());
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			GL.UseProgram(TextureShader.ID);

			View = Parent.Camera.OpenTKViewMatrix;
			Projection = OpenTK.Matrix4d.CreatePerspectiveFieldOfView(MathHelper.PiOver3, (float)Parent.Window.Width / Parent.Window.Height, 10.0F, 10000.0F);

			TextureShader.SetMatrix4("mat_view", new Lux.Framework.Matrix4(View));
			TextureShader.SetMatrix4("mat_proj", new Lux.Framework.Matrix4(Projection));
			lock (Parent.Entities)
			{
				foreach (Entity entity in Parent.Entities)
				{
					entity.Model.Render(entity, TextureShader);
				}
			}
			GL.UseProgram(0);

			GL.Disable(EnableCap.CullFace);
			GL.Disable(EnableCap.Blend);


			// Render to screen
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			GL.ClearColor(Color4.BlueViolet);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Enable(EnableCap.Texture2D);

			GL.UseProgram(ScreenShader.ID);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, ColorFramebuffer.TextureID);
			GL.Uniform1(GL.GetUniformLocation(ScreenShader.ID, "colorTexture"), 0);


			GL.Begin(BeginMode.Quads);
			{
				GL.Vertex2(-1, -1);
				GL.Vertex2(1, -1);
				GL.Vertex2(1, 1);
				GL.Vertex2(-1, 1);
			}
			GL.End();
			GL.Disable(EnableCap.Texture2D);

			GraphicsContext.CurrentContext.SwapBuffers();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Lux.Graphics
{
	internal class Framebuffer
	{
		public int ID;
		public int ColorBufferID;
		public int NormalBufferID;
		public int DepthBufferID;
		public int TangentBufferID;

		public Framebuffer(int width, int height)
		{
			GL.GenTextures(1, out ColorBufferID);
			GL.BindTexture(TextureTarget.Texture2DMultisample, ColorBufferID);
			GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, Properties.Settings.Default.Multisamples, PixelInternalFormat.Rgba, width, height, false);

			GL.GenTextures(1, out NormalBufferID);
			GL.BindTexture(TextureTarget.Texture2DMultisample, NormalBufferID);
			GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, Properties.Settings.Default.Multisamples, PixelInternalFormat.Rgba, width, height, false);

			GL.GenTextures(1, out TangentBufferID);
			GL.BindTexture(TextureTarget.Texture2DMultisample, TangentBufferID);
			GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, Properties.Settings.Default.Multisamples, PixelInternalFormat.Rgba, width, height, false);

			GL.GenTextures(1, out DepthBufferID);
			GL.BindTexture(TextureTarget.Texture2DMultisample, DepthBufferID);
			GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, Properties.Settings.Default.Multisamples, PixelInternalFormat.DepthComponent, width, height, false);

			GL.GenFramebuffers(1, out ID);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);

			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2DMultisample, ColorBufferID, 0);
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2DMultisample, NormalBufferID, 0);
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2DMultisample, TangentBufferID, 0);
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2DMultisample, DepthBufferID, 0);
			FramebufferErrorCode stanEnum = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

			GL.Ext.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
		}
	}
}

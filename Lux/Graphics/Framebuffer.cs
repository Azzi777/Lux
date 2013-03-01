﻿using System;
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
		public int DepthBufferID;

		public Framebuffer(int width, int height)
		{
			GL.GetString(StringName.Extensions).Contains("GL_ARB_texture_multisample");

			GL.GenTextures(1, out ColorBufferID);
			GL.BindTexture(TextureTarget.Texture2DMultisample, ColorBufferID);
			GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, Properties.Settings.Default.Multisamples, PixelInternalFormat.Rgb, width, height, false);

			GL.GenTextures(1, out DepthBufferID);
			GL.BindTexture(TextureTarget.Texture2DMultisample, DepthBufferID);
			GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, Properties.Settings.Default.Multisamples, PixelInternalFormat.DepthComponent, width, height, false);

			GL.GenFramebuffers(1, out ID);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);

			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, ColorBufferID, 0);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, DepthBufferID, 0);
			FramebufferErrorCode stanEnum = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
		}
	}
}

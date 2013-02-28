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
		public int TextureID;

		public Framebuffer(int width, int height)
		{
			int depthbuffer;

			// Create framebuffer
			GL.GenFramebuffers(1, out ID);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);

			// Create depthbuffer
			GL.GenRenderbuffers(1, out depthbuffer);
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthbuffer);
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32, width, height);
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthbuffer);

			// Create texture
			GL.GenTextures(1, out TextureID);
			GL.BindTexture(TextureTarget.Texture2D, TextureID);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureID, 0);
		}
	}
}

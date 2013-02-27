using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Lux.Graphics
{
	internal class Texture
	{
		private bool IsFinished { get; set; }

		public uint TextureID;
		private Bitmap TempBitmap;

		public Texture(string path)
		{
			IsFinished = false;
			TextureID = 0;

			if (!File.Exists(path))
			{
				return;
				throw new FileNotFoundException("Texture " + path + " could not be found!");
			}
			
			switch (path.Substring(path.LastIndexOf('.')))
			{
				case ".tga":
				{
					TempBitmap = Paloma.TargaImage.LoadTargaImage(path);
					break;
				}

				case ".png":
				{
					TempBitmap = new Bitmap(path);
					break;
				}
			}
		}

		public void Finish()
		{
			if (IsFinished || TempBitmap == null)
			{
				return;
			}

			System.Drawing.Imaging.BitmapData TextureData =
			TempBitmap.LockBits(new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			GL.GenTextures(1, out TextureID);
			GL.BindTexture(TextureTarget.Texture2D, TextureID);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, TempBitmap.Width, TempBitmap.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, TextureData.Scan0);

			GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			TempBitmap.UnlockBits(TextureData);

			TempBitmap.Dispose();
			IsFinished = true;
		}
	}
}

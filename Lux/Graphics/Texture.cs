using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
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

		static public void FinishTextures(Texture[] textures)
		{
			foreach(Texture t in textures)
			{
				int textureID;
				GL.GenTextures(1, out textureID);

				GL.BindTexture(TextureTarget.Texture2D, textureID);

				BitmapData bmpData = t.TempBitmap.LockBits(new Rectangle(0, 0, t.TempBitmap.Width, t.TempBitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				t.TempBitmap.UnlockBits(bmpData);


				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, t.TempBitmap.Width, t.TempBitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

				GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

				float maxAniso;
				GL.GetFloat((GetPName)ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out maxAniso);
				GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, maxAniso);

				GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

				t.TextureID = (uint)textureID;
				t.TempBitmap.Dispose();
				t.TempBitmap = null;
				t.IsFinished = true;
			}
		}
	}
}

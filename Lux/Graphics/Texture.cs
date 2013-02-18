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
	public class Texture
	{
		internal uint TextureID;

		public Texture(string path)
		{
			TextureID = 0;

			if (!File.Exists(path))
			{
				return;
			}

			Bitmap TextureBitmap = null;

			switch (path.Substring(path.LastIndexOf('.')))
			{
				case ".tga":
				{
					TextureBitmap = Paloma.TargaImage.LoadTargaImage(path);
					break;
				}

				case ".png":
				{
					TextureBitmap = new Bitmap(path);
					break;
				}
			}
			//get the data out of the bitmap
			System.Drawing.Imaging.BitmapData TextureData =
			TextureBitmap.LockBits( new System.Drawing.Rectangle(0, 0, TextureBitmap.Width, TextureBitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb );

			//Code to get the data to the OpenGL Driver

			//generate one texture and put its ID number into the "Texture" variable
			GL.GenTextures(1, out TextureID);
			//tell OpenGL that this is a 2D texture
			GL.BindTexture(TextureTarget.Texture2D, TextureID);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, TextureBitmap.Width, TextureBitmap.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, TextureData.Scan0);

			//the following code sets certian parameters for the texture
			GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

			//load the data by telling OpenGL to build mipmaps out of the bitmap data
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			//free the bitmap data (we dont need it anymore because it has been passed to the OpenGL driver
			TextureBitmap.UnlockBits(TextureData);

			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
	}
}

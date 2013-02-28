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

			TempBitmap.Save(@"C:\Users\Azzi777\Desktop\test.png");
		}

		static public uint CreateTexture2DArray(Texture[] textures)
		{
			int textureID;
			GL.GenTextures(1, out textureID);

			GL.BindTexture(TextureTarget.Texture2DArray, textureID);


			int largestTexture = 0;
			for (int i = 0; i < textures.Length; i++)
			{
				if (textures[i].TempBitmap.Width > textures[largestTexture].TempBitmap.Width)
				{
					largestTexture = i;
				}
			}

			BitmapData bmpData = textures[largestTexture].TempBitmap.LockBits(new Rectangle(0, 0, textures[largestTexture].TempBitmap.Width, textures[largestTexture].TempBitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			textures[largestTexture].TempBitmap.UnlockBits(bmpData);

			int largestStride = Math.Abs(bmpData.Stride) * textures[largestTexture].TempBitmap.Height;
			int width = textures[largestTexture].TempBitmap.Width;
			int height = textures[largestTexture].TempBitmap.Height;
			byte[] texturesData = new byte[largestStride * textures.Length];

			for (int i = 0; i < textures.Length; i++)
			{
				bmpData = textures[i].TempBitmap.LockBits(new Rectangle(0, 0, textures[i].TempBitmap.Width, textures[i].TempBitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, texturesData, largestStride * i, Math.Abs(bmpData.Stride) * textures[i].TempBitmap.Height);
				textures[i].TempBitmap.UnlockBits(bmpData);

				textures[i].TextureID = (uint)i;
				textures[i].TempBitmap = null;
				textures[i].IsFinished = true;
			}

			GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba, width, height, textures.Length, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, texturesData);

			GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
			GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
			GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			float maxAniso;
			GL.GetFloat((GetPName)ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out maxAniso);
			GL.TexParameter(TextureTarget.Texture2DArray, (TextureParameterName)ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, maxAniso);

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);

			return (uint)textureID;
		}
	}
}

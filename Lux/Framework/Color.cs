using System;

namespace Lux.Framework
{
	/// <summary>
	/// Color struct for RGBA colors. Components are stored as bytes, but accesed through doubles for easier calculations.
	/// </summary>
	public struct Color
	{
		#region - Data -
		private byte _R;
		private byte _G;
		private byte _B;
		private byte _A;

		/// <summary>
		/// Gets or sets the red component.
		/// </summary>
		public double R
		{
			get
			{
				return (double)_R / 255.0;
			}
			set
			{
				if (value < 0 || value > 1)
				{
					throw new ArgumentException("Component must be in range 0-1!");
				}

				_R = (byte)(value * 255.0);
			}
		}

		/// <summary>
		/// Gets or sets the green component.
		/// </summary>
		public double G
		{
			get
			{
				return (double)_G / 255.0;
			}
			set
			{
				if (value < 0 || value > 1)
				{
					throw new ArgumentException("Component must be in range 0-1!");
				}

				_G = (byte)(value * 255.0);
			}
		}

		/// <summary>
		/// Gets or sets the blue component.
		/// </summary>
		public double B
		{
			get
			{
				return (double)_B / 255.0;
			}
			set
			{
				if (value < 0 || value > 1)
				{
					throw new ArgumentException("Color component must be in range 0-1!");
				}

				_B = (byte)(value * 255.0);
			}
		}

		/// <summary>
		/// Gets or sets the alpha component.
		/// </summary>
		public double A
		{
			get
			{
				return (double)_A / 255.0;
			}
			set
			{
				if (value < 0 || value > 1)
				{
					throw new ArgumentException("Component must be in range 0-1!");
				}

				_A = (byte)(value * 255.0);
			}
		}

		/// <summary>
		/// Gets or sets the hue of the color in HVS space.
		/// Setting the hue preserves the saturation and value.
		/// </summary>
		public double Hue
		{
			get
			{
				double min = Math.Min(R, Math.Min(G, B));
				double max = Math.Max(R, Math.Max(G, B));
				double delta = max - min;
				double h = 0.0;

				if (max == 0 || delta == 0)
				{
					return 0.0;
				}

				if (R == max)
				{
					h = (G - B) / delta;
				}

				if (G == max)
				{
					h = 2 + (B - R) / delta;
				}

				if (B == max)
				{
					h = 4 + (R - G) / delta;
				}

				h *= 60.0;
				if (h < 0.0) h += 360.0;

				return h;
			}
			set
			{
				Color newColor = CreateFromHSV(value, Saturation, Value);
				_R = newColor._R;
				_G = newColor._G;
				_B = newColor._B;
			}
		}

		/// <summary>
		/// Gets or sets the saturation of the color in HVS space.
		/// Setting the saturation preserves the hue and value.
		/// </summary>
		public double Saturation
		{
			get
			{
				double min = Math.Min(R, Math.Min(G, B));
				double max = Math.Max(R, Math.Max(G, B));

				if (max == 0)
				{
					return 0.0; // Color is black, saturation is defined to 0
				}

				return (max - min) / max;
			}
			set
			{
				Color newColor = CreateFromHSV(Hue, value, Value);
				_R = newColor._R;
				_G = newColor._G;
				_B = newColor._B;
			}
		}

		/// <summary>
		/// Gets or sets the value of the color in HVS space.
		/// Setting the value preserves the hue and saturation.
		/// </summary>
		public double Value
		{
			get
			{
				return Math.Max(R, Math.Max(G, B));
			}
			set
			{
				Color newColor = CreateFromHSV(Hue, Saturation, value);
				_R = newColor._R;
				_G = newColor._G;
				_B = newColor._B;
			}
		}

		private uint RGBA
		{
			get
			{
				return ((uint)(_A << 24)) | ((uint)(_R << 16)) | ((uint)(_G << 8)) | ((uint)(_B << 0));
			}
			set
			{
				_R = (byte)((value & 0xFF000000) >> 24);
				_G = (byte)((value & 0x00FF0000) >> 16);
				_B = (byte)((value & 0x0000FF00) >> 8);
				_A = (byte)((value & 0x000000FF) >> 9);
			}
		}
		#endregion

		#region - Constructors -
		/// <summary>
		/// Create a new Color from red, green, blue and alpha components.
		/// </summary>
		/// <param name="r">Red component</param>
		/// <param name="g">Green component</param>
		/// <param name="b">Blue component</param>
		/// <param name="a">Alpha component</param>
		public Color(byte r, byte g, byte b, byte a)
		{
			_R = r;
			_G = g;
			_B = b;
			_A = a;
		}

		private Color(uint rgba)
			: this((byte)0, (byte)0, (byte)0, (byte)0)
		{
			RGBA = rgba;
		}

		private Color(uint argb, bool flipped)
			: this((byte)0, (byte)0, (byte)0, (byte)0)
		{
			if (!flipped)
			{
				throw new Exception("Then use the other one, idiot!");
			}

			RGBA = (argb << 8) | ((argb & 0xFF000000) >> 24);
		}

		/// <summary>
		/// Create a new Color from red, green and blue components. Alpha is set to 1.0.
		/// </summary>
		/// <param name="r">Red component</param>
		/// <param name="g">Green component</param>
		/// <param name="b">Blue component</param>
		public Color(double r, double g, double b)
			: this((byte)0, (byte)0, (byte)0, (byte)0)
		{
			R = r;
			G = g;
			B = b;
			A = 1.0;
		}

		/// <summary>
		/// Create a new Color from red, green, blue and alpha components.
		/// </summary>
		/// <param name="r">Red component</param>
		/// <param name="g">Green component</param>
		/// <param name="b">Blue component</param>
		public Color(double r, double g, double b, double a)
			: this((byte)0, (byte)0, (byte)0, (byte)0)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		/// <summary>
		/// Create a new gray Color from a shade.
		/// </summary>
		/// <param name="shade">Shade of gray</param>
		public Color(double shade)
			: this((byte)0, (byte)0, (byte)0, (byte)0)
		{
			R = shade;
			G = shade;
			B = shade;
			A = 1.0;
		}

		/// <summary>
		/// Create a new color from hue (in degrees), saturation and value.
		/// </summary>
		/// <param name="h">Hue of the color. Range [0-360)</param>
		/// <param name="s">Saturation of the color. Range [0-1]</param>
		/// <param name="v">Value of the color. Range [0-1]</param>
		/// <returns></returns>
		static public Color CreateFromHSV(double h, double s, double v)
		{
			double preHue = h;

			h %= 360.0;
			if (h < 0) h += 360.0;
			if (h == 360.0) h = 0;

			double v1s = v * (1 - s);
			int hexant = (int)Math.Floor(h / 60.0);

			switch (hexant)
			{
				case 0:
					{
						return new Color(v, lerp(v1s, v, h / 60.0), v1s);
					}
				case 1:
					{
						return new Color(lerp(v, v1s, (h - 60.0) / 60.0), v, v1s);
					}
				case 2:
					{
						return new Color(v1s, v, lerp(v1s, v, (h - 120.0) / 60.0));
					}
				case 3:
					{
						return new Color(v1s, lerp(v, v1s, (h - 180.0) / 60.0), v);
					}
				case 4:
					{
						return new Color(lerp(v1s, v, (h - 240.0) / 60.0), v1s, v);
					}
				case 5:
					{
						return new Color(v, v1s, lerp(v, v1s, (h - 300.0) / 60.0));
					}
				default: throw new Exception("Invalid hue!");
			}
		}

		static private double lerp(double a, double b, double v) { return a + v * (b - a); }
		#endregion

		#region - Colors -
		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFF0F8FF.
		/// <summary>
		static public Color AliceBlue { get { return new Color(0xFFF0F8FF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFAEBD7.
		/// <summary>
		static public Color AntiqueWhite { get { return new Color(0xFFFAEBD7, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF00FFFF.
		/// <summary>
		static public Color Aqua { get { return new Color(0xFF00FFFF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF7FFFD4.
		/// <summary>
		static public Color Aquamarine { get { return new Color(0xFF7FFFD4, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFF0FFFF.
		/// <summary>
		static public Color Azure { get { return new Color(0xFFF0FFFF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFF5F5DC.
		/// <summary>
		static public Color Beige { get { return new Color(0xFFF5F5DC, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFE4C4.
		/// <summary>
		static public Color Bisque { get { return new Color(0xFFFFE4C4, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF000000.
		/// <summary>
		static public Color Black { get { return new Color(0xFF000000, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFEBCD.
		/// <summary>
		static public Color BlanchedAlmond { get { return new Color(0xFFFFEBCD, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF0000FF.
		/// <summary>
		static public Color Blue { get { return new Color(0xFF0000FF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF8A2BE2.
		/// <summary>
		static public Color BlueViolet { get { return new Color(0xFF8A2BE2, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFA52A2A.
		/// <summary>
		static public Color Brown { get { return new Color(0xFFA52A2A, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFDEB887.
		/// <summary>
		static public Color BurlyWood { get { return new Color(0xFFDEB887, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF5F9EA0.
		/// <summary>
		static public Color CadetBlue { get { return new Color(0xFF5F9EA0, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF7FFF00.
		/// <summary>
		static public Color Chartreuse { get { return new Color(0xFF7FFF00, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFD2691E.
		/// <summary>
		static public Color Chocolate { get { return new Color(0xFFD2691E, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFF7F50.
		/// <summary>
		static public Color Coral { get { return new Color(0xFFFF7F50, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF6495ED.
		/// <summary>
		static public Color CornflowerBlue { get { return new Color(0xFF6495ED, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFF8DC.
		/// <summary>
		static public Color Cornsilk { get { return new Color(0xFFFFF8DC, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFDC143C.
		/// <summary>
		static public Color Crimson { get { return new Color(0xFFDC143C, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF00FFFF.
		/// <summary>
		static public Color Cyan { get { return new Color(0xFF00FFFF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF00008B.
		/// <summary>
		static public Color DarkBlue { get { return new Color(0xFF00008B, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF008B8B.
		/// <summary>
		static public Color DarkCyan { get { return new Color(0xFF008B8B, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFB8860B.
		/// <summary>
		static public Color DarkGoldenrod { get { return new Color(0xFFB8860B, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFA9A9A9.
		/// <summary>
		static public Color DarkGray { get { return new Color(0xFFA9A9A9, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF006400.
		/// <summary>
		static public Color DarkGreen { get { return new Color(0xFF006400, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFBDB76B.
		/// <summary>
		static public Color DarkKhaki { get { return new Color(0xFFBDB76B, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF8B008B.
		/// <summary>
		static public Color DarkMagenta { get { return new Color(0xFF8B008B, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF556B2F.
		/// <summary>
		static public Color DarkOliveGreen { get { return new Color(0xFF556B2F, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFF8C00.
		/// <summary>
		static public Color DarkOrange { get { return new Color(0xFFFF8C00, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF9932CC.
		/// <summary>
		static public Color DarkOrchid { get { return new Color(0xFF9932CC, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF8B0000.
		/// <summary>
		static public Color DarkRed { get { return new Color(0xFF8B0000, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFE9967A.
		/// <summary>
		static public Color DarkSalmon { get { return new Color(0xFFE9967A, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF8FBC8F.
		/// <summary>
		static public Color DarkSeaGreen { get { return new Color(0xFF8FBC8F, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF483D8B.
		/// <summary>
		static public Color DarkSlateBlue { get { return new Color(0xFF483D8B, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF2F4F4F.
		/// <summary>
		static public Color DarkSlateGray { get { return new Color(0xFF2F4F4F, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF00CED1.
		/// <summary>
		static public Color DarkTurquoise { get { return new Color(0xFF00CED1, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF9400D3.
		/// <summary>
		static public Color DarkViolet { get { return new Color(0xFF9400D3, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFF1493.
		/// <summary>
		static public Color DeepPink { get { return new Color(0xFFFF1493, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF00BFFF.
		/// <summary>
		static public Color DeepSkyBlue { get { return new Color(0xFF00BFFF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF696969.
		/// <summary>
		static public Color DimGray { get { return new Color(0xFF696969, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF1E90FF.
		/// <summary>
		static public Color DodgerBlue { get { return new Color(0xFF1E90FF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFB22222.
		/// <summary>
		static public Color Firebrick { get { return new Color(0xFFB22222, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFFAF0.
		/// <summary>
		static public Color FloralWhite { get { return new Color(0xFFFFFAF0, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF228B22.
		/// <summary>
		static public Color ForestGreen { get { return new Color(0xFF228B22, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFF00FF.
		/// <summary>
		static public Color Fuchsia { get { return new Color(0xFFFF00FF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFDCDCDC.
		/// <summary>
		static public Color Gainsboro { get { return new Color(0xFFDCDCDC, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFF8F8FF.
		/// <summary>
		static public Color GhostWhite { get { return new Color(0xFFF8F8FF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFD700.
		/// <summary>
		static public Color Gold { get { return new Color(0xFFFFD700, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFDAA520.
		/// <summary>
		static public Color Goldenrod { get { return new Color(0xFFDAA520, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF808080.
		/// <summary>
		static public Color Gray { get { return new Color(0xFF808080, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF008000.
		/// <summary>
		static public Color Green { get { return new Color(0xFF008000, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFADFF2F.
		/// <summary>
		static public Color GreenYellow { get { return new Color(0xFFADFF2F, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFF0FFF0.
		/// <summary>
		static public Color Honeydew { get { return new Color(0xFFF0FFF0, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFF69B4.
		/// <summary>
		static public Color HotPink { get { return new Color(0xFFFF69B4, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFCD5C5C.
		/// <summary>
		static public Color IndianRed { get { return new Color(0xFFCD5C5C, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF4B0082.
		/// <summary>
		static public Color Indigo { get { return new Color(0xFF4B0082, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFFFF0.
		/// <summary>
		static public Color Ivory { get { return new Color(0xFFFFFFF0, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFF0E68C.
		/// <summary>
		static public Color Khaki { get { return new Color(0xFFF0E68C, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFE6E6FA.
		/// <summary>
		static public Color Lavender { get { return new Color(0xFFE6E6FA, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFF0F5.
		/// <summary>
		static public Color LavenderBlush { get { return new Color(0xFFFFF0F5, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF7CFC00.
		/// <summary>
		static public Color LawnGreen { get { return new Color(0xFF7CFC00, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFFACD.
		/// <summary>
		static public Color LemonChiffon { get { return new Color(0xFFFFFACD, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFADD8E6.
		/// <summary>
		static public Color LightBlue { get { return new Color(0xFFADD8E6, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFF08080.
		/// <summary>
		static public Color LightCoral { get { return new Color(0xFFF08080, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFE0FFFF.
		/// <summary>
		static public Color LightCyan { get { return new Color(0xFFE0FFFF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFAFAD2.
		/// <summary>
		static public Color LightGoldenrodYellow { get { return new Color(0xFFFAFAD2, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFD3D3D3.
		/// <summary>
		static public Color LightGray { get { return new Color(0xFFD3D3D3, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF90EE90.
		/// <summary>
		static public Color LightGreen { get { return new Color(0xFF90EE90, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFB6C1.
		/// <summary>
		static public Color LightPink { get { return new Color(0xFFFFB6C1, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFA07A.
		/// <summary>
		static public Color LightSalmon { get { return new Color(0xFFFFA07A, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF20B2AA.
		/// <summary>
		static public Color LightSeaGreen { get { return new Color(0xFF20B2AA, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF87CEFA.
		/// <summary>
		static public Color LightSkyBlue { get { return new Color(0xFF87CEFA, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF778899.
		/// <summary>
		static public Color LightSlateGray { get { return new Color(0xFF778899, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFB0C4DE.
		/// <summary>
		static public Color LightSteelBlue { get { return new Color(0xFFB0C4DE, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFFFE0.
		/// <summary>
		static public Color LightYellow { get { return new Color(0xFFFFFFE0, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF00FF00.
		/// <summary>
		static public Color Lime { get { return new Color(0xFF00FF00, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF32CD32.
		/// <summary>
		static public Color LimeGreen { get { return new Color(0xFF32CD32, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFAF0E6.
		/// <summary>
		static public Color Linen { get { return new Color(0xFFFAF0E6, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFF00FF.
		/// <summary>
		static public Color Magenta { get { return new Color(0xFFFF00FF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF800000.
		/// <summary>
		static public Color Maroon { get { return new Color(0xFF800000, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF66CDAA.
		/// <summary>
		static public Color MediumAquamarine { get { return new Color(0xFF66CDAA, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF0000CD.
		/// <summary>
		static public Color MediumBlue { get { return new Color(0xFF0000CD, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFBA55D3.
		/// <summary>
		static public Color MediumOrchid { get { return new Color(0xFFBA55D3, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF9370DB.
		/// <summary>
		static public Color MediumPurple { get { return new Color(0xFF9370DB, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF3CB371.
		/// <summary>
		static public Color MediumSeaGreen { get { return new Color(0xFF3CB371, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF7B68EE.
		/// <summary>
		static public Color MediumSlateBlue { get { return new Color(0xFF7B68EE, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF00FA9A.
		/// <summary>
		static public Color MediumSpringGreen { get { return new Color(0xFF00FA9A, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF48D1CC.
		/// <summary>
		static public Color MediumTurquoise { get { return new Color(0xFF48D1CC, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFC71585.
		/// <summary>
		static public Color MediumVioletRed { get { return new Color(0xFFC71585, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF191970.
		/// <summary>
		static public Color MidnightBlue { get { return new Color(0xFF191970, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFF5FFFA.
		/// <summary>
		static public Color MintCream { get { return new Color(0xFFF5FFFA, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFE4E1.
		/// <summary>
		static public Color MistyRose { get { return new Color(0xFFFFE4E1, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFE4B5.
		/// <summary>
		static public Color Moccasin { get { return new Color(0xFFFFE4B5, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFDEAD.
		/// <summary>
		static public Color NavajoWhite { get { return new Color(0xFFFFDEAD, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF000080.
		/// <summary>
		static public Color Navy { get { return new Color(0xFF000080, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFDF5E6.
		/// <summary>
		static public Color OldLace { get { return new Color(0xFFFDF5E6, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF808000.
		/// <summary>
		static public Color Olive { get { return new Color(0xFF808000, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF6B8E23.
		/// <summary>
		static public Color OliveDrab { get { return new Color(0xFF6B8E23, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFA500.
		/// <summary>
		static public Color Orange { get { return new Color(0xFFFFA500, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFF4500.
		/// <summary>
		static public Color OrangeRed { get { return new Color(0xFFFF4500, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFDA70D6.
		/// <summary>
		static public Color Orchid { get { return new Color(0xFFDA70D6, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFEEE8AA.
		/// <summary>
		static public Color PaleGoldenrod { get { return new Color(0xFFEEE8AA, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF98FB98.
		/// <summary>
		static public Color PaleGreen { get { return new Color(0xFF98FB98, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFAFEEEE.
		/// <summary>
		static public Color PaleTurquoise { get { return new Color(0xFFAFEEEE, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFDB7093.
		/// <summary>
		static public Color PaleVioletRed { get { return new Color(0xFFDB7093, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFEFD5.
		/// <summary>
		static public Color PapayaWhip { get { return new Color(0xFFFFEFD5, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFDAB9.
		/// <summary>
		static public Color PeachPuff { get { return new Color(0xFFFFDAB9, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFCD853F.
		/// <summary>
		static public Color Peru { get { return new Color(0xFFCD853F, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFC0CB.
		/// <summary>
		static public Color Pink { get { return new Color(0xFFFFC0CB, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFDDA0DD.
		/// <summary>
		static public Color Plum { get { return new Color(0xFFDDA0DD, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFB0E0E6.
		/// <summary>
		static public Color PowderBlue { get { return new Color(0xFFB0E0E6, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF800080.
		/// <summary>
		static public Color Purple { get { return new Color(0xFF800080, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFF0000.
		/// <summary>
		static public Color Red { get { return new Color(0xFFFF0000, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFBC8F8F.
		/// <summary>
		static public Color RosyBrown { get { return new Color(0xFFBC8F8F, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF4169E1.
		/// <summary>
		static public Color RoyalBlue { get { return new Color(0xFF4169E1, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF8B4513.
		/// <summary>
		static public Color SaddleBrown { get { return new Color(0xFF8B4513, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFA8072.
		/// <summary>
		static public Color Salmon { get { return new Color(0xFFFA8072, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFF4A460.
		/// <summary>
		static public Color SandyBrown { get { return new Color(0xFFF4A460, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF2E8B57.
		/// <summary>
		static public Color SeaGreen { get { return new Color(0xFF2E8B57, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFF5EE.
		/// <summary>
		static public Color SeaShell { get { return new Color(0xFFFFF5EE, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFA0522D.
		/// <summary>
		static public Color Sienna { get { return new Color(0xFFA0522D, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFC0C0C0.
		/// <summary>
		static public Color Silver { get { return new Color(0xFFC0C0C0, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF87CEEB.
		/// <summary>
		static public Color SkyBlue { get { return new Color(0xFF87CEEB, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF6A5ACD.
		/// <summary>
		static public Color SlateBlue { get { return new Color(0xFF6A5ACD, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF708090.
		/// <summary>
		static public Color SlateGray { get { return new Color(0xFF708090, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFFAFA.
		/// <summary>
		static public Color Snow { get { return new Color(0xFFFFFAFA, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF00FF7F.
		/// <summary>
		static public Color SpringGreen { get { return new Color(0xFF00FF7F, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF4682B4.
		/// <summary>
		static public Color SteelBlue { get { return new Color(0xFF4682B4, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFD2B48C.
		/// <summary>
		static public Color Tan { get { return new Color(0xFFD2B48C, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF008080.
		/// <summary>
		static public Color Teal { get { return new Color(0xFF008080, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFD8BFD8.
		/// <summary>
		static public Color Thistle { get { return new Color(0xFFD8BFD8, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFF6347.
		/// <summary>
		static public Color Tomato { get { return new Color(0xFFFF6347, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF40E0D0.
		/// <summary>
		static public Color Turquoise { get { return new Color(0xFF40E0D0, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFEE82EE.
		/// <summary>
		static public Color Violet { get { return new Color(0xFFEE82EE, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFF5DEB3.
		/// <summary>
		static public Color Wheat { get { return new Color(0xFFF5DEB3, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFFFFF.
		/// <summary>
		static public Color White { get { return new Color(0xFFFFFFFF, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFF5F5F5.
		/// <summary>
		static public Color WhiteSmoke { get { return new Color(0xFFF5F5F5, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FFFFFF00.
		/// <summary>
		static public Color Yellow { get { return new Color(0xFFFFFF00, true); } }

		/// <summary>
		/// Gets a system-defined color that has an ARGB value of FF9ACD32.
		/// <summary>
		static public Color YellowGreen { get { return new Color(0xFF9ACD32, true); } }


		#endregion

		#region - Methods -
		/// <summary>
		/// Gets the equivalent System.Drawing.Color.
		/// </summary>
		/// <returns>A System.Drawing.Color representing the same color.</returns>
		internal System.Drawing.Color GetSystemEquivalent()
		{
			return System.Drawing.Color.FromArgb(_A, _R, _G, _B);
		}

		/// <summary>
		/// Returns a value indicating if this instance is equal to a specified object.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			if (obj.GetType() != typeof(Color))
			{
				return false;
			}

			Color c = (Color)obj;
			return (c._R == _R && c._G == _G && c._B == _B && c._A == _A);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>The hash code for this instance.</returns>
		public override int GetHashCode()
		{
			return (int)RGBA;
		}
		#endregion

		#region - Static Methods -
		/// <summary>
		/// Interpolates linearly between two colors. Excpect unexcpected results with a mix-value outside range 0-1.
		/// </summary>
		/// <param name="a">First color</param>
		/// <param name="b">Second color</param>
		/// <param name="mix">Mix amount. 0 = first color, 1 = second color.</param>
		/// <returns>Blend of the two input colors.</returns>
		static public Color Mix(Color a, Color b, double mix)
		{
			return new Color(
				a.R + mix * (b.R - a.R),
				a.G + mix * (b.G - a.G),
				a.B + mix * (b.B - a.B),
				a.A + mix * (b.A - a.A)
				);
		}
		#endregion

		#region - Operators -
		static public bool operator ==(Color a, Color b)
		{
			if (object.ReferenceEquals(a, b))
			{
				return true;
			}

			if ((object)a == null || (object)b == null)
			{
				return false;
			}

			return (a._R == b._R && a._G == b._G && a._B == b._B && a._A == b._A);
		}

		static public bool operator !=(Color a, Color b)
		{
			return !(a == b);
		}
		#endregion
	}
}

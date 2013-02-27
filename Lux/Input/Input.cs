using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Input;

using Lux.Framework;

namespace Lux.Input
{
	public class InputEngine
	{
		private Engine Parent { get; set; }
		private Dictionary<Key, Tuple<Action, Action, Action>> KeyBinds { get; set; }
		private Dictionary<Key, bool> KeyHeld { get; set; }
		private Dictionary<MouseEvent, Action<MouseEventArguments>> MouseBinds { get; set; }
		private KeyboardDevice Keyboard { get; set; }
		private MouseDevice Mouse { get; set; }

		internal InputEngine(Engine parent)
		{
			Parent = parent;
			KeyHeld = new Dictionary<Key, bool>();
			KeyBinds = new Dictionary<Key, Tuple<Action, Action, Action>>();

			foreach (Key k in Enum.GetValues(typeof(Key)))
			{
				if (!KeyBinds.ContainsKey(k))
				{
					KeyBinds.Add(k, new Tuple<Action, Action, Action>(() => { }, () => { }, () => { }));
					KeyHeld.Add(k, false);
				}
			}

			MouseBinds = new Dictionary<MouseEvent, Action<MouseEventArguments>>();
			foreach (MouseEvent m in Enum.GetValues(typeof(MouseEvent)))
			{
				if (!MouseBinds.ContainsKey(m))
				{
					MouseBinds.Add(m, (MouseEventArguments e) => { });
				}
			}
		}

		internal void Finish()
		{
			Keyboard = Parent.Window.Keyboard;
			Keyboard.KeyDown += KeyDown;
			Keyboard.KeyUp += KeyUp;

			Mouse = Parent.Window.Mouse;
			Mouse.Move += MouseMove;
			Mouse.ButtonDown += Mouse_ButtonDown;
			Mouse.ButtonUp += Mouse_ButtonUp;
			Mouse.WheelChanged += Mouse_WheelChanged;
		}

		public void BindKeyDown(Key key, Action routine)
		{
			KeyBinds[key] = new Tuple<Action, Action, Action>(routine, KeyBinds[key].Item2, KeyBinds[key].Item3);
		}

		public void BindKeyUp(Key key, Action routine)
		{
			KeyBinds[key] = new Tuple<Action, Action, Action>(KeyBinds[key].Item1, routine, KeyBinds[key].Item3);
		}

		public void BindKeyHold(Key key, Action routine)
		{
			KeyBinds[key] = new Tuple<Action, Action, Action>(KeyBinds[key].Item1, KeyBinds[key].Item2, routine);
		}

		public void BindMouseEvent(MouseEvent mouseEvent, Action<MouseEventArguments> routine)
		{
			MouseBinds[mouseEvent] = routine;
		}

		private void KeyDown(object sender, KeyboardKeyEventArgs args)
		{
			KeyBinds[GetFromOpenTKEquivalent(args.Key)].Item1.Invoke();

			KeyHeld[GetFromOpenTKEquivalent(args.Key)] = true;
		}

		private void KeyUp(object sender, KeyboardKeyEventArgs args)
		{
			KeyBinds[GetFromOpenTKEquivalent(args.Key)].Item2.Invoke();

			KeyHeld[GetFromOpenTKEquivalent(args.Key)] = false;
		}

		internal void Update()
		{
			foreach (KeyValuePair<Key, bool> kvp in KeyHeld)
			{
				if (kvp.Value)
				{
					KeyBinds[kvp.Key].Item3.Invoke();
				}
			}
		}

		private Key GetFromOpenTKEquivalent(OpenTK.Input.Key key)
		{
			switch (key)
			{
				#region Keys
				case OpenTK.Input.Key.F1: return Key.F1;
				case OpenTK.Input.Key.F2: return Key.F2;
				case OpenTK.Input.Key.F3: return Key.F3;
				case OpenTK.Input.Key.F4: return Key.F4;
				case OpenTK.Input.Key.F5: return Key.F5;
				case OpenTK.Input.Key.F6: return Key.F6;
				case OpenTK.Input.Key.F7: return Key.F7;
				case OpenTK.Input.Key.F8: return Key.F8;
				case OpenTK.Input.Key.F9: return Key.F9;
				case OpenTK.Input.Key.F10: return Key.F10;
				case OpenTK.Input.Key.F11: return Key.F11;
				case OpenTK.Input.Key.F12: return Key.F12;
				case OpenTK.Input.Key.F13: return Key.F13;
				case OpenTK.Input.Key.F14: return Key.F14;
				case OpenTK.Input.Key.F15: return Key.F15;
				case OpenTK.Input.Key.F16: return Key.F16;
				case OpenTK.Input.Key.F17: return Key.F17;
				case OpenTK.Input.Key.F18: return Key.F18;
				case OpenTK.Input.Key.F19: return Key.F19;
				case OpenTK.Input.Key.F20: return Key.F20;
				case OpenTK.Input.Key.F21: return Key.F21;
				case OpenTK.Input.Key.F22: return Key.F22;
				case OpenTK.Input.Key.F23: return Key.F23;
				case OpenTK.Input.Key.F24: return Key.F24;
				case OpenTK.Input.Key.F25: return Key.F25;
				case OpenTK.Input.Key.F26: return Key.F26;
				case OpenTK.Input.Key.F27: return Key.F27;
				case OpenTK.Input.Key.F28: return Key.F28;
				case OpenTK.Input.Key.F29: return Key.F29;
				case OpenTK.Input.Key.F30: return Key.F30;
				case OpenTK.Input.Key.F31: return Key.F31;
				case OpenTK.Input.Key.F32: return Key.F32;
				case OpenTK.Input.Key.F33: return Key.F33;
				case OpenTK.Input.Key.F34: return Key.F34;
				case OpenTK.Input.Key.F35: return Key.F35;
				case OpenTK.Input.Key.LShift: return Key.LeftShift;
				case OpenTK.Input.Key.RShift: return Key.RightShift;
				case OpenTK.Input.Key.LControl: return Key.LeftControl;
				case OpenTK.Input.Key.RControl: return Key.RightControl;
				case OpenTK.Input.Key.LAlt: return Key.LeftAlt;
				case OpenTK.Input.Key.RAlt: return Key.RightAlt;
				case OpenTK.Input.Key.LWin: return Key.LeftWin;
				case OpenTK.Input.Key.RWin: return Key.RightWin;
				case OpenTK.Input.Key.Menu: return Key.Menu;
				case OpenTK.Input.Key.Up: return Key.Up;
				case OpenTK.Input.Key.Down: return Key.Down;
				case OpenTK.Input.Key.Left: return Key.Left;
				case OpenTK.Input.Key.Right: return Key.Right;
				case OpenTK.Input.Key.Enter: return Key.Enter;
				case OpenTK.Input.Key.Escape: return Key.Escape;
				case OpenTK.Input.Key.Space: return Key.Space;
				case OpenTK.Input.Key.Tab: return Key.Tab;
				case OpenTK.Input.Key.BackSpace: return Key.BackSpace;
				case OpenTK.Input.Key.Insert: return Key.Insert;
				case OpenTK.Input.Key.Delete: return Key.Delete;
				case OpenTK.Input.Key.PageUp: return Key.PageUp;
				case OpenTK.Input.Key.PageDown: return Key.PageDown;
				case OpenTK.Input.Key.Home: return Key.Home;
				case OpenTK.Input.Key.End: return Key.End;
				case OpenTK.Input.Key.CapsLock: return Key.CapsLock;
				case OpenTK.Input.Key.ScrollLock: return Key.ScrollLock;
				case OpenTK.Input.Key.PrintScreen: return Key.PrintScreen;
				case OpenTK.Input.Key.Pause: return Key.Pause;
				case OpenTK.Input.Key.NumLock: return Key.NumLock;
				case OpenTK.Input.Key.Clear: return Key.Clear;
				case OpenTK.Input.Key.Sleep: return Key.Sleep;
				case OpenTK.Input.Key.Keypad0: return Key.Keypad0;
				case OpenTK.Input.Key.Keypad1: return Key.Keypad1;
				case OpenTK.Input.Key.Keypad2: return Key.Keypad2;
				case OpenTK.Input.Key.Keypad3: return Key.Keypad3;
				case OpenTK.Input.Key.Keypad4: return Key.Keypad4;
				case OpenTK.Input.Key.Keypad5: return Key.Keypad5;
				case OpenTK.Input.Key.Keypad6: return Key.Keypad6;
				case OpenTK.Input.Key.Keypad7: return Key.Keypad7;
				case OpenTK.Input.Key.Keypad8: return Key.Keypad8;
				case OpenTK.Input.Key.Keypad9: return Key.Keypad9;
				case OpenTK.Input.Key.KeypadDivide: return Key.KeypadDivide;
				case OpenTK.Input.Key.KeypadMultiply: return Key.KeypadMultiply;
				case OpenTK.Input.Key.KeypadSubtract: return Key.KeypadSubtract;
				case OpenTK.Input.Key.KeypadAdd: return Key.KeypadAdd;
				case OpenTK.Input.Key.KeypadDecimal: return Key.KeypadDecimal;
				case OpenTK.Input.Key.KeypadEnter: return Key.KeypadEnter;
				case OpenTK.Input.Key.A: return Key.A;
				case OpenTK.Input.Key.B: return Key.B;
				case OpenTK.Input.Key.C: return Key.C;
				case OpenTK.Input.Key.D: return Key.D;
				case OpenTK.Input.Key.E: return Key.E;
				case OpenTK.Input.Key.F: return Key.F;
				case OpenTK.Input.Key.G: return Key.G;
				case OpenTK.Input.Key.H: return Key.H;
				case OpenTK.Input.Key.I: return Key.I;
				case OpenTK.Input.Key.J: return Key.J;
				case OpenTK.Input.Key.K: return Key.K;
				case OpenTK.Input.Key.L: return Key.L;
				case OpenTK.Input.Key.M: return Key.M;
				case OpenTK.Input.Key.N: return Key.N;
				case OpenTK.Input.Key.O: return Key.O;
				case OpenTK.Input.Key.P: return Key.P;
				case OpenTK.Input.Key.Q: return Key.Q;
				case OpenTK.Input.Key.R: return Key.R;
				case OpenTK.Input.Key.S: return Key.S;
				case OpenTK.Input.Key.T: return Key.T;
				case OpenTK.Input.Key.U: return Key.U;
				case OpenTK.Input.Key.V: return Key.V;
				case OpenTK.Input.Key.W: return Key.W;
				case OpenTK.Input.Key.X: return Key.X;
				case OpenTK.Input.Key.Y: return Key.Y;
				case OpenTK.Input.Key.Z: return Key.Z;
				case OpenTK.Input.Key.Number0: return Key.Number0;
				case OpenTK.Input.Key.Number1: return Key.Number1;
				case OpenTK.Input.Key.Number2: return Key.Number2;
				case OpenTK.Input.Key.Number3: return Key.Number3;
				case OpenTK.Input.Key.Number4: return Key.Number4;
				case OpenTK.Input.Key.Number5: return Key.Number5;
				case OpenTK.Input.Key.Number6: return Key.Number6;
				case OpenTK.Input.Key.Number7: return Key.Number7;
				case OpenTK.Input.Key.Number8: return Key.Number8;
				case OpenTK.Input.Key.Number9: return Key.Number9;
				case OpenTK.Input.Key.Tilde: return Key.Tilde;
				case OpenTK.Input.Key.Minus: return Key.Minus;
				case OpenTK.Input.Key.Plus: return Key.Plus;
				case OpenTK.Input.Key.BracketLeft: return Key.BracketLeft;
				case OpenTK.Input.Key.BracketRight: return Key.BracketRight;
				case OpenTK.Input.Key.Semicolon: return Key.Semicolon;
				case OpenTK.Input.Key.Quote: return Key.Quote;
				case OpenTK.Input.Key.Comma: return Key.Comma;
				case OpenTK.Input.Key.Period: return Key.Period;
				case OpenTK.Input.Key.Slash: return Key.Slash;
				case OpenTK.Input.Key.BackSlash: return Key.BackSlash;
				case OpenTK.Input.Key.LastKey: return Key.LastKey;
				default: return Key.Unknown;
				#endregion
			}
		}

		private void MouseMove(object sender, MouseMoveEventArgs e)
		{
			MouseBinds[MouseEvent.Move](new MouseEventArguments(e.X, e.Y, e.XDelta, e.YDelta, 0, Mouse.Wheel));
		}

		void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
		{
			switch (e.Button)
			{
				case MouseButton.Left: MouseBinds[MouseEvent.LeftDown](new MouseEventArguments(e.X, e.Y, 0, 0, 0, Mouse.Wheel)); break;
				case MouseButton.Middle: MouseBinds[MouseEvent.MiddleDown](new MouseEventArguments(e.X, e.Y, 0, 0, 0, Mouse.Wheel)); break;
				case MouseButton.Right: MouseBinds[MouseEvent.RightDown](new MouseEventArguments(e.X, e.Y, 0, 0, 0, Mouse.Wheel)); break;
				default: throw new InvalidOperationException("Button not supported!");
			}
		}

		void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
		{
			switch (e.Button)
			{
				case MouseButton.Left: MouseBinds[MouseEvent.LeftUp](new MouseEventArguments(e.X, e.Y, 0, 0, 0, Mouse.Wheel)); break;
				case MouseButton.Middle: MouseBinds[MouseEvent.MiddleUp](new MouseEventArguments(e.X, e.Y, 0, 0, 0, Mouse.Wheel)); break;
				case MouseButton.Right: MouseBinds[MouseEvent.RightUp](new MouseEventArguments(e.X, e.Y, 0, 0, 0, Mouse.Wheel)); break;
				default: throw new InvalidOperationException("Button not supported!");
			}
		}

		void Mouse_WheelChanged(object sender, MouseWheelEventArgs e)
		{
			MouseBinds[MouseEvent.WheelMove](new MouseEventArguments(e.X, e.Y, 0, 0, e.Delta, e.Value));
		}
	}

	public enum Key
	{
		#region Keys
		Unknown,
		F1,
		F2,
		F3,
		F4,
		F5,
		F6,
		F7,
		F8,
		F9,
		F10,
		F11,
		F12,
		F13,
		F14,
		F15,
		F16,
		F17,
		F18,
		F19,
		F20,
		F21,
		F22,
		F23,
		F24,
		F25,
		F26,
		F27,
		F28,
		F29,
		F30,
		F31,
		F32,
		F33,
		F34,
		F35,
		LeftShift,
		RightShift,
		LeftControl,
		RightControl,
		LeftAlt,
		RightAlt,
		LeftWin,
		RightWin,
		Menu,
		Up,
		Down,
		Left,
		Right,
		Enter,
		Escape,
		Space,
		Tab,
		BackSpace,
		Back = BackSpace,
		Insert,
		Delete,
		PageUp,
		PageDown,
		Home,
		End,
		CapsLock,
		ScrollLock,
		PrintScreen,
		Pause,
		NumLock,
		Clear,
		Sleep,
		Keypad0,
		Keypad1,
		Keypad2,
		Keypad3,
		Keypad4,
		Keypad5,
		Keypad6,
		Keypad7,
		Keypad8,
		Keypad9,
		KeypadDivide,
		KeypadMultiply,
		KeypadSubtract,
		KeypadMinus = KeypadSubtract,
		KeypadAdd,
		KeypadPlus = KeypadAdd,
		KeypadDecimal,
		KeypadEnter,
		A,
		B,
		C,
		D,
		E,
		F,
		G,
		H,
		I,
		J,
		K,
		L,
		M,
		N,
		O,
		P,
		Q,
		R,
		S,
		T,
		U,
		V,
		W,
		X,
		Y,
		Z,
		Number0,
		Number1,
		Number2,
		Number3,
		Number4,
		Number5,
		Number6,
		Number7,
		Number8,
		Number9,
		Tilde,
		Minus,
		Plus,
		BracketLeft,
		LBracket = BracketLeft,
		BracketRight,
		RBracket = BracketRight,
		Semicolon,
		Quote,
		Comma,
		Period,
		Slash,
		BackSlash,
		LastKey
		#endregion
	}

	public enum MouseEvent
	{
		Move,
		LeftDown,
		LeftUp,
		RightDown,
		RightUp,
		MiddleDown,
		MiddleUp,
		WheelMove
	}

	public struct MouseEventArguments
	{
		public int X;
		public int Y;
		public int XDelta;
		public int YDelta;
		public int WheelDelta;
		public int WheelPosition;

		public MouseEventArguments(int x, int y, int dx, int dy, int dw, int wp)
		{
			X = x;
			Y = y;
			XDelta = dx;
			YDelta = dy;
			WheelDelta = dw;
			WheelPosition = wp;
		}
	}
}

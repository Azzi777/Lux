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
		private Dictionary<MouseEvent, Action> MouseBinds { get; set; }
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

			MouseBinds = new Dictionary<MouseEvent, Action>();
			foreach (MouseEvent m in Enum.GetValues(typeof(MouseEvent)))
			{
				if (!MouseBinds.ContainsKey(m))
				{
					MouseBinds.Add(m, () => { });
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
			return LookupTable[key];
		}

		private Dictionary<OpenTK.Input.Key, Key> LookupTable = new Dictionary<OpenTK.Input.Key, Key>
		{
			{OpenTK.Input.Key.W, Key.W},
			{OpenTK.Input.Key.A, Key.A},
			{OpenTK.Input.Key.S, Key.S},
			{OpenTK.Input.Key.D, Key.D},
			{OpenTK.Input.Key.Space, Key.Space},
			{OpenTK.Input.Key.LShift, Key.LeftShift}
		};

		private void MouseMove(object sender, MouseMoveEventArgs e)
		{
			MouseBinds[MouseEvent.Move].Invoke();
		}
	}

	public enum Key
	{
		W,
		A,
		S,
		D,
		Space,
		LeftShift
	}

	public enum MouseEvent
	{
		Move
	}
}

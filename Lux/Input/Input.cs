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
		private Dictionary<OpenTK.Input.Key, KeyValuePair<Action, Action>> KeyBinds { get; set; }
		private KeyboardDevice Keyboard { get; set; }
		private MouseDevice Mouse { get; set; }

		internal InputEngine(Engine parent)
		{
			Parent = parent;
			KeyBinds = new Dictionary<OpenTK.Input.Key, KeyValuePair<Action, Action>>();

			foreach (OpenTK.Input.Key k in Enum.GetValues(typeof(OpenTK.Input.Key)))
			{
				if (!KeyBinds.ContainsKey(k))
				{
					KeyBinds.Add(k, new KeyValuePair<Action, Action>(() => { }, () => { }));
				}
			}
		}

		internal void Finish()
		{
			Keyboard = Parent.Window.InputDriver.Keyboard[0];
			Keyboard.KeyDown += KeyDown;
			Keyboard.KeyUp += KeyUp;

			Mouse = Parent.Window.InputDriver.Mouse[0];
		}

		public void BindKeyDown(Key key, Action routine)
		{
			KeyBinds[GetOpenTKEquivalent(key)] = new KeyValuePair<Action,Action>(routine, KeyBinds[GetOpenTKEquivalent(key)].Value);
		}

		public void BindKeyUp(Key key, Action routine)
		{
			KeyBinds[GetOpenTKEquivalent(key)] = new KeyValuePair<Action, Action>(KeyBinds[GetOpenTKEquivalent(key)].Key, routine);
		}

		private void KeyDown(object sender, KeyboardKeyEventArgs args)
		{
			KeyBinds[args.Key].Key.Invoke();
		}

		private void KeyUp(object sender, KeyboardKeyEventArgs args)
		{
			KeyBinds[args.Key].Key.Invoke();
		}

		private OpenTK.Input.Key GetOpenTKEquivalent(Key key)
		{
			return LookupTable[key];
		}

		private Dictionary<Key, OpenTK.Input.Key> LookupTable = new Dictionary<Key, OpenTK.Input.Key>
		{
			{Key.W, OpenTK.Input.Key.W},
			{Key.A, OpenTK.Input.Key.A},
			{Key.S, OpenTK.Input.Key.S},
			{Key.D, OpenTK.Input.Key.D}
		};
	}

	public enum Key
	{
		W,
		A,
		S,
		D
	}
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;

using Lux.Resources;
using Lux.Graphics;
using Lux.Physics;

namespace Lux.Framework
{
	/// <summary>
	/// The main engine class.
	/// </summary>
	public class Engine
	{
		/// <summary>
		/// Loads the external resources and dependencies. Necessary in order to use Lux.
		/// </summary>
		static public void LoadDependencies()
		{
			InternalLibraryLinker.LoadDLLs();
		}

		/// <summary>
		/// The game stage currently being executed
		/// </summary>
		public IStage Stage { get; private set; }

		/// <summary>
		/// The physics part of the engine
		/// </summary>
		public PhysicsEngine Physics { get; private set; }

		/// <summary>
		/// The graphics part of the engine
		/// </summary>
		public GraphicsEngine Graphics { get; private set; }

		internal Queue<KeyValuePair<Entity, string>> EntityFinalizeQueue;
		internal List<Entity> Entities;
		internal NativeWindow Window;
		private Thread UpdateThread;
		private Thread RenderThread;
		public int UpdateRate { get; private set; }
		public int RenderRate { get; private set; }
		public Vector3 CameraPosition;
		public Vector3 CameraLookat;

		/// <summary>
		/// Creates a new instance of the Engine class
		/// </summary>
		public Engine()
		{
			UpdateThread = new Thread(new ThreadStart(Update));
			RenderThread = new Thread(new ThreadStart(Render));

			Physics = new PhysicsEngine(this);
			Graphics = new GraphicsEngine(this);

			Entities = new List<Entity>();
			EntityFinalizeQueue = new Queue<KeyValuePair<Entity, string>>();
			CameraPosition = Vector3.Backwards;
			CameraPosition = Vector3.Zero;
		}

		/// <summary>
		/// Starts the egnine
		/// </summary>
		/// <param name="updaterate">The number of updates per second (default 60)</param>
		/// <param name="framerate">The number of frames per seconed (default 60)</param>
		public void Run(int updaterate = 60, int renderrate = 60)
		{
			UpdateRate = updaterate;
			RenderRate = renderrate;
			UpdateThread.Start();
			RenderThread.Start();
		}

		private void Update()
		{
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			Stopwatch timer = new Stopwatch();
			Stopwatch framerate = new Stopwatch();
			framerate.Start();
			timer.Start();
			while (true)
			{
				if (timer.ElapsedMilliseconds < (1.0 / UpdateRate) * 1000)
				{
					continue;
				}

				Physics.Update(timer.Elapsed.TotalSeconds);

				timer.Restart();
			}
		}

		private void Render()
		{
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			Window = new NativeWindow(1024, 768, "Game Engine", GameWindowFlags.Default, GraphicsMode.Default, DisplayDevice.Default);
			Window.Closing += WindowClosing;
			Graphics.SetupRender();

			Stopwatch timer = new Stopwatch();
			Stopwatch framerate = new Stopwatch();
			framerate.Start();
			timer.Start();

			uint frames = 0;
			while (true)
			{
				Monitor.Enter(EntityFinalizeQueue);
				while (EntityFinalizeQueue.Count > 0)
				{
					Monitor.Enter(Entities);

					KeyValuePair<Entity, string> entity = EntityFinalizeQueue.Dequeue();
					entity.Key.Finalize(entity.Value);
					Entities.Add(entity.Key);

					Monitor.Exit(Entities);
				}
				Monitor.Exit(EntityFinalizeQueue);

				if (timer.ElapsedMilliseconds < (1.0 / RenderRate) * 1000)
				{
					continue;
				}

				Graphics.Render(timer.Elapsed.TotalSeconds);

				Window.ProcessEvents();
				timer.Restart();
				frames++;

				if (framerate.ElapsedMilliseconds > 1000)
				{
					Console.WriteLine("FPS: " + ((double)frames / framerate.ElapsedMilliseconds * 1000));
					frames = 0;
					framerate.Restart();
				}
			}
		}

		private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			UpdateThread.Abort();
			RenderThread.Abort();
		}

		public Entity CreateEntity(string model, string body)
		{
			Entity entity = new Entity(body);
			Monitor.Enter(EntityFinalizeQueue);
			EntityFinalizeQueue.Enqueue(new KeyValuePair<Entity, string>(entity, model));
			Monitor.Exit(EntityFinalizeQueue);

			return entity;
		}

		/// <summary>
		/// Set the stage to currently be in focus.
		/// </summary>
		/// <param name="Stage">The stage to focus</param>
		public void SetStage(IStage Stage)
		{
		}
	}
}

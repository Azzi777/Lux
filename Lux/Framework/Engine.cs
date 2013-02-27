using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;

using Lux.Resources;
using Lux.Graphics;
using Lux.Physics;
using Lux.Input;

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

		public bool IsRunning
		{
			get
			{
				return Window.Visible;
			}
		}

		/// <summary>
		/// The input part of the engine
		/// </summary>
		public InputEngine Input { get; private set; }

		internal List<Entity> Entities;
		internal Window Window;
		private Thread RunThread;
		public int UpdateRate { get; private set; }
		public int RenderRate { get; private set; }
		public Vector3 CameraPosition;
		public Vector3 CameraLookat;

		/// <summary>
		/// Creates a new instance of the Engine class
		/// </summary>
		public Engine()
		{

			RunThread = new Thread(new ThreadStart(RunWindow));

			Physics = new PhysicsEngine(this);
			Graphics = new GraphicsEngine(this);
			Input = new InputEngine(this);

			Entities = new List<Entity>();
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
			RunThread.Start();
		}

		private void RunWindow()
		{
			Window = new Window(this);

			Input.Finish();

			Window.Run(UpdateRate, RenderRate);
		}

		public Entity CreateEntity(string model, string body)
		{
			Entity entity = new Entity(body, model);

			lock(Entities)
			{
				Entities.Add(entity);
			}

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

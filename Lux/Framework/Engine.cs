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
		#region - Fields -
		internal List<Entity> Entities;
		internal Window Window;
		private Thread RunThread;

		public Camera Camera = new Camera(0, 0, 0);
		#endregion

		#region - Properties -
		/// <summary>
		/// Gets the game stage currently being executed
		/// </summary>
		public IStage Stage { get; private set; }

		/// <summary>
		/// Gets the physics part of the engine
		/// </summary>
		public PhysicsEngine Physics { get; private set; }

		/// <summary>
		/// Gets the graphics part of the engine
		/// </summary>
		public GraphicsEngine Graphics { get; private set; }

		/// <summary>
		/// Gets the input part of the engine
		/// </summary>
		public InputEngine Input { get; private set; }

		/// <summary>
		/// Gets weather the engine is running or not.
		/// </summary>
		public bool IsRunning
		{
			get
			{
				return Window.Visible;
			}
		}

		/// <summary>
		/// Gets the update rate.
		/// </summary>
		public int UpdateRate { get; private set; }

		/// <summary>
		/// Gets the render rate.
		/// </summary>
		public int RenderRate { get; private set; }
		#endregion

		#region - Constructors -
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
		}
		#endregion

		#region - Methods -
		/// <summary>
		/// Loads the external resources and dependencies. Necessary in order to use Lux.
		/// </summary>
		static public void LoadDependencies()
		{
			InternalLibraryLinker.LoadDLLs();
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

		/// <summary>
		/// Creates and adds a new entity to the game.
		/// </summary>
		/// <param name="model">Path to the 3D-model file for the entity.</param>
		/// <param name="body">Path to the physics body file for the entity.</param>
		/// <returns>The new entity</returns>
		public Entity CreateEntity(string model, string body)
		{
			Entity entity = new Entity(body, model);

			lock (Entities)
			{
				Entities.Add(entity);
			}

			return entity;
		}

		/// <summary>
		/// Halts the engine, and closes the window.
		/// </summary>
		public void Stop()
		{
			Window.Close();
		}

		/// <summary>
		/// Set the stage to currently be in focus.
		/// </summary>
		/// <param name="Stage">The stage to focus</param>
		public void SetStage(IStage Stage)
		{
		}
		#endregion

		#region - Static Methods -
		#endregion

		#region - Private Methods -
		private void RunWindow()
		{
			Window = new Window(this);

			Input.Finish();

			Window.Run(UpdateRate, RenderRate);
		}

		uint frames = 0;
		Stopwatch framerate;
		internal void Render(double time)
		{
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			UpdateExposedValues();

			Graphics.Render(time);

			frames++;

			if (framerate == null)
			{
				framerate = new Stopwatch();
				framerate.Start();
			}
			if (framerate.ElapsedMilliseconds > 1000)
			{
				Console.WriteLine("FPS: " + ((double)frames / framerate.ElapsedMilliseconds * 1000));
				frames = 0;
				framerate.Restart();
			}
		}

		internal void Update(double time)
		{
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
			UpdateExposedValues();

			Physics.Update(time);
			Input.Update();
		}

		private void UpdateExposedValues()
		{

		}
		#endregion
	}
}

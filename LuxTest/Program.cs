using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lux.Framework;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.LoadDependencies();

            Engine engine = new Engine();

			engine.Physics.AddGlobalForceGenerator(new Gravity());
            engine.CreateEntity("ent.obj", "ent.phys");
            engine.Run();

            while (true) ;
        }
    }
}

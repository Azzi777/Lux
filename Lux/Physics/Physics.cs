using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Lux.Framework;

namespace Lux.Physics
{
    /// <summary>
    /// The physics engine.
    /// (This documentation should be elaborated.)
    /// </summary>
    public class PhysicsEngine
    {
        private List<Tuple<IForceGenerator, Entity>> ForceGeneratorRegistry;
        private List<IForceGenerator> GlobalForces;

        private Engine Parent;

        internal PhysicsEngine(Engine parent)
        {
            ForceGeneratorRegistry = new List<Tuple<IForceGenerator, Entity>>();
            GlobalForces = new List<IForceGenerator>();
            Parent = parent;
        }

        internal void Update(double deltaTime)
        {
            UpdateForces(deltaTime);

            Monitor.Enter(Parent.Entities);
            foreach (Entity entity in Parent.Entities)
            {
                entity.Integrate(deltaTime);
            }
            Monitor.Exit(Parent.Entities);
        }

        private void UpdateForces(double deltaTime)
        {
            Monitor.Enter(ForceGeneratorRegistry);
            foreach (Tuple<IForceGenerator, Entity> registration in ForceGeneratorRegistry)
            {
                registration.Item1.UpdateForce(registration.Item2, deltaTime);
            }
            Monitor.Exit(ForceGeneratorRegistry);

            Monitor.Enter(GlobalForces);
            Monitor.Enter(Parent.Entities);
            foreach (IForceGenerator forceGenerator in GlobalForces)
            {
                foreach (Entity entity in Parent.Entities)
                {
                    forceGenerator.UpdateForce(entity, deltaTime);
                }
            }
            Monitor.Exit(Parent.Entities);
            Monitor.Exit(GlobalForces);
        }

        public void AddForceGenerator(IForceGenerator forceGenerator, Entity entity)
        {
            Monitor.Enter(ForceGeneratorRegistry);
            ForceGeneratorRegistry.Add(new Tuple<IForceGenerator, Entity>(forceGenerator, entity));
            Monitor.Exit(ForceGeneratorRegistry);
        }

        public void RemoveForceGenerator(IForceGenerator forceGenerator)
        {
            Monitor.Exit(ForceGeneratorRegistry);
            for (int i = 0; i < ForceGeneratorRegistry.Count; )
            {
                var registration = ForceGeneratorRegistry[i];

                if (registration.Item1 == forceGenerator)
                {
                    ForceGeneratorRegistry.Remove(registration);
                }
                else
                {
                    i++;
                }
            }
            Monitor.Exit(ForceGeneratorRegistry);
        }

        public void RemoveForceGenerator(IForceGenerator forceGenerator, Entity entity)
        {
            Monitor.Exit(ForceGeneratorRegistry);
            for (int i = 0; i < ForceGeneratorRegistry.Count; )
            {
                var registration = ForceGeneratorRegistry[i];

                if (registration.Item1 == forceGenerator && registration.Item2 == entity)
                {
                    ForceGeneratorRegistry.Remove(registration);
                }
                else
                {
                    i++;
                }
            }
            Monitor.Exit(ForceGeneratorRegistry);
        }

        public void AddGlobalForceGenerator(IForceGenerator forceGenerator)
        {
            Monitor.Enter(GlobalForces);
            GlobalForces.Add(forceGenerator);
            Monitor.Exit(GlobalForces);
        }

        public void RemoveGlobalForceGenerator(IForceGenerator forceGenerator)
        {
            Monitor.Enter(GlobalForces);
            GlobalForces.Remove(forceGenerator);
            Monitor.Exit(GlobalForces);
        }
    }
}

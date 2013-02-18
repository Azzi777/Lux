using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lux.Framework;
using Lux.Physics;

namespace Test
{
    public class Gravity : IForceGenerator
	{
		public void UpdateForce(Entity entity, double deltaTime)
		{
			entity.AddForce(Vector3.Down * entity.Mass);
			entity.AddTorque(Vector3.Forwards * 0.01D);
		}
    }
}

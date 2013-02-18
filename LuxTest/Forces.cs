using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lux.Framework;
using Lux.Physics;

namespace Test
{
	public class Force1 : IForceGenerator
	{
		public void UpdateForce(Entity entity, double deltaTime)
		{
			entity.AddForce(new Vector3(0.5D, -1, 0) * entity.Mass);
			entity.AddTorque(Vector3.Forwards * 0.01D);
		}
	}
	public class Force2 : IForceGenerator
	{
		public void UpdateForce(Entity entity, double deltaTime)
		{
			entity.AddForce(new Vector3(-0.5D, -1, 0) * entity.Mass);
			entity.AddTorque(Vector3.Backwards * 0.1D);
		}
	}
}

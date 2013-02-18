using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lux.Framework;

namespace Lux.Physics
{
	public interface IForceGenerator
	{
		void UpdateForce(Entity entity, double deltaTime);
	}
}

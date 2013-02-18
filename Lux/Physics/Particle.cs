using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lux.Framework;

namespace Lux.Physics
{
	//internal class Particle
	//{
	//    public Vector3 Position { get; private set; }
	//    public Vector3 Velocity { get; private set; }
	//    //public Vector3 Acceleration { get; private set; }
	//    private Vector3 ForceAccumulator { get; set; }

	//    public double Mass { get { return (InverseMass != 0) ? (1 / InverseMass) : double.PositiveInfinity; } }
	//    public double InverseMass { get; private set; }

	//    public Particle()
	//    {
	//        Position = Vector3.Zero;
	//        Velocity = Vector3.Zero;
	//        //Acceleration = Vector3.Zero;
	//        ForceAccumulator = Vector3.Zero;
	//        InverseMass = 0;
	//    }

	//    public void Integrate(double deltaTime)
	//    {
	//        Position += Velocity * deltaTime;

	//        Velocity += ForceAccumulator * InverseMass * deltaTime;

	//        ForceAccumulator = Vector3.Zero;
	//    }

	//    public void SetMass(double mass)
	//    {
	//        if (mass == 0)
	//        {
	//            InverseMass = double.PositiveInfinity;
	//        }
	//        else
	//        {
	//            InverseMass = 1 / mass;
	//        }
	//    }

	//    public void SetPosition(Vector3 position)
	//    {
	//        Position = position;
	//    }

	//    public void SetVelocity(Vector3 velocity)
	//    {
	//        Velocity = velocity;
	//    }

	//    //public void SetAcceleration(Vector3 acceleration)
	//    //{
	//    //    Acceleration = acceleration;
	//    //}

	//    public void AddForce(Vector3 force)
	//    {
	//        ForceAccumulator += force;
	//    }

	//    public void ClearForces()
	//    {
	//        ForceAccumulator = Vector3.Zero;
	//    }
	//}
}

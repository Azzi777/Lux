using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lux.Physics;
using Lux.Physics.Collision;
using Lux.Graphics;

namespace Lux.Framework
{
    /// <summary>
    /// An object in the game world
    /// </summary>
    public class Entity
    {
        internal Body Body;
        internal Model Model;

        /// <summary>
        /// The position of the entity.
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// The current velocity of the entity
        /// </summary>
        public Vector3 Velocity { get; private set; }

        /// <summary>
        /// The orientation of the entity, represented by a quaternion.
        /// </summary>
        public Quaternion Orientation { get; private set; }
        /// <summary>
        /// The rotation, or angular velocity, of the entity.
        /// </summary>
        public Quaternion AngularVelocity { get; private set; }

        private Vector3 ForceAccumulator { get; set; }
        private Vector3 TorqueAccumulator { get; set; }

        internal Matrix4 TransformMatrix { get; private set; }

        /// <summary>
        /// The mass of the entity
        /// </summary>
        public double Mass { get { return (InverseMass != 0) ? (1 / InverseMass) : double.PositiveInfinity; } }
        internal double InverseMass { get; private set; }

        private Matrix3 InertiaTensor { get; set; }

        /// <summary>
        /// Creates a new enitity
        /// </summary>
        /// <param name="body">The full or relative path to the body file.</param>
        /// <param name="model">The full or relative path to the model file.</param>
        internal Entity(string body, string model)
        {
            Position = Vector3.Zero;
            Velocity = Vector3.Zero;
            SetMass(1);
            Orientation = Quaternion.Identity;
            AngularVelocity = Quaternion.Identity;
            InertiaTensor = Matrix3.Identity;
            TransformMatrix = Matrix4.Identity;

            //Model = Model.LoadFromFile(modelpath);
            Model = Model.UnitIcosahedron;
        }

        /// <summary>
        /// Creates a new enitity
        /// </summary>
        /// <param name="body">The full or relative path to the body file.</param>
        internal Entity(string body)
        {
            Position = Vector3.Zero;
            Velocity = Vector3.Zero;
            SetMass(1);
            Orientation = Quaternion.Identity;
            AngularVelocity = Quaternion.Identity;
            InertiaTensor = Matrix3.Identity;
            TransformMatrix = Matrix4.Identity;
        }

        internal void Finalize(string modelpath)
        {
            //Model = Model.LoadFromFile(modelpath);
            Model = Model.UnitIcosahedron;
        }

        #region Accessor functions

        /// <summary>
        /// Set the mass of the entity.
        /// </summary>
        /// <param name="mass">The new mass.</param>
        public void SetMass(double mass)
        {
            if (mass == 0)
            {
                InverseMass = double.PositiveInfinity;
            }
            else
            {
                InverseMass = 1 / mass;
            }
        }

        /// <summary>
        /// Set the position of the entity instantly.
        /// </summary>
        /// <param name="position">The new position of the entity.</param>
        public void SetPosition(Vector3 position)
        {
            Position = position;
        }

        /// <summary>
        /// Set the velocity of the entity instantly.
        /// </summary>
        /// <param name="velocity">The new velocity of the entity.</param>
        public void SetVelocity(Vector3 velocity)
        {
            Velocity = velocity;
        }

        /// <summary>
        /// Set the orientation of the entity instantly.
        /// </summary>
        /// <param name="orientation">The new orientation of the entity.</param>
        public void SetOrientation(Quaternion orientation)
        {
            Orientation = orientation;
        }

        /// <summary>
        /// Set the angular velocity of the entity instantly.
        /// </summary>
        /// <param name="angularVelocity">The new angular velocity of the entity.</param>
        public void SetAngularVelocity(Quaternion angularVelocity)
        {
            AngularVelocity = angularVelocity;
        }

        #endregion

        private void CalculateTransformMatrix()
        {
            TransformMatrix = Matrix4.CreateTranslation(Position);
            TransformMatrix.Rotate(Orientation);
        }

        /// <summary>
        /// Applies a force to the center of the entity.
        /// </summary>
        /// <param name="force">The force to be added.</param>
        public void AddForce(Vector3 force)
        {
            ForceAccumulator += force;
        }

        /// <summary>
        /// Applies a torque to the center of the entity.
        /// </summary>
        /// <param name="force">The force to be added.</param>
        public void AddTorque(Vector3 torque)
        {
            TorqueAccumulator += torque;
        }

        /// <summary>
        /// Applies a force, resulting in both torque and acceleration.
        /// </summary>
        /// <param name="force">The force to apply.</param>
        /// <param name="point">The point at which the force should be applied.</param>
        /// <param name="localCoordPoint">Whether or not the point is in local or world co-ordinates.</param>
        public void ApplyForce(Vector3 force, Vector3 point, bool localCoordPoint)
        {
            if (localCoordPoint)
            {
                point = TransformMatrix.Transform(point);
            }

            TorqueAccumulator += Vector3.Cross(point - Position, force);
        }

        internal void Integrate(double deltaTime)
        {
            Vector3 Acceleration = ForceAccumulator * InverseMass;

            Velocity += Acceleration * deltaTime;

            Vector3 AngularAcceleration = InertiaTensor.Transform(TorqueAccumulator);
            AngularVelocity *= new Quaternion(AngularAcceleration * deltaTime);

            Position += Velocity * deltaTime;

            Orientation *= AngularVelocity * deltaTime;
            Orientation.Normalize();

            ForceAccumulator = Vector3.Zero;
            TorqueAccumulator = Vector3.Zero;

            CalculateTransformMatrix();
        }
    }
}

namespace GameObjects {
    using UnityEngine;

    public interface IGameObjectComponentSettings {
        /// <summary>
        ///     Gets the rigid body.
        /// </summary>
        Rigidbody RigidBody { get; }


        /// <summary>
        ///     Gets the transform.
        /// </summary>
        Transform Transform { get; }
    }
}

namespace GameObjects.Player {
    using UniRx;

    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The player.
    /// </summary>
    public class Player : IInitializable {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="movementSettings">
        /// The movement settings.
        /// </param>
        /// <param name="componentSettings">
        /// The component settings.
        /// </param>
        [Inject]
        internal Player(
            PlayerInstaller.Settings.Movement movementSettings,
            PlayerInstaller.Settings.Components componentSettings) {
            this.MovementSpeed = movementSettings.Speed;
            this.Transform = componentSettings.Transform;
            this.RigidBody = componentSettings.RigidBody;
        }

        /// <summary>
        /// Gets or sets the movement speed.
        /// </summary>
        public FloatReactiveProperty MovementSpeed { get; set; }

        /// <summary>
        /// Gets the rigid body.
        /// </summary>
        public Rigidbody RigidBody { get; }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        public Transform Transform { get; }

        /// <summary>
        /// The initialize.
        /// </summary>
        public void Initialize() { }
    }
}
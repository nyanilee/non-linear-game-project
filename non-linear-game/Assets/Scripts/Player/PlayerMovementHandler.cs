namespace Player {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using log4net;

    using UniRx;

    using UnityEngine;

    using Zenject;

    /// <summary>
    ///     The player movement handler.
    /// </summary>
    public class PlayerMovementHandler : UniRx.IObserver<long>, IDisposable {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     The camera.
        /// </summary>
        [InjectOptional]
        private Camera camera;

        /// <summary>
        ///     The movement speed.
        /// </summary>
        private readonly FloatReactiveProperty movementSpeed;

        /// <summary>
        ///     The observers.
        /// </summary>
        private readonly LinkedList<IDisposable> observers;

        /// <summary>
        ///     The raycast layer as displayed in the Unity Editor.
        /// </summary>
        private readonly IntReactiveProperty raycastLayer;

        /// <summary>
        ///     The transform.
        /// </summary>
        private readonly Transform transform;

        private readonly Rigidbody rigidBody;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerMovementHandler" /> class.
        /// </summary>
        /// <param name="camera">
        ///     The camera.
        /// </param>
        /// <param name="movementSettings">
        ///     The movement settings.
        /// </param>
        /// <param name="componentSettings">
        ///     The component settings.
        /// </param>
        [Inject]
        internal PlayerMovementHandler(
                PlayerInstaller.Settings.Movement movementSettings,
                PlayerInstaller.Settings.Components componentSettings) {
            this.movementSpeed = movementSettings.Speed;
            this.raycastLayer = movementSettings.RaycastLayer;
            this.transform = componentSettings.Transform;
            this.rigidBody = componentSettings.RigidBody;
            this.observers = new LinkedList<IDisposable>();
        }

        /// <summary>
        ///     Gets the raycast layer.
        /// </summary>
        public int RaycastLayer {
            get {
                return 1 << this.raycastLayer.Value;
            }
        }

        /// <inheritdoc />
        public void Dispose() {
            this.movementSpeed?.Dispose();
            this.raycastLayer?.Dispose();
            this.DisposeObservers();
        }

        /// <summary>
        ///     Dispose observers when complete.
        /// </summary>
        public void OnCompleted() {
            Log.InfoFormat(
                "Completed handling player movement\nGameobject: {0}",
                this.transform.name);
            this.DisposeObservers();
        }

        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="error">
        ///     The error.
        /// </param>
        public void OnError(Exception error) {
            Log.WarnFormat(
                "Error handling player movement\nGameobject: {0}",
                this.transform.name);
        }

        /// <summary>
        ///     Moves the gameobject.
        /// </summary>
        /// <param name="value">
        ///     The time since this method was last called.
        /// </param>
        public void OnNext(long value) {
            this.DisposeObservers();
            var mouseToCharRay =
                this.camera.ScreenPointToRay(Input.mousePosition);
            var charToScreenRay =
                this.camera.WorldToScreenPoint(this.transform.position);

            // Only raycast on the first fixed update.
            var isRaycast = false;
            var hit = new RaycastHit();
            float speedMulti = 0;
            this.observers.AddLast(
                Observable.EveryFixedUpdate().Subscribe(
                    _ => {
                        if (!isRaycast
                            && this.camera.pixelRect.Contains(
                                Input.mousePosition)) {
                            if (!Physics.Raycast(
                                    mouseToCharRay,
                                    out hit,
                                    100.0f,
                                    this.RaycastLayer)) {
                                this.DisposeObservers();
                                return;
                            }

                            isRaycast = true;
                            var screenDistance = Vector3.Distance(
                                Input.mousePosition,
                                charToScreenRay);
                            var time =
                                screenDistance / this.movementSpeed.Value;
                            var worldDistance = Vector3.Distance(
                                hit.point,
                                this.transform.position);
                            speedMulti = worldDistance / time;
                        }

                        this.transform.position = Vector3.MoveTowards(
                            this.transform.position,
                            new Vector3(
                                hit.point.x,
                                Mathf.Epsilon,
                                hit.point.z),
                            speedMulti * Time.deltaTime);
                        if (Vector3.Distance(hit.point, this.transform.position)
                                <= 0.01f) {
                            this.DisposeObservers();
                        }
                    }));
        }

        /// <summary>
        ///     Disposes all observers.
        /// </summary>
        private void DisposeObservers() {
            while (this.observers.Any()) {
                this.observers.Last.Value.Dispose();
                this.observers.RemoveLast();
            }
        }

        /// <summary>
        ///     Represents a memory pool for the
        ///     <see cref="PlayerMovementHandler" /> class.
        /// </summary>
        public class Pool : MemoryPool<Camera, PlayerMovementHandler> {
            /// <summary>
            ///     Re-initializes the handler.
            /// </summary>
            /// <param name="item">
            ///     The handler.
            /// </param>
            protected override void Reinitialize(
                    Camera camera,
                    PlayerMovementHandler item) {
                item.camera = camera;
            }

            protected override void OnDespawned(PlayerMovementHandler item) {
                item.DisposeObservers();
            }
        }
    }
}
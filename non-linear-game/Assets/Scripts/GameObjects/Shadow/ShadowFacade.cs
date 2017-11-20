namespace GameObjects.Shadow {
    using System;
    using System.Collections.Generic;

    using UniRx.Triggers;

    using UnityEngine;

    using Zenject;

    public class ShadowFacade : AbstractFacadeGameObject {
        private Shadow model;

        private ShadowMovementHandler movementHandler;

        private ShadowMovementHandler.Pool movementHandlerFactory;

        /// <summary>
        ///     The observers.
        /// </summary>
        private LinkedList<IDisposable> observers;

        /// <summary>
        ///     Prevents a default instance of the
        ///     <see cref="ShadowFacade" /> class from being created.
        /// </summary>
        private ShadowFacade() {
        }

        [Inject]
        internal void Construct(
                // ReSharper disable ParameterHidesMember
                Shadow shadow,
                ShadowMovementHandler.Pool movementHandlerFactory) {
                // ReSharper restore ParameterHidesMember
            this.observers = new LinkedList<IDisposable>();
            this.movementHandlerFactory = movementHandlerFactory;

            this.model = shadow;
        }

        public class Pool : MonoMemoryPool<Camera, Rigidbody, ShadowFacade> {
            protected override void Reinitialize(
                Camera camera,
                Rigidbody rb,
                ShadowFacade item) {
                item.movementHandler =
                    item.movementHandlerFactory.Spawn(
                        camera,
                        rb,
                        item.model.Rigidbody);
                item.observers.AddLast(
                    rb.FixedUpdateAsObservable()
                        .Subscribe(item.movementHandler));
            }

            protected override void OnDespawned(ShadowFacade item) {
                item.gameObject.SetActive(false);
                item.DisposeObservers();
            }
        }
    }
}
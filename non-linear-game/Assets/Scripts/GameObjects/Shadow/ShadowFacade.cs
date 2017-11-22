namespace GameObjects.Shadow {
    using System;
    using System.Collections.Generic;

    using UniRx;
    using UniRx.Triggers;

    using UnityEngine;

    using Zenject;

    public class ShadowFacade : AbstractFacadeGameObject {
        private Shadow model;

        private ShadowMovementHandler movementHandler;

        private ShadowMovementHandler.Pool movementHandlerFactory;

        private GameObjectScaleHandler scaleHandler;

        private GameObjectScaleHandler.Pool scaleHandlerFactory;


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
                ShadowMovementHandler.Pool movementHandlerFactory,
                GameObjectScaleHandler.Pool scaleHandlerFactory) {
                // ReSharper restore ParameterHidesMember
            this.movementHandlerFactory = movementHandlerFactory;
            this.scaleHandlerFactory = scaleHandlerFactory;

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
                item.Observers.AddLast(
                    rb.FixedUpdateAsObservable()
                        .Subscribe(item.movementHandler));
                item.scaleHandler = item.scaleHandlerFactory.Spawn(camera);
                item.Observers.AddLast(
                    Observable.EveryUpdate().Subscribe(item.scaleHandler));
            }

            protected override void OnDespawned(ShadowFacade item) {
                item.gameObject.SetActive(false);
                item.DisposeObservers();
                item.movementHandlerFactory.Despawn(item.movementHandler);
                item.scaleHandlerFactory.Despawn(item.scaleHandler);
            }
        }
    }
}
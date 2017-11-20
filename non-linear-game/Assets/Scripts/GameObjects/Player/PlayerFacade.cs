namespace GameObjects.Player {
    using System.Reflection;

    using log4net;

    using UniRx;

    using UnityEngine;

    using Zenject;

    /// <summary>
    ///     Represents a facade for the player.
    /// </summary>
    public class PlayerFacade : AbstractFacadeGameObject {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     The player model.
        /// </summary>
        private Player model;

        private PlayerMovementHandler movementHandler;

        private PlayerMovementHandler.Pool movementHandlerFactory;

        private GameObjectScaleHandler scaleHandler;

        private GameObjectScaleHandler.Pool scaleHandlerFactory;

        public Rigidbody Rigidbody {
            get {
                return this.model.RigidBody;
            }
        }

        /// <summary>
        ///     Initializes an instance of the
        ///     <see cref="PlayerFacade" /> class.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="movementHandlerFactory">
        ///     The movement handler factory.
        /// </param>
        /// <param name="scaleHandlerFactory">
        ///     The scale Handler Factory.
        /// </param>
        [Inject]
        public void Construct(
                //// ReSharper disable ParameterHidesMember
                Player model,
                PlayerMovementHandler.Pool movementHandlerFactory,
                GameObjectScaleHandler.Pool scaleHandlerFactory) {
                //// ReSharper enable ParameterHidesMember
            this.movementHandlerFactory = movementHandlerFactory;
            this.scaleHandlerFactory = scaleHandlerFactory;

            this.model = model;
        }

        public class Pool : MonoMemoryPool<Camera, PlayerFacade> {
            protected override void OnDespawned(PlayerFacade item) {
                item.gameObject.SetActive(false);
                item.DisposeObservers();
                item.movementHandlerFactory.Despawn(item.movementHandler);
                item.scaleHandlerFactory.Despawn(item.scaleHandler);
            }

            protected override void Reinitialize(
                Camera camera,
                PlayerFacade item) {
                item.movementHandler =
                    item.movementHandlerFactory.Spawn(camera);
                item.scaleHandler = item.scaleHandlerFactory.Spawn(camera);
                item.Observers.AddLast(
                    Observable.EveryUpdate()
                        .Where(_ => Input.GetMouseButtonDown(0))
                        .Subscribe(item.movementHandler));
                item.Observers.AddLast(
                    Observable.EveryUpdate().Subscribe(item.scaleHandler));
            }
        }
    }
}
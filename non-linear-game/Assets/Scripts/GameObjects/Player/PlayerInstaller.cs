namespace GameObjects.Player {
    using System;

    using UniRx;

    using UnityEngine;

    using Zenject;

    /// <summary>
    ///     The player installer.
    /// </summary>
    public class PlayerInstaller : MonoInstaller {
        /// <summary>
        ///     The component settings.
        /// </summary>
        [SerializeField]
        private Settings.Components componentSettings;

        /// <summary>
        ///     The movement settings.
        /// </summary>
        [SerializeField]
        private Settings.Movement movementSettings;

        [SerializeField]
        private Settings.Scale scaleSettings;

        /// <summary>
        ///     The install bindings.
        /// </summary>
        public override void InstallBindings() {
            this.Container.BindInterfacesAndSelfTo<Player>().AsSingle();
            this.Container.Bind<Settings.Movement>()
                .FromInstance(this.movementSettings);
            this.Container.Bind<IGameObjectComponentSettings>()
                .To<Settings.Components>().FromInstance(this.componentSettings);
            this.Container.Bind<Settings.Components>()
                .FromInstance(this.componentSettings);
            this.Container.Bind<IGameObjectScaleSettings>().To<Settings.Scale>()
                .FromInstance(this.scaleSettings);
            this.Container.Bind<Settings.Scale>()
                .FromInstance(this.scaleSettings);
            this.Container
                .BindMemoryPool<PlayerMovementHandler,
                    PlayerMovementHandler.Pool>();
            this.Container
                .BindMemoryPool<GameObjectScaleHandler,
                    GameObjectScaleHandler.Pool>();
            this.Container
                .BindMemoryPool<PlayerAnimatorHandler,
                    PlayerAnimatorHandler.Pool>();
            this.Container.Bind<Transform>()
                .FromInstance(this.componentSettings.Transform);
            this.Container.Bind<Animator>()
                .FromInstance(this.componentSettings.Animator);
        }

        /// <summary>
        ///     The settings.
        /// </summary>
        [Serializable]
        internal class Settings {
            /// <summary>
            ///     The components.
            /// </summary>
            [Serializable]
            internal class Components : IGameObjectComponentSettings {
                [SerializeField]
                private Rigidbody rigidbody;

                [SerializeField]
                private Transform transform;

                [SerializeField]
                private Animator animator;

                public Animator Animator {
                    get {
                        return this.animator;
                    }
                }

                public Rigidbody RigidBody {
                    get {
                        return this.rigidbody;
                    }
                }

                public Transform Transform {
                    get {
                        return this.transform;
                    }
                }
            }

            /// <summary>
            ///     The movement.
            /// </summary>
            [Serializable]
            internal class Movement {
                [SerializeField]
                private IntReactiveProperty raycastLayer;

                /// <summary>
                ///     The speed.
                /// </summary>
                [SerializeField]
                private FloatReactiveProperty speed;

                /// <summary>
                ///     Gets the raycast layer.
                /// </summary>
                internal IntReactiveProperty RaycastLayer {
                    get {
                        return this.raycastLayer;
                    }
                }

                /// <summary>
                ///     Gets the speed.
                /// </summary>
                internal FloatReactiveProperty Speed {
                    get {
                        return this.speed;
                    }
                }
            }

            [Serializable]
            internal class Scale : IGameObjectScaleSettings {
                [SerializeField]
                private Vector3ReactiveProperty defaultScale;

                [SerializeField]
                private FloatReactiveProperty defaultZDistance;

                public Vector3ReactiveProperty DefaultScale {
                    get {
                        return this.defaultScale;
                    }
                }

                public FloatReactiveProperty DefaultZDistance {
                    get {
                        return this.defaultZDistance;
                    }
                }
            }
        }
    }
}
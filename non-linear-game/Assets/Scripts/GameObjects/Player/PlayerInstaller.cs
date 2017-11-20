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
            this.Container.Bind<Settings.Components>()
                .FromInstance(this.componentSettings);
            this.Container.Bind<Settings.Scale>()
                .FromInstance(this.scaleSettings);
            this.Container
                .BindMemoryPool<PlayerMovementHandler,
                    PlayerMovementHandler.Pool>();
            this.Container
                .BindMemoryPool<PlayerScaleHandler,
                    PlayerScaleHandler.Pool>();
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
            internal class Components {
                /// <summary>
                ///     The rigid body.
                /// </summary>
                [SerializeField]
                private Rigidbody rigidBody;

                /// <summary>
                ///     The transform.
                /// </summary>
                [SerializeField]
                private Transform transform;

                /// <summary>
                ///     Gets the rigid body.
                /// </summary>
                internal Rigidbody RigidBody {
                    get {
                        return this.rigidBody;
                    }
                }

                /// <summary>
                ///     Gets the transform.
                /// </summary>
                internal Transform Transform {
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
            internal class Scale {
                [SerializeField]
                private Vector3ReactiveProperty defaultScale;

                internal Vector3ReactiveProperty DefaultScale {
                    get {
                        return this.defaultScale;
                    }
                }

                [SerializeField]
                private FloatReactiveProperty defaultZDistance;

                internal FloatReactiveProperty DefaultZDistance {
                    get {
                        return this.defaultZDistance;
                    }
                }
            }
        }
    }
}
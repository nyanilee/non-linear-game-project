namespace GameObjects.Shadow {
    using System;

    using UniRx;

    using UnityEngine;

    using Zenject;

    internal class ShadowInstaller : MonoInstaller {
        [SerializeField]
        private Settings.Components componentSettings;

        [SerializeField]
        private Settings.Movement movementSettings;

        [SerializeField]
        private Settings.Scale scaleSettings;

        /// <summary>
        ///     Prevents a default instance of the
        ///     <see cref="ShadowInstaller" /> class from being created.
        /// </summary>
        private ShadowInstaller() {
        }

        public override void InstallBindings() {
            this.Container.Bind<Shadow>().AsSingle();
            this.Container
                .BindMemoryPool<ShadowMovementHandler,
                    ShadowMovementHandler.Pool>();
            this.Container
                .BindMemoryPool<GameObjectScaleHandler,
                    GameObjectScaleHandler.Pool>();
            this.Container.Bind<Rigidbody>()
                .FromInstance(this.componentSettings.RigidBody);
            this.Container.Bind<FloatReactiveProperty>().FromInstance(
                this.movementSettings.XFollowDistance);
            this.Container.Bind<FloatReactiveProperty>().FromInstance(
                this.movementSettings.ZFollowDistance);
            this.Container.Bind<IGameObjectComponentSettings>()
                .To<Settings.Components>()
                .FromInstance(this.componentSettings);
            this.Container.Bind<IGameObjectScaleSettings>()
                .To<Settings.Scale>()
                .FromInstance(this.scaleSettings);
        }

        [Serializable]
        internal class Settings {
            [Serializable]
            internal class Components : IGameObjectComponentSettings {
                [SerializeField]
                private Rigidbody rigidbody;

                [SerializeField]
                private Transform transform;

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

            [Serializable]
            internal class Movement {
                [SerializeField]
                private FloatReactiveProperty xFollowDistance;

                [SerializeField]
                private FloatReactiveProperty zFollowDistance;

                internal FloatReactiveProperty XFollowDistance {
                    get {
                        return this.xFollowDistance;
                    }
                }

                internal FloatReactiveProperty ZFollowDistance {
                    get {
                        return this.zFollowDistance;
                    }
                }
            }
        }
    }
}
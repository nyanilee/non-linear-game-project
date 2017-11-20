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
            this.Container.Bind<Rigidbody>()
                .FromInstance(this.componentSettings.Rigidbody);
            this.Container.Bind<FloatReactiveProperty>().FromInstance(
                this.movementSettings.XFollowDistance);
            this.Container.Bind<FloatReactiveProperty>().FromInstance(
                this.movementSettings.ZFollowDistance);
        }

        [Serializable]
        internal class Settings {
            [Serializable]
            internal class Components {
                [SerializeField]
                private Rigidbody rigidbody;

                internal Rigidbody Rigidbody {
                    get {
                        return this.rigidbody;
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
namespace Shadow {
    using Zenject;

    internal class ShadowInstaller : MonoInstaller {
        /// <summary>
        ///     Prevents a default instance of the
        ///     <see cref="ShadowInstaller" /> class from being created.
        /// </summary>
        private ShadowInstaller() {
        }

        public override void InstallBindings() {
            this.Container.Bind<Shadow>().AsSingle();
        }
    }
}
namespace Project {
    using System;

    using Buttons;

    using GameObjects.Player;
    using GameObjects.Shadow;

    using log4net;

    using Logging;

    using SceneLoadedHandlers;

    using UnityEngine;

    using Vuforia;

    using Zenject;

    /// <summary>
    /// Represents a dependency injector for this project.
    /// </summary>
    public class ProjectInstaller : MonoInstaller {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [SerializeField]
        private Settings settings;

        /// <summary>
        /// Installs the bindings for this project.
        /// </summary>
        public override void InstallBindings() {
            // Setup Log4Net
            LogConfigurationManager.ConfigureAllLogging();
            Log.Info("[Success] Logging configured");
            this.Container.Bind<ISceneLoadedHandler>()
                .To<SceneLoadedHandler>()
                .AsSingle();
            this.Container.Bind<IInitializable>()
                .To<SceneLoadedHandler>()
                .AsSingle();
            this.Container.Bind<IDisposable>()
                .To<SceneLoadedHandler>()
                .AsSingle();
            this.Container.BindInterfacesAndSelfTo<ZenjectSceneLoader>()
                .AsSingle();
            this.Container.Bind<TrackerManager>()
                .FromInstance(TrackerManager.Instance);
            this.Container
                .BindMemoryPool<LoadSceneButtonHandler,
                    // ReSharper disable once StyleCop.SA1110
                    LoadSceneButtonHandler.Pool>();
            this.Container.BindMemoryPool<PlayerFacade, PlayerFacade.Pool>()
                .FromSubContainerResolve()
                .ByNewPrefab(this.settings.PlayerPrefab);
            this.Container.BindMemoryPool<ShadowFacade, ShadowFacade.Pool>()
                .FromSubContainerResolve()
                .ByNewPrefab(this.settings.ShadowPrefab);
            Log.Info("[Success] Project bindings installed");
/*            ((log4net.Repository.Hierarchy.Logger)log.Logger).Level = log4net.Core.Level.Debug;*/
        }

        [Serializable]
        public class Settings {
            [SerializeField]
            private GameObject playerPrefab;

            public GameObject PlayerPrefab {
                get {
                    return this.playerPrefab;
                }
            }

            [SerializeField]
            private GameObject shadowPrefab;

            public GameObject ShadowPrefab {
                get {
                    return this.shadowPrefab;
                }
            }
        }
    }
}

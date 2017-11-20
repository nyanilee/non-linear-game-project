namespace Scenes.StandardScene {
    using System;
    using System.Reflection;

    using log4net;

    using UnityEngine;
    using UnityEngine.UI;

    using Zenject;

    /// <summary>
    ///     Represents a dependency injector for a standard scene.
    /// </summary>
    public class StandardSceneInstaller : MonoInstaller {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [SerializeField]
        private Settings settings;
        /// <summary>
        ///     Installs the bindings for this scene.
        /// </summary>
        public override void InstallBindings() {
            this.Container.Bind<Tuple<Button, string>>().FromInstance(
                new Tuple<Button, string>(
                    this.settings.LoadScrapbookSceneButton,
                    this.settings.ScrapbookSceneName));
            this.Container.Bind<Camera>()
                .FromInstance(this.settings.MainCamera);
        }

        [Serializable]
        internal class Settings {
            [SerializeField]
            private Button loadScrapbookSceneButton;

            [SerializeField]
            private string scrapbookSceneName;

            [SerializeField]
            private Camera mainCamera;

            internal Button LoadScrapbookSceneButton {
                get {
                    return this.loadScrapbookSceneButton;
                }
            }

            internal string ScrapbookSceneName {
                get {
                    return this.scrapbookSceneName;
                }
            }

            internal Camera MainCamera {
                get {
                    return this.mainCamera;
                }
            }
        }
    }
}
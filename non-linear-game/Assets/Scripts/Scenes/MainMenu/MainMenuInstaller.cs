namespace Scenes.MainMenu {
    using System;

    using UnityEngine;
    using UnityEngine.UI;

    using Zenject;

    /// <summary>
    ///     Represents the dependency injector for the <code>MainMenu</code>
    ///     scene.
    /// </summary>
    public class MainMenuInstaller : MonoInstaller {
        [SerializeField]
        private Settings settings;

        public override void InstallBindings() {
            this.Container.Bind<Button>()
                .FromInstance(this.settings.StartButton)
                .WhenInjectedInto<MainMenu>();
            this.Container.Bind<string>()
                .FromInstance(this.settings.FirstScene)
                .WhenInjectedInto<MainMenu>();
            this.Container.BindInterfacesAndSelfTo<MainMenu>().AsSingle();
        }

        [Serializable]
        internal class Settings {
            /// <summary>
            ///     The start button.
            /// </summary>
            [SerializeField]
            private Button startButton;

            [SerializeField]
            private string firstScene;

            public Button StartButton {
                get {
                    return this.startButton;
                }
            }

            public string FirstScene {
                get {
                    return this.firstScene;
                }
            }
        }
    }
}
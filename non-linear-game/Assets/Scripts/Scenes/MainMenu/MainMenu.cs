namespace Scenes.MainMenu {
    using System;

    using Buttons;

    using UnityEngine.UI;

    using Zenject;

    /// <summary>
    /// The main menu.
    /// </summary>
    public class MainMenu : IInitializable, IDisposable {
        private Button startButton;

        private string firstSceneName;

        private LoadSceneButtonHandler.Pool factory;

        private LoadSceneButtonHandler handler;

        public MainMenu(Button startButton, string firstSceneName, LoadSceneButtonHandler.Pool factory) {
            this.startButton = startButton;
            this.firstSceneName = firstSceneName;
            this.factory = factory;
        }

        public void Initialize() {
            this.handler = this.factory.Spawn(this.startButton, this.firstSceneName);
        }

        public void Dispose() {
            this.factory.Despawn(this.handler);
        }
    }
}

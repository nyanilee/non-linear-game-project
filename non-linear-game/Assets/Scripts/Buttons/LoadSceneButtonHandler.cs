namespace Buttons {
    using log4net;

    using UniRx;

    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using Zenject;

    /// <summary>
    /// The load scene button handler.
    /// </summary>
    public class LoadSceneButtonHandler {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Button button;

        private string sceneName;

        public void LoadScene() {
            SceneManager.LoadSceneAsync("Loading");
            SceneManager.LoadSceneAsync(this.sceneName);
        }

        public void Dispose() {
            this.button.onClick.RemoveListener(this.LoadScene);
        }

        public class Pool : MemoryPool<Button, string, LoadSceneButtonHandler> {
            protected override void Reinitialize(
                    Button button,
                    string sceneName,
                    LoadSceneButtonHandler handler) {
                handler.button = button;
                handler.sceneName = sceneName;
                button.OnClickAsObservable().Subscribe(
                    e => {
                        handler.LoadScene();
                    });
            }

            protected override void OnDespawned(LoadSceneButtonHandler handler) {
                handler.Dispose();
            }
        }
    }
}

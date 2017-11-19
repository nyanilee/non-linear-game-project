namespace SceneLoadedHandlers {
    using System;

    using log4net;

    using UnityEngine.SceneManagement;

    using Zenject;

    /// <inheritdoc cref="ISceneLoadedHandler"/>
   public class SceneLoadedHandler : ISceneLoadedHandler,
            IInitializable,
            IDisposable {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <inheritdoc/>
        /// <summary>
        /// Logs the scene loaded event.
        /// </summary>
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            Log.InfoFormat("[Success] Loaded scene with name {0}", scene.name);
        }

        /// <summary>
        /// Adds the <see cref="OnSceneLoaded"/> event handler to the
        ///  <code>sceneLoaded</code> event.
        /// </summary>
        public void Initialize() {
            SceneManager.sceneLoaded += this.OnSceneLoaded;
        }

        /// <summary>
        /// Removes the <see cref="OnSceneLoaded"/> event handler from the
        ///  <code>sceneLoaded</code> event.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        public void Dispose() {
            SceneManager.sceneLoaded -= this.OnSceneLoaded;
        }
    }
}

namespace SceneLoadedHandlers {
    using System;

    using UnityEngine.SceneManagement;

    using Zenject;

    /// <summary>
    /// Represents a handler for when a scene is loaded.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public interface ISceneLoadedHandler {
        /// <summary>
        /// Handles the scene loaded event.
        /// </summary>
        /// <param name="scene">
        /// The scene.
        /// </param>
        /// <param name="mode">
        /// The mode for how the scene should be loaded.
        /// </param>
        void OnSceneLoaded(Scene scene, LoadSceneMode mode);
    }
}

namespace Scenes.StandardScene {
    using System;
    using System.Reflection;

    using Buttons;

    using GameObjects.Player;
    using GameObjects.Shadow;

    using log4net;

    using UniRx.Triggers;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using Zenject;

    /// <summary>
    /// Represents a standard level scene.
    /// </summary>
    public class StandardScene : MonoBehaviour {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private LoadSceneButtonHandler.Pool loadSceneButtonFactory;

        private Tuple<Button, string> loadScrapBookSceneButton;

        private PlayerFacade.Pool playerFactory;

        private PlayerFacade player;

        private ShadowFacade.Pool shadowFactory;

        private ShadowFacade shadow;

        private new Camera camera;

        [Inject]
        public void Construct(
                // ReSharper disable ParameterHidesMember
                LoadSceneButtonHandler.Pool buttonFactory,
                Tuple<Button, string> scrapBookButton,
                PlayerFacade.Pool playerFactory,
                ShadowFacade.Pool shadowFactory,
                Camera camera) {
                // ReSharper restore ParameterHidesMember
            this.loadSceneButtonFactory = buttonFactory;
            this.loadScrapBookSceneButton = scrapBookButton;
            this.playerFactory = playerFactory;
            this.shadowFactory = shadowFactory;
            this.camera = camera;
        }

        private void Start() {
            this.loadSceneButtonFactory.Spawn(
                this.loadScrapBookSceneButton.Item1,
                this.loadScrapBookSceneButton.Item2);
            this.player = this.playerFactory.Spawn(this.camera);
            this.shadow = this.shadowFactory.Spawn(
                this.camera,
                this.player.Rigidbody);
        }

        private void OnDestroy() {
            this.playerFactory.Despawn(this.player);
            this.shadowFactory.Despawn(this.shadow);
        }
    }
}

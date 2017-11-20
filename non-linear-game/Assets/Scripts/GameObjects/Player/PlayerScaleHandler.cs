namespace GameObjects.Player {
    using System;
    using System.Reflection;

    using log4net;

    using UniRx;

    using UnityEngine;

    using Zenject;

    public class PlayerScaleHandler : UniRx.IObserver<long> {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [InjectOptional]
        private Camera camera;

        private readonly Vector3ReactiveProperty defaultScale;

        private readonly FloatReactiveProperty defaultZDistance;

        /// <summary>
        ///     The transform.
        /// </summary>
        private readonly Transform transform;

        [Inject]
        internal PlayerScaleHandler(
                PlayerInstaller.Settings.Components componentSettings,
                PlayerInstaller.Settings.Scale scaleSettings) {
            this.transform = componentSettings.Transform;
            this.defaultZDistance = scaleSettings.DefaultZDistance;
            this.defaultScale = scaleSettings.DefaultScale;
        }

        public void OnCompleted() {
            // throw new NotImplementedException();
        }

        public void OnError(Exception error) {
            // throw new NotImplementedException();
        }

        public void OnNext(long value) {
            Vector3 newScale;
            try {
                newScale = Vector3.Distance(
                               new Vector3(
                                   0,
                                   0,
                                   this.camera.transform.position.z),
                               new Vector3(0, 0, this.transform.position.z))
                           / this.defaultZDistance.Value
                           * this.defaultScale.Value;
            }
            catch (MissingReferenceException e) {
                Log.Warn("Missing reference to camera", e);
                return;
            }

            this.transform.localScale = Vector3.Lerp(
                this.transform.localScale,
                newScale,
                Time.timeScale);
        }

        public class Pool : MemoryPool<Camera, PlayerScaleHandler> {
            /// <summary>
            ///     Re-initializes the handler.
            /// </summary>
            /// <param name="item">
            ///     The handler.
            /// </param>
            protected override void Reinitialize(
                Camera camera,
                PlayerScaleHandler item) {
                item.camera = camera;
            }
        }
    }
}
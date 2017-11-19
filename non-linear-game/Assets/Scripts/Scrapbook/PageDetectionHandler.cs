namespace Scrapbook {
    using log4net;

    using Vuforia;

    using Zenject;

    /// <summary>
    /// The page detection handler.
    /// </summary>
    public class PageDetectionHandler : ITrackableEventHandler {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The tracker manager.
        /// </summary>
        private ScrapbookManager scrapbookManager;

        public TrackableBehaviour TrackableBehaviour { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageDetectionHandler"/> class.
        /// </summary>
        /// <param name="scrapbookManager">
        /// The scrapbook manager.
        /// </param>
        [Inject]
        public PageDetectionHandler(ScrapbookManager scrapbookManager) {
            this.scrapbookManager = scrapbookManager;
        }

        /// <inheritdoc />
        /// <summary>
        /// The on trackable state changed.
        /// </summary>
        /// <param name="previousStatus">
        /// The previous status.
        /// </param>
        /// <param name="newStatus">
        /// The new status.
        /// </param>
        public void OnTrackableStateChanged(
                TrackableBehaviour.Status previousStatus,
                TrackableBehaviour.Status newStatus) {
            if (newStatus == TrackableBehaviour.Status.DETECTED
                    || newStatus == TrackableBehaviour.Status.TRACKED
                    || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
                this.scrapbookManager.newScene.Value = this.TrackableBehaviour.TrackableName;
            }
        }

        public class Pool : MemoryPool<TrackableBehaviour, PageDetectionHandler> {
            protected override void Reinitialize(TrackableBehaviour tb, PageDetectionHandler handler) {
                handler.TrackableBehaviour = tb;
                handler.TrackableBehaviour.RegisterTrackableEventHandler(handler);
            }

            protected override void OnDespawned(PageDetectionHandler handler) {
                base.OnDespawned(handler);
                handler.TrackableBehaviour.UnregisterTrackableEventHandler(handler);
            }
        }
    }
}

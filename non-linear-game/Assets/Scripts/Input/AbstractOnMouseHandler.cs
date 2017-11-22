namespace Input {
    using System;

    using UniRx;

    /// <summary>
    ///     Represents a mouse handler.
    /// </summary>
    public abstract class AbstractOnMouseHandler : UniRx.IObserver<Unit> {
        /// <summary>
        /// Gets or sets the unsubscriber.
        /// </summary>
        // ReSharper disable once StyleCop.SA1650
        private IDisposable unsubscriber;

        /// <summary>
        ///     Handles the on completed event.
        /// </summary>
        public abstract void OnCompleted();

        /// <summary>
        ///     Handles the error event.
        /// </summary>
        /// <param name="error">
        ///     The error.
        /// </param>
        public abstract void OnError(Exception error);

        /// <summary>
        ///     Handles the mouse event.
        /// </summary>
        /// <param name="value">
        ///     The event.
        /// </param>
        public abstract void OnNext(Unit value);

        /// <summary>
        ///     Subscribes this observer to the provider.
        /// </summary>
        /// <param name="provider">
        ///     The provider.
        /// </param>
        public void Subscribe(UniRx.IObservable<Unit> provider) {
            this.unsubscriber = provider.Subscribe(this);
        }

        /// <summary>
        ///     Subscribes this observer from the provider.
        /// </summary>
        public void UnSubscribe() {
            this.unsubscriber.Dispose();
        }
    }
}

namespace Input {
    using System;
    using System.Reflection;

    using log4net;

    using UniRx;

    /// <inheritdoc />
    /// <summary>
    ///     Represents the on mouse button down handler.
    /// </summary>
    public class OnMouseButtonDownHandler : AbstractOnMouseHandler {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnCompleted() {
            throw new NotImplementedException();
        }

        public override void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public override void OnNext(Unit value) {
            throw new NotImplementedException();
        }
    }
}

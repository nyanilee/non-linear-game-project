namespace Input {
    using System;
    using System.Reflection;

    using log4net;

    using UniRx;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    ///     Represents an on mouse exit handler.
    /// </summary>
    public sealed class OnMouseExitHandler : AbstractOnMouseHandler {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     The sprite renderer.
        /// </summary>
        private readonly SpriteRenderer spriteRenderer;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="OnMouseExitHandler" /> class.
        /// </summary>
        /// <param name="spriteRenderer">
        ///     The sprite renderer that will be deactivated.
        /// </param>
        public OnMouseExitHandler(SpriteRenderer spriteRenderer) {
            this.spriteRenderer = spriteRenderer;
        }

        /// <inheritdoc />
        public override void OnCompleted() {
            Log.InfoFormat(
                "Done observing mouse exit events on \"{0}\" gameobject",
                this.spriteRenderer.name);
        }

        /// <inheritdoc />
        public override void OnError(Exception error) {
            Log.WarnFormat(
                "Unable to observe mouse exit events on \"{0}\" gameobject",
                this.spriteRenderer.name);
        }

        /// <inheritdoc />
        public override void OnNext(Unit value) {
            this.spriteRenderer.enabled = false;
        }
    }
}
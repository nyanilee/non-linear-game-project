namespace GameObjects.Shadow {
    using UnityEngine;

    using Zenject;

    internal class Shadow {
        /// <summary>
        /// The rigidbody.
        /// </summary>
        internal Rigidbody Rigidbody { get; }

        [Inject]
        internal Shadow(Rigidbody rb) {
            this.Rigidbody = rb;
        }
    }
}

namespace Shadow {
    using UnityEngine;

    using Zenject;

    public class ShadowFacade : MonoBehaviour {
        private Shadow shadow;

        /// <summary>
        ///     Prevents a default instance of the
        ///     <see cref="ShadowFacade" /> class from being created.
        /// </summary>
        private ShadowFacade() {
        }

        [Inject]
        internal void Construct(Shadow shadow) {
            this.shadow = shadow;
        }
    }
}
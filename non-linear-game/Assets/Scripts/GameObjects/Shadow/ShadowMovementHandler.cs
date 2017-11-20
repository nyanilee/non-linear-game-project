namespace GameObjects.Shadow {
    using System;

    using UniRx;

    using UnityEngine;

    using Zenject;

    internal class ShadowMovementHandler : UniRx.IObserver<Unit> {
        private Rigidbody followingRB;

        private Rigidbody trackableRb;

        private Camera camera;

        private FloatReactiveProperty XFollowDistance;

        [Inject]
        internal ShadowMovementHandler(FloatReactiveProperty hFollow) {
            this.XFollowDistance = hFollow;
        }

        public void OnCompleted() {
        }

        public void OnError(Exception error) {
        }

        public void OnNext(Unit value) {
            // Compute the position of the trackable in viewport coordinates
            var viewportPoint = this.camera.WorldToViewportPoint
                (this.trackableRb.position);

            this.followingRB.position = this.trackableRb.position;
        }

        internal class
            Pool : MemoryPool<Camera, Rigidbody, Rigidbody, ShadowMovementHandler> {
            protected override void Reinitialize(
                Camera camera,
                Rigidbody trackableRb,
                Rigidbody followingRb,
                ShadowMovementHandler item) {
                item.camera = camera;
                item.trackableRb = trackableRb;
                item.followingRB = followingRb;
            }
        }
    }
}
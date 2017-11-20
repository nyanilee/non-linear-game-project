namespace GameObjects.Shadow {
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using log4net;

    using UniRx;

    using UnityEngine;

    using Zenject;

    internal class ShadowMovementHandler : UniRx.IObserver<Unit> {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly FloatReactiveProperty XFollowDistance;

        private readonly FloatReactiveProperty ZFollowDistance;

        private Camera camera;

        private Rigidbody followingRB;

        private Rigidbody trackableRb;

        [Inject]
        internal ShadowMovementHandler(List<FloatReactiveProperty> follow) {
            this.XFollowDistance = follow[0];
            this.ZFollowDistance = follow[1];
        }

        public void OnCompleted() {
        }

        public void OnError(Exception error) {
            Log.Error("", error);
        }

        public void OnNext(Unit value) {
            // Compute the position of the trackable in viewport coordinates
            var viewportPoint =
                this.camera.WorldToViewportPoint(this.trackableRb.position);
            if (this.trackableRb.transform.rotation.eulerAngles.y % 360
                <= 180) {
                viewportPoint.x -= Mathf.Abs(this.XFollowDistance.Value);
            }
            else {
                viewportPoint.x += Mathf.Abs(this.XFollowDistance.Value);
            }

            // After translate convert back to world coordinates
            var worldPoint = this.camera.ViewportToWorldPoint(viewportPoint);
            worldPoint.z += this.ZFollowDistance.Value;
            if (Vector3.Distance(
                    this.followingRB.transform.position,
                    worldPoint)
                <= 1) {
                this.followingRB.transform.LookAt(this.trackableRb.transform);
            }

            this.followingRB.position = Vector3.MoveTowards(
                this.followingRB.position,
                worldPoint,
                10 * Time.deltaTime);
        }

        internal class Pool : MemoryPool<Camera, Rigidbody, Rigidbody,
            ShadowMovementHandler> {
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
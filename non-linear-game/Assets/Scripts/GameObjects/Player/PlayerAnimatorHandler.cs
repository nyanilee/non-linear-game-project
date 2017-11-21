namespace GameObjects.Player {
    using System;

    using UnityEngine;

    using Zenject;

    internal class PlayerAnimatorHandler : UniRx.IObserver<long> {
        private readonly Animator animator;

        private readonly Transform transform;

        private Vector3 lastPosition;

        [Inject]
        public PlayerAnimatorHandler(Animator animator, Transform transform) {
            this.animator = animator;
            this.transform = transform;
            this.lastPosition = this.transform.position;
        }

        public void OnCompleted() {
        }

        public void OnError(Exception error) {
        }

        public void OnNext(long value) {
            // Animate
            var distance = Vector3.Distance(
                this.lastPosition,
                this.transform.position);
            this.animator.SetBool("isMoving", distance > Mathf.Epsilon);

            // Set direction of sprite
            var euler = this.transform.rotation.eulerAngles;
            this.transform.rotation =
                (this.transform.position - this.lastPosition).x >= 0
                    ? Quaternion.Euler(euler.x, 90, euler.z)
                    : Quaternion.Euler(euler.x, -90, euler.z);

            this.lastPosition = this.transform.position;
        }

        public class Pool : MemoryPool<PlayerAnimatorHandler> {
        }
    }
}
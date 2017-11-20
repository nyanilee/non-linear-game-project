namespace GameObjects {
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public abstract class AbstractFacadeGameObject : AbstractGameObject {
        protected LinkedList<IDisposable> Observers { get; private set; }

        protected void Awake() {
            this.Observers = new LinkedList<IDisposable>();
        }

        protected void DisposeObservers() {
            foreach (var o in this.Observers) {
                o.Dispose();
            }
        }
    }
}

namespace Scenes.Props {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dialogue;

    using log4net;

    using UniRx;
    using UniRx.Triggers;

    using UnityEngine;

    using Zenject;

    /// <summary>
    ///     Represents a prop
    /// </summary>
    public class Prop : MonoBehaviour, IDisposable {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     The collider.
        /// </summary>
        [SerializeField]
        private new Collider collider;

        /// <summary>
        ///     The observers.
        /// </summary>
        private LinkedList<IDisposable> observers;

        /// <summary>
        ///     The sprite renderer.
        /// </summary>
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private string dialogueName;

        private DialogueManager dialogueManager;

        [Inject]
        public void Construct(DialogueManager dialogueManager) {
            this.dialogueManager = dialogueManager;
        }

        /// <inheritdoc />
        public void Dispose() {
            while (this.observers.Any()) {
                this.observers.Last.Value.Dispose();
                this.observers.RemoveLast();
            }
        }

        /// <summary>
        ///     Initializes the <see cref="Prop"/> class.
        /// </summary>
        protected void Awake() {
            this.observers = new LinkedList<IDisposable>();
        }

        /// <summary>
        ///     Subscribers mouse enter and exit observers to toggle
        ///      a sprite renderer.
        /// </summary>
        protected void Start() {
            this.observers.AddLast(
                this.collider.OnMouseEnterAsObservable().Subscribe(
                    _ => {
                        this.spriteRenderer.enabled = true;
                        Log.InfoFormat(
                            "Activated \"{0}\" sprite renderer\nGameobject: {0}",
                            this.spriteRenderer.name);
                    }));
            this.observers.AddLast(
                this.collider.OnMouseExitAsObservable().Subscribe(
                    _ => {
                        this.spriteRenderer.enabled = false;
                        Log.InfoFormat(
                            "Deactivated \"{0}\" sprite renderer\nGameobject: {0}",
                            this.spriteRenderer.name);
                    }));
            this.collider.OnMouseDownAsObservable().Subscribe(
                _ => {
                    this.dialogueManager.StartDialogue(this.dialogueName);
                    Log.InfoFormat("Start dialogue with name \"{0}\"",
                        this.dialogueName);
                });
        }
    }
}
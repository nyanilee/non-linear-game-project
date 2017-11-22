namespace Dialogue {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using Dialogue.Signals;

    using log4net;

    using UniRx;

    using UnityEngine;

    using Yarn;
    using Yarn.Unity;

    using Zenject;

    public class DialogueManager : IInitializable, IDisposable {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TextAsset[] sourceText;

        private Dialogue dialogue;

        private StartDialogueSignal startDialogueSignal;

        private RunDialogueSignal runDialogueSignal;

        private PrintLineSignal printLineSignal;

        private StopDialogueSignal stopDialogueSignal;

        private FinishDialogueSignal finishDialogueSignal;

        private NextSignal nextSignal;

        private string startNode;

        private bool isRunning;

        private IEnumerator<Dialogue.RunnerResult> dialogueRunner;

        [Inject]
        public DialogueManager(
                TextAsset[] sourceText,
                VariableStorageBehaviour vsBehaviour,
                StartDialogueSignal startDialogueSignal,
                RunDialogueSignal runDialogueSignal,
                PrintLineSignal printLineSignal,
                NextSignal nextSignal,
                StopDialogueSignal stopDialogueSignal,
                FinishDialogueSignal finishDialogueSignal) {
            this.dialogue = new Yarn.Dialogue(vsBehaviour);
            this.sourceText = sourceText;
            this.startDialogueSignal = startDialogueSignal;
            this.runDialogueSignal = runDialogueSignal;
            this.printLineSignal = printLineSignal;
            this.nextSignal = nextSignal;
            this.stopDialogueSignal = stopDialogueSignal;
            this.finishDialogueSignal = finishDialogueSignal;
            this.startNode = Dialogue.DEFAULT_START;

            // Setup loggers
            this.dialogue.LogDebugMessage = message => { Log.Debug(message); };
            this.dialogue.LogErrorMessage = message => { Log.Error(message); };

            // Setup listeners
            this.runDialogueSignal.Listen(this.RunDialogue);
            this.nextSignal.Listen(this.Next);
            this.finishDialogueSignal.Listen(this.FinishDialogue);

            this.LoadScripts();
            this.isRunning = false;
            Log.InfoFormat("[Success] {0} initialized. ", this.GetType().Name);
        }

        public void StartDialogue(string node) {
            if (this.isRunning) {
                return;
            }
            this.startNode = node;
            this.startDialogueSignal.Fire();
            this.isRunning = true;
        }

        private void FinishDialogue() {
            this.isRunning = false;
        }

        private void RunDialogue() {
            this.RunDialogue(this.startNode);
        }

        private void RunDialogue(string node) {
            this.dialogueRunner = this.dialogue.Run(node).GetEnumerator();
            this.Next();

        }

        private void Next() {
            this.dialogueRunner.MoveNext();
            var current = this.dialogueRunner.Current;
            if (current == null) {
                return;
            }

            if (current is Dialogue.NodeCompleteResult) {
                this.stopDialogueSignal.Fire();
            }

            if (current is Dialogue.LineResult) {
                var currentText = current as Dialogue.LineResult;
                this.printLineSignal.Fire(currentText.line.text);
            }
        }

        private void LoadScripts() {
            foreach (var src in this.sourceText) {
                this.dialogue.LoadString(src.text, src.name);
            }
        }

        public void Initialize() {
        }

        public void Dispose() {
            this.runDialogueSignal.Unlisten(this.RunDialogue);
            this.nextSignal.Unlisten(this.Next);
            this.finishDialogueSignal.Unlisten(this.FinishDialogue);
        }
    }
}

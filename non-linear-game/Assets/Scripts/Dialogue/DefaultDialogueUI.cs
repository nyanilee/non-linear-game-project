namespace Dialogue {
    using System;
    using System.Reflection;
    using System.Text;

    using Dialogue.Signals;

    using log4net;

    using TMPro;

    using UnityEngine;

    using UniRx;

    using Zenject;

    public class DefaultDialogueUi : IDisposable {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private StartDialogueSignal startDialogueSignal;

        private RunDialogueSignal runDialogueSignal;

        private GameObject dialogueContainer;

        private TextMeshProUGUI lineText;

        private PrintLineSignal printLineSignal;

        private StopDialogueSignal stopDialogueSignal;

        private FinishDialogueSignal finishDialogueSignal;

        [Inject(Id = "printSpeed")]
        private IntReactiveProperty printSpeed;

        [Inject(Id = "delayBetweenLines")]
        private IntReactiveProperty delayBetweenLines;

        private NextSignal nextSignal;

        [Inject]
        public DefaultDialogueUi(
                GameObject dialogueContainer,
                TextMeshProUGUI lineText,
                StartDialogueSignal startDialogueSignal,
                RunDialogueSignal runDialogueSignal,
                PrintLineSignal printLineSignal,
                NextSignal nextSignal,
                StopDialogueSignal stopDialogueSignal,
                FinishDialogueSignal finishDialogueSignal) {
            this.dialogueContainer = dialogueContainer;
            this.lineText = lineText;
            this.startDialogueSignal = startDialogueSignal;
            this.runDialogueSignal = runDialogueSignal;
            this.printLineSignal = printLineSignal;
            this.nextSignal = nextSignal;
            this.stopDialogueSignal = stopDialogueSignal;
            this.finishDialogueSignal = finishDialogueSignal;

            this.startDialogueSignal.Listen(this.ActivateDialogue);
            this.printLineSignal.Listen(this.PrintLine);
            this.stopDialogueSignal.Listen(this.DeactivateDialogue);
        }

        private void ActivateDialogue() {
            this.dialogueContainer.SetActive(true);
            this.runDialogueSignal.Fire();
        }

        private void DeactivateDialogue() {
            this.dialogueContainer.SetActive(false);
            this.finishDialogueSignal.Fire();
        }

        public void Dispose() {
            this.startDialogueSignal.Unlisten(this.ActivateDialogue);
            this.printLineSignal.Unlisten(this.PrintLine);
            this.stopDialogueSignal.Unlisten(this.DeactivateDialogue);
        }

        private void PrintLine(string line) {
            var output = new StringBuilder();
            var counter = 0;
            this.lineText.text = string.Empty;

            Observable
                .Interval(TimeSpan.FromMilliseconds(this.printSpeed.Value))
                .Take(line.Length).Subscribe(
                    _ => {
                        if (counter >= line.Length) {
                            return;
                        }

                        output.Append(line[counter]);
                        this.lineText.text = output.ToString();
                        counter++;
                    },
                    () => {
                        Observable
                            .Timer(
                                TimeSpan.FromMilliseconds(
                                    this.delayBetweenLines.Value))
                            .Subscribe(_ => { this.nextSignal.Fire(); });
                    });

        }
    }
}
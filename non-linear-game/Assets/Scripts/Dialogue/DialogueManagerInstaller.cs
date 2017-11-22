namespace Dialogue {
    using System;

    using Dialogue.Signals;

    using TMPro;

    using UniRx;

    using UnityEngine;

    using Yarn.Unity;

    using Zenject;

    public class DialogueManagerInstaller : MonoInstaller {
        [SerializeField]
        private Settings settings;

        [SerializeField]
        private Settings.Ui uiSettings;

        public override void InstallBindings() {
            // Manager Settings
            this.Container.Bind<TextAsset[]>()
                .FromInstance(this.settings.SourceText);

            // Ui Settings
            this.Container.Bind<GameObject>()
                .FromInstance(this.uiSettings.DialogueContainer);
            this.Container.Bind<TextMeshProUGUI>()
                .FromInstance(this.uiSettings.LineText);
            this.Container.Bind<IntReactiveProperty>()
                .WithId("printSpeed")
                .FromInstance(this.uiSettings.PrintSpeed);
            this.Container.Bind<IntReactiveProperty>()
                .WithId("delayBetweenLines")
                .FromInstance(this.uiSettings.DelayBetweenLines);

            // Signals
            this.Container.DeclareSignal<StartDialogueSignal>();
            this.Container.DeclareSignal<RunDialogueSignal>();
            this.Container.DeclareSignal<PrintLineSignal>();
            this.Container.DeclareSignal<NextSignal>();
            this.Container.DeclareSignal<StopDialogueSignal>();
            this.Container.DeclareSignal<FinishDialogueSignal>();

            // Instances
            this.Container.BindInterfacesAndSelfTo<DialogueManager>()
                .AsSingle();
            this.Container.Bind<DefaultDialogueUi>().AsSingle();
        }

        [Serializable]
        internal class Settings {
            [SerializeField]
            private TextAsset[] sourceText;

            internal TextAsset[] SourceText {
                get {
                    return this.sourceText;
                }
            }

            [Serializable]
            internal class Ui {
                [SerializeField]
                private GameObject dialogueContainer;

                [SerializeField]
                private TextMeshProUGUI lineText;

                [SerializeField]
                private IntReactiveProperty printSpeed;

                [SerializeField]
                private IntReactiveProperty delayBetweenLines;

                internal GameObject DialogueContainer {
                    get {
                        return this.dialogueContainer;
                    }
                }

                internal TextMeshProUGUI LineText {
                    get {
                        return this.lineText;
                    }
                }

                internal IntReactiveProperty PrintSpeed {
                    get {
                        return this.printSpeed;
                    }
                }

                internal IntReactiveProperty DelayBetweenLines {
                    get {
                        return this.delayBetweenLines;
                    }
                }

            }
        }
    }
}
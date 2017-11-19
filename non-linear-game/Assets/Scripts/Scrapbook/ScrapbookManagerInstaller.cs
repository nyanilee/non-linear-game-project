namespace Scrapbook {
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Shadow;

    using UniRx;

    using UnityEngine;
    using UnityEngine.UI;

    using Zenject;

    /// <summary>
    ///     Represents a dependency injection installer for a ScrapbookManager
    /// </summary>
    public class ScrapbookManagerInstaller : MonoInstaller {
        /// <summary>
        ///     The settings for the scrapbook manager.
        /// </summary>
        [SerializeField]
        private Settings settings;

        /// <summary>
        ///     Installs the bindings for this ScrapbookManager.
        /// </summary>
        [SuppressMessage(
            "StyleCop.CSharp.ReadabilityRules",
            "SA1110:OpeningParenthesisMustBeOnDeclarationLine",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1015:ClosingGenericBracketsMustBeSpacedCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1014:OpeningGenericBracketsMustBeSpacedCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        public override void InstallBindings() {
            this.Container.Bind<Settings>().FromInstance(this.settings);
            this.Container.BindInterfacesAndSelfTo<ScrapbookManager>()
                .AsSingle();
            this.Container
                .BindMemoryPool<PageDetectionHandler, PageDetectionHandler.Pool
                >().WhenInjectedInto<ScrapbookManager>();
            this.Container.Bind<Camera>().FromInstance(this.settings.ArCamera);
        }
    }

    /// <summary>
    ///     The settings for this class.
    /// </summary>
    [Serializable]
    internal class Settings {
        /// <summary>
        ///     The database name.
        /// </summary>
        [SerializeField]
        private StringReactiveProperty databaseName;

        [SerializeField]
        private Button loadSceneButton;

        [SerializeField]
        private Camera arCamera;

        internal Camera ArCamera {
            get {
                return this.arCamera;
            }
        }

        /// <summary>
        ///     Gets the name of the database.
        /// </summary>
        internal StringReactiveProperty DatabaseName {
            get {
                return this.databaseName;
            }
        }

        internal Button LoadSceneButton {
            get {
                return this.loadSceneButton;
            }
        }
    }
}
namespace Scrapbook {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Buttons;

    using log4net;

    using UniRx;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using Vuforia;

    using Zenject;

    /// <summary>
    /// The scrapbook manager.
    /// </summary>
    [Serializable]
    // ReSharper disable once InheritdocConsiderUsage
    public sealed class ScrapbookManager : IInitializable, IDisposable {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The tracker manager.
        /// </summary>
        private TrackerManager trackerManager;

        /// <summary>
        /// The settings for this <see cref="ScrapbookManager"/>.
        /// </summary>
        [Inject]
        private Settings settings;

        /// <summary>
        /// The database name.
        /// </summary>
        private string databaseName;

        /// <summary>
        /// The database of image targets.
        /// </summary>
        private readonly XmlDocument database;

        /// <summary>
        /// The collection of disposable action listeners.
        /// </summary>
        private readonly LinkedList<IDisposable> disposableActionListeners;

        /// <summary>
        /// Gets the page dictionary.
        /// </summary>
        public Dictionary<string, PageDetectionHandler> PageDictionary { get; private set; }

        public ReactiveProperty<string> newScene { get; set; }

        /// <summary>
        /// The factory.
        /// </summary>
        [Inject]
        private PageDetectionHandler.Pool pageDetectionHandlerFactory;

        [Inject]
        private LoadSceneButtonHandler.Pool loadSceneButtonHandlerFactory;

        private LoadSceneButtonHandler loadSceneButtonHandler;

        private Button LoadSceneButton;

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName {
            get {
                return this.databaseName;
            }

            set {
                var filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Vuforia", value + ".xml");
                try {
                    this.database.Load(filePath);
                }
                catch (DirectoryNotFoundException e) {
                    if (this.database.DocumentElement != null) {
                        Log.WarnFormat("[Failure] Unable to load {0}", filePath);
                    }
                    else {
                        Log.Error("[Failure] Unable to load " + filePath, e);
                        throw;
                    }

                    return;
                }
                this.databaseName = value;
                var imageTargets = this.database.DocumentElement?.SelectNodes("Tracking/ImageTarget");
                if (imageTargets == null) {
                    Log.Warn("No image targets in database");
                    return;
                }
                foreach (XmlNode node in imageTargets) {
                    string imageName;
                    try {
                        // ReSharper disable once PossibleNullReferenceException
                        imageName = node.Attributes["name"].Value;
                    }
                    catch (NullReferenceException) {
                        Log.WarnFormat("[Failure] ImageTarget does not have a name attribute in {0}", filePath);
                        continue;
                    }
                    var enumerator = this.trackerManager.GetStateManager().GetTrackableBehaviours().GetEnumerator();
                    while (enumerator.MoveNext()) {
                        // ReSharper disable once PossibleNullReferenceException
                        if (enumerator.Current.TrackableName.Equals(imageName)) {
                            this.PageDictionary.Add(
                                imageName,
                                this.pageDetectionHandlerFactory.Spawn(enumerator.Current));
                        }
                    }

                    enumerator.Dispose();
                }
            }
        }

        [Inject]
        public ScrapbookManager(TrackerManager tm) {
            this.trackerManager = tm;
            this.database = new XmlDocument();
            this.PageDictionary = new Dictionary<string, PageDetectionHandler>();
            this.disposableActionListeners = new LinkedList<IDisposable>();
            this.newScene = new ReactiveProperty<string>(string.Empty);
        }

        public void Initialize() {
            this.LoadSceneButton = this.settings.LoadSceneButton;
            this.disposableActionListeners.AddLast(this.settings.DatabaseName.Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(
                dbName => { this.DatabaseName = dbName; }));
            this.newScene.Subscribe(
                e => {
                    if (string.IsNullOrEmpty(e)) {
                        this.LoadSceneButton.interactable = false;
                        return;
                    }

                    Log.InfoFormat("The detected new scene is {0}", e);
                    this.LoadSceneButton.interactable = true;
                    if (this.loadSceneButtonHandler != null) {
                        this.loadSceneButtonHandlerFactory
                            .Despawn(this.loadSceneButtonHandler);
                    }
                    this.loadSceneButtonHandler =
                        this.loadSceneButtonHandlerFactory.Spawn(
                            this.LoadSceneButton,
                            this.newScene.Value);
                });
        }


        public void Dispose() {
            var length = this.disposableActionListeners.Count;
            for (var i = 0; i < length; i++) {
                this.disposableActionListeners.Last.Value.Dispose();
                this.disposableActionListeners.RemoveLast();
            }
        }
    }
}

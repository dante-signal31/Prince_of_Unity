using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Prince
{
    // TODO: Add an event to GameTimer to load Bad Ending scene when time expires.
    /// <summary>
    /// Component to manage level loading.
    /// </summary>
    // Great article about the topic: https://gamedevbeginner.com/how-to-load-a-new-scene-in-unity-with-a-loading-screen/
    public class LevelLoader : MonoBehaviour
    {
        [Serializable]
        public struct Level
        {
            public string levelName;
            public SceneAsset levelScene;
        }

        [Header("WIRING:")] 
        [Tooltip("Needed to show loading progress.")] 
        [SerializeField] private LoadingScreen loadingScreen;
        [Tooltip("Needed to print level name at message bar.")] 
        [SerializeField] private HUDManager hudManager;
        [Tooltip("Needed to trigger level loaded events.")]
        [SerializeField] private EventBus eventBus;

        [Header("CONFIGURATION:")] 
        [Tooltip("Loadable level list. Order is important.")] 
        [SerializeField] private Level[] gameLevels;
        [Tooltip("Time in seconds to show level name when loaded.")]
        [SerializeField] private float levelMessageTime;
        [Tooltip("Delay in seconds to show level message.")]
        [SerializeField] private float delayToShowLevelMessage;
        // [Tooltip("Event listener to notify when a new level is loaded.")]
        // [SerializeField] private UnityEvent<Level> levelLoaded;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;

        /// <summary>
        /// Current scene index.
        ///
        /// This index is the one of gameLevels, here at LevelLoader, not the one of build settings.
        /// </summary>
        public int CurrentSceneIndex { get; private set; }
        
        /// <summary>
        /// Current scene index.
        ///
        /// This name is the one of gameLevels, here at LevelLoader, not the one of build settings.
        /// </summary>
        public string CurrentSceneName { get; private set; }
        
        private AsyncOperation _loadingOperation = null;
        private bool _showingLoadingScreen = false;
        private bool _levelNameAlreadyShown = false;

        private void Start()
        {
            eventBus.AddListener<GameEvents.TextScreenTimeout>(OnTextScreenTimeout);
            eventBus.AddListener<GameEvents.GameEnded>(OnGameEnded);
        }

        private void OnTextScreenTimeout(object _, GameEvents.TextScreenTimeout __)
        {
            LoadScene("Opening");
        }

        private void OnGameEnded(object _, GameEvents.GameEnded __)
        {
            LoadScene("Opening");
        }

        /// <summary>
        /// Load scene of given name.
        /// </summary>
        /// <param name="sceneName">Name of scene to load.</param>
        public void LoadScene(string sceneName)
        {
            int index = 0;
            foreach (Level level in gameLevels)
            {
                if (level.levelName == sceneName)
                {
                    CurrentSceneIndex = index;
                    CurrentSceneName = sceneName;
                    _loadingOperation = SceneManager.LoadSceneAsync(level.levelScene.name);
                    this.Log($"(LevelManager - {transform.root.name}) Scene {level.levelScene.name} loaded.", showLogs);
                    break;
                }
                index++;
            }
        }

        /// <summary>
        /// Load scene of given index.
        ///
        /// This index is the one of gameLevels, not the one of build settings.
        /// </summary>
        /// <param name="sceneName">Index of scene to load.</param>
        public void LoadScene(int sceneIndex)
        {
            CurrentSceneIndex = sceneIndex;
            Level level = gameLevels[sceneIndex];
            CurrentSceneName = level.levelName;
            _loadingOperation = SceneManager.LoadSceneAsync(level.levelScene.name);
            this.Log($"(LevelManager - {transform.root.name}) Scene {level.levelScene.name} loaded by index {sceneIndex}.", showLogs);
        }

        /// <summary>
        /// Load next scene in gameLevels.
        /// </summary>
        public void LoadNextScene()
        {
            LoadScene(++CurrentSceneIndex);
            this.Log($"(LevelManager - {transform.root.name}) Next scene loaded by index {CurrentSceneIndex}.", showLogs);
        }

        private void Update()
        {
            if (_loadingOperation != null)
            {
                if (_loadingOperation.isDone && !_levelNameAlreadyShown)
                {
                    HideLoadScreen();
                    StartCoroutine(ShowLevelNameAtMessageBar());
                    eventBus.TriggerEvent(new GameEvents.LevelLoaded(CurrentSceneName), this);
                    _levelNameAlreadyShown = true;
                    return;
                }
                if (!_showingLoadingScreen)
                {
                    ShowLoadScreen();
                    _levelNameAlreadyShown = false;
                }
                loadingScreen.SetProgressBarValue(_loadingOperation.progress * loadingScreen.ProgressBarMaxValue);
            }
        }

        /// <summary>
        /// Show level name at message bar after a delay.
        ///
        /// Delay is used because otherwise loading operation is done before hud is enabled again. So we must give
        /// hud a time (this delay) to activate itself an prepare to be ready to print messages again.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowLevelNameAtMessageBar()
        {
            yield return new WaitForSeconds(delayToShowLevelMessage);
            hudManager.SetMessageForATime(CurrentSceneName, levelMessageTime);
        }

        private void ShowLoadScreen()
        {
            loadingScreen.ShowLoadingScreen();
            _showingLoadingScreen = true;
        }

        private void HideLoadScreen()
        {
            loadingScreen.HideLoadingScreen();
            _showingLoadingScreen = false;
            _loadingOperation = null;
        }
    }
}
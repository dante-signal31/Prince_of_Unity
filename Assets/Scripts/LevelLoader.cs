using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prince
{
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

        [Header("CONFIGURATION:")] 
        [Tooltip("Loadable level list. Order is important.")] 
        [SerializeField] private Level[] gameLevels;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;

        /// <summary>
        /// Current scene index.
        ///
        /// This index is the one of gameLevels, not the one of build settings.
        /// </summary>
        public int CurrentSceneIndex { get; private set; }
        
        private AsyncOperation _loadingOperation = null;
        private bool _showingLoadingScreen = false;

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
                if (_loadingOperation.isDone)
                {
                    HideLoadScreen();
                    return;
                }
                if (!_showingLoadingScreen)
                {
                    ShowLoadScreen();
                }
                loadingScreen.SetProgressBarValue(_loadingOperation.progress * loadingScreen.ProgressBarMaxValue);
            }
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
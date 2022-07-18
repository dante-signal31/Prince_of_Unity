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
                    SceneManager.LoadScene(level.levelScene.name);
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
            SceneManager.LoadScene(level.levelScene.name);
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
    }
}
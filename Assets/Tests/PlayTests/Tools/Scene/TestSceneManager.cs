using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests.PlayTests.Tools.Scene
{
    /// <summary>
    /// Tools to load and unload tests scenes.
    /// </summary>
    public static class TestSceneManager
    {
        /// <summary>
        /// Unload given scene and load it again.
        /// </summary>
        /// <param name="scene">String name of the scene.</param>
        public static IEnumerator ReLoadScene(string scene)
        {
            _ = UnLoadScene(scene);
            return LoadScene(scene);
        }

        /// <summary>
        /// Unload given scene.
        /// </summary>
        /// <param name="scene">String name of the scene</param>
        public static IEnumerator UnLoadScene(string scene)
        {
            if (SceneManager.GetSceneByName(scene).isLoaded)
            {
                AsyncOperation asyncUnLoad = SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
                if (asyncUnLoad != null)
                    yield return new WaitUntil(() => asyncUnLoad.isDone);
                else
                {
                    yield break;
                }
            }
        }

        /// <summary>
        /// Unload current scene.
        /// </summary>
        public static IEnumerator UnLoadCurrentScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            yield return UnLoadScene(currentSceneName);
        }

        /// <summary>
        /// Load given scene.
        /// </summary>
        /// <param name="scene">String name of scene.</param>
        public static IEnumerator LoadScene(string scene)
        {
            //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            yield return new WaitUntil(() => asyncLoad.isDone);
        }
    }
}
using System;
using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class EnemyChaseTests
    {
        private GameObject _prince;
        private GameObject _enemy;

        private GameObject _startPosition1;
        private GameObject _startPosition2;
        private GameObject _startPosition3;

        private string _currentScene = "TwoLevelPit";

        private IEnumerator ReLoadScene(string scene)
        {
            if (SceneManager.GetSceneByName(scene).isLoaded)
            {
                AsyncOperation asyncUnLoad = SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
                yield return new WaitUntil(() => asyncUnLoad.isDone);
            }

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            yield return new WaitUntil(() => asyncLoad.isDone);
        }

        [UnitySetUp]
        public IEnumerator Setup()
        {
            // if (!SceneManager.GetSceneByName("ThePit").isLoaded)
            // {
            //     AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ThePit", LoadSceneMode.Additive);
            //     yield return new WaitUntil(() => asyncLoad.isDone);
            // }

            yield return ReLoadScene("TwoLevelPit");

            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_enemy == null) _enemy = GameObject.Find("Enemy");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            if (_startPosition2 == null) _startPosition2 = GameObject.Find("StartPosition2");
            if (_startPosition3 == null) _startPosition3 = GameObject.Find("StartPosition3");
            
            yield return new EnterPlayMode();
        }

        // [NUnit.Framework.Test]
        // public void CharacterMovementSimplePasses()
        // {
        //     // Use the Assert class to test conditions.
        //     
        // }

        /// <summary>
        /// Test guard stops if trying to get Prince he finds a hole in the way.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator GuardDoNotFallTest()
        {
            // Setup test.
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.transform.SetPositionAndRotation(_startPosition3.transform.position, Quaternion.identity);
            Vector2 startPosition = _enemy.transform.position;
            // Let movements perform.
            yield return new WaitForSeconds(5);
            Vector2 endPosition = _enemy.transform.position;
            float fallenDistance = startPosition.y - endPosition.y;
            Assert.IsTrue(fallenDistance < 0.10f);
            float advancedDistance = endPosition.x - startPosition.y;
            Assert.IsTrue(advancedDistance > 0.3f);
            yield return null;
        }
    }
}
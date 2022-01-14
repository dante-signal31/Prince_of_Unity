using System;
using System.Collections;
using NUnit.Framework;
using Prince;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Tests.PlayTests.Tools.Lang;

namespace Tests.PlayTests
{
    public class CharacterMovementTests
    {
        private GameObject _prince;
        private GameObject _enemy;

        private string _currentScene = "ThePit";

        private IEnumerator ReLoadScene(string scene)
        {
            _ = UnLoadScene(scene);
            yield return LoadScene(scene);
        }

        private IEnumerator UnLoadScene(string scene)
        {
            if (SceneManager.GetSceneByName(scene).isLoaded)
            {
                AsyncOperation asyncUnLoad = SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
                yield return new WaitUntil(() => asyncUnLoad.isDone);
            }
        }

        private IEnumerator LoadScene(string scene)
        {
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

            yield return ReLoadScene(_currentScene);
            
            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_enemy == null) _enemy = GameObject.Find("Enemy");
            
            _prince.SetActive(false);
            _enemy.SetActive(false);

            yield return new EnterPlayMode();
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return UnLoadScene(_currentScene);
        }
        
        // [NUnit.Framework.Test]
        // public void CharacterMovementSimplePasses()
        // {
        //     // Use the Assert class to test conditions.
        //     
        // }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        
        /// <summary>
        /// Test Prince advance with sword movement.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceAdvanceWithSwordTest()
        {
            // Setup test.
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _enemy.GetComponentInChildren<GuardController>().enabled = false;
            float expected_distance = 0.4161f;
            string commandFile = @"Assets\Tests\TestResources\advanceWithSword";
            Vector2 startPosition = _prince.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Vector2 endPosition = _prince.transform.position;
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            // Assert Prince has advanced what we expected.
            Assert.True(Math.Abs(error) < 0.02);
        }
        
        /// <summary>
        /// Test Prince retreat with sword movement.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceRetreatWithSwordTest()
        {
            // Setup test.
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _enemy.GetComponentInChildren<GuardController>().enabled = false;
            float expected_distance = 0.369f;
            string commandFile = @"Assets\Tests\TestResources\retreatWithSword";
            Vector2 startPosition = _prince.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Vector2 endPosition = _prince.transform.position;
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            // Assert Prince has retreated the distance we expected.
            Assert.IsTrue(Math.Abs(error) < 0.02);
            yield return null;
        }
        
        /// <summary>
        /// Test guard advance with sword movement.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardAdvanceWithSwordTest()
        {
            // We are going to control guard on our own so we disable its AI controllers.
            _enemy.GetComponentInChildren<GuardController>().enabled = false;
            _enemy.GetComponentInChildren<EnemyPursuer>().enabled = false;
            // Setup test.
            _prince.SetActive(true);
            _enemy.SetActive(true);
            float expected_distance = 0.4295f;
            string commandFile = @"Assets\Tests\TestResources\advanceWithSwordGuard";
            Vector2 startPosition = _enemy.transform.position;
            InputController inputController = _enemy.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            Vector2 endPosition = _enemy.transform.position;
            // Guard is looking leftwards so we must check it has moved to the left.
            Assert.IsTrue(endPosition.x < startPosition.x);
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            // Assert Guard has advanced what we expected.
            Assert.True(Math.Abs(error) < 0.02);
        }
    }
}
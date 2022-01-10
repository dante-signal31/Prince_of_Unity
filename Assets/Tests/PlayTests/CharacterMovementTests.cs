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

            yield return ReLoadScene("ThePit");
            
            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_enemy == null) _enemy = GameObject.Find("Enemy");

            yield return new EnterPlayMode();
        }
        
        // [NUnit.Framework.Test]
        // public void CharacterMovementSimplePasses()
        // {
        //     // Use the Assert class to test conditions.
        //     
        // }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
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
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Vector2 endPosition = _prince.transform.position;
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            Assert.True(Math.Abs(error) < 0.02);
        }
        
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
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Vector2 endPosition = _prince.transform.position;
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            Assert.IsTrue(Math.Abs(error) < 0.02);
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator GuardAdvanceWithSwordTest()
        {
            // Setup test.
            _prince.SetActive(true);
            _enemy.SetActive(true);
            // We are going to control guard on our own so we disable its AI controller.
            _enemy.GetComponentInChildren<GuardController>().enabled = false;
            float expected_distance = 0.4295f;
            string commandFile = @"Assets\Tests\TestResources\advanceWithSwordGuard";
            Vector2 startPosition = _enemy.transform.position;
            InputController inputController = _enemy.GetComponent<InputController>();
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Vector2 endPosition = _enemy.transform.position;
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            Assert.True(Math.Abs(error) < 0.02);
        }
    }
}
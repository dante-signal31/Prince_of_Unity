using System;
using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class FightingTests
    {
        private GameObject _prince;
        private GameObject _enemy;

        private GameObject _startPosition1;
        private GameObject _startPosition2;

        private string _currentScene = "ThePit";
        
        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);

            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_enemy == null) _enemy = GameObject.Find("Enemy");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            if (_startPosition2 == null) _startPosition2 = GameObject.Find("StartPosition2");

            yield return new EnterPlayMode();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return TestSceneManager.UnLoadScene(_currentScene);
        }
        
        /// <summary>
        /// Test guard moves backwards when hit by a sword.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardHitBySwordMovementTest()
        {
            // Setup test.
            LogAssert.ignoreFailingMessages = true;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _enemy.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            // I dont want enemy to move or defend by itself.
            _enemy.GetComponentInChildren<GuardController>().enabled = false;
            yield return null;
            float expected_distance = 0.697f;
            string commandFile = @"Assets\Tests\TestResources\oneSwordHit";
            Vector2 startPosition = _enemy.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Vector2 endPosition = _enemy.transform.position;
            float movedDistance = Vector2.Distance(startPosition, endPosition);
            float error = movedDistance - expected_distance;
            // Assert Guard has moved what we expected.
            Assert.True(Math.Abs(error) < 0.08);
            yield return null;
        }
        
        /// <summary>
        /// Test prince moves backwards when hit by a sword.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceHitBySwordMovementTest()
        {
            // Setup test.
            LogAssert.ignoreFailingMessages = true;
            // // I dont want enemy to move or defend by itself.
            // _enemy.GetComponentInChildren<GuardController>().enabled = false;
            // // I don't want EnemyPursuer to clutter log.
            // _enemy.GetComponentInChildren<EnemyPursuer>().enabled = false;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 1;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _enemy.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            yield return null;
            float expected_distance = 0.68f;
            string commandFile = @"Assets\Tests\TestResources\oneSwordHit";
            Vector2 startPosition = _prince.transform.position;
            InputController inputController = _enemy.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Vector2 endPosition = _prince.transform.position;
            float movedDistance = Vector2.Distance(startPosition, endPosition);
            float error = movedDistance - expected_distance;
            // Assert Prince has moved what we expected.
            Assert.True(Math.Abs(error) < 0.08);
            yield return null;
        }
    }
}
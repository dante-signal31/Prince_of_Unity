using System;
using System.Collections;
using NUnit.Framework;
using Prince;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Tests.PlayTests.Tools.Lang;
using Tests.PlayTests.Tools.Scene;

namespace Tests.PlayTests
{
    public class CharacterMovementTests
    {
        private GameObject _prince;
        private GameObject _enemy;
        
        private GameObject _startPosition1;
        private GameObject _startPosition2;
        private GameObject _startPosition3;

        private string _currentScene = "ThePit";

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);
            
            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_enemy == null) _enemy = GameObject.Find("Enemy");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            if (_startPosition2 == null) _startPosition2 = GameObject.Find("StartPosition2");
            if (_startPosition3 == null) _startPosition3 = GameObject.Find("StartPosition3");
            
            _prince.SetActive(false);
            _enemy.SetActive(false);

            yield return new EnterPlayMode();
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return TestSceneManager.UnLoadScene(_currentScene);
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
        /// Test Prince running movement.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceRunningTest()
        {
            // Setup test.
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            float expected_distance = 4.13f;
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            Vector2 startPosition = _prince.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            Vector2 endPosition = _prince.transform.position;
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            // Assert Prince has advanced what we expected.
            Assert.True(Math.Abs(error) < 0.10);
        }
        
        /// <summary>
        /// Test Prince end running while starting.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceAbortRunningTest()
        {
            // Setup test.
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            float expected_distance = 1.36f;
            string commandFile = @"Assets\Tests\TestResources\abortRunningSequence";
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
            Assert.True(Math.Abs(error) < 0.10);
        }
        
        /// <summary>
        /// Test Prince turn while running movement.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceTurnWhileRunningTest()
        {
            // Setup test.
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            float expected_distance = 5.33f;
            float expected_end_distance = 0.04f;
            string commandFile = @"Assets\Tests\TestResources\turnWhileRunningSequence";
            Vector2 startPosition = _prince.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            // Advance phase.
            yield return new WaitForSeconds(3.5f);
            Vector2 endPosition = _prince.transform.position;
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            // Assert Prince has advanced what we expected.
            Assert.True(Math.Abs(error) < 0.10);
            // Retreat phase
            yield return new WaitForSeconds(4);
            endPosition = _prince.transform.position;
            advancedDistance = Vector2.Distance(startPosition, endPosition);
            error = advancedDistance - expected_end_distance;
            // Assert Prince has retreated what we expected.
            Assert.True(Math.Abs(error) < 0.30);
        }
        
        /// <summary>
        /// Test Prince walking movement.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceWalkingTest()
        {
            // Setup test.
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            float expected_distance = 0.75f;
            string commandFile = @"Assets\Tests\TestResources\walkingSequence";
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
            Assert.True(Math.Abs(error) < 0.04);
        }
        
        /// <summary>
        /// Test Prince standing from crouch movement.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceStandingFromCrouchTest()
        {
            // Setup test.
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            float expected_distance = 0.20f;
            string commandFile = @"Assets\Tests\TestResources\standingFromCrouchSequence";
            Vector2 startPosition = _prince.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            Vector2 endPosition = _prince.transform.position;
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            // Assert Prince has advanced what we expected.
            Assert.True(Math.Abs(error) < 0.04);
        }

        /// <summary>
        /// Test Prince crouch walking movement.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceCrouchWalkingTest()
        {
            // Setup test.
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            float expected_distance = 0.50f;
            string commandFile = @"Assets\Tests\TestResources\crouchWalkingSequence";
            Vector2 startPosition = _prince.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            Vector2 endPosition = _prince.transform.position;
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            // Assert Prince has advanced what we expected.
            Assert.True(Math.Abs(error) < 0.10);
        }
        
        /// <summary>
        /// Test Prince advance with sword movement.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceAdvanceWithSwordTest()
        {
            // Setup test.
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _enemy.transform.SetPositionAndRotation(_startPosition3.transform.position, Quaternion.identity);
            // I dont want enemy to move.
            _enemy.GetComponentInChildren<GuardController>().enabled = false;
            // I don't want EnemyPursuer clutter logs.
            _enemy.GetComponentInChildren<EnemyPursuer>().enabled = false;
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
            Assert.True(Math.Abs(error) < 0.15);
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
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            // I dont want enemy to move.
            _enemy.GetComponentInChildren<GuardController>().enabled = false;
            // I don't want EnemyPursuer clutter logs.
            _enemy.GetComponentInChildren<EnemyPursuer>().enabled = false;
            float expected_distance = 0.369f;
            string commandFile = @"Assets\Tests\TestResources\retreatWithSword";
            Vector2 startPosition = _prince.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            Vector2 endPosition = _prince.transform.position;
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            // Assert Prince has retreated the distance we expected.
            Assert.IsTrue(Math.Abs(error) < 0.10);
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
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _enemy.transform.SetPositionAndRotation(_startPosition3.transform.position, Quaternion.identity);
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
            Assert.True(Math.Abs(error) < 0.10);
        }
    }
}
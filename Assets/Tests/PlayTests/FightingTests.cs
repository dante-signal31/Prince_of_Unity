using System;
using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using Tests.PlayTests.Tools.Scene;
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
            if (_enemy == null) _enemy = GameObject.Find("Enemy-Level_1");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            if (_startPosition2 == null) _startPosition2 = GameObject.Find("StartPosition2");

            yield return new EnterPlayMode();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            // Remove GameManagers to avoid having multiple instances of it in the next test.
            GameObject.Destroy(GameObject.Find("GameManagers"));
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
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            // I dont want enemy to move or defend by itself.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            yield return null;
            float expected_distance = 0.697f;
            string commandFile = @"Assets\Tests\TestResources\oneSwordHit";
            Vector2 startPosition = _enemy.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
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
            // Let enemy attack Prince..
            _enemy.GetComponentInChildren<GuardController>().enabled = true;
            _enemy.GetComponentInChildren<EnemyPursuer>().enabled = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 1;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            yield return null;
            float expected_distance = 0.68f;
            Vector2 startPosition = _prince.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            // Put Prince in fighting mode.
            inputController.Action();
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Vector2 endPosition = _prince.transform.position;
            float movedDistance = Vector2.Distance(startPosition, endPosition);
            float error = movedDistance - expected_distance;
            // Assert Prince has moved what we expected.
            Assert.True(Math.Abs(error) < 0.08);
            yield return null;
        }
        
        /// <summary>
        /// Test prince dies immediately if he is hit with sword sheathed.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceDeadIfDoesNotFightFightTest()
        {
            // Setup test.
            LogAssert.ignoreFailingMessages = true;
            // Let enemy attack Prince..
            _enemy.GetComponentInChildren<GuardController>().enabled = true;
            _enemy.GetComponentInChildren<EnemyPursuer>().enabled = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 1;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Assert.IsTrue(_prince.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.Dead);
            yield return null;
        }
        
        /// <summary>
        /// Test prince dies after been repeatedly hit by a guard.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceDeadBySwordFightTest()
        {
            // Setup test.
            LogAssert.ignoreFailingMessages = true;
            // Let enemy attack Prince..
            _enemy.GetComponentInChildren<GuardController>().enabled = true;
            _enemy.GetComponentInChildren<EnemyPursuer>().enabled = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 1;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            // Put Prince in fighting mode.
            inputController.Action();
            // Let movements perform.
            yield return new WaitForSeconds(7);
            Assert.IsTrue(_prince.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.Dead);
            yield return null;
        }
        
        /// <summary>
        /// Test prince is not hit by a guard with an attack profile of 0.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceAliveByInactiveSwordFightTest()
        {
            // Setup test.
            LogAssert.ignoreFailingMessages = true;
            // Let enemy attack Prince..
            _enemy.GetComponentInChildren<GuardController>().enabled = true;
            _enemy.GetComponentInChildren<EnemyPursuer>().enabled = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            int startLife = _prince.GetComponent<CharacterStatus>().Life;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            // Put Prince in fighting mode.
            inputController.Action();
            // Let movements perform.
            yield return new WaitForSeconds(4);
            int endLife = _prince.GetComponent<CharacterStatus>().Life;
            Assert.IsTrue(startLife == endLife);
            yield return null;
        }
        
        /// <summary>
        /// Test prince is only wounded by a guard with an attack profile of 0,4.
        /// </summary>
        [UnityTest]
        public IEnumerator PrinceWoundedBySwordFightTest()
        {
            // Setup test.
            LogAssert.ignoreFailingMessages = true;
            // Let enemy attack Prince..
            _enemy.GetComponentInChildren<GuardController>().enabled = true;
            _enemy.GetComponentInChildren<EnemyPursuer>().enabled = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0.2f;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0.3f;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            int startLife = _prince.GetComponent<CharacterStatus>().Life;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            // Put Prince in fighting mode.
            inputController.Action();
            // Let movements perform.
            yield return new WaitForSeconds(7);
            int endLife = _prince.GetComponent<CharacterStatus>().Life;
            Assert.IsTrue(startLife > endLife);
            Assert.IsTrue(endLife > 0);
            yield return null;
        }
        
        /// <summary>
        /// Test guard dies after been repeatedly hit by prince.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardDeadBySwordFightTest()
        {
            // Setup test.
            LogAssert.ignoreFailingMessages = true;
            // Let enemy attack Prince..
            _enemy.GetComponentInChildren<GuardController>().enabled = true;
            _enemy.GetComponentInChildren<EnemyPursuer>().enabled = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            InputController inputController = _prince.GetComponent<InputController>();
            string commandFile = @"Assets\Tests\TestResources\attackingGuard";
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(11);
            Assert.IsTrue(_enemy.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.Dead);
            yield return null;
        }
        
        /// <summary>
        /// Test guard moves backwards when blocks an strike.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardBlockStrikeTest()
        {
            // Setup test.
            LogAssert.ignoreFailingMessages = true;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            // I dont want enemy to move, but I want him to be a perfect defender.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 1;
            yield return null;
            // float expected_distance = 0.3885f;
            float expected_distance = 0;
            string commandFile = @"Assets\Tests\TestResources\oneSwordHit";
            Vector2 startPosition = _enemy.transform.position;
            int startLife = _enemy.GetComponent<CharacterStatus>().Life;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            // Assert Guard has not lost any life point.
            int endLife = _enemy.GetComponent<CharacterStatus>().Life;
            Assert.True(startLife == endLife);
            // Assert Guard has moved what we expected.
            Vector2 endPosition = _enemy.transform.position;
            float movedDistance = Vector2.Distance(startPosition, endPosition);
            float error = movedDistance - expected_distance;
            Assert.True(Math.Abs(error) < 0.35);
            yield return null;
        }
        
        /// <summary>
        /// Test guard is invulnerable when his defense is 1.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardBlockEveryStrikeWhenDefenseOneTest()
        {
            // Setup test.
            LogAssert.ignoreFailingMessages = true;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            // I dont want enemy to move, but I want him to be a perfect defender.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 1;
            yield return null;
            // float expected_distance = 0.3885f;
            float expected_distance = 0;
            string commandFile = @"Assets\Tests\TestResources\massiveSwordHit";
            int startLife = _enemy.GetComponent<CharacterStatus>().Life;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(15);
            // Assert Guard has not lost any life point.
            int endLife = _enemy.GetComponent<CharacterStatus>().Life;
            Assert.True(startLife == endLife);
            yield return null;
        }
    }
}
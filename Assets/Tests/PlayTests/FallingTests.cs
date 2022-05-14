using System;
using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class FallingTests
    {
        private GameObject _prince;
        private GameObject _enemy;
        
        private GameObject _startPosition1;
        private GameObject _startPosition2;
        private GameObject _startPosition3;
        private GameObject _startPosition4;
        private GameObject _startPosition5;
        private GameObject _startPosition6;
        private GameObject _startPosition7;
        private GameObject _startPosition8;

        private CameraController _cameraController;
        private Room _room00;
        private Room _room01;
        private Room _room02;

        private string _currentScene = "TheAbyss";

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);
            
            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_enemy == null) _enemy = GameObject.Find("Enemy");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            if (_startPosition2 == null) _startPosition2 = GameObject.Find("StartPosition2");
            if (_startPosition3 == null) _startPosition3 = GameObject.Find("StartPosition3");
            if (_startPosition4 == null) _startPosition4 = GameObject.Find("StartPosition4");
            if (_startPosition5 == null) _startPosition5 = GameObject.Find("StartPosition5");
            if (_startPosition6 == null) _startPosition6 = GameObject.Find("StartPosition6");
            if (_startPosition7 == null) _startPosition7 = GameObject.Find("StartPosition7");
            if (_startPosition8 == null) _startPosition8 = GameObject.Find("StartPosition8");
            if (_cameraController == null)
                _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
            if (_room00 == null)
                _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();
            if (_room01 == null)
                _room01 = GameObject.Find("Room_0_1").GetComponentInChildren<Room>();
            if (_room02 == null)
                _room02 = GameObject.Find("Room_0_2").GetComponentInChildren<Room>();
            
            _prince.SetActive(false);
            _enemy.SetActive(false);

            yield return new EnterPlayMode();
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return TestSceneManager.UnLoadScene(_currentScene);
        }

        /// <summary>
        /// Test that Prince survives unharmed a 1 level fall.
        /// </summary>
        [UnityTest]
        public IEnumerator FallingOneLevelTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            float expectedFinalHeight = 0.32f;
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Assert Prince has fallen to where we expected.
            float endHeight = _prince.transform.position.y;
            float heightError = endHeight - expectedFinalHeight;
            Assert.True(Math.Abs(heightError) < 0.04);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }
        
        /// <summary>
        /// Test that Prince do not fall if he walks over a hole.
        /// </summary>
        [UnityTest]
        public IEnumerator WalkingDoNotFallTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            Vector3 expectedPosition = _startPosition5.transform.position;
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            string commandFile = @"Assets\Tests\TestResources\walkingThreeSteps";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Assert Prince has not fallen.
            Assert.True(Math.Abs(expectedPosition.x - _prince.transform.position.x) < 0.10);
            Assert.True(Math.Abs(expectedPosition.y - _prince.transform.position.y) < 0.10);
        }
        
        /// <summary>
        /// Test that Prince survives a 2 level fall but loses 1 point.
        /// </summary>
        [UnityTest]
        public IEnumerator FallingTwoLevelsTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            float expectedFinalHeight = 0.32f;
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert Prince has fallen to where we expected.
            float endHeight = _prince.transform.position.y;
            float heightError = endHeight - expectedFinalHeight;
            Assert.True(Math.Abs(heightError) < 0.04);
            // Assert Prince is alive but has lost 1 point.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(endHealth == (startingHealth - 1) );
        }
        
        /// <summary>
        /// Test that Prince dies if falls 3 levels.
        /// </summary>
        [UnityTest]
        public IEnumerator FallingThreeLevelsTest()
        {
            _cameraController.PlaceInRoom(_room01);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition3.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            float expectedFinalHeight = 0.32f;
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            Vector2 startPosition = _prince.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            // Assert Prince has fallen to where we expected.
            float endHeight = _prince.transform.position.y;
            float heightError = endHeight - expectedFinalHeight;
            Assert.True(Math.Abs(heightError) < 0.20);
            // Assert Prince is dead.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        /// <summary>
        /// Test that Prince screams after a more than 3 levels fall.
        /// </summary>
        [UnityTest]
        public IEnumerator FallingScreamTest()
        {
            _cameraController.PlaceInRoom(_room02);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition4.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            float startingHeight = _prince.transform.position.y;
            float expectedFinalHeight = 0.32f;
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            Vector2 startPosition = _prince.transform.position;
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we have fallen enough.
            float currentHeight = _prince.transform.position.y;
            Assert.True((startingHeight - currentHeight) > 6f);
            // Assert Prince is screaming.
            FallingController fallingController = _prince.GetComponentInChildren<FallingController>();
            bool hasScreamed = AccessPrivateHelper.GetPrivateField<bool>(fallingController, "_alreadyScreamed");
            Assert.True(hasScreamed);
        }
        
        /// <summary>
        /// Test that Guard survives unharmed a 1 level fall.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardFallingOneLevelTest()
        {
            _cameraController.PlaceInRoom(_room00);
            // I dont want enemy to move or defend by itself.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _enemy.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            int startingHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            float expectedFinalHeight = 0.32f;
            string commandFile = @"Assets\Tests\TestResources\oneSwordHit";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert guard has fallen to where we expected.
            float endHeight = _enemy.transform.position.y;
            float heightError = endHeight - expectedFinalHeight;
            Assert.True(Math.Abs(heightError) < 0.04);
            // Assert guard keeps his life (minus 1 point because the hit we needed to make him fall.
            Assert.False(_enemy.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True((startingHealth - 1) == endHealth);
        }
        
        /// <summary>
        /// Test that Guard survives a 2 level fall but receives 1 additional hit.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardFallingTwoLevelsTest()
        {
            _cameraController.PlaceInRoom(_room00);
            // I dont want enemy to move or defend by itself.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _enemy.transform.SetPositionAndRotation(_startPosition6.transform.position, Quaternion.identity);
            int startingHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            float expectedFinalHeight = 0.32f;
            string commandFile = @"Assets\Tests\TestResources\oneSwordHit";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            // Assert guard has fallen to where we expected.
            float endHeight = _enemy.transform.position.y;
            float heightError = endHeight - expectedFinalHeight;
            Assert.True(Math.Abs(heightError) < 0.04);
            // Assert guard has lost 1 point because of fall (minus 1 point becuase the hit we needed to make him fall.
            Assert.False(_enemy.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True((startingHealth - 2) == endHealth);
        }
        
        /// <summary>
        /// Test that Guard dies if he falls more than three levels.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardFallingThreeLevelsTest()
        {
            _cameraController.PlaceInRoom(_room01);
            // I dont want enemy to move or defend by itself.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition3.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _enemy.transform.SetPositionAndRotation(_startPosition7.transform.position, Quaternion.identity);
            int startingHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            float expectedFinalHeight = 0.32f;
            string commandFile = @"Assets\Tests\TestResources\oneSwordHit";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(6);
            // Assert guard has fallen to where we expected.
            float endHeight = _enemy.transform.position.y;
            float heightError = endHeight - expectedFinalHeight;
            Assert.True(Math.Abs(heightError) < 0.04);
            // Assert guard is dead.
            Assert.True(_enemy.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        
        /// <summary>
        /// Test that Guard screams if he falls more than three levels.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardScreamTest()
        {
            _cameraController.PlaceInRoom(_room02);
            // I dont want enemy to move or defend by itself.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition4.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _enemy.transform.SetPositionAndRotation(_startPosition8.transform.position, Quaternion.identity);
            float startingHeight = _enemy.transform.position.y;
            float expectedFinalHeight = 0.32f;
            string commandFile = @"Assets\Tests\TestResources\oneSwordHit";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(6);
            // Assert we have fallen enough.
            float currentHeight = _enemy.transform.position.y;
            Assert.True((startingHeight - currentHeight) > 6f);
            // Assert guard is screaming.
            FallingController fallingController = _enemy.GetComponentInChildren<FallingController>();
            bool hasScreamed = AccessPrivateHelper.GetPrivateField<bool>(fallingController, "_alreadyScreamed");
            Assert.True(hasScreamed);
        }
    }
}
using System;
using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.PlayTests.Tools.Scene;

namespace Tests.PlayTests
{
    public class ClimbingTests
    {
        private GameObject _prince;
        private GameObject _enemy;
        private GameObject _potion1;
        private GameObject _potion2;

        private GameObject _startPosition1;
        private GameObject _startPosition2;
        private GameObject _startPosition3;
        private GameObject _startPosition4;
        private GameObject _startPosition5;
        private GameObject _startPosition6;
        private GameObject _startPosition7;
        private GameObject _startPosition8;
        private GameObject _startPosition12;
        private GameObject _startPosition13;
        private GameObject _startPosition16;
        private GameObject _startPosition17;
        private GameObject _startPosition18;
        private GameObject _startPosition19;
        private GameObject _startPosition20;
        private GameObject _startPosition21;
        private GameObject _startPosition22;
        private GameObject _startPosition23;
        private GameObject _startPosition24;
        private GameObject _startPosition25;
        private GameObject _startPosition30;
        private GameObject _startPosition31;
        private GameObject _startPosition32;

        private CameraController _cameraController;
        private Room _room00;
        private Room _room01;
        private Room _room02;
        private Room _room10;
        private Room _room20;

        private string _currentScene = "TheAbyss";

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);

            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_enemy == null) _enemy = GameObject.Find("Enemy");
            if (_potion1 == null) _potion1 = GameObject.Find("SmallPotion1");
            if (_potion2 == null) _potion2 = GameObject.Find("SmallPotion2");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            if (_startPosition2 == null) _startPosition2 = GameObject.Find("StartPosition2");
            if (_startPosition3 == null) _startPosition3 = GameObject.Find("StartPosition3");
            if (_startPosition4 == null) _startPosition4 = GameObject.Find("StartPosition4");
            if (_startPosition5 == null) _startPosition5 = GameObject.Find("StartPosition5");
            if (_startPosition6 == null) _startPosition6 = GameObject.Find("StartPosition6");
            if (_startPosition7 == null) _startPosition7 = GameObject.Find("StartPosition7");
            if (_startPosition8 == null) _startPosition8 = GameObject.Find("StartPosition8");
            if (_startPosition12 == null) _startPosition12 = GameObject.Find("StartPosition12");
            if (_startPosition13 == null) _startPosition13 = GameObject.Find("StartPosition13");
            if (_startPosition16 == null) _startPosition16 = GameObject.Find("StartPosition16");
            if (_startPosition17 == null) _startPosition17 = GameObject.Find("StartPosition17");
            if (_startPosition18 == null) _startPosition18 = GameObject.Find("StartPosition18");
            if (_startPosition19 == null) _startPosition19 = GameObject.Find("StartPosition19");
            if (_startPosition20 == null) _startPosition20 = GameObject.Find("StartPosition20");
            if (_startPosition21 == null) _startPosition21 = GameObject.Find("StartPosition21");
            if (_startPosition22 == null) _startPosition22 = GameObject.Find("StartPosition22");
            if (_startPosition23 == null) _startPosition23 = GameObject.Find("StartPosition23");
            if (_startPosition24 == null) _startPosition24 = GameObject.Find("StartPosition24");
            if (_startPosition25 == null) _startPosition25 = GameObject.Find("StartPosition25");
            if (_startPosition30 == null) _startPosition30 = GameObject.Find("StartPosition30");
            if (_startPosition31 == null) _startPosition31 = GameObject.Find("StartPosition31");
            if (_startPosition32 == null) _startPosition32 = GameObject.Find("StartPosition32");
            if (_cameraController == null)
                _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
            if (_room00 == null)
                _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();
            if (_room01 == null)
                _room01 = GameObject.Find("Room_0_1").GetComponentInChildren<Room>();
            if (_room02 == null)
                _room02 = GameObject.Find("Room_0_2").GetComponentInChildren<Room>();
            if (_room10 == null)
                _room10 = GameObject.Find("Room_1_0").GetComponentInChildren<Room>();
            if (_room20 == null)
                _room20 = GameObject.Find("Room_2_0").GetComponentInChildren<Room>();

            _prince.SetActive(false);
            _enemy.SetActive(false);
            _potion1.SetActive(false);
            _potion2.SetActive(false);

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
        /// Test we can climb one level.
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbingOneLevelTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition16.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition18.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\climbingOneLevel";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can climb two levels.
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbingTwoLevelsTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition16.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition17.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\climbingTwoLevels";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(8);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we have a chance to abort climbing just releasing jump button..
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbingAbortTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition16.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition16.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\climbingAborted";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can hang in a hollow brick and that we fall if we don´t complete climbing in a time.
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbingHangingHollowIncompleteTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition16.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition16.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\climbingKeepHanged";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still climbing.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(3);
            // Now Prince should have fallen to ground again.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can hang in a hollow brick and then let us fall.
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbingHangingHollowAbortedTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition16.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition16.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\climbingKeepHangedAndFall";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still climbing.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(2);
            // Now Prince should have fallen to ground again.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can hang in a not hollow brick and that we don`t fall if we don´t complete climbing in a time.
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbingHangingNotHollowIncompleteTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition20.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition20.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\climbingKeepHanged";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still climbing.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(3);
            // Now Prince should stay climbing.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is still at the same position..
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can hang in a not hollow brick and that we can leave us fall.
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbingHangingNotHollowAbortedTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition20.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition20.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\climbingKeepHangedAndFall";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still climbing.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(3);
            // Now Prince should have landed.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is still at the same position..
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can hang in a hollow brick and that we can climb from hanging.
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbingHangingHollowCompleteTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition16.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition18.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\climbingAfterHanged";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still climbing.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(4);
            // Now Prince should have climbed to upper ground.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can hang in a hollow brick and that we can climb from hanging. Everything should be done near a potion
        /// without taking it.
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbingHangingNearAPickableDoesNotTakeItTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _potion1.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition16.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition18.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\climbingAfterHanged";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still climbing.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(4);
            // Now Prince should have climbed to upper ground.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
            // Assert potion is still active.
            Assert.True(_potion1.activeSelf);
        }

        /// <summary>
        /// Test we can hang in a not hollow brick and that we can climb from hanging.
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbingHangingNotHollowCompleteTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition20.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition21.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\climbingAfterHanged";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still climbing.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(4);
            // Now Prince should have climbed to upper ground.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can descend one level.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendOneLevelTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition19.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendOneLevel";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can climb two levels.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendTwoLevelsTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition17.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition16.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendTwoLevels";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(7);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.30f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can keep hanged while descending but we fall after a while hanging from a hollow brick.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendHangingHollowIncompleteTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition19.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendKeepHanged";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still hanged.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(6);
            // Now Prince should have fallen to below ground.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can keep hanged while descending and after we can leave as fall.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendHangingHollowCompleteTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition19.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendKeepHangedAndFall";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still hanged.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(4);
            // Now Prince should have fallen to below ground.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can keep hanged while descending and after we can leave as fall. Everything should be done near a potion
        /// without taking it.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendHangingNearAPickableDoesNotTakeIt()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _potion2.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition19.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendKeepHangedAndFall";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still hanged.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(4);
            // Now Prince should have fallen to below ground.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
            // Assert potion is still active.
            Assert.True(_potion2.activeSelf);
        }

        /// <summary>
        /// Test we can keep hanged while descending a hollow brick and then we can climb up again.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendHangingHollowAbortedTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition5.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendKeepHangedAndClimb";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still hanged.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(5);
            // Now Prince should have climbed again to upper ground.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can keep hanged while descending and that we can keep hanging indefinitely from a not hollow brick.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendHangingNotHollowIncompleteTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition21.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition21.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendKeepHanged";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still hanged.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(3);
            // Now Prince should still be hanging. Actually this position is the same as the beginning
            // because descending has not completed.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can keep hanged while descending and then we can leave us fall.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendHangingNotHollowCompleteTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition21.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition20.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendKeepHangedAndFall";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still hanged.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while we let us fall.
            yield return new WaitForSeconds(4);
            // Now Prince should have landed.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can keep hanged while descending and that we can keep hanging indefinitely from a not hollow brick.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendHangingNotHollowAbortedTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition21.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition21.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendKeepHangedAndClimb";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert we are still hanged.
            Assert.True(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Let time pass while hanged.
            yield return new WaitForSeconds(4);
            // Now Prince should be back to the upper ground.
            Assert.False(_prince.GetComponentInChildren<ClimberInteractions>().ClimbingInProgress);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can hang while falling through one unit wide hole.
        /// </summary>
        [UnityTest]
        public IEnumerator HangingWhileFallingClimbingSuccessTest()
        {
            _cameraController.PlaceInRoom(_room02);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition22.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition23.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\hangingWhileFallingAndClimbing";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(7);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can hang while falling through one unit wide hole.
        /// </summary>
        [UnityTest]
        public IEnumerator HangingWhileRunningJumpingClimbingSuccessTest()
        {
            _cameraController.PlaceInRoom(_room02);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition12.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = true;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition13.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\hangingWhileRunningJumpingAndClimbing";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            // Assert Prince is hanging.
            Assert.True(princeStatus.CurrentState == CharacterStatus.States.Climbing);
            // Let prince end its climbing.
            yield return new WaitForSeconds(3);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x) < 0.50f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y) < 0.30f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test we can hang while falling through one unit wide hole, stay hang for a while and let Prince fall again.
        /// </summary>
        [UnityTest]
        public IEnumerator HangingWhileFallingButLetFallTest()
        {
            _cameraController.PlaceInRoom(_room02);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition22.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            float expectedLandingHeight = _startPosition19.transform.position.y;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\hangingWhileFallingAndFallingAgain";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(7);
            // Assert Prince is at expected position: splashed on ground.
            Assert.True(Math.Abs(expectedLandingHeight - _prince.transform.position.y) < 0.15f);
            // Assert Prince is dead.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
        }

        /// <summary>
        /// Test we can hang while falling through one unit wide hole, stay hang for a while and let Prince fall
        /// again through well of one unit wide.
        /// </summary>
        [UnityTest]
        public IEnumerator HangingWhileFallingButLetFallThroughNarrowHoleTest()
        {
            _cameraController.PlaceInRoom(_room20);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition24.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedPosition = _startPosition25.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\hangingWhileFallingAndFallingAgain";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(7);
            // Assert Prince is at expected position.
            Assert.True(Vector3.Distance(expectedPosition, _prince.transform.position) < 0.30f);
            // Assert Prince is alive and has not lost any life point.
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }

        /// <summary>
        /// Test that when we climb in a room border camera changes while hanging.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendHangingHollowAbortedChangesCameraTest()
        {
            _cameraController.PlaceInRoom(_room01);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition30.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            princeStatus.Life = 3;
            Vector3 expectedLandingPosition = _startPosition5.transform.position;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendKeepHangedAndClimb";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert camera now its at another room.
            Assert.True(_cameraController.CurrentRoom == _room00);
            // Let time pass while hanged.
            yield return new WaitForSeconds(5);
            // Now Prince should have climbed again to upper ground and camera should have changed back to initial room.
            Assert.True(_cameraController.CurrentRoom == _room01);
        }

        /// <summary>
        /// Test that when we descend in a room border camera changes camera to room below.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendHangingChangesCameraTest()
        {
            _cameraController.PlaceInRoom(_room02);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition32.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendOneLevel";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(6);
            // Assert camera now its at room below.
            Assert.True(_cameraController.CurrentRoom == _room01);
        }

        /// <summary>
        /// Test that when we descend in a room border camera changes camera to room below.
        /// </summary>
        [UnityTest]
        public IEnumerator DescendKeepHangingAndFallChangesCameraTest()
        {
            _cameraController.PlaceInRoom(_room02);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition32.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\descendKeepHangedAndFall";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(9);
            // Assert camera now its at room below.
            Assert.True(_cameraController.CurrentRoom == _room01);
        }

        /// <summary>
        /// Test that when we descend in a room border camera changes camera to room below.
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbChangesCameraTest()
        {
            _cameraController.PlaceInRoom(_room01);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition31.transform.position, Quaternion.identity);
            CharacterStatus princeStatus = _prince.GetComponentInChildren<CharacterStatus>();
            princeStatus.LookingRightWards = false;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\climbingOneLevel";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert camera now its at room above.
            Assert.True(_cameraController.CurrentRoom == _room02);
        }
    }
}
using System;
using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class JumpingTests : MonoBehaviour
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
        private GameObject _startPosition11;
        private GameObject _startPosition12;
        private GameObject _startPosition13;
        private GameObject _startPosition14;
        private GameObject _startPosition15;

        private CameraController _cameraController;
        private Room _room00;
        private Room _room01;
        private Room _room02;
        private Room _room10;

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
            if (_startPosition11 == null) _startPosition11 = GameObject.Find("StartPosition11");
            if (_startPosition12 == null) _startPosition12 = GameObject.Find("StartPosition12");
            if (_startPosition13 == null) _startPosition13 = GameObject.Find("StartPosition13");
            if (_startPosition14 == null) _startPosition14 = GameObject.Find("StartPosition14");
            if (_startPosition15 == null) _startPosition15 = GameObject.Find("StartPosition15");
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
        /// Test that running jumping can get over a 3 unit wide gap.
        /// </summary>
        [UnityTest]
        public IEnumerator RunningJumpingTest()
        {
            _cameraController.PlaceInRoom(_room02);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition11.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            float startingHeight = _prince.transform.position.y;
            float startingHorizontalPosition = _prince.transform.position.x;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            float expectedAdvancedHorizontalPosition = 10.8f;
            string commandFile = @"Assets\Tests\TestResources\runningJumpingSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Assert Prince has not fallen.
            float endHeight = _prince.transform.position.y;
            Assert.True(Math.Abs(endHeight - startingHeight)< 0.04f);
            // Assert Prince has advanced what we expected.
            float endHorizontalPosition = _prince.transform.position.x;
            float advancedHorizontalPosition = endHorizontalPosition - startingHorizontalPosition;
            Assert.True(Math.Abs(advancedHorizontalPosition - expectedAdvancedHorizontalPosition) < 0.15);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }
        
        /// <summary>
        /// Test that running jumping can get over a 4 unit wide gap if it land one level below.
        /// </summary>
        [UnityTest]
        public IEnumerator RunningJumpingFallingTest()
        {
            _cameraController.PlaceInRoom(_room01);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition12.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            Vector3 expectedLandingPosition = _startPosition13.transform.position;  
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\runningJumpingSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(6);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x)< 0.20f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y)< 0.20f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }
        
        /// <summary>
        /// Test that walking jumping can get over a 2 unit wide gap.
        /// </summary>
        [UnityTest]
        public IEnumerator WalkingJumpingTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition13.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            Vector3 expectedLandingPosition = _startPosition14.transform.position;  
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\walkingJumpingSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x)< 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y)< 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }
        
        /// <summary>
        /// Test that walking jumping can get over a 3 unit wide gap if it lands 2 levels below.
        /// </summary>
        [UnityTest]
        public IEnumerator WalkingJumpingFallingTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition14.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            Vector3 expectedLandingPosition = _startPosition15.transform.position;  
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            string commandFile = @"Assets\Tests\TestResources\walkingJumpingSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            // Assert Prince is at expected position.
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x)< 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y)< 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth - 1 == endHealth);
        }
        
        /// <summary>
        /// Test that vertical jumping advances horizontally some centimetres.
        /// </summary>
        [UnityTest]
        public IEnumerator VerticalJumpingTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition15.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            float startingHeight = _prince.transform.position.y;
            float startingHorizontalPosition = _prince.transform.position.x;
            float expectedAdvancedHorizontalPosition = 0.22f;
            string commandFile = @"Assets\Tests\TestResources\verticalJumpingSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Assert Prince has not fallen.
            float endHeight = _prince.transform.position.y;
            Assert.True(Math.Abs(endHeight - startingHeight)< 0.04f);
            // Assert Prince has advanced what we expected.
            float endHorizontalPosition = _prince.transform.position.x;
            float advancedHorizontalPosition = endHorizontalPosition - startingHorizontalPosition;
            Assert.True(Math.Abs(advancedHorizontalPosition - expectedAdvancedHorizontalPosition) < 0.05);
        }
    }
}
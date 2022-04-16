using System;
using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class ClimbingTests
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
        private GameObject _startPosition16;
        private GameObject _startPosition17;
        private GameObject _startPosition18;

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
            if (_startPosition16 == null) _startPosition16 = GameObject.Find("StartPosition16");
            if (_startPosition17 == null) _startPosition17 = GameObject.Find("StartPosition17");
            if (_startPosition18 == null) _startPosition18 = GameObject.Find("StartPosition18");
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
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x)< 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y)< 0.15f);
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
            Assert.True(Math.Abs(expectedLandingPosition.x - _prince.transform.position.x)< 0.15f);
            Assert.True(Math.Abs(expectedLandingPosition.y - _prince.transform.position.y)< 0.15f);
            // Assert Prince keeps his life.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }
    }
}
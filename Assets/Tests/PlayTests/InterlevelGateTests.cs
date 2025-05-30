﻿using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using Tests.PlayTests.Tools.Scene;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class Z_InterlevelGateTests
    {
        private GameObject _prince;
        private GameObject _enemy;

        private GameObject _startPosition1;
        private GameObject _startPosition2;
        private GameObject _startPosition3;
        private GameObject _startPosition4;
        private GameObject _startPosition5;
        

        private GameObject _portcullis;
        private GameObject _portcullis2;
        
        private CameraController _cameraController;
        private LevelLoader _levelLoader;
        private Room _room00;
        private Room _room10;
        private Room _room20;

        private string _currentScene = "TheDoors";

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);
            
            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_enemy == null) _enemy = GameObject.Find("Enemy");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            if (_cameraController == null)
                _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
            if (_levelLoader == null)
                _levelLoader = GameObject.Find("LevelLoader").GetComponentInChildren<LevelLoader>();
            if (_room00 == null)
                _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();

            _prince.SetActive(false);
            _enemy.SetActive(false);

            // yield return new EnterPlayMode();
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            // Remove GameManagers to avoid having multiple instances of it in the next test.
            GameObject.Destroy(GameObject.Find("GameManagers"));
            // yield return TestSceneManager.UnLoadScene(_currentScene);
            yield return null;
        }
        
        /// <summary>
        /// Test Prince can go to next level through an interlevel gate.
        /// </summary>
        [UnityTest]
        public IEnumerator CanGoThroughInterlevelGate()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            string expectedFinalLevelName = "doors1";
            string commandFile = @"Assets\Tests\TestResources\enterInterlevelGate";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(25);
            string endFinalLevelName = _levelLoader.CurrentSceneName;
            // Assert Prince has gone through inter level gate to desired level.
            Assert.True(expectedFinalLevelName == endFinalLevelName);
        }
        
    }
}
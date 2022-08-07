using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using Tests.PlayTests.Tools.Scene;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class InterlevelGateTests
    {
        private GameObject _prince;

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

        private string _currentScene = "TheTrap";

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);
            
            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_portcullis == null) _portcullis = GameObject.Find("Portcullis");
            if (_portcullis2 == null) _portcullis2 = GameObject.Find("Portcullis2");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            if (_startPosition2 == null) _startPosition2 = GameObject.Find("StartPosition2");
            if (_startPosition3 == null) _startPosition3 = GameObject.Find("StartPosition3");
            if (_startPosition4 == null) _startPosition4 = GameObject.Find("StartPosition4");
            if (_startPosition5 == null) _startPosition5 = GameObject.Find("StartPosition5");
            if (_cameraController == null)
                _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
            if (_levelLoader == null)
                _levelLoader = GameObject.Find("LevelLoader").GetComponentInChildren<LevelLoader>();
            if (_room00 == null)
                _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();
            if (_room10 == null)
                _room10 = GameObject.Find("Room_1_0").GetComponentInChildren<Room>();
            if (_room20 == null)
                _room20 = GameObject.Find("Room_2_0").GetComponentInChildren<Room>();

            _prince.SetActive(false);

            yield return new EnterPlayMode();
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return TestSceneManager.UnLoadScene(_currentScene);
        }
        
        /// <summary>
        /// Test Prince can go to next level through an interlevel gate.
        /// </summary>
        [UnityTest]
        public IEnumerator CanGoThroughInterlevelGate()
        {
            _cameraController.PlaceInRoom(_room20);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            string expectedFinalLevelName = "doors1";
            string commandFile = @"Assets\Tests\TestResources\enterInterlevelGate";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(21);
            string endFinalLevelName = _levelLoader.CurrentSceneName;
            // Assert Prince has not gone through inter level gate to desired level.
            Assert.True(expectedFinalLevelName == endFinalLevelName);
        }
    }
}
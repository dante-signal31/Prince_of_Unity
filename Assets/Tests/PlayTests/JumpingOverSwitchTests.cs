using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using Tests.PlayTests.Tools.Scene;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class JumpingOverSwitchTests
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
        private InterlevelGateStatus _gateStatus;
        private Room _room00;
        private Room _room10;
        private Room _room20;

        private string _currentScene = "TheDoors";

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);
            
            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            if (_startPosition2 == null) _startPosition2 = GameObject.Find("StartPosition2");
            if (_cameraController == null)
                _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
            if (_levelLoader == null)
                _levelLoader = GameObject.Find("LevelLoader").GetComponentInChildren<LevelLoader>();
            if (_gateStatus == null)
                _gateStatus = GameObject.Find("InterlevelGate").GetComponentInChildren<InterlevelGateStatus>();
            if (_room00 == null)
                _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();

            _prince.SetActive(false);

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
        /// Test Prince can run-jump over a switch without activating it.
        /// </summary>
        [UnityTest]
        public IEnumerator WalkingJumpingOverSwitchDoesNotActivateItTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            string expectedFinalLevelName = "doors1";
            string commandFile = @"Assets\Tests\TestResources\walkingJumpingSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            Assert.True(_gateStatus.CurrentState == InterlevelGateStatus.GateStates.Closed);
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            Assert.True(_gateStatus.CurrentState == InterlevelGateStatus.GateStates.Closed);
        }
        
        /// <summary>
        /// Test Prince walk-jump over a switch without activating it.
        /// </summary>
        [UnityTest]
        public IEnumerator RunningJumpingOverSwitchDoesNotActivateItTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            string expectedFinalLevelName = "doors1";
            string commandFile = @"Assets\Tests\TestResources\runningJumpingLeftSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            Assert.True(_gateStatus.CurrentState == InterlevelGateStatus.GateStates.Closed);
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            Assert.True(_gateStatus.CurrentState == InterlevelGateStatus.GateStates.Closed);
        }
        
    }
}
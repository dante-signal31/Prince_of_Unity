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
    public class PortcullisTests
    {
        private GameObject _prince;
        private GameObject _enemy;

        private GameObject _startPosition1;
        private GameObject _startPosition2;
        private GameObject _startPosition3;
        private GameObject _startPosition4;
        private GameObject _startPosition17;
        private GameObject _startPosition18;
        private GameObject _startPosition19;

        private GameObject _portcullis;
        private GameObject _portcullis2;
        private GameObject _portcullis3;
        private GameObject _openingSwitch;
        
        private CameraController _cameraController;
        private Room _room00;
        private Room _room01;
        private Room _room10;
        // private Room _room02;

        private string _currentScene = "TheTrap";

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);
            
            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_enemy == null) _enemy = GameObject.Find("Enemy");
            if (_portcullis == null) _portcullis = GameObject.Find("Portcullis");
            if (_portcullis2 == null) _portcullis2 = GameObject.Find("Portcullis2");
            if (_portcullis3 == null) _portcullis3 = GameObject.Find("Portcullis3");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            if (_startPosition2 == null) _startPosition2 = GameObject.Find("StartPosition2");
            if (_startPosition3 == null) _startPosition3 = GameObject.Find("StartPosition3");
            if (_startPosition4 == null) _startPosition4 = GameObject.Find("StartPosition4");
            if (_startPosition17 == null) _startPosition17 = GameObject.Find("StartPosition17");
            if (_startPosition18 == null) _startPosition18 = GameObject.Find("StartPosition18");
            if (_startPosition19 == null) _startPosition19 = GameObject.Find("StartPosition19");
            if (_cameraController == null)
                _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
            if (_openingSwitch == null)
                _openingSwitch = GameObject.Find("OpeningSwitch (2)");
            if (_room00 == null)
                _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();
            if (_room01 == null)
                _room01 = GameObject.Find("Room_0_1").GetComponentInChildren<Room>();
            if (_room10 == null)
                _room10 = GameObject.Find("Room_1_0").GetComponentInChildren<Room>();

            _prince.SetActive(false);
            _enemy.SetActive(false);

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
        /// Test Prince can not run through a closed portcullis.
        /// </summary>
        [UnityTest]
        public IEnumerator CannotGoThroughClosedPortcullisTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            Vector3 expectedFinalPosition = _startPosition2.transform.position;
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Vector3 endPosition = _prince.transform.position;
            // Assert Prince has not gone through portcullis.
            Assert.True(Vector3.Distance(expectedFinalPosition, endPosition) < 0.50);
        }
        
        /// <summary>
        /// Test Prince can run through an open portcullis.
        /// </summary>
        [UnityTest]
        public IEnumerator CanGoThroughOpenPortcullisTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            Vector3 expectedFinalPosition = _startPosition3.transform.position;
            string commandFile = @"Assets\Tests\TestResources\openPortcullisSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(8);
            Vector3 endPosition = _prince.transform.position;
            // Assert Prince has gone through portcullis.
            Assert.True(Vector3.Distance(expectedFinalPosition, endPosition) < 0.30);
        }
        
        /// <summary>
        /// Test Prince can not climb through a closed portcullis.
        /// </summary>
        [UnityTest]
        public IEnumerator CannotClimbThroughClosedPortcullisTest()
        {
            _cameraController.PlaceInRoom(_room01);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition17.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            PortcullisStatus portcullisStatus = _portcullis3.GetComponentInChildren<PortcullisStatus>();
            AccessPrivateHelper.SetPrivateField(_portcullis3.GetComponentInChildren<PortcullisStatus>(), "initialState", PortcullisStatus.PortcullisStates.Closed);
            AccessPrivateHelper.AccessPrivateMethod(portcullisStatus, "SetInitialState");
            Vector3 expectedFinalPosition = _startPosition17.transform.position;
            string commandFile = @"Assets\Tests\TestResources\climbingOneLevel";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            Vector3 endPosition = _prince.transform.position;
            // Assert Prince has not gone through portcullis.
            Assert.True(Vector3.Distance(expectedFinalPosition, endPosition) < 0.50);
        }
        
        /// <summary>
        /// Test Prince can climb through a open portcullis.
        /// </summary>
        [UnityTest]
        public IEnumerator CanClimbThroughOpenPortcullisTest()
        {
            _cameraController.PlaceInRoom(_room01);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition17.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            PortcullisStatus portcullisStatus = _portcullis3.GetComponentInChildren<PortcullisStatus>();
            AccessPrivateHelper.SetPrivateField(portcullisStatus, "initialState", PortcullisStatus.PortcullisStates.Open);
            AccessPrivateHelper.AccessPrivateMethod(portcullisStatus, "SetInitialState");
            yield return null;
            Vector3 expectedFinalPosition = _startPosition18.transform.position;
            string commandFile = @"Assets\Tests\TestResources\climbingOneLevel";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            Vector3 endPosition = _prince.transform.position;
            // Assert Prince has not gone through portcullis.
            Assert.True(Vector3.Distance(expectedFinalPosition, endPosition) < 0.50);
        }
        
        /// <summary>
        /// Test Prince can climb through a portcullis closing slow if he is quick enough.
        /// </summary>
        [UnityTest]
        public IEnumerator CanClimbThroughClosingSlowPortcullisTest()
        {
            _cameraController.PlaceInRoom(_room01);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition17.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            PortcullisStatus portcullisStatus = _portcullis3.GetComponentInChildren<PortcullisStatus>();
            AccessPrivateHelper.SetPrivateField(portcullisStatus, "initialState", PortcullisStatus.PortcullisStates.Open);
            AccessPrivateHelper.AccessPrivateMethod(portcullisStatus, "SetInitialState");
            yield return null;
            Vector3 expectedFinalPosition = _startPosition18.transform.position;
            string commandFile = @"Assets\Tests\TestResources\climbingOneLevel";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return new WaitForSeconds(8);
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            Vector3 endPosition = _prince.transform.position;
            // Assert Prince has not gone through portcullis.
            Assert.True(Vector3.Distance(expectedFinalPosition, endPosition) < 0.50);
        }
        
        /// <summary>
        /// Test Prince can climb through a portcullis closing slow if he is not quick enough.
        /// </summary>
        [UnityTest]
        public IEnumerator CannotClimbThroughTooClosedPortcullisTest()
        {
            _cameraController.PlaceInRoom(_room01);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition17.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            PortcullisStatus portcullisStatus = _portcullis3.GetComponentInChildren<PortcullisStatus>();
            AccessPrivateHelper.SetPrivateField(portcullisStatus, "initialState", PortcullisStatus.PortcullisStates.Open);
            AccessPrivateHelper.AccessPrivateMethod(portcullisStatus, "SetInitialState");
            yield return null;
            Vector3 expectedFinalPosition = _startPosition17.transform.position;
            string commandFile = @"Assets\Tests\TestResources\climbingOneLevel";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return new WaitForSeconds(12);
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(5);
            Vector3 endPosition = _prince.transform.position;
            // Assert Prince has not gone through portcullis.
            Assert.True(Vector3.Distance(expectedFinalPosition, endPosition) < 0.50);
        }
        
        /// <summary>
        /// Test Prince can fast close a portcullis.
        /// </summary>
        [UnityTest]
        public IEnumerator QuicklyClosePortcullisTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            PortcullisStatus portcullisStatus = _portcullis.GetComponentInChildren<PortcullisStatus>();
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            Vector3 expectedFinalPosition = _startPosition2.transform.position;
            string commandFile = @"Assets\Tests\TestResources\openAndClosePortcullisSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            Assert.True(portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.Closed);
            yield return new WaitForSeconds(3);
            Assert.True(portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.Opening);
            yield return new WaitForSeconds(3);
            Assert.True(portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.Closed);
        }
        
        /// <summary>
        /// Test Prince can open a portcullis hanging from a switch.
        /// </summary>
        [UnityTest]
        public IEnumerator OpenPortcullisHangingTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _prince.SetActive(true);
            PortcullisStatus portcullisStatus = _portcullis2.GetComponentInChildren<PortcullisStatus>();
            _prince.transform.SetPositionAndRotation(_startPosition4.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            string commandFile = @"Assets\Tests\TestResources\climbingKeepHanged";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            Assert.True(portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.Closed);
            yield return new WaitForSeconds(3);
            Assert.True(portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.Opening);
            yield return new WaitForSeconds(3);
            Assert.True(portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.Open);
        }
        
        /// <summary>
        /// Test Prince can jump from a switch and that switch deactivated.
        /// </summary>
        [UnityTest]
        public IEnumerator JumpFromSwitchDeactivatesItTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _prince.SetActive(true);
            SwitchStatus switchStatus = _openingSwitch.GetComponentInChildren<SwitchStatus>();
            _prince.transform.SetPositionAndRotation(_startPosition19.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            string commandFile = @"Assets\Tests\TestResources\walkingJumpingSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(0.5f);
            Assert.True(switchStatus.CurrentState == SwitchStatus.States.Activated);
            yield return new WaitForSeconds(4);
            Assert.True(switchStatus.CurrentState == SwitchStatus.States.Idle);
        }
    }
}
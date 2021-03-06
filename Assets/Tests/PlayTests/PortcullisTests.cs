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

        private GameObject _startPosition1;
        private GameObject _startPosition2;
        private GameObject _startPosition3;
        private GameObject _startPosition4;

        private GameObject _portcullis;
        private GameObject _portcullis2;
        
        private CameraController _cameraController;
        private Room _room00;
        private Room _room10;
        // private Room _room02;

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
            if (_cameraController == null)
                _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
            if (_room00 == null)
                _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();
            if (_room10 == null)
                _room10 = GameObject.Find("Room_1_0").GetComponentInChildren<Room>();

            _prince.SetActive(false);

            yield return new EnterPlayMode();
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
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
    }
}
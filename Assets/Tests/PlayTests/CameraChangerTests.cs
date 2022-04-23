using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class CameraChangerTests
    {
        private GameObject _prince;
        private GameObject _enemy;
        private GameObject _levelCamera;
        
        private GameObject _startPosition1;
        private GameObject _startPosition2;
        private GameObject _startPosition3;
        private GameObject _startPosition4;
        private GameObject _startPosition9;
        private GameObject _startPosition10;
        
        private string _currentScene = "TheAbyss";
        
        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);
            
            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_enemy == null) _enemy = GameObject.Find("Enemy");
            if (_levelCamera == null) _levelCamera = GameObject.Find("LevelCamera");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            if (_startPosition2 == null) _startPosition2 = GameObject.Find("StartPosition2");
            if (_startPosition3 == null) _startPosition3 = GameObject.Find("StartPosition3");
            if (_startPosition4 == null) _startPosition4 = GameObject.Find("StartPosition4");
            if (_startPosition9 == null) _startPosition9 = GameObject.Find("StartPosition9");
            if (_startPosition10 == null) _startPosition10 = GameObject.Find("StartPosition10");
            
            _prince.SetActive(false);

            yield return new EnterPlayMode();
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return TestSceneManager.UnLoadScene(_currentScene);
        }
        
        // [Test]
        // public void CameraChangerTestsSimplePasses()
        // {
        //     // Use the Assert class to test conditions.
        //     
        // }

        /// <summary>
        /// Check camera moves to right room properly.
        /// </summary>
        [UnityTest]
        public IEnumerator CameraChangerHorizontalToRightTest()
        {
            _prince.SetActive(true);
            _enemy.SetActive(false);
            _prince.transform.SetPositionAndRotation(_startPosition9.transform.position, Quaternion.identity);
            Room room00 = GameObject.Find("Room_0_0").GetComponent<Room>();
            Room room10 = GameObject.Find("Room_1_0").GetComponent<Room>();
            CameraController cameraController = _levelCamera.GetComponentInChildren<CameraController>();
            cameraController.PlaceInRoom(room00);
            // Check starting conditions are right.
            Assert.True(cameraController.CurrentRoom.Name == "Room_0_0");
            Assert.True(room00.IsActiveRoom());
            Assert.False(room10.IsActiveRoom());
            // Prepare input sequence replay.
            string commandFile = @"Assets\Tests\TestResources\changingScreenToRight";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Check everything ended as we expected.
            Assert.True(cameraController.CurrentRoom.Name == "Room_1_0");
            Assert.False(room00.IsActiveRoom());
            Assert.True(room10.IsActiveRoom());
        }
        
        /// <summary>
        /// Check camera moves to left room properly.
        /// </summary>
        [UnityTest]
        public IEnumerator CameraChangerHorizontalToLeftTest()
        {
            _prince.SetActive(true);
            _enemy.SetActive(false);
            _prince.transform.SetPositionAndRotation(_startPosition10.transform.position, Quaternion.identity);
            // _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            Room room00 = GameObject.Find("Room_0_0").GetComponent<Room>();
            Room room10 = GameObject.Find("Room_1_0").GetComponent<Room>();
            CameraController cameraController = _levelCamera.GetComponentInChildren<CameraController>();
            cameraController.PlaceInRoom(room10);
            // Check starting conditions are right.
            Assert.True(cameraController.CurrentRoom.Name == "Room_1_0");
            Assert.False(room00.IsActiveRoom());
            Assert.True(room10.IsActiveRoom());
            // Prepare input sequence replay.
            string commandFile = @"Assets\Tests\TestResources\changingScreenToLeft";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            // Check everything ended as we expected.
            Assert.True(cameraController.CurrentRoom.Name == "Room_0_0");
            Assert.True(room00.IsActiveRoom());
            Assert.False(room10.IsActiveRoom());
        }
        
        /// <summary>
        /// Check camera moves to below rooms properly when falling.
        /// </summary>
        [UnityTest]
        public IEnumerator CameraChangerVerticalTest()
        {
            _prince.SetActive(true);
            _enemy.SetActive(false);
            _prince.transform.SetPositionAndRotation(_startPosition4.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            Room room00 = GameObject.Find("Room_0_0").GetComponent<Room>();
            Room room01 = GameObject.Find("Room_0_1").GetComponent<Room>();
            Room room02 = GameObject.Find("Room_0_2").GetComponent<Room>();
            CameraController cameraController = _levelCamera.GetComponentInChildren<CameraController>();
            cameraController.PlaceInRoom(room02);
            // Check starting conditions are right.
            Assert.True(cameraController.CurrentRoom.Name == "Room_0_2");
            Assert.False(room00.IsActiveRoom());
            Assert.False(room01.IsActiveRoom());
            Assert.True(room02.IsActiveRoom());
            // Prepare input sequence replay.
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3f);
            // Check state in the middle of the fall.
            Assert.True(cameraController.CurrentRoom.Name == "Room_0_1");
            Assert.False(room00.IsActiveRoom());
            Assert.True(room01.IsActiveRoom());
            Assert.False(room02.IsActiveRoom());
            // Let Prince fall further.
            yield return new WaitForSeconds(2f);
            // Check everything ended as we expected.
            Assert.True(cameraController.CurrentRoom.Name == "Room_0_0");
            Assert.True(room00.IsActiveRoom());
            Assert.False(room01.IsActiveRoom());
            Assert.False(room02.IsActiveRoom());
        }
    }
}
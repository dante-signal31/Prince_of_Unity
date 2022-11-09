using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using Tests.PlayTests.Tools.Scene;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class TestCheatKeys
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
        private EventBus _eventBus;
        private GameTimer _gameTimer;
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
            if (_cameraController == null)
                _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
            if (_levelLoader == null)
                _levelLoader = GameObject.Find("LevelLoader").GetComponentInChildren<LevelLoader>();
            if (_eventBus == null)
                _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
            if (_gameTimer == null)
                _gameTimer = GameObject.Find("GameTimer").GetComponentInChildren<GameTimer>();
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
            yield return TestSceneManager.UnLoadCurrentScene();
        }

        /// <summary>
        /// Test Prince can use cheat key to skip level.
        /// </summary>
        [UnityTest]
        public IEnumerator CanSkipLevel()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            string expectedFinalLevelName = "doors1";
            string commandFile = @"Assets\Tests\TestResources\useSkipLevelCheatKey";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            string endFinalLevelName = _levelLoader.CurrentSceneName;
            // Assert Prince has not gone through inter level gate to desired level.
            Assert.True(expectedFinalLevelName == endFinalLevelName);
        }
        
        /// <summary>
        /// Test Prince can use cheat key to pause play.
        /// </summary>
        [UnityTest]
        public IEnumerator CanPauseGame()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            yield return null;
            // I have to launch level loaded event to force game timer to activate.
            _eventBus.TriggerEvent(new GameEvents.LevelLoaded(_currentScene), this);
            string expectedFinalLevelName = "doors1";
            string commandFile = @"Assets\Tests\TestResources\usePauseKey";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            yield return new WaitForSecondsRealtime(2);
            // Game is paused.
            float startingTime = _gameTimer.ElapsedSeconds;
            // Let time pass.
            yield return new WaitForSecondsRealtime(2);
            float currentTime = _gameTimer.ElapsedSeconds;
            // As game is still paused, elapsed time should not have been incremented.
            Assert.True(startingTime == currentTime);
            yield return new WaitForSecondsRealtime(3);
            currentTime = _gameTimer.ElapsedSeconds;
            // As game has been unpaused, so elapsed time should have been incremented.
            Assert.True(startingTime < currentTime);
        }
        
        
        /// <summary>
        /// Test Prince can use cheat key to increment life and maximum life.
        /// </summary>
        [UnityTest]
        public IEnumerator CanUseLifeCheatKeys()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            yield return null;
            int startingLife = _prince.GetComponentInChildren<CharacterStatus>().Life;
            int startingMaximumLife = _prince.GetComponentInChildren<CharacterStatus>().MaximumLife;
            // I have to launch level loaded event to force game timer to activate.
            _eventBus.TriggerEvent(new GameEvents.LevelLoaded(_currentScene), this);
            string expectedFinalLevelName = "doors1";
            string commandFile = @"Assets\Tests\TestResources\useLifeCheatKeys";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            yield return new WaitForSecondsRealtime(2);
            // Only maximum life value should have increased.
            int currentLife = _prince.GetComponentInChildren<CharacterStatus>().Life;
            int currentMaximumLife = _prince.GetComponentInChildren<CharacterStatus>().MaximumLife;
            Assert.True(startingLife == currentLife);
            Assert.True(startingMaximumLife + 1 == currentMaximumLife);
            // Let time pass.
            yield return new WaitForSecondsRealtime(2);
            // Now only current value should have incremented.
            currentLife = _prince.GetComponentInChildren<CharacterStatus>().Life;
            currentMaximumLife = _prince.GetComponentInChildren<CharacterStatus>().MaximumLife;
            Assert.True(startingLife + 1 == currentLife);
            Assert.True(startingMaximumLife + 1 == currentMaximumLife);
        }
        
        /// <summary>
        /// Test Prince can use cheat key to increase time.
        /// </summary>
        [UnityTest]
        public IEnumerator CanIncreaseTime()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            yield return null;
            // I have to launch level loaded event to force game timer to activate.
            _eventBus.TriggerEvent(new GameEvents.LevelLoaded(_currentScene), this);
            yield return new WaitForSecondsRealtime(1);
            float startingElapsedTime = GameObject.Find("GameManagers").GetComponentInChildren<GameTimer>().ElapsedSeconds;
            string commandFile = @"Assets\Tests\TestResources\useIncreaseTimeCheatKeys";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            yield return new WaitForSecondsRealtime(1);
            // Only maximum life value should have increased.
            float currentElapsedTime = GameObject.Find("GameManagers").GetComponentInChildren<GameTimer>().ElapsedSeconds;
            Assert.True(currentElapsedTime < startingElapsedTime);
        }
        
        /// <summary>
        /// Test Prince can use cheat key to decrease time.
        /// </summary>
        [UnityTest]
        public IEnumerator CanDecreaseTime()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            yield return null;
            // I have to launch level loaded event to force game timer to activate.
            _eventBus.TriggerEvent(new GameEvents.LevelLoaded(_currentScene), this);
            yield return new WaitForSecondsRealtime(1);
            // float startingElapsedTime = GameObject.Find("GameManagers").GetComponentInChildren<GameTimer>().ElapsedSeconds;
            string commandFile = @"Assets\Tests\TestResources\useDecreaseTimeCheatKeys";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            yield return new WaitForSecondsRealtime(2);
            // Only maximum life value should have increased.
            float currentElapsedTime = GameObject.Find("GameManagers").GetComponentInChildren<GameTimer>().ElapsedSeconds;
            Assert.True(currentElapsedTime >= 60);
            Assert.True(currentElapsedTime < 120);
        }
    }
}
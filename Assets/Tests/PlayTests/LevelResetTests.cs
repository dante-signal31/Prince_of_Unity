using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using Tests.PlayTests.Tools.Scene;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class LevelResetTests
    {
        private GameObject _prince;

        private CameraController _cameraController;
        private EventBus _eventBus;
        private GameObject _startPosition1;
        private LevelLoader _levelLoader;
        private Room _room00;

        private float _startLevelElapsedTime;
        private float _reloadLevelElapsedTime;
        private bool _levelReloaded = false;

        private string _currentScene = "DoorToTrap";

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);
            
            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_startPosition1 == null) _startPosition1 = GameObject.Find("StartPosition1");
            // if (_cameraController == null)
            //     _cameraController= GameObject.Find("CameraController").GetComponentInChildren<CameraController>();
            if (_levelLoader == null)
                _levelLoader = GameObject.Find("LevelLoader").GetComponentInChildren<LevelLoader>();
            if (_eventBus == null)
                _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
            // if (_room00 == null)
            //     _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();
        
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
        /// Test level is reloaded after Prince death with Prince starting stats a that level..
        /// </summary>
        [UnityTest]
        public IEnumerator ReloadedLevelGetsStartingStatsTest()
        {
            // _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _prince.GetComponentInChildren<CharacterStatus>().MaximumLife = 5;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 2;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            // Register listener to take note of elapsed time when "The room with the trap" is started.
            _eventBus.AddListener<GameEvents.LevelLoaded>(OnLevelLoaded);
            _eventBus.AddListener<GameEvents.LevelReloaded>(OnLevelReloaded);
            string expectedFinalLevelName = "The room with the trap";
            // Commands to enter the door.
            string commandFile = @"Assets\Tests\TestResources\runToTheTrapRoom";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            // As we are not taking a sword we must launch SwordTaken manually to male PrinceStatus update.
            _eventBus.TriggerEvent(new GameEvents.SwordTaken(), this);
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(18);
            string endFinalLevelName = _levelLoader.CurrentSceneName;
            // Assert Prince has gone through inter level gate to desired level.
            Assert.True(expectedFinalLevelName == endFinalLevelName);
            // Level has changed, so some references should be refreshed.
            _prince = GameObject.Find("Prince");
            // // Commands to run to trap.
            commandFile = @"Assets\Tests\TestResources\runningToTrap";
            inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            yield return new WaitForSeconds(3);
            // Assert Prince is dead.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            // Wait to level be restarted.
            yield return new WaitUntil(() => _levelReloaded);
            // Level has changed, so some references should be refreshed.
            _prince = GameObject.Find("Prince");
            // Assert Prince is alive again, with the starting elapsed time, at last level and with starting life.
            _reloadLevelElapsedTime = GameObject.Find("GameManagers").GetComponentInChildren<GameTimer>().ElapsedSeconds;
            yield return new WaitForSeconds(2);
            Assert.True(Mathf.Abs(_startLevelElapsedTime - _reloadLevelElapsedTime) < 0.10f);
            Assert.True(expectedFinalLevelName == endFinalLevelName);
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().MaximumLife == 5);
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().Life == 2);
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().HasSword);
        }

        private void OnLevelLoaded(object _, GameEvents.LevelLoaded __)
        {
            _startLevelElapsedTime = GameObject.Find("GameManagers").GetComponentInChildren<GameTimer>().ElapsedSeconds;
        }

        private void OnLevelReloaded(object _, GameEvents.LevelReloaded __)
        {
            _levelReloaded = true;
        }
    }
}
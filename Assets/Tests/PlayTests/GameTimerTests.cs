using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using Tests.PlayTests.Tools.Scene;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class GameTimerTests
    {
        private GameObject _prince;
        private GameObject _enemy;

        private CameraController _cameraController;
        private LevelLoader _levelLoader;
        private GameConfiguration _gameConfiguration;
        private HUDManager _hudManager;
        private Room _room00;

        private string _currentScene = "TheTrap";

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);
            
            if (_prince == null) _prince = GameObject.Find("Prince");
            if (_enemy == null) _enemy = GameObject.Find("Enemy");
            if (_cameraController == null)
                _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
            if (_levelLoader == null)
                _levelLoader = GameObject.Find("LevelLoader").GetComponentInChildren<LevelLoader>();
            if (_gameConfiguration == null)
                _gameConfiguration = GameObject.Find("GameManagers").GetComponentInChildren<GameConfiguration>();
            if (_hudManager== null)
                _hudManager = GameObject.Find("HUDManager").GetComponentInChildren<HUDManager>();
            if (_room00 == null)
                _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();

            _prince.SetActive(false);

            yield return new EnterPlayMode();
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return TestSceneManager.UnLoadScene(_currentScene);
        }

        // Test that guard is killed by blades trap if walks over it.
        [UnityTest]
        public IEnumerator GameTimerTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(false);
            AccessPrivateHelper.SetPrivateField(_gameConfiguration, "gameTotalTime", 10);
            // Let movement happen.
            yield return new WaitForSeconds(1);
            Assert.True(_hudManager.CurrentMessage == "Start message");
            yield return new WaitForSeconds(4);
            Assert.True(_hudManager.CurrentMessage == "Middle game message");
            yield return new WaitForSeconds(5);
            Assert.True(_hudManager.CurrentMessage == "End game message");
        }
    }
}
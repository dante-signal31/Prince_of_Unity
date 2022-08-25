using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using Tests.PlayTests.Tools.Scene;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class PickableTests
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
        private GameObject _startPosition9;
        private GameObject _startPosition10;
        private GameObject _startPosition11;
        private GameObject _startPosition12;
        private GameObject _startPosition13;
        private GameObject _startPosition14;
        private GameObject _startPosition15;

        private CameraController _cameraController;
        private LevelLoader _levelLoader;
        private Room _room00;
        private Room _room10;
        private Room _room20;
        private Room _room21;

        private string _currentScene = "TheTrap";

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
            if (_startPosition9 == null) _startPosition9 = GameObject.Find("StartPosition9");
            if (_startPosition10 == null) _startPosition10 = GameObject.Find("StartPosition10");
            if (_startPosition11 == null) _startPosition11 = GameObject.Find("StartPosition11");
            if (_startPosition12 == null) _startPosition12 = GameObject.Find("StartPosition12");
            if (_startPosition13 == null) _startPosition13 = GameObject.Find("StartPosition13");
            if (_startPosition14 == null) _startPosition14 = GameObject.Find("StartPosition14");
            if (_startPosition15 == null) _startPosition15 = GameObject.Find("StartPosition15");
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
            if (_room21 == null)
                _room21 = GameObject.Find("Room_2_1").GetComponentInChildren<Room>();

            _prince.SetActive(false);

            yield return new EnterPlayMode();
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return TestSceneManager.UnLoadScene(_currentScene);
        }

        /// <summary>
        /// Tests that taking a potion restores one life point.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator SmallPotionTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition13.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _prince.GetComponentInChildren<CharacterStatus>().MaximumLife = 4;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _enemy.SetActive(false);
            int initial_life = _prince.GetComponentInChildren<CharacterStatus>().Life;
            int initial_maximum_life = _prince.GetComponentInChildren<CharacterStatus>().MaximumLife;
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\unsheathe";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(4);
            // Assert Prince incremented his life.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().Life == initial_life + 1);
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().MaximumLife == initial_maximum_life);
        }
        
        /// <summary>
        /// Tests that taking a potion restores increments its maximum life and restores all his points.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator BigPotionTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition15.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _prince.GetComponentInChildren<CharacterStatus>().MaximumLife = 4;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _enemy.SetActive(false);
            int initial_maximum_life = _prince.GetComponentInChildren<CharacterStatus>().MaximumLife;
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\unsheathe";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(4);
            // Assert Prince incremented his life.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().Life == initial_maximum_life + 1);
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().MaximumLife == initial_maximum_life + 1);
        }
        
        /// <summary>
        /// Tests that taking a potion restores increments its maximum life and restores all his points.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator SwordTakingTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition14.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = false;
            _enemy.SetActive(false);
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\unsheathe";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(4);
            // Assert Prince incremented his life.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().HasSword);
        }
    }
}
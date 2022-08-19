using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Scene;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class TrapTests
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

        // Test that guard is killed by blades trap.
        [UnityTest]
        public IEnumerator GuardKilledByBladesTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition6.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            yield return new WaitForSeconds(1);
            _enemy.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition7.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            // I want enemy to move.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            // Let movement happen.
            yield return new WaitForSeconds(3);
            // Assert Guard is dead.
            Assert.True(_enemy.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        // Test that guard is killed by spikes trap.
        [UnityTest]
        public IEnumerator GuardKilledBySpikesTest()
        {
            _cameraController.PlaceInRoom(_room20);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition8.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _enemy.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition9.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            // I want enemy to move.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            // Let movement happen.
            yield return new WaitForSeconds(3);
            // Assert Guard is dead.
            Assert.True(_enemy.GetComponentInChildren<CharacterStatus>().IsDead);
        }
    }
}
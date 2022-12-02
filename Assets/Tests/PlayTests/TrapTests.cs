using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
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
        private GameObject _startPosition10;
        private GameObject _startPosition11;
        private GameObject _startPosition12;
        private GameObject _startPosition16;
        private GameObject _startPosition20;
        private GameObject _startPosition21;

        private CameraController _cameraController;
        private LevelLoader _levelLoader;
        private Room _room00;
        private Room _room01;
        private Room _room10;
        private Room _room11;
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
            if (_startPosition16 == null) _startPosition16 = GameObject.Find("StartPosition16");
            if (_startPosition20 == null) _startPosition20 = GameObject.Find("StartPosition20");
            if (_startPosition21 == null) _startPosition21 = GameObject.Find("StartPosition21");
            if (_cameraController == null)
                _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
            if (_levelLoader == null)
                _levelLoader = GameObject.Find("LevelLoader").GetComponentInChildren<LevelLoader>();
            if (_room00 == null)
                _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();
            if (_room01 == null)
                _room01 = GameObject.Find("Room_0_1").GetComponentInChildren<Room>();
            if (_room10 == null)
                _room10 = GameObject.Find("Room_1_0").GetComponentInChildren<Room>();
            if (_room11 == null)
                _room11 = GameObject.Find("Room_1_1").GetComponentInChildren<Room>();
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
            // Remove GameManagers to avoid having multiple instances of it in the next test.
            GameObject.Destroy(GameObject.Find("GameManagers"));
            yield return TestSceneManager.UnLoadScene(_currentScene);
        }
 
        // Test that guard is killed by blades trap if walks over it.
        [UnityTest]
        public IEnumerator GuardKilledByBladesTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition6.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _enemy.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition7.transform.position, Quaternion.identity);
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
        
        // Test that guard is killed by spikes trap if walks over it.
        [UnityTest]
        public IEnumerator GuardKilledBySpikesTest()
        {
            _cameraController.PlaceInRoom(_room20);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition8.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _enemy.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition9.transform.position, Quaternion.identity);
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
        
        // Test that guard is killed by spikes trap if falls over it.
        [UnityTest]
        public IEnumerator GuardKilledIfFallsOverSpikesTest()
        {
            _cameraController.PlaceInRoom(_room21);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition10.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition11.transform.position, Quaternion.identity);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            // I don't want enemy to move.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            // yield return new WaitForSeconds(1);
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\oneSwordHit";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(5);
            // Assert Guard is dead.
            Assert.True(_enemy.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        // Test that Prince is killed by spikes trap if falls over it.
        [UnityTest]
        public IEnumerator PrinceKilledIfFallsOverSpikesTest()
        {
            _cameraController.PlaceInRoom(_room21);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition10.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _enemy.SetActive(false);
            // _enemy.transform.SetPositionAndRotation(_startPosition11.transform.position, Quaternion.identity);
            // _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            // // I don't want enemy to move.
            // _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0;
            // _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            // _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            // yield return new WaitForSeconds(1);
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(5);
            // Assert Prince is dead.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        // Test that guard is killed by blades trap if retreats over it.
        [UnityTest]
        public IEnumerator GuardKilledIfRetreatsOverBladesTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition12.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.SetActive(true);
            // I don't want enemy to move.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            // Place guard.
            _enemy.transform.SetPositionAndRotation(_startPosition7.transform.position, Quaternion.identity);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            // yield return new WaitForSeconds(1);
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\oneSwordHit";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(6);
            // Assert Guard is dead.
            Assert.True(_enemy.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        // Test that prince is killed by blades trap if retreats over it.
        [UnityTest]
        public IEnumerator PrinceKilledIfRetreatsOverBladesTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition7.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.SetActive(true);
            // I want enemy to attack.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            // Place guard.
            _enemy.transform.SetPositionAndRotation(_startPosition12.transform.position, Quaternion.identity);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            // yield return new WaitForSeconds(1);
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\unsheathe";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(5);
            // Assert Prince is dead.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        // Test that Prince is killed by spikes trap if retreats over it.
        [UnityTest]
        public IEnumerator PrinceKilledIfRetreatsOverSpikesTest()
        {
            _cameraController.PlaceInRoom(_room21);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition11.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().HasSword = true;
            _enemy.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition10.transform.position, Quaternion.identity);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            // I don't want enemy to move.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            // yield return new WaitForSeconds(1);
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\unsheathe";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(5);
            // Assert Prince is dead.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        // Test that Prince is killed by spikes trap if runs over it.
        [UnityTest]
        public IEnumerator PrinceKilledIfRunOverSpikesTest()
        {
            _cameraController.PlaceInRoom(_room20);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition8.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _enemy.SetActive(false);
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(3);
            // Assert Guard is dead.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        // Test that Prince is not killed by spikes trap if walks over it.
        [UnityTest]
        public IEnumerator PrinceSurvivesIfWalksOverSpikesTest()
        {
            _cameraController.PlaceInRoom(_room20);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition8.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _enemy.SetActive(false);
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\walkingThreeSteps";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(8);
            // Assert Prince is alive.
            Assert.True(!_prince.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        // Test that Prince is not killed by spikes trap if vertical jumps over it.
        [UnityTest]
        public IEnumerator PrinceSurvivesIfVerticalJumpsOverSpikesTest()
        {
            _cameraController.PlaceInRoom(_room20);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition20.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _enemy.SetActive(false);
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\verticalJumpingSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(4);
            // Assert Prince is alive.
            Assert.True(!_prince.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        // Test that Prince is not killed by spikes trap if descends from it.
        [UnityTest]
        public IEnumerator PrinceSurvivesIfDescendsFromSpikesTest()
        {
            _cameraController.PlaceInRoom(_room11);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition21.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _enemy.SetActive(false);
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\descendOneLevel";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(4);
            // Assert Prince has descended.
            float newHeight = _prince.transform.position.y;
            Assert.True(Mathf.Abs(_startPosition21.transform.position.y - newHeight) > 1.9f);
            // Assert Prince is alive.
            Assert.True(!_prince.GetComponentInChildren<CharacterStatus>().IsDead);
        }
        
        // Test that Prince is not killed by spikes trap if walks over it.
        [UnityTest]
        public IEnumerator PrinceSurvivesIfJumpsOverSpikesTest()
        {
            _cameraController.PlaceInRoom(_room01);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition16.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _enemy.SetActive(false);
            // Command sequence.
            string commandFile = @"Assets\Tests\TestResources\jumpingOverSpikes";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movement happen.
            yield return new WaitForSeconds(5);
            // Assert Prince is alive.
            Assert.True(!_prince.GetComponentInChildren<CharacterStatus>().IsDead);
        }
    }
}
using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using Tests.PlayTests.Tools.Scene;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Tests.PlayTests
{
    public class HUDTests : MonoBehaviour
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
        private GameObject _startPosition19;
        private GameObject _startPosition20;
        private GameObject _startPosition29;

        private CameraController _cameraController;
        private LevelLoader _levelLoader;
        private Room _room00;
        private Room _room01;
        private Room _room10;
        private Room _room20;
        private Room _room21;

        private string _currentScene = "TheAbyss";

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
            if (_startPosition19 == null) _startPosition19 = GameObject.Find("StartPosition19");
            if (_startPosition20 == null) _startPosition20 = GameObject.Find("StartPosition20");
            if (_startPosition29 == null) _startPosition29 = GameObject.Find("StartPosition29");
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

        // Test that Prince life is properly represented at HUD.
        [UnityTest]
        public IEnumerator PrinceLifesTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition19.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _enemy.SetActive(false);
            HUDManager hudManager = GameObject.Find("HUDManager").GetComponentInChildren<HUDManager>();
            VisualElement[] princeLifes = AccessPrivateHelper.GetPrivateField<VisualElement[]>(hudManager, "_princeLifes");
            Sprite princeLifePoint = AccessPrivateHelper.GetPrivateField<Sprite>(hudManager, "princeLifePoint");
            Sprite princeHollowLifePoint = AccessPrivateHelper.GetPrivateField<Sprite>(hudManager, "princeHollowLifePoint");
            // Assert initial life is correctly represented.
            int Life = 4;
            _prince.GetComponentInChildren<CharacterStatus>().MaximumLife = Life;
            _prince.GetComponentInChildren<CharacterStatus>().Life = Life;
            yield return null;
            for (int i = 0; i < Life; i++)
            {
                Assert.True(princeLifes[i].style.backgroundImage == new StyleBackground(princeLifePoint));
            }
            Assert.True(princeLifes[Life].style.backgroundImage.value == null);
            // Assert a life increase is correctly shown.
            Life = 5;
            _prince.GetComponentInChildren<CharacterStatus>().MaximumLife = Life;
            _prince.GetComponentInChildren<CharacterStatus>().Life = Life;
            yield return null;
            for (int i = 0; i < Life; i++)
            {
                Assert.True(princeLifes[i].style.backgroundImage == new StyleBackground(princeLifePoint));
            }
            Assert.True(princeLifes[Life].style.backgroundImage.value == null);
            // Assert a life decrease is correctly shown.
            Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().MaximumLife = Life;
            _prince.GetComponentInChildren<CharacterStatus>().Life = Life;
            yield return null;
            for (int i = 0; i < Life; i++)
            {
                Assert.True(princeLifes[i].style.backgroundImage == new StyleBackground(princeLifePoint));
            }
            Assert.True(princeLifes[Life].style.backgroundImage.value == null);
            // Assert a life under maximum is correctly shown.
            Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().MaximumLife = Life;
            _prince.GetComponentInChildren<CharacterStatus>().Life = Life - 1;
            yield return null;
            for (int i = 0; i < Life - 1; i++)
            {
                Assert.True(princeLifes[i].style.backgroundImage == new StyleBackground(princeLifePoint));
            }
            Assert.True(princeLifes[Life-1].style.backgroundImage == new StyleBackground(princeHollowLifePoint));
            Assert.True(princeLifes[Life].style.backgroundImage.value == null);
        }
        
        
        // Test that guard life is properly represented at HUD.
        [UnityTest]
        public IEnumerator GuardLifesTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(false);
            _enemy.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition19.transform.position, Quaternion.identity);
            HUDManager hudManager = GameObject.Find("HUDManager").GetComponentInChildren<HUDManager>();
            VisualElement[] enemyLifes = AccessPrivateHelper.GetPrivateField<VisualElement[]>(hudManager, "_enemyLifes");
            Sprite enemyLifePoint = AccessPrivateHelper.GetPrivateField<Sprite>(hudManager, "enemyLifePoint");
            // Assert initial life is correctly represented.
            int Life = 4;
            _enemy.GetComponentInChildren<CharacterStatus>().MaximumLife = Life;
            _enemy.GetComponentInChildren<CharacterStatus>().Life = Life;
            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i < Life; i++)
            {
                Assert.True(enemyLifes[i].style.backgroundImage == new StyleBackground(enemyLifePoint));
            }
            Assert.True(enemyLifes[Life].style.backgroundImage.value == null);
            // Assert a life increase is correctly shown.
            Life = 5;
            _enemy.GetComponentInChildren<CharacterStatus>().MaximumLife = Life;
            _enemy.GetComponentInChildren<CharacterStatus>().Life = Life;
            yield return null;
            for (int i = 0; i < Life; i++)
            {
                Assert.True(enemyLifes[i].style.backgroundImage == new StyleBackground(enemyLifePoint));
            }
            Assert.True(enemyLifes[Life].style.backgroundImage.value == null);
            // Assert a life decrease is correctly shown.
            Life = 3;
            _enemy.GetComponentInChildren<CharacterStatus>().MaximumLife = Life;
            _enemy.GetComponentInChildren<CharacterStatus>().Life = Life;
            yield return null;
            for (int i = 0; i < Life; i++)
            {
                Assert.True(enemyLifes[i].style.backgroundImage == new StyleBackground(enemyLifePoint));
            }
            Assert.True(enemyLifes[Life].style.backgroundImage.value == null);
        }
        
        // Test that guard life is properly represented at HUD when he appears at the room and not before.
        [UnityTest]
        public IEnumerator GuardLifesAppearsWhenHeEntersRoomTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition29.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1.0f;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0.0f;
            _enemy.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition10.transform.position, Quaternion.identity);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            HUDManager hudManager = GameObject.Find("HUDManager").GetComponentInChildren<HUDManager>();
            VisualElement[] enemyLifes = AccessPrivateHelper.GetPrivateField<VisualElement[]>(hudManager, "_enemyLifes");
            Sprite enemyLifePoint = AccessPrivateHelper.GetPrivateField<Sprite>(hudManager, "enemyLifePoint");
            // Assert enemy life is not shown because he is out of current room.
            yield return new WaitForSeconds(0.5f);
            Assert.True(enemyLifes[0].style.backgroundImage.value == null);
            // Wait for enemy to enter room.
            yield return new WaitForSeconds(3.0f);
            // Assert enemy life is shown because he is in current room.
            Assert.True(enemyLifes[0].style.backgroundImage == new StyleBackground(enemyLifePoint));

        }
        
        // Test that guard life is properly represented at HUD when he appears at the room and not before.
        [UnityTest]
        public IEnumerator GuardLifesDisappearWhenPrinceLeavesRoomTest()
        {
            _cameraController.PlaceInRoom(_room10);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition20.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().Life = 3;
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _enemy.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition16.transform.position, Quaternion.identity);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            HUDManager hudManager = GameObject.Find("HUDManager").GetComponentInChildren<HUDManager>();
            VisualElement[] enemyLifes = AccessPrivateHelper.GetPrivateField<VisualElement[]>(hudManager, "_enemyLifes");
            Sprite enemyLifePoint = AccessPrivateHelper.GetPrivateField<Sprite>(hudManager, "enemyLifePoint");
            // Assert enemy life is shown because he is in current room.
            yield return new WaitForSeconds(1.0f);
            Assert.True(enemyLifes[0].style.backgroundImage == new StyleBackground(enemyLifePoint));
            // Ask for Prince movement.
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Wait movement to perform.
            yield return new WaitForSeconds(3.0f);
            // Assert enemy life is not shown because he is no longer in current room.
            Assert.True(enemyLifes[0].style.backgroundImage.value == null);

        }
    }
}
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
    public class FallingGroundTests
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

        private CameraController _cameraController;
        private Room _room00;
        private Room _room01;
        // private Room _room02;

        private GameObject _fallingGround;
        private GameObject _fallingGround1;
        private GameObject _fallingGround2;
        private GameObject _fallingGround3;
        private GameObject _fallingGround4;

        private string _currentScene = "TheRuins";

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
            if (_cameraController == null)
                _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
            if (_room00 == null)
                _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();
            if (_room01 == null)
                _room01 = GameObject.Find("Room_0_1").GetComponentInChildren<Room>();
            // if (_room02 == null)
            //     _room02 = GameObject.Find("Room_0_2").GetComponentInChildren<Room>();
            
            if (_fallingGround == null) _fallingGround = GameObject.Find("FallingGround");
            if (_fallingGround1 == null) _fallingGround1 = GameObject.Find("FallingGround (1)");
            if (_fallingGround2 == null) _fallingGround2 = GameObject.Find("FallingGround (2)");
            if (_fallingGround3 == null) _fallingGround3 = GameObject.Find("FallingGround (3)");
            if (_fallingGround4 == null) _fallingGround4 = GameObject.Find("FallingGround (4)");
            
            _prince.SetActive(false);
            _enemy.SetActive(false);

            yield return new EnterPlayMode();
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return TestSceneManager.UnLoadScene(_currentScene);
        }

        /// <summary>
        /// Test Prince can run over falling ground, activating them but reaching the other side.
        /// </summary>
        [UnityTest]
        public IEnumerator RunningOverFallingGroundTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            float startingHeight = _prince.transform.position.y;
            float groundStartingHeight = _fallingGround.transform.position.y;
            // float expectedFinalHeight = 0.32f;
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(4);
            
            float endHeight = _prince.transform.position.y;
            float groundEndHeight = _fallingGround.transform.position.y;
            float heightError = endHeight - startingHeight;
            float groundHeightDescended = groundStartingHeight - groundEndHeight;
            // Assert Prince has not fallen.
            Assert.True(Math.Abs(heightError) < 0.04);
            // Assert falling ground has fallen.
            Assert.True(Math.Abs(groundHeightDescended - 1.48f) < 0.04);
        }
        
        /// <summary>
        /// Test Prince can climb over falling ground, activating it and falling with ground.
        /// </summary>
        [UnityTest]
        public IEnumerator ClimbingActivatesFallingGroundTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition6.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            float startingHeight = _prince.transform.position.y;
            float groundStartingHeight = _fallingGround4.transform.position.y;
            string commandFile = @"Assets\Tests\TestResources\climbingToFallingGround";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return new WaitForSeconds(1.0f);
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(2.9f);
            // Assert prince has climbed.
            float middleHeight = _prince.transform.position.y;
            float ascendedHeight = middleHeight - startingHeight;
            Assert.True(Math.Abs(ascendedHeight - 2.0f) < 0.10);
            // Assert falling ground is still at its original position.
            float groundMiddleHeight = _fallingGround4.transform.position.y;
            float middleHeightError = groundMiddleHeight - groundStartingHeight;
            Assert.True(Math.Abs(middleHeightError) < 0.15);
            // Let ground fall.
            yield return new WaitForSeconds(2);
            // Assert Prince is now one level below (e.g. where he originally was)
            float endHeight = _prince.transform.position.y;
            float heightError = endHeight - startingHeight;
            Assert.True(Math.Abs(heightError) < 0.04);
            // Assert falling ground has fallen.
            float groundEndHeight = _fallingGround.transform.position.y;
            float groundHeightDescended = groundStartingHeight - groundEndHeight;
            // Assert falling ground has fallen.
            Assert.True(Math.Abs(groundHeightDescended - 2.0f) < 0.04);
            // Assert Prince has not suffered any damage.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }
        
        /// <summary>
        /// Test Prince hang from falling ground, activating it and making it fall.
        /// </summary>
        [UnityTest]
        public IEnumerator HangingActivatesFallingGroundTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition6.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            float startingHeight = _prince.transform.position.y;
            float groundStartingHeight = _fallingGround4.transform.position.y;
            string commandFile = @"Assets\Tests\TestResources\climbingKeepHanged";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(2.7f);
            // Assert prince is hanged.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.Climbing);
            // Let falling ground start its fall.
            yield return new WaitForSeconds(0.5f);
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.VerticalJumpEnd);
            Assert.True(_fallingGround4.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Falling);
            // Let falling ground fall.
            yield return new WaitForSeconds(2);
            // Assert Prince is crouching by hit and ground has crashed.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.HitByFallingGround);
            Assert.True(_fallingGround4.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Crashed);
            // Assert Prince has suffered damage by hit.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth - 1 == endHealth);
            // Assert Prince is now one level below (e.g. where he originally was)
            float endHeight = _prince.transform.position.y;
            float heightError = endHeight - startingHeight;
            Assert.True(Math.Abs(heightError) < 0.10);
            // Assert falling ground has fallen.
            float groundEndHeight = _fallingGround4.transform.position.y;
            float groundHeightDescended = groundStartingHeight - groundEndHeight;
            Assert.True(Math.Abs(groundHeightDescended - 1.49f) < 0.10);
        }
        
        /// <summary>
        /// Test Prince is hit by a falling ground over its head, receiving one point of damage if
        /// falling ground comes from one level above.
        /// </summary>
        [UnityTest]
        public IEnumerator ReceivingHitFromOneLevelFallingGroundTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            AccessPrivateHelper.SetPrivateField(_fallingGround.GetComponentInChildren<FallingGroundStatus>(), 
                "startFalling", true);
            yield return null;
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Assert Prince is crouching by hit and ground has crashed.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.HitByFallingGround);
            Assert.True(_fallingGround.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Crashed);
            // Assert Prince has suffered damage by hit.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth - 1 == endHealth);
        }
        
        /// <summary>
        /// Test Guard is hit by a falling ground over its head, receiving one point of damage if
        /// falling ground comes from one level above.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardReceivingHitFromOneLevelFallingGroundTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(true);
            _prince.SetActive(false);
            _enemy.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            int startingHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            AccessPrivateHelper.SetPrivateField(_fallingGround.GetComponentInChildren<FallingGroundStatus>(), 
                "startFalling", true);
            yield return null;
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Assert Guard has not changed his state.
            Assert.True(_enemy.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.Idle);
            Assert.True(_fallingGround.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Crashed);
            // Assert Guard has suffered damage by hit.
            Assert.False(_enemy.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth - 1 == endHealth);
        }
        
        /// <summary>
        /// Test Prince is hit by a falling ground over its head, receiving two points of damage if
        /// falling ground comes from two levels above.
        /// </summary>
        [UnityTest]
        public IEnumerator ReceivingHitFromTwoLevelsFallingGroundTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition3.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            float groundStartingHeight = _fallingGround2.transform.position.y;
            AccessPrivateHelper.SetPrivateField(_fallingGround2.GetComponentInChildren<FallingGroundStatus>(), 
                "startFalling", true);
            yield return null;
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Assert Prince is crouching by hit and ground has crashed.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.HitByFallingGround);
            Assert.True(_fallingGround2.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Crashed);
            // Assert ground has crashed at the level it should.
            float groundEndHeight = _fallingGround2.transform.position.y;
            float groundHeightDescended = groundStartingHeight - groundEndHeight;
            Assert.True(Math.Abs(groundHeightDescended - 3.54f) < 0.04);
            // Assert Prince has suffered damage by hit.
            Assert.False(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth - 2 == endHealth);
        }
        
        /// <summary>
        /// Test Guard is hit by a falling ground over its head, receiving two points of damage if
        /// falling ground comes from two levels above.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardReceivingHitFromTwoLevelsFallingGroundTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(true);
            _prince.SetActive(false);
            _enemy.transform.SetPositionAndRotation(_startPosition3.transform.position, Quaternion.identity);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            int startingHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            float groundStartingHeight = _fallingGround2.transform.position.y;
            AccessPrivateHelper.SetPrivateField(_fallingGround2.GetComponentInChildren<FallingGroundStatus>(), 
                "startFalling", true);
            yield return null;
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Assert Guard has not changed his state.
            Assert.True(_enemy.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.Idle);
            Assert.True(_fallingGround2.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Crashed);
            // Assert ground has crashed at the level it should.
            float groundEndHeight = _fallingGround2.transform.position.y;
            float groundHeightDescended = groundStartingHeight - groundEndHeight;
            Assert.True(Math.Abs(groundHeightDescended - 3.54f) < 0.04);
            // Assert Guard has suffered damage by hit.
            Assert.False(_enemy.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth - 2 == endHealth);
        }
        
        /// <summary>
        /// Test Prince is hit by a falling ground over its head, receiving three points of damage if
        /// falling ground comes from three levels above.
        /// </summary>
        [UnityTest]
        public IEnumerator ReceivingHitFromThreeLevelsFallingGroundTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition4.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            float groundStartingHeight = _fallingGround3.transform.position.y;
            AccessPrivateHelper.SetPrivateField(_fallingGround3.GetComponentInChildren<FallingGroundStatus>(), 
                "startFalling", true);
            yield return null;
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Assert Prince is dead and ground has crashed.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.Dead);
            Assert.True(_fallingGround3.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Crashed);
            // Assert ground has crashed at the level it should.
            float groundEndHeight = _fallingGround3.transform.position.y;
            float groundHeightDescended = groundStartingHeight - groundEndHeight;
            Assert.True(Math.Abs(groundHeightDescended - 5.61f) < 0.04);
            // Assert Prince is now dead.
            Assert.True(_prince.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth - 3 == endHealth);
        }
        
        /// <summary>
        /// Test Guard is hit by a falling ground over its head, receiving three points of damage if
        /// falling ground comes from three levels above.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardReceivingHitFromThreeLevelsFallingGroundTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(true);
            _prince.SetActive(false);
            _enemy.transform.SetPositionAndRotation(_startPosition4.transform.position, Quaternion.identity);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            int startingHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            float groundStartingHeight = _fallingGround3.transform.position.y;
            AccessPrivateHelper.SetPrivateField(_fallingGround3.GetComponentInChildren<FallingGroundStatus>(), 
                "startFalling", true);
            yield return null;
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Assert Guard is dead by hit and ground has crashed.
            Assert.True(_enemy.GetComponentInChildren<CharacterStatus>().CurrentState == CharacterStatus.States.Dead);
            Assert.True(_fallingGround3.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Crashed);
            // Assert ground has crashed at the level it should.
            float groundEndHeight = _fallingGround3.transform.position.y;
            float groundHeightDescended = groundStartingHeight - groundEndHeight;
            Assert.True(Math.Abs(groundHeightDescended - 5.61f) < 0.04);
            // Assert Guard is now dead.
            Assert.True(_enemy.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth - 3 == endHealth);
        }
        
        /// <summary>
        /// Test an enemy character activates a falling ground and falls down through its hole.
        /// </summary>
        [UnityTest]
        public IEnumerator GuardActivatesFallingGroundTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            _enemy.SetActive(true);
            _enemy.GetComponentInChildren<CharacterStatus>().LookingRightWards = false;
            // I want enemy to move.
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.defense = 0;
            int startingHealth = _prince.GetComponentInChildren<CharacterStatus>().Life;
            float startingHeight = _enemy.transform.position.y;
            float groundStartingHeight = _fallingGround1.transform.position.y;
            // Let movement happen.
            yield return new WaitForSeconds(4);
            float endHeight = _enemy.transform.position.y;
            float groundEndHeight = _fallingGround1.transform.position.y;
            float guardHeightDescended = startingHeight - endHeight;
            float groundHeightDescended = groundStartingHeight - groundEndHeight;
            // Assert Guard has fallen.
            Assert.True(Math.Abs(guardHeightDescended - 2.0) < 0.04);
            // Assert falling ground has fallen.
            Assert.True(Math.Abs(groundHeightDescended - 1.48f) < 0.04);
            // Assert Guard has not suffered any damage.
            Assert.False(_enemy.GetComponentInChildren<CharacterStatus>().IsDead);
            int endHealth = _enemy.GetComponentInChildren<CharacterStatus>().Life;
            Assert.True(startingHealth == endHealth);
        }
        
        /// <summary>
        /// Test Prince lands can make falling ground nearer than 3 units vibrate.
        /// </summary>
        [UnityTest]
        public IEnumerator LandingVibrationsTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition7.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            string commandFile = @"Assets\Tests\TestResources\runningSequence";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Assert nearer than 3 units falling grounds are trembling.
            Assert.True(_fallingGround.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Trembling);
            Assert.True(_fallingGround1.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Trembling);
            // Assert farther than 3 units falling grounds are not trembling.
            Assert.True(_fallingGround2.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Idle);
            Assert.True(_fallingGround3.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Idle);
            Assert.True(_fallingGround4.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Idle);
        }
        
        /// <summary>
        /// Test Prince jumps can make falling ground nearer than 3 units vibrate.
        /// </summary>
        [UnityTest]
        public IEnumerator JumpingVibrationsTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition7.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            string commandFile = @"Assets\Tests\TestResources\climbingOneLevel";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(1.5f);
            // Assert nearer than 3 units falling grounds are trembling.
            Assert.True(_fallingGround2.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Trembling);
            Assert.True(_fallingGround4.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Trembling);
            // Assert farther than 3 units falling grounds are not trembling.
            Assert.True(_fallingGround.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Idle);
            Assert.True(_fallingGround1.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Idle);
            Assert.True(_fallingGround3.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Idle);
        }
        
        /// <summary>
        /// Test Prince jumps under a falling ground makes it fall.
        /// </summary>
        [UnityTest]
        public IEnumerator JumpingFallTest()
        {
            _cameraController.PlaceInRoom(_room00);
            _enemy.SetActive(false);
            _prince.SetActive(true);
            _prince.transform.SetPositionAndRotation(_startPosition8.transform.position, Quaternion.identity);
            _prince.GetComponentInChildren<CharacterStatus>().LookingRightWards = true;
            string commandFile = @"Assets\Tests\TestResources\climbingOneLevel";
            InputController inputController = _prince.GetComponent<InputController>();
            yield return null;
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            // Assert falling ground has crashed.
            Assert.True(_fallingGround4.GetComponentInChildren<FallingGroundStatus>().CurrentState == FallingGroundStatus.FallingGroundStates.Crashed);
        }
    }
}
using System;
using System.Collections;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class EnemyChaseTests
    {
        private GameObject _prince;
        private GameObject _enemy;

        private GameObject _startPosition1;
        private GameObject _startPosition2;
        private GameObject _startPosition3;
        private GameObject _startPosition4;
        private GameObject _startPosition5;
        private GameObject _startPosition6;

        private string _currentScene = "TwoLevelPit";
        
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

            yield return new EnterPlayMode();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return TestSceneManager.UnLoadScene(_currentScene);
        }

        // [NUnit.Framework.Test]
        // public void CharacterMovementSimplePasses()
        // {
        //     // Use the Assert class to test conditions.
        //     
        // }

        /// <summary>
        /// Test guard stops if trying to get Prince he finds a hole in the way.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator GuardDoNotFallTest()
        {
            // Setup test.
            // LogAssert.ignoreFailingMessages = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.transform.SetPositionAndRotation(_startPosition3.transform.position, Quaternion.identity);
            Vector2 startPosition = _enemy.transform.position;
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Vector2 endPosition = _enemy.transform.position;
            float fallenDistance = startPosition.y - endPosition.y;
            // Assert enemy has not fallen through hole.
            Assert.IsTrue(fallenDistance < 0.10f);
            float advancedDistance = endPosition.x - startPosition.x;
            // Assert enemy has advanced to the edge.
            Assert.IsTrue(advancedDistance > 0.5f);
            yield return null;
        }
        
        /// <summary>
        /// Test guard follows Prince if he sneaks through a hole.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator GuardJumpToHoleWhenChasingTest()
        {
            // Setup test.
            LogAssert.ignoreFailingMessages = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.transform.SetPositionAndRotation(_startPosition3.transform.position, Quaternion.identity);
            Vector2 startPosition = _enemy.transform.position;
            yield return new WaitForSeconds(2);
            // Let fall perform.
            _prince.transform.SetPositionAndRotation(_startPosition1.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(4);
            Vector2 endPosition = _enemy.transform.position;
            float fallenDistance = startPosition.y - endPosition.y;
            // Assert enemy has fallen through hole.
            Assert.IsTrue(fallenDistance > 1.0f);
            float separationDistance = Math.Abs(_prince.transform.position.x - _enemy.transform.position.x);
            float hittingRange = _enemy.GetComponentInChildren<FightingSensors>().HittingRange;
            float difference = Math.Abs(separationDistance - hittingRange);
            // Assert enemy is at hitting range of Prince.
            Assert.IsTrue(difference < 0.2f);
            yield return null;
        }
        
        
        /// <summary>
        /// Test guard chases Prince when he is forward detected.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator BoldestGuardChaseForwardTest()
        {
            // Setup test.
            // LogAssert.ignoreFailingMessages = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.attack = 0;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            Vector2 startPosition = _enemy.transform.position;
            // Let chase happen.
            yield return new WaitForSeconds(4);
            float separationDistance = _prince.transform.position.x - _enemy.transform.position.x;
            float hittingRange = _enemy.GetComponentInChildren<FightingSensors>().HittingRange;
            float difference = Math.Abs(separationDistance - hittingRange);
            // Assert enemy is at hitting range of Prince.
            Assert.IsTrue(difference < 0.25f);
            yield return null;
        }
        
        /// <summary>
        /// Test coward guard stays idle when Prince is forward detected.
        /// </summary>
        [UnityTest]
        public IEnumerator CowardGuardChaseForwardTest()
        {
            // Setup test.
            // LogAssert.ignoreFailingMessages = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            Vector2 startPosition = _enemy.transform.position;
            // Let chase happen.
            yield return new WaitForSeconds(4);
            float advancedDistance = _enemy.transform.position.x - startPosition.x;
            // Assert enemy is in the same place.
            Assert.IsTrue(Math.Abs(advancedDistance) < 0.2f);
            yield return null;
        }
        
        /// <summary>
        /// Test prudent guard advance, but not much, when Prince is forward detected.
        /// </summary>
        [UnityTest]
        public IEnumerator PrudentGuardChaseForwardTest()
        {
            // Setup test.
            // LogAssert.ignoreFailingMessages = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 0.5f;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition5.transform.position, Quaternion.identity);
            _prince.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            Vector2 startPosition = _enemy.transform.position;
            // Let chase happen.
            yield return new WaitForSeconds(4);
            float advancedDistance = _enemy.transform.position.x - startPosition.x;
            float separationDistance = _prince.transform.position.x - _enemy.transform.position.x;
            float hittingRange = _enemy.GetComponentInChildren<FightingSensors>().HittingRange;
            float difference = Math.Abs(separationDistance - hittingRange);
            // Assert enemy has advanced something but is still out of hitting range.
            Assert.IsTrue(Math.Abs(advancedDistance) > 0.2f);
            Assert.IsTrue(separationDistance > hittingRange);
            yield return null;
        }
        
        /// <summary>
        /// Test guard chases Prince when he is forward detected..
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator GuardChaseBackwardTest()
        {
            // Setup test.
            // LogAssert.ignoreFailingMessages = true;
            _enemy.GetComponentInChildren<GuardFightingProfile>().fightingProfile.boldness = 1;
            _enemy.SetActive(true);
            _prince.SetActive(true);
            _enemy.transform.SetPositionAndRotation(_startPosition2.transform.position, Quaternion.identity);
            _prince.transform.SetPositionAndRotation(_startPosition6.transform.position, Quaternion.identity);
            // Vector2 startPosition = _enemy.transform.position;
            // Let chase happen.
            yield return new WaitForSeconds(4);
            float separationDistance = Math.Abs(_prince.transform.position.x - _enemy.transform.position.x);
            float hittingRange = _enemy.GetComponentInChildren<FightingSensors>().HittingRange;
            float difference = Math.Abs(separationDistance - hittingRange);
            // Assert enemy is at hitting range of Prince.
            Assert.IsTrue(difference < 0.2f);
            yield return null;
        }
    }
}
using System;
using System.Collections;
using NUnit.Framework;
using Prince;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Tests.PlayTests.Tools.Lang;

namespace Tests.PlayTests
{
    public class CharacterMovementTests
    {

        [UnitySetUp]
        public IEnumerator Setup()
        {
            if (!SceneManager.GetSceneByName("Scenes/ThePit").isLoaded)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/ThePit", LoadSceneMode.Additive);
                yield return new WaitUntil(() => asyncLoad.isDone);
            }

            yield return new EnterPlayMode();
        }
        
        // [NUnit.Framework.Test]
        // public void CharacterMovementSimplePasses()
        // {
        //     // Use the Assert class to test conditions.
        //     
        // }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator AdvanceWithSwordTests()
        {
            // Setup test.
            float expected_distance = 0.4161f;
            string commandFile = @"Assets\Tests\TestResources\advanceWithSword";
            GameObject princeGameObject = GameObject.Find("Prince");
            Vector2 startPosition = princeGameObject.transform.position;
            InputController inputController = princeGameObject.GetComponent<InputController>();
            AccessPrivateHelper.SetPrivateField(inputController, "recordedCommandsFile", commandFile);
            AccessPrivateHelper.AccessPrivateMethod(inputController, "ReplayRecordedCommands");
            // Let movements perform.
            yield return new WaitForSeconds(3);
            Vector2 endPosition = princeGameObject.transform.position;
            float advancedDistance = Vector2.Distance(startPosition, endPosition);
            float error = advancedDistance - expected_distance;
            Assert.True(Math.Abs(error) < 0.01);
        }
    }
}
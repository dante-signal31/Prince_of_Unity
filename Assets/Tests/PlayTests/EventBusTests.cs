using System.Collections;
using NUnit.Framework;
using Tests.PlayTests.Scripts;
using UnityEngine.TestTools;
using Tests.PlayTests.Tools.Scene;
using UnityEngine;

namespace Tests.PlayTests
{
    public class EventBusTests
    {
        private GameObject _circleEmitter;
        private GameObject _boxEmitter;
        private GameObject _receiver1;
        private GameObject _receiver2;
        
        private string _currentScene = "TheEvent";
        
        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return TestSceneManager.ReLoadScene(_currentScene);
            
            if (_circleEmitter == null) _circleEmitter= GameObject.Find("CircleEmitter");
            if (_boxEmitter == null) _boxEmitter= GameObject.Find("BoxEmitter");
            if (_receiver1 == null) _receiver1= GameObject.Find("Receiver1");
            if (_receiver2 == null) _receiver2= GameObject.Find("Receiver2");

            yield return new EnterPlayMode();
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return TestSceneManager.UnLoadScene(_currentScene);
        }

        /// <summary>
        /// Check both circle and box can register their events and that those events are properly received by receivers.
        /// </summary>
        [UnityTest]
        public IEnumerator MultipleEventsTest()
        {
            BoxEventEmitter boxEventEmitter = _boxEmitter.GetComponentInChildren<BoxEventEmitter>();
            CircleEventEmitter circleEventEmitter = _circleEmitter.GetComponentInChildren<CircleEventEmitter>();
            CircleEventReceiver circleEventReceiver1 = _receiver1.GetComponentInChildren<CircleEventReceiver>();
            CircleEventReceiver2 circleEventReceiver2 = _receiver2.GetComponentInChildren<CircleEventReceiver2>();
            yield return null;
            // Check starting colors are different.
            Assert.False(circleEventEmitter.SpriteColor == circleEventReceiver1.SpriteColor ||
                         circleEventEmitter.SpriteColor == circleEventReceiver2.SpriteColor ||
                         circleEventReceiver1.SpriteColor == circleEventReceiver2.SpriteColor);
            // Check starting values are different.
            Assert.False(boxEventEmitter.RandomValue == circleEventReceiver1.RandomValue ||
                         boxEventEmitter.RandomValue == circleEventReceiver2.RandomValue ||
                         circleEventReceiver1.RandomValue == circleEventReceiver2.RandomValue);
            yield return null;
            // Trigger events.
            circleEventEmitter.ActivateCircle();
            boxEventEmitter.ActivateBox();
            // Let receivers react to event.
            yield return new WaitForSeconds(1.0f);
            // Check colors are now the same.
            Assert.True(circleEventEmitter.SpriteColor == circleEventReceiver1.SpriteColor &&
                         circleEventEmitter.SpriteColor == circleEventReceiver2.SpriteColor &&
                         circleEventReceiver1.SpriteColor == circleEventReceiver2.SpriteColor);
            // Check values are now the same.
            Assert.True(boxEventEmitter.RandomValue == circleEventReceiver1.RandomValue &&
                         boxEventEmitter.RandomValue == circleEventReceiver2.RandomValue &&
                         circleEventReceiver1.RandomValue == circleEventReceiver2.RandomValue);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Prince
{
    /// <summary>
    /// Component to detect if this ground is falling over a character.
    ///
    /// It has a hitSensorActivated UnityEvent were listeners can subscribe callbacks
    /// to be activated when a character was hit, with its game object passed as parameter. 
    /// </summary>
    public class FallingGroundCharacterDetector : MonoBehaviour
    {
        [Serializable] public class CharacterHit : UnityEvent<GameObject> { }
        
        [Header("CONFIGURATION:")]
        // I could have used polling to know if sensor is triggered, but as this sensor is only
        // activated few times I found interesting using unity events. That way I'm not going 
        // to waste CPU polling a sensor that is idle most of the time.
        //
        // In this specific use case (intra-prefab communication) I could have wired
        // FallingGroundCharacterInteractions to call its public methods directly. Probably
        // the result would have been the same, and possibly it would have been more performant. But
        // this way at least I have a chance to practice with UnityEvents.
        // 
        // While CharacterWeightSensor emitted a parameterless event, this one includes
        // hit game object as parameter.
        [Tooltip("Callbacks to activate when sensor is triggered.")]
        [SerializeField] private CharacterHit hitSensorActivated;

        public HashSet<GameObject> GameObjectsHit { get; private set; }

        private void Awake()
        {
            GameObjectsHit = new HashSet<GameObject>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            GameObject detectedGameObject = GameObjectTools.Collider2GameObject(col);
            if (GameObjectTools.IsACharacter(detectedGameObject) &&
                !GameObjectsHit.Contains(detectedGameObject))
            {
                GameObjectsHit.Add(detectedGameObject);
                hitSensorActivated.Invoke(detectedGameObject);
            }

        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            GameObject detectedGameObject = GameObjectTools.Collider2GameObject(other);
            // HashSet remove does not throw an exception if element to remove is not found.
            GameObjectsHit.Remove(detectedGameObject);
        }
    }
}
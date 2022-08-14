using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Prince
{
    /// <summary>
    /// Component to warn an state machine that a sensor volume has been activated by a character.
    /// </summary>
    public class ProximitySensor : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("State machine to warn when a character interacts with sensor.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to know which character are on sensor volume.")] 
        [SerializeField] private CharacterWeightSensor sensor;

        [Header("CONFIGURATION:")]
        [Tooltip("State machine flag to set to true when character is in proximity sensor range.")]
        [SerializeField] private string flagToActivate;
        [Tooltip("Who to notify a new character has been detected.")] 
        [SerializeField] private UnityEvent<GameObject> newCharacterDetected;
        [Tooltip("Who to notify a new character has gone.")] 
        [SerializeField] private UnityEvent<GameObject> characterGone;

        /// <summary>
        /// Characters game objects inside this sensor detection volume.
        /// </summary>
        public HashSet<GameObject> CharactersDetected => sensor.CharactersOverSensor;

        private HashSet<GameObject> _charactersCurrentlyDetected;

        /// <summary>
        /// Called when sensor activated event is triggered.
        /// </summary>
        public void SensorActivated()
        {
            NotifyStateMachine(true);
            AddNewDetectedCharacters();
        }

        private void AddNewDetectedCharacters()
        {
            // I use a Linq query because I don't want to modify in place nor CharactersDetected nor _characterCurrentlyDetected.
            IEnumerable<GameObject> newCharacters = from character in CharactersDetected
                where !_charactersCurrentlyDetected.Contains(character)
                select character;
            foreach (GameObject character in newCharacters)
            {
                _charactersCurrentlyDetected.Add(character);
                if (newCharacterDetected != null) newCharacterDetected.Invoke(character);
            }
        }

        /// <summary>
        /// Set flagToActivate state machine flag to given value.
        /// </summary>
        /// <param name="newValue"></param>
        private void NotifyStateMachine(bool newValue)
        {
            stateMachine.SetBool(flagToActivate, newValue);
        }

        /// <summary>
        /// Called when sensor deactivated event is triggered.
        /// </summary>
        public void SensorDeactivated()
        {
            RemoveGoneCharacters();
            if (_charactersCurrentlyDetected.Count == 0) NotifyStateMachine(false);
        }

        private void RemoveGoneCharacters()
        {
            // I use a Linq query because I don't want to modify in place nor CharactersDetected nor _characterCurrentlyDetected.
            IEnumerable<GameObject> goneCharacters = from character in _charactersCurrentlyDetected
                where !CharactersDetected.Contains(character)
                select character;
            foreach (GameObject character in goneCharacters)
            {
                _charactersCurrentlyDetected.Remove(character);
                if (characterGone != null) characterGone.Invoke(character);
            }
        }
    }
}
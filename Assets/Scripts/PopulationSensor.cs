using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to keep a registry of enemy characters in current room.
    /// </summary>
    public class PopulationSensor : MonoBehaviour
    {
        /// <summary>
        /// Enemy present at current room.
        /// </summary>
        public GameObject EnemyCharacter { get; private set; }

        private EventBus _eventBus;

        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            GameObject character = col.transform.root.gameObject;
            CharacterStatus characterStatus = character.GetComponentInChildren<CharacterStatus>();
            if (characterStatus != null && !characterStatus.IsPrince)
            {
                EnemyCharacter = character;
                _eventBus.TriggerEvent(new GameEvents.GuardEnteredTheRoom(character), this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            GameObject character = other.transform.root.gameObject;
            CharacterStatus characterStatus = character.GetComponentInChildren<CharacterStatus>();
            if (characterStatus != null && !character.GetComponentInChildren<CharacterStatus>().IsPrince)
            {
                if (EnemyCharacter == character)
                {
                    EnemyCharacter = null;
                    _eventBus.TriggerEvent(new GameEvents.GuardLeftTheRoom(character), this);
                }
            }
        }
    }
}
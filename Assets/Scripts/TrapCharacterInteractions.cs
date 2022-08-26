using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Trap component used to interact with characters.
    /// </summary>
    public abstract class TrapCharacterInteractions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when a character enters killing range.")] 
        [SerializeField] protected TrapStatus trapStatus;
        [Tooltip("Needed to know who is in killing range.")] 
        [SerializeField] protected ProximitySensor killingSensor;
        [Tooltip("Needed to update trap appearance.")]
        [SerializeField] protected TrapAppearance trapAppearance;
        [Tooltip("Needed to trigger damage effect when a character is killed.")] 
        [SerializeField] protected TrapDamageEffect damageEffect;

        protected int _charactersInTrap;
        
        /// <summary>
        /// Listener for killingSensor newCharacterEvent event.
        /// </summary>
        /// <param name="_">New character detected. (Currently not used)</param>
        public void NewCharacterInKillingZone(GameObject _)
        {
            _charactersInTrap++;
        }

        /// <summary>
        /// Listener for killingSensor characterGone event.
        /// </summary>
        /// <param name="_">Character who has exited from killing zone. (Currently not used)</param>
        public void CharacterGoneFromKillingZone(GameObject _)
        {
            if (_charactersInTrap > 0) _charactersInTrap--;
        }

        /// <summary>
        /// Kill whoever is in trap kill zone.
        /// </summary>
        protected abstract void KillCharactersInKillingZone();

        private void FixedUpdate()
        {
            if (trapStatus.CanKill && _charactersInTrap != 0)
            {
                KillCharactersInKillingZone();
            }
        }
    }
}
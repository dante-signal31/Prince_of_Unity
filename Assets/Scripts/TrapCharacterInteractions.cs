using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Trap component used to interact with characters.
    /// </summary>
    public class TrapCharacterInteractions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when a character enters killing range.")] 
        [SerializeField] private TrapStatus trapStatus;
        [Tooltip("Needed to know who is in killing range.")] 
        [SerializeField] private ProximitySensor killingSensor;
        [Tooltip("Needed to update trap appearance.")]
        [SerializeField] private TrapAppearance trapAppearance;
        [Tooltip("Needed to trigger damage effect when a character is killed.")] 
        [SerializeField] private TrapDamageEffect damageEffect;

        private int _charactersInTrap;
        
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
        public void KillCharactersInKillingZone()
        {
            foreach (GameObject character in killingSensor.CharactersDetected)
            {
                TrapInteractions characterTrapInteractions = character.GetComponentInChildren<TrapInteractions>();
                CharacterStatus characterStatus = character.GetComponentInChildren<CharacterStatus>();
                Sprite corpse = characterTrapInteractions.GetKilledByTrapCorpse(trapStatus.KillMode);
                // if (characterStatus.IsPrince)
                // {
                //     damageEffect.ShowTrapHit(characterStatus.LookingRightWards? 
                //         TrapDamageEffect.DamageEffectType.CharacterCameFromLeft: 
                //         TrapDamageEffect.DamageEffectType.CharacterCameFromRight);
                // }
                damageEffect.ShowTrapHit(characterStatus.LookingRightWards? 
                    TrapDamageEffect.DamageEffectType.CharacterCameFromLeft: 
                    TrapDamageEffect.DamageEffectType.CharacterCameFromRight, characterStatus.IsPrince);
                trapAppearance.ShowCorpse(characterStatus.IsPrince, characterStatus.LookingRightWards, corpse);
                characterTrapInteractions.KilledByTrap();
                _charactersInTrap--;
            }
        }

        private void FixedUpdate()
        {
            if (trapStatus.CanKill && _charactersInTrap != 0)
            {
                KillCharactersInKillingZone();
            }
        }
    }
}
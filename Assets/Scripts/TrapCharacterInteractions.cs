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

        /// <summary>
        /// Listener for killingSensor newCharacterEvent event.
        /// </summary>
        /// <param name="character">New character detected.</param>
        public void NewCharacterInKillingZone(GameObject character)
        {
            if (trapStatus.CanKill)
            {
                TrapInteractions characterTrapInteractions = character.GetComponentInChildren<TrapInteractions>();
                CharacterStatus characterStatus = character.GetComponentInChildren<CharacterStatus>();
                Sprite corpse = characterTrapInteractions.GetKilledByTrapCorpse(trapStatus.KillMode);
                if (characterStatus.IsPrince)
                {
                    damageEffect.ShowTrapHit(characterStatus.LookingRightWards? 
                        TrapDamageEffect.DamageEffectType.CharacterCameFromLeft: 
                        TrapDamageEffect.DamageEffectType.CharacterCameFromRight);
                }
                trapAppearance.ShowCorpse(characterStatus.IsPrince, characterStatus.LookingRightWards, corpse);
                characterTrapInteractions.KilledByTrap();
            }
        }
    }
}
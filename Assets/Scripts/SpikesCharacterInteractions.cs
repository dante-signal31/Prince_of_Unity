using UnityEngine;

namespace Prince
{
    public class SpikesCharacterInteractions: TrapCharacterInteractions
    {
        /// <summary>
        /// Spikes component used to interact with characters.
        /// </summary>
        protected override void KillCharactersInKillingZone()
        {
            foreach (GameObject character in killingSensor.CharactersDetected)
            {
                TrapInteractions characterTrapInteractions = character.GetComponentInChildren<TrapInteractions>();
                CharacterStatus characterStatus = character.GetComponentInChildren<CharacterStatus>();
                // The only way to get through spikes is walking, so in that case don't kill character.
                if (characterStatus.CurrentState == CharacterStatus.States.Walk ||
                    characterStatus.CurrentState == CharacterStatus.States.RunningJump ||
                    characterStatus.CurrentState == CharacterStatus.States.Idle) return;
                Sprite corpse = characterTrapInteractions.GetKilledByTrapCorpse(trapStatus.KillMode);
                damageEffect.ShowTrapHit(characterStatus.LookingRightWards? 
                    TrapDamageEffect.DamageEffectType.CharacterCameFromLeft: 
                    TrapDamageEffect.DamageEffectType.CharacterCameFromRight, characterStatus.IsPrince);
                trapAppearance.ShowCorpse(characterStatus.IsPrince, characterStatus.LookingRightWards, corpse);
                characterTrapInteractions.KilledByTrap();
                _charactersInTrap--;
            }
        }
    }
}
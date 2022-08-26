using UnityEngine;

namespace Prince
{
    public class BladesCharacterInteractions: TrapCharacterInteractions
    {
        /// <summary>
        /// Blades component used to interact with characters.
        /// </summary>
        protected override void KillCharactersInKillingZone()
        {
            foreach (GameObject character in killingSensor.CharactersDetected)
            {
                TrapInteractions characterTrapInteractions = character.GetComponentInChildren<TrapInteractions>();
                CharacterStatus characterStatus = character.GetComponentInChildren<CharacterStatus>();
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
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
        [SerializeField] private TrapStatus trapStatus;
        [Tooltip("Needed to warn that every character in killing zone is invulnerable")] 
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to know who is in killing range.")] 
        [SerializeField] private ProximitySensor killingSensor;
        [Tooltip("Needed to update trap appearance.")]
        [SerializeField] private TrapAppearance trapAppearance;
        [Tooltip("Needed to trigger damage effect when a character is killed.")] 
        [SerializeField] private TrapDamageEffect damageEffect;

        /// <summary>
        /// Whether this character is untouchable by this trap even being in kill zone.
        /// </summary>
        protected abstract bool InvulnerableCharacter(GameObject character);
        
        /// <summary>
        /// Whether every character in killing zone is doing anything to be untouchable by this trap.
        /// </summary>
        private bool AllAreInvulnerableCharacters
        {
            get
            {
                foreach (GameObject character in killingSensor.CharactersDetected)
                {
                    if (!InvulnerableCharacter(character))
                    {
                        stateMachine.SetBool("AllAreInvulnerableCharacters", false);
                        return false;
                    }
                }
                stateMachine.SetBool("AllAreInvulnerableCharacters", true);
                return true;
            }
            set { }
        }
        
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
        private void KillCharactersInKillingZone()
        {
            foreach (GameObject character in killingSensor.CharactersDetected)
            {
                if (InvulnerableCharacter(character)) continue;
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

        private void FixedUpdate()
        {
            if (!AllAreInvulnerableCharacters && trapStatus.CanKill && _charactersInTrap != 0)
            {
                KillCharactersInKillingZone();
            }
        }
    }
}
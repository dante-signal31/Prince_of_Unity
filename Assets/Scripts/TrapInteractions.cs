using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Character component used to interact with traps.
    /// </summary>
    public class TrapInteractions : MonoBehaviour
    {
        public enum CorpseTypes
        {
            Stabbed,
            Cut
        }

        [Header("WIRING:")] 
        [Tooltip("Needed to get corpse sprite configuration.")] 
        [SerializeField] private CharacterAppearance characterAppearance;
        
        /// <summary>
        /// Call from trap to signal this character has been killed by that trap.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void KilledByTrap()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get this character corpse for a given trap mode.
        /// </summary>
        /// <param name="corpseType">Trap mode.</param>
        /// <returns>Sprite used to represent this character killed by a trap of that mode.</returns>
        public Sprite GetKilledByTrapCorpse(CorpseTypes corpseType)
        {
            return characterAppearance.GetCorpseSprite(corpseType);
        }
        
    }
}
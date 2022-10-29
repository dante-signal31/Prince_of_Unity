using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Static library to get information about Prince of Unity GameObjects.
    /// </summary>
    public static class GameObjectTools
    {
        /// <summary>
        /// Exception raised when you try a character operation over an game object that is not one.
        /// </summary>
        public class NotACharacterException: Exception { }

        /// <summary>
        /// Assess given game object to find out it it is a Character.
        /// </summary>
        /// <param name="detectedGameObject">Given game object to assess.</param>
        /// <returns>True if given game object is a Character.</returns>
        public static bool IsACharacter(GameObject detectedGameObject)
        {
            CharacterStatus characterStatus = detectedGameObject.GetComponentInChildren<CharacterStatus>();
            return characterStatus != null;
        }

        /// <summary>
        /// Get current state of given character.
        /// </summary>
        /// <param name="detectedGameObject">Character game object.</param>
        /// <returns>State of current character.</returns>
        /// <exception cref="NotACharacterException">Raised if given game object is not actually a character.</exception>
        public static CharacterStatus.States GetCharacterCurrentState(GameObject detectedGameObject)
        {
            CharacterStatus characterStatus = detectedGameObject.GetComponentInChildren<CharacterStatus>();
            try
            {
                return characterStatus.CurrentState;
            }
            catch (NullReferenceException ex)
            {
                throw new NotACharacterException();
            }
        }

        /// <summary>
        /// Whether given character is in any of given states.
        /// </summary>
        /// <param name="detectedGameObject">Character game object.</param>
        /// <param name="statesToCheck">A set of states to check.</param>
        /// <returns>True if character is in any of those states to check. False instead.</returns>
        /// <exception cref="NotACharacterException">Raised if given game object is not actually a character.</exception>
        public static bool CharacterInState(GameObject detectedGameObject, HashSet<CharacterStatus.States> statesToCheck)
        {
            CharacterStatus.States currentState;
            CharacterStatus characterStatus = detectedGameObject.GetComponentInChildren<CharacterStatus>();
            try
            {
                currentState = characterStatus.CurrentState;
            }
            catch (NullReferenceException ex)
            {
                throw new NotACharacterException();
            }
            return statesToCheck.Contains(currentState);
        }

        /// <summary>
        /// Get game object this collider is attached to.
        /// </summary>
        /// <param name="col">Collider whose parent game object we want to get.</param>
        /// <returns>Parent game object this collider is attached to.</returns>
        public static GameObject Collider2GameObject(Collider2D col)
        {
            return col.transform.root.gameObject;
        }
        
    }
}
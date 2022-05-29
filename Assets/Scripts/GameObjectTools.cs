using UnityEngine;

namespace Prince
{
    public static class GameObjectTools
    {
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
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Sometimes we don't want to disable an entire game object when reaching an state, just part of it.
    ///
    /// This component disables given components when reaching given state.
    /// </summary>
    public class CharacterPartialDisablingController: DisablingController<CharacterStatus.States>
    {
        [Tooltip("List of components to disable when entering given state.")]
        [SerializeField] private List<GameObject> componentsToDisable = new List<GameObject>();

        protected override void DisableGameObject()
        {
            foreach (GameObject _gameObject in componentsToDisable)
            {
                _gameObject.SetActive(false);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Prince
{
    /// <summary>
    /// This sensor is attached to Prince character to detect pickables just in front of character.
    /// </summary>
    public class PickableSensor : MonoBehaviour
    {
        [Header("CONFIGURATION:")] 
        [Tooltip("Add listeners to be triggered when any pickable enters range.")]
        [SerializeField] private UnityEvent<PickableCharacterInteractions> pickableEnteredRange;
        [Tooltip("Add listeners to be triggered when any pickable leaves range.")]
        [SerializeField] private UnityEvent<PickableCharacterInteractions> pickableLeftRange;

        private HashSet<PickableCharacterInteractions> _pickableCharacterInteractionsSet =
            new HashSet<PickableCharacterInteractions>();

        /// <summary>
        /// Set of pickables in range.
        /// </summary>
        public HashSet<PickableCharacterInteractions> PickablesInRange
        {
            get => _pickableCharacterInteractionsSet; 
            private set => _pickableCharacterInteractionsSet = value;
        }
        
        /// <summary>
        /// Whether we have any pickable in range to be taken.
        /// </summary>
        public bool AnythingPickable => PickablesInRange.Count >= 1;

        private void OnTriggerEnter2D(Collider2D col)
        {
            PickableCharacterInteractions pickable =
                col.transform.root.gameObject.GetComponentInChildren<PickableCharacterInteractions>();
            if (pickable != null)
            {
                PickablesInRange.Add(pickable);
                if (pickableEnteredRange != null) pickableEnteredRange.Invoke(pickable);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            PickableCharacterInteractions pickable =
                other.transform.root.gameObject.GetComponentInChildren<PickableCharacterInteractions>();
            if (pickable != null)
            {
                PickablesInRange.Remove(pickable);
                if (pickableLeftRange != null) pickableLeftRange.Invoke(pickable);
            }
        }
    }
}
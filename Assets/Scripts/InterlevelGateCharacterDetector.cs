using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to detect prince character and signal him when gate is open and
    /// ready to load next level.
    /// </summary>
    public class InterlevelGateCharacterDetector : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when gate is open.")]
        [SerializeField] private InterlevelGateStatus gateStatus;
       
        /// <summary>
        /// Reference to detected Prince character while he stays in sensor volume.
        /// </summary>
        public InterlevelGateInteractions PrinceCharacter { get; private set; }

        private bool _princeAlreadyNoticedThatGateIsOpen = false;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            InterlevelGateInteractions characterInteractions = col.transform.root.gameObject.GetComponentInChildren<InterlevelGateInteractions>();
            if (characterInteractions != null) PrinceCharacter = characterInteractions;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.transform.root.gameObject.GetComponentInChildren<InterlevelGateInteractions>() != null)
            {
                if (PrinceCharacter.InterlevelGateAvailable) PrinceCharacter.InterlevelGateAvailable = false;
                PrinceCharacter = null;
            }
        }

        private void Update()
        {
            if (PrinceCharacter != null &&
                !_princeAlreadyNoticedThatGateIsOpen && 
                gateStatus.CurrentState == InterlevelGateStatus.GateStates.Open)
            {
                PrinceCharacter.InterlevelGateAvailable = true;
            }
        }
    }
}
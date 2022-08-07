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
        [Tooltip("Needed to give it to Prince character when he is in front of gate.")]
        [SerializeField] private InterlevelGateCharacterInteractions characterInteractions;
       
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
                if (PrinceCharacter != null && 
                    PrinceCharacter.InterlevelGateAvailable) PrinceCharacter.InterlevelGate = null;
                PrinceCharacter = null;
            }
        }

        private void Update()
        {
            if (PrinceCharacter != null &&
                !_princeAlreadyNoticedThatGateIsOpen && 
                gateStatus.CurrentState == InterlevelGateStatus.GateStates.Open)
            {
                PrinceCharacter.InterlevelGate = characterInteractions;
            }
        }
    }
}
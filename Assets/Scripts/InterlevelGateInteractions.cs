using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component is used by Prince to interact with interlevel gates.
    /// </summary>
    public class InterlevelGateInteractions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to signal we have a gate available.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to know when Prince starts to enter interlevel gate.")]
        [SerializeField] private CharacterStatus characterStatus;
        
        /// <summary>
        /// Interlevel gate gives Prince a reference to its interaction API when detects Prince
        /// is in front of gate. 
        /// </summary>
        public InterlevelGateCharacterInteractions InterlevelGate { get; set; }

        /// <summary>
        /// Interlevel gate set this to true if Prince character is in front of one of them and gate is open
        /// and ready to be entered.
        /// </summary>
        public bool InterlevelGateAvailable => InterlevelGate != null;

        private bool _gateAvailableAlreadySignaled = false;
        private bool _enteringGateAlreadySignaled = false;

        private void Update()
        {
            SignalIfInterlevelGateIsAvailable();

            SignalIfWeWantToEnterGate();
        }

        /// <summary>
        /// Signal character state machine than an inter level gate is available.
        /// </summary>
        private void SignalIfWeWantToEnterGate()
        {
            if (!_enteringGateAlreadySignaled &&
                characterStatus.CurrentState == CharacterStatus.States.EnteringInterlevelGate)
            {
                InterlevelGate.EnterGate();
                _enteringGateAlreadySignaled = true;
            }
        }

        /// <summary>
        /// Signal interlevel gate we want to enter it.
        /// </summary>
        private void SignalIfInterlevelGateIsAvailable()
        {
            if (InterlevelGateAvailable != _gateAvailableAlreadySignaled)
            {
                stateMachine.SetBool("InterlevelGateAvailable", InterlevelGateAvailable);
                _gateAvailableAlreadySignaled = InterlevelGateAvailable;
            }
        }
    }
}
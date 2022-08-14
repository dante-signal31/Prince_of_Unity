using System;
using UnityEngine;

namespace Prince
{
    public class TrapStatus : MonoBehaviour, IStateMachineStatus<TrapStatus.States>
    {
        public enum States
        {
            Idle,
            Activated,
            Deactivated,
            Killed
        }

        [Header("WIRING:")] 
        [Tooltip("Needed to play some sounds.")] 
        [SerializeField] private SoundController soundController;

        [Header("CONFIGURATION:")] 
        [Tooltip("Which kind of corpse this trap generates:")] 
        [SerializeField] private TrapInteractions.CorpseTypes killMode;

        public States CurrentState { get; set; }
        
        /// <summary>
        /// Whether this trap can kill a character at this moment.
        /// </summary>
        public bool CanKill { get; set; }

        /// <summary>
        /// Which kind of corpse this trap generates.
        /// </summary>
        public TrapInteractions.CorpseTypes KillMode => killMode;

        private bool _activationSoundAlreadyPlayed = false;

        private void Update()
        {
            switch (CurrentState)
            {
                case States.Activated:
                    if (!_activationSoundAlreadyPlayed)
                    {
                        soundController.PlaySound("activated");
                        _activationSoundAlreadyPlayed = true;
                    }
                    break;
                case States.Deactivated:
                    _activationSoundAlreadyPlayed = false;
                    break;
            }
        }
    }
}
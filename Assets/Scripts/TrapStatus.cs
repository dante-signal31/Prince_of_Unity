using System;
using System.Collections;
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
            Killed,
            Waiting
        }

        [Header("WIRING:")] 
        [Tooltip("Needed to set default flags given at configuration.")] 
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to play some sounds.")] 
        [SerializeField] private SoundController soundController;

        [Header("CONFIGURATION:")] 
        [Tooltip("Which kind of corpse this trap generates:")] 
        [SerializeField] private TrapInteractions.CorpseTypes killMode;
        [Tooltip("Whether this trap should activate-deactivate as a cyclic.")] 
        [SerializeField] private bool loop;
        [Tooltip("Time to wait to restart loop.")]
        [SerializeField] private float loopWaitingTime;

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

        private bool _waiting;
        
        /// <summary>
        /// Whether this trap is waiting to restart a loop.
        /// </summary>
        private bool Waiting
        {
            get => _waiting;
            set
            {
                _waiting = value;
                if (!_waiting) stateMachine.SetTrigger("WaitingEnded");
            }
        }

        private void Awake()
        {
            stateMachine.SetBool("Loop", loop);
        }

        private void FixedUpdate()
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
                case States.Waiting:
                    if (!Waiting)
                    {
                        Waiting = true;
                        StartCoroutine(SignalEndWaitingTime()); 
                    }
                    break;
            }
        }

        private IEnumerator SignalEndWaitingTime()
        {
            yield return new WaitForSeconds(loopWaitingTime);
            Waiting = false;
        }
    }
}
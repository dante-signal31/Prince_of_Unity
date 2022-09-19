using System;
using UnityEngine;

namespace Prince
{
    public class PortcullisStatus : MonoBehaviour, IStateMachineStatus<PortcullisStatus.PortcullisStates>
    {
        // TODO: Portcullis should disable climbable while closed or closing. Otherwise Prince could pass a portcullis climbing.
        [Header("WIRING:")]
        [Tooltip("Needed to set initial state.")]
        [SerializeField] private Animator stateMachine;

        [Header("CONFIGURATION:")] 
        [Tooltip("Initial state for this portcullis.")] 
        [SerializeField] private PortcullisStates initialState;
        
        public enum PortcullisStates
        {
            Closed,
            Opening,
            Open,
            ClosingFast,
            ClosingSlow
        }

        public PortcullisStates CurrentState { get; set; }

        private void Start()
        {
            SetInitialState();
        }

        /// <summary>
        /// Put portcullis at its given initial state.
        /// </summary>
        private void SetInitialState()
        {
            switch (initialState)
            {
                case PortcullisStates.Closed:
                    stateMachine.SetTrigger("InitialStateClosed");
                    break;
                case PortcullisStates.Open:
                    stateMachine.SetTrigger("InitialStateOpen");
                    break;
                case PortcullisStates.Opening:
                    stateMachine.SetTrigger("InitialStateOpening");
                    break;
                case PortcullisStates.ClosingFast:
                    stateMachine.SetTrigger("InitialStateClosingFast");
                    break;
                case PortcullisStates.ClosingSlow:
                    stateMachine.SetTrigger("InitialStateClosingSlow");
                    break;
            }
        }
    }
}
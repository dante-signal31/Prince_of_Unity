using System.Collections;
using UnityEngine;

namespace Prince
{
    // TODO: Interlevel gate closing sound is out rather out of sinch. Must fix it.
    
    public class InterlevelGateStatus : MonoBehaviour, IStateMachineStatus<InterlevelGateStatus.GateStates>
    {
        [Header("WIRING:")]
        [Tooltip("Needed to set initial state.")]
        [SerializeField] private Animator stateMachine;

        [Header("CONFIGURATION:")] 
        [Tooltip("Initial state for this portcullis.")] 
        [SerializeField] private GateStates initialState;
        
        public enum GateStates
        {
            Closed,
            Opening,
            Open,
            ClosingFast,
            Entering,
            Entered,
        }

        public GateStates CurrentState { get; set; }

        private void Start()
        {
            StartCoroutine(SetInitialState());
        }

        /// <summary>
        /// Put portcullis at its given initial state.
        /// </summary>
        private IEnumerator SetInitialState()
        {
            switch (initialState)
            {
                case GateStates.Closed:
                    stateMachine.SetTrigger("InitialStateClosed");
                    break;
                case GateStates.Open:
                    stateMachine.SetTrigger("InitialStateOpen");
                    break;
                case GateStates.Opening:
                    stateMachine.SetTrigger("InitialStateOpening");
                    break;
                case GateStates.ClosingFast:
                    stateMachine.SetTrigger("InitialStateClosingFast");
                    break;
                case GateStates.Entering:
                    stateMachine.SetTrigger("InitialStateOpen");
                    yield return new WaitForSeconds(0.5f);
                    stateMachine.SetTrigger("InitialStateEntering");
                    break;
                case GateStates.Entered:
                    stateMachine.SetTrigger("InitialStateOpen");
                    yield return new WaitForSeconds(0.5f);
                    stateMachine.SetTrigger("InitialStateEntered");
                    break;
            }
        }
    }
}
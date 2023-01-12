using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to keep track of current falling ground state.
    /// </summary>
    public class FallingGroundStatus : MonoBehaviour, IStateMachineStatus<FallingGroundStatus.FallingGroundStates>
    {
        [Header("WIRING:")]
        [Tooltip("Needed to signal starting state is falling.")]
        [SerializeField] private Animator stateMachine;

        [Header("CONFIGURATION:")] 
        [Tooltip("Whether it should fall directly at spawn.")] 
        [SerializeField] private bool startFalling;

        private bool _startFallingAlreadySignaled = false;

        private void Update()
        {
            if (startFalling && !_startFallingAlreadySignaled)
            {
                stateMachine.SetBool("Fall", true);
                _startFallingAlreadySignaled = true;
            }
        }

        public enum FallingGroundStates
        {
            Idle,
            Trembling,
            Falling,
            Crashing,
            Crashed
        }

        private FallingGroundStates _currentState;

        /// <summary>
        /// Current falling ground state.
        /// </summary>
        public FallingGroundStates CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
            }
        }
    }
}
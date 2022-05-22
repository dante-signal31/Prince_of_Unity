using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to keep track of current falling ground state.
    /// </summary>
    public class FallingGroundStatus : MonoBehaviour, IStateMachineStatus<FallingGroundStatus.FallingGroundStates>
    {
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
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// There is not an easy way to ask Animator which is the current state. So we make every state update
    /// an annotation at at its respective game object state machine status tracker.
    ///
    /// <ul>
    /// <li> T: Enum type with all states. </li>
    /// <li>TU: Type of state machine status tracker. </li>
    /// </ul>
    /// </summary>
    public class GenericStateUpdater<T,TU> : StateMachineBehaviour where TU: IStateMachineStatus<T>
    {
        [Header("CONFIGURATION:")]
        [Tooltip("State to set as active at character status.")]
        [SerializeField] private T stateToUpdate;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private TU _stateMachineStatus;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            // I have to initialize stateMachineStatus here because is where I get animator to go up and
            // get root game object which has our target game object state machine status tracker.
            if (_stateMachineStatus == null)
                _stateMachineStatus = animator.transform.root.gameObject.GetComponentInChildren<TU>();
            _stateMachineStatus.CurrentState = stateToUpdate;
            this.Log($"(StateUpdater - {animator.transform.root.name}) State changed to {stateToUpdate}", showLogs);
        }
        
    }
}
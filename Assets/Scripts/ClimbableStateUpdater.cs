using UnityEngine;

namespace Prince
{
    /// <summary>
    /// There is not an easy way to ask Animator which is the current state. So we make every state update
    /// an annotation at ClimbableStatus. 
    /// </summary>
    public class ClimbableStateUpdater : StateMachineBehaviour
    {
        [Header("CONFIGURATION:")]
        [Tooltip("State to set as active at character status.")]
        [SerializeField] private ClimbableStatus.States stateToUpdate;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private ClimbableStatus _climbableStatus;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            // I have to initialize climbableStatus here because is where I get animator to go up and
            // get parent gameobject which has our target ClimbableStatus.
            if (_climbableStatus == null)
                _climbableStatus = animator.transform.root.gameObject.GetComponentInChildren<ClimbableStatus>();
            _climbableStatus.CurrentState = stateToUpdate;
            this.Log($"(StateUpdater - {animator.transform.root.name}) State changed to {stateToUpdate}, " +
                     $"while looking to rightwards:{_climbableStatus.LookingRightWards}", showLogs);
        }
    }
}
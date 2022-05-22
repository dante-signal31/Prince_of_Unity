using System;
using UnityEngine;

namespace Prince
{
    // TODO: Make this component inherit from GenericStateUpdater, as FallingGroundStateUpdater does.
    /// <summary>
    /// There is not an easy way to ask Animator which is the current state. So we make every state update
    /// an annotation at CharacterStatus. 
    /// </summary>
    public class StateUpdater : StateMachineBehaviour
    {
        [Header("CONFIGURATION:")]
        [Tooltip("State to set as active at character status.")]
        [SerializeField] private CharacterStatus.States stateToUpdate;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private CharacterStatus _characterStatus;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            // I have to initialize characterStatus here because is where I get animator to go up and
            // get parent gameobject which has our target CharacterStatus.
            if (_characterStatus == null)
                _characterStatus = animator.transform.parent.gameObject.GetComponent<CharacterStatus>();
            _characterStatus.CurrentState = stateToUpdate;
            this.Log($"(StateUpdater - {animator.transform.root.name}) State changed to {stateToUpdate}, " +
                      $"while looking to rightwards:{_characterStatus.LookingRightWards}", showLogs);
        }
        
    }
}
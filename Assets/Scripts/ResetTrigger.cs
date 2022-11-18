using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Sometimes triggers get stuck. This behaviour cleans a trigger when an state is entered.
    /// </summary>
    public class ResetTrigger : StateMachineBehaviour
    {
        [Tooltip("Trigger to reset when this state is entered.")]
        [SerializeField] private string triggerToReset;

        [Tooltip("Reset trigger when state is entered.")]
        [SerializeField] private bool resetOnEnter = true;
        [Tooltip("Reset trigger when state is left.")]
        [SerializeField] private bool resetOnExit;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            if (resetOnEnter) animator.ResetTrigger(triggerToReset);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            if (resetOnExit) animator.ResetTrigger(triggerToReset);
        }
        
        // public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
        //     int layerIndex)
        // {
        // }
        //
        // public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
        //     int layerIndex)
        // {
        // }
        //
        // public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
        //     int layerIndex)
        // {
        // }
    }
}
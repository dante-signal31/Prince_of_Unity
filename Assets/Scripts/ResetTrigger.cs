using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Sometimes triggers get stuck. This behaviour cleans a trigger when an state is entered.
    /// </summary>
    public class ResetTrigger : StateMachineBehaviour
    {
        // TODO: Add an enum to define when to reset trigger (on enter, exit or update).
        [Tooltip("Trigger to reset when this state is entered.")]
        [SerializeField] private string triggerToReset;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.ResetTrigger(triggerToReset);
        }

        // public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
        //     int layerIndex)
        // {
        // }
        //
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
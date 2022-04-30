using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Sometimes bool get stuck between state machine cycles.
    /// This behaviour cleans a bool when an state is entered.
    /// </summary>
    public class ResetBool : StateMachineBehaviour
    {
        [Tooltip("Trigger to reset when this state is entered.")]
        [SerializeField] private string boolToReset;
        [Tooltip("Bool value to set.")]
        [SerializeField] private bool defaultValue;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.SetBool(boolToReset, defaultValue);
        }
    }
}
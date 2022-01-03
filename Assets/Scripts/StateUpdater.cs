using UnityEngine;

namespace Prince
{
    /// <summary>
    /// There is not an easy way to ask Animator which is the current state. So we make every state update
    /// an annotation at CharacterStatus. 
    /// </summary>
    public class StateUpdater : StateMachineBehaviour
    {
        [SerializeField] private CharacterStatus.States stateToUpdate;
        
        // I was not able to set this field through inspector nor from this class. So it is initialized
        // from CharacterStatus LinkWithStateMachine().
        [HideInInspector]
        public CharacterStatus characterStatus;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            characterStatus.CurrentState = stateToUpdate;
            Debug.Log($"(StateUpdater) State changed to {stateToUpdate}");
        }
        
    }
}
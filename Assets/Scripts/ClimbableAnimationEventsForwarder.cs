using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Animators can send animations events only to components in the same transform. I don't want
    /// to overload the CharacterAnimations subtransform, so this component is next to Animator to
    /// forward events to every other component scattered through the game object.
    /// </summary>
    public class ClimbableAnimationEventsForwarder : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to signal abortable climbing period.")]
        [SerializeField] private ClimbableStatus climbableStatus;

        public void AbortClimbingChanceStart()
        {
            climbableStatus.ClimbingAbortable = true;
        }

        public void AbortClimbingChanceEnd()
        {
            climbableStatus.ClimbingAbortable = false;
        }
    }
}
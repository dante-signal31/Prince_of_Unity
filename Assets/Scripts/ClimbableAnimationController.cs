using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to control the animations of climbing/hanging/descending.
    /// </summary>
    public class ClimbableAnimationController : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to show and hide animations.")] 
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Tooltip("Needed to know where to place animations for left ledge animations.")]
        [SerializeField] private Transform leftLedgeTransform;
        [Tooltip("Needed to know where to place animations for right ledge animations.")]
        [SerializeField] private Transform rightLedgeTransform;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        /// <summary>
        /// True if climbing/hanging/descending animations are visible.
        /// </summary>
        public bool AnimationEnabled => spriteRenderer.enabled;
        
        /// <summary>
        /// Make animations invisible.
        /// </summary>
        public void HideAnimation()
        {
            spriteRenderer.enabled = false;
        }
        
        /// <summary>
        /// Make animations visible.
        /// </summary>
        public void ShowAnimation()
        {
            spriteRenderer.enabled = true;
        }

        /// <summary>
        /// Place animation depending on the side the character is facing when hanging.
        /// </summary>
        /// <param name="lookingRightWards"></param>
        public void PlaceAnimation(bool lookingRightWards)
        {
            SetAnimationOrientation(lookingRightWards);
            SetAnimationPosition(lookingRightWards);
            this.Log($"(ClimbableAnimationController - {transform.root.name}) Climbable animation reconfigured to lookingRightWards: {lookingRightWards}.", showLogs);
        } 
        
        private void SetAnimationOrientation(bool lookingRightWards)
        {
            spriteRenderer.flipX = !lookingRightWards;
        }

        private void SetAnimationPosition(bool lookingRightWards)
        {
            transform.position = (lookingRightWards)
                ? leftLedgeTransform.position 
                : rightLedgeTransform.position;
        }
    }
}
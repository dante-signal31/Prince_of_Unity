using UnityEngine;

namespace Prince
{
    /// <summary>
    /// FallingGround uses the same appearance features than Ground but adds the capacity to hide when crashes.
    /// </summary>
    public class FallingGroundAppearance: GroundAppearance
    {
        [Header("SUB-WIRING:")]
        [Tooltip("Needed to know when we are crashing.")]
        [SerializeField] private FallingGroundStatus fallingGroundStatus;

        private void Update()
        {
            switch (fallingGroundStatus.CurrentState)
            {
                case FallingGroundStatus.FallingGroundStates.Crashing:
                case FallingGroundStatus.FallingGroundStates.Crashed:
                    HideGround();
                    break;
                default:
                    ShowGround();
                    break;
            }
        }
        
        /// <summary>
        /// Make this ground invisible.
        ///
        /// Used in falling ground to hide ground when it crashes against below brick.
        /// </summary>
        public void HideGround()
        {
            if (!_hidden)
            {
                groundSpriteRenderer.enabled = false;
                occluderSpriteRenderer.enabled = false;
                rubbishAppearance.ShowRubbish = false;
                _hidden = true;
            }
        }

        /// <summary>
        /// Make this ground visible.
        /// </summary>
        public void ShowGround()
        {
            if (_hidden)
            {
                groundSpriteRenderer.enabled = true;
                occluderSpriteRenderer.enabled = true;
                rubbishAppearance.ShowRubbish = true;
                _hidden = false;
            }
        }
    }
}
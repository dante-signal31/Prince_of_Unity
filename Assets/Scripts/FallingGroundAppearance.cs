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
        [Tooltip("Needed to signal state machine whether we have border or not.")] 
        [SerializeField] private Animator stateMachine;

        private bool _currentShowBorder;

        private void Awake()
        {
            UpdateBorder();
        }

        private void Update()
        {
            UpdateBorder();
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

        /// <summary>
        /// Update border only if base GroundAppearance hasBorder value has change.
        /// </summary>
        private void UpdateBorder()
        {
            if (_currentShowBorder != base.hasBorder)
            {
                _currentShowBorder = base.hasBorder;
                ShowBorder(_currentShowBorder);
            }
        }

        /// <summary>
        /// Despite what happens with Ground, it seems that animator setting prevails over
        /// direct sprite access to show or not border in FallingGround. So in FallingGround
        /// you must have a one frame animation with border and another without it.
        /// </summary>
        /// <param name="showIt">Whether to show border or not.</param>
        public new void ShowBorder(bool showIt)
        {
            _currentShowBorder = showIt;
            base.ShowBorder(showIt);
            stateMachine.SetBool("ShowBorder", showIt);
        }
    }
}
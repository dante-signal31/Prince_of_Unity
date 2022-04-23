using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component keeps current state from climbable state machine.
    /// </summary>
    public class ClimbableStatus : MonoBehaviour
    {

        [Header("WIRING:")] 
        [Tooltip("Needed to show and hide animations.")] 
        [SerializeField] private ClimbableAnimationController animationController;

        public enum States
        {
            Hanging,
            Climbing,
            HangingLong,
            HangingBlocked,
            Inactive,
            Descending
        }
        
        /// <summary>
        /// Current animation state.
        /// </summary>
        public States CurrentState{ get; set; }
        
        /// <summary>
        /// True if no animation is being played.
        /// </summary>
        public bool Inactive => CurrentState == States.Inactive;

        private bool _lookingRightWards;
        
        /// <summary>
        /// Whether player animations this component shows are looking rightwards.
        /// </summary>
        public bool LookingRightWards
        {
            get => _lookingRightWards;
            set
            {
                if (value != _lookingRightWards)
                {
                    animationController.PlaceAnimation(value);
                    _lookingRightWards = value;
                }
            }
        }

        private void Start()
        {
            animationController.PlaceAnimation(LookingRightWards);
        }

        private void Update()
        {
            switch (CurrentState)
            {
                case States.Inactive:
                    if (animationController.AnimationEnabled) animationController.HideAnimation();
                    break;
                default:
                    if (!animationController.AnimationEnabled) animationController.ShowAnimation();
                    break;
            }
        }
    }
}
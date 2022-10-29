using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to manage switch visual appearance. 
    /// </summary>
    [ExecuteAlways]
    public class SwitchAppearance : MonoBehaviour, IBorder
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to change animation depending of parameters set.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Sprites for editor previewing.")] 
        [SerializeField] private SpriteRenderer groundSpriteRenderer;
        [SerializeField] private SpriteRenderer occluderSpriteRenderer;
        [SerializeField] private Sprite noBorderSprite;
        [SerializeField] private Sprite borderSprite;
        [SerializeField] private Sprite noBorderLefEdgeSprite;
        [SerializeField] private Sprite borderLeftEdgeSprite;
        [SerializeField] private Sprite occluderSprite;
        
        [Header("CONFIGURATION:")] 
        [Tooltip("Has this brick a hole at right so its border is visible?")]
        [SerializeField] private bool hasBorder;
        [Tooltip("Has this brick a hole at left?")]
        [SerializeField] private bool AtLeftEdge;
        [Tooltip("Animation controller for different parameter options.")] 
        [SerializeField] private AnimatorOverrideController AnimatorControllerBorder;
        [SerializeField] private AnimatorOverrideController AnimatorControllerBorderLeftEdge;
        [SerializeField] private AnimatorOverrideController AnimatorControllerNoBorder;
        [SerializeField] private AnimatorOverrideController AnimatorControllerNoBorderLeftEdge;
        
        private bool _appearanceUpdateNeeded = false;

        private void Awake()
        {
            UpdateAnimationController();
        }

        private void UpdateAnimationController()
        {
            switch ((hasBorder, AtLeftEdge))
            {
                case (true, false):
                    stateMachine.runtimeAnimatorController = AnimatorControllerBorder;
                    break;
                case (true, true):
                    stateMachine.runtimeAnimatorController = AnimatorControllerBorderLeftEdge;
                    break;
                case (false, false):
                    stateMachine.runtimeAnimatorController = AnimatorControllerNoBorder;
                    break;
                case (false, true):
                    stateMachine.runtimeAnimatorController = AnimatorControllerNoBorderLeftEdge;
                    break;
            }
        }

        private void UpdateAppearance()
        {
            switch ((hasBorder, AtLeftEdge))
            {
                case (true, false):
                    groundSpriteRenderer.sprite = borderSprite;
                    break;
                case (true, true):
                    groundSpriteRenderer.sprite = borderLeftEdgeSprite;
                    break;
                case (false, false):
                    groundSpriteRenderer.sprite = noBorderSprite;
                    break;
                case (false, true):
                    groundSpriteRenderer.sprite = noBorderLefEdgeSprite;
                    break;
            }

            occluderSpriteRenderer.sprite = occluderSprite;
        }

        private void OnValidate()
        {
            _appearanceUpdateNeeded = true;
        }
        
        /// <summary>
        /// Update appearance if needed.
        /// </summary>
        private void LateUpdate()
        {
            #if UNITY_EDITOR
            if (_appearanceUpdateNeeded)
            {
                UpdateAppearance();
                _appearanceUpdateNeeded = false; 
            }
            #endif
        }

        public bool IsBorderShown()
        {
            return hasBorder;
        }
        
        public void ShowBorder(bool showIt)
        {
            hasBorder = showIt;
            UpdateAnimationController();
        }
    }
}
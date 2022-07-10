using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to manage switch visual appearance. 
    /// </summary>
    public class SwitchAppearance : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to change animation depending of parameters set.")]
        [SerializeField] private Animator stateMachine;
        
        [Header("CONFIGURATION:")] 
        [Tooltip("Has this brick a hole at right so its border is visible?")]
        [SerializeField] private bool ShowBorder;
        [Tooltip("Has this brick a hole at left?")]
        [SerializeField] private bool AtLeftEdge; 
        [Tooltip("Animation controller for different parameter options.")] 
        [SerializeField] private AnimatorOverrideController AnimatorControllerBorder;
        [SerializeField] private AnimatorOverrideController AnimatorControllerBorderLeftEdge;
        [SerializeField] private AnimatorOverrideController AnimatorControlleNoBorder;
        [SerializeField] private AnimatorOverrideController AnimatorControllerNoBorderLeftEdge;
        

        private void Awake()
        {
            UpdateAnimationController();
        }

        private void UpdateAnimationController()
        {
            switch ((ShowBorder, AtLeftEdge))
            {
                case (true, false):
                    stateMachine.runtimeAnimatorController = AnimatorControllerBorder;
                    break;
                case (true, true):
                    stateMachine.runtimeAnimatorController = AnimatorControllerBorderLeftEdge;
                    break;
                case (false, false):
                    stateMachine.runtimeAnimatorController = AnimatorControlleNoBorder;
                    break;
                case (false, true):
                    stateMachine.runtimeAnimatorController = AnimatorControllerNoBorderLeftEdge;
                    break;
            }
        }

        private void OnValidate()
        {
            UpdateAnimationController();
        }
    }
}
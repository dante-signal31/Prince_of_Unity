using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component is used at portcullis to avoid it to be climbable when it is closed.
    /// </summary>
    public class PortcullisClimbableDisabler : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when portcullis is closed.")] 
        [SerializeField] private PortcullisStatus portcullisStatus;
        [Tooltip("Needed to disable it when portcullis is closed.")] 
        [SerializeField] private GameObject climbableComponent;
        [Tooltip("Needed to know how much gap left to climb through.")]
        [SerializeField] private IronCurtainController ironCurtainController;

        [Header("CONFIGURATION:")] 
        [Tooltip("Minimum opening to climb.")]
        [Range(0, 1)]
        [SerializeField] private float minimumOpening;

        private ClimbableStatus _climbableStatus;
        
        private void Awake()
        {
            _climbableStatus = climbableComponent.GetComponentInChildren<ClimbableStatus>();
        }

        private void FixedUpdate()
        {
            switch (portcullisStatus.CurrentState)
            {
                case PortcullisStatus.PortcullisStates.Closed:
                case PortcullisStatus.PortcullisStates.ClosingFast:
                    // if (climbableComponent.activeSelf)
                    // {
                    //     // switch (_climbableStatus.CurrentState)
                    //     // {
                    //     //     case ClimbableStatus.States.Hanging:
                    //     //     case ClimbableStatus.States.HangingBlocked:
                    //     //     case ClimbableStatus.States.HangingLong:
                    //     //         _climbableStatus.ClimbingClear = false;
                    //     //         break;
                    //     //     default:
                    //     //         climbableComponent.SetActive(false);
                    //     //         break;
                    //     // }
                    // }
                    _climbableStatus.ClimbingClear = false;
                    break;
                case PortcullisStatus.PortcullisStates.ClosingSlow:
                    if (ironCurtainController.GateOpening < minimumOpening && climbableComponent.activeSelf)
                    {
                        // switch (_climbableStatus.CurrentState)
                        // {
                        //     case ClimbableStatus.States.Inactive:
                        //         climbableComponent.SetActive(false);
                        //         break;
                        //     case ClimbableStatus.States.Hanging:
                        //     case ClimbableStatus.States.HangingBlocked:
                        //     case ClimbableStatus.States.HangingLong:
                        //         _climbableStatus.ClimbingClear = false;
                        //         break;
                        //     default:
                        //         break;
                        // }
                        _climbableStatus.ClimbingClear = false;
                    }
                    break;
                default:
                    if (!climbableComponent.activeSelf) climbableComponent.SetActive(true);
                    if (!_climbableStatus.ClimbingClear) _climbableStatus.ClimbingClear = true;
                    break;
            }
        }
    }
}
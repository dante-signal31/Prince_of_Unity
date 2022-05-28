using UnityEngine;

namespace Prince
{
    public class FallingGroundCharacterInteractions : MonoBehaviour
    {
        [Header("WIRING")]
        [Tooltip("Needed to know current falling ground state")]
        [SerializeField] private FallingGroundStatus fallingGroundStatus;
        [Tooltip("Needed to signal we are going to fall.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to play sound when ground is about to fall.")]
        [SerializeField] private SoundController soundController;

        /// <summary>
        /// When weight sensor is triggered, then this brick must fall.
        /// </summary>
        public void OnCharacterWeightSensorTriggered()
        {
            stateMachine.SetTrigger("Fall");
            soundController.PlaySound("ground_moving_2");
        }
    }
}
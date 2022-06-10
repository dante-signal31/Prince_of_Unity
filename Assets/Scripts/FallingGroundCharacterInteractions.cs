using System;
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
        [Tooltip("Needed to know how much this ground has fallen.")]
        [SerializeField] private FallingGroundFallenHeightCounter fallingHeightCounter;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private bool _fallingSoundAlreadyPlayed = false;
        
        /// <summary>
        /// When weight sensor is triggered, then this brick must fall.
        /// </summary>
        public void OnCharacterWeightSensorTriggered()
        {
            stateMachine.SetBool("Fall", true);
        }

        /// <summary>
        /// When a character is hit, then we must hurt him.
        ///
        /// The inflicted damage depends on how much this ground has fallen. 
        /// </summary>
        /// <param name="characterHit">Character to apply damage.</param>
        public void OnCharacterHit(GameObject characterHit)
        {
            int damage = (int)Mathf.Ceil(fallingHeightCounter.FallenHeight/2);
            HealthController characterHitHealthController = characterHit.GetComponentInChildren<HealthController>();
            characterHitHealthController.GroundHit(damage);
            this.Log($"(FallingGroundCharacterInteractions - {transform.root.name}) Character hit wit {damage} damage.", showLogs);
        }

        private void Update()
        {
            switch (fallingGroundStatus.CurrentState)
            {
                case FallingGroundStatus.FallingGroundStates.Falling:
                    if (!_fallingSoundAlreadyPlayed)
                    {
                        soundController.PlaySound("ground_moving_2");
                        _fallingSoundAlreadyPlayed = true;
                    }
                    break;
            }
        }
    }
}
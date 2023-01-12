using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to manage falling ground interactions with characters.
    /// </summary>
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
        [Tooltip("Needed to know when a climbing is in progress.")]
        [SerializeField] private ClimbableStatus climbableStatus;
        [Tooltip("Needed to abort character climbing if he tries it while ground falls.")] 
        [SerializeField] private Climbable climbable;

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
            if (fallingGroundStatus.CurrentState == FallingGroundStatus.FallingGroundStates.Falling)
            {
                if (characterHit.GetComponentInChildren<CharacterStatus>().CurrentState != CharacterStatus.States.Falling)
                {
                    int damage = (int)Mathf.Ceil(fallingHeightCounter.FallenHeight/2);
                    HealthController characterHitHealthController = characterHit.GetComponentInChildren<HealthController>();
                    characterHitHealthController.GroundHit(damage);
                    this.Log($"(FallingGroundCharacterInteractions - {transform.root.name}) Character hit wit {damage} damage.", showLogs);
                }
            }
        }

        private void FixedUpdate()
        {
            switch (fallingGroundStatus.CurrentState)
            {
                case FallingGroundStatus.FallingGroundStates.Falling:
                    playFallingSound();
                    signalClimbingCharacterIfAny();
                    break;
            }
        }

        /// <summary>
        /// If any character is climbing this brick when it falls we need to warn him so he can
        /// update his state accordingly.
        /// </summary>
        private void signalClimbingCharacterIfAny()
        {
            switch (climbableStatus.CurrentState)
            {
                case ClimbableStatus.States.Hanging:
                case ClimbableStatus.States.HangingBlocked:
                case ClimbableStatus.States.HangingLong:
                case ClimbableStatus.States.Climbing:
                case ClimbableStatus.States.Descending:
                    signalCharacterToEndHanging();
                    break;
                case ClimbableStatus.States.Inactive:
                    break;
            }
        }

        private void playFallingSound()
        {
            if (!_fallingSoundAlreadyPlayed)
            {
                soundController.PlaySound("ground_moving_2");
                _fallingSoundAlreadyPlayed = true;
            }
        }

        /// <summary>
        /// Make character to end his hanging.
        /// </summary>
        private void signalCharacterToEndHanging()
        {
            climbable.AbortClimbing();
        }
    }
}
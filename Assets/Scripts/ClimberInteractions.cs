using System;
using System.Collections;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component that handles climbing, hanging and descending interactions with Climbable game objects.
    /// </summary>
    public class ClimberInteractions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when we enter climbing states.")] 
        [SerializeField] private CharacterStatus characterStatus;
        [Tooltip("Needed to send signals to character state machine")] 
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to reposition character after climbing.")] 
        [SerializeField] private Transform characterTransform;
        [Tooltip("Needed to get a reference to climbable game object.")]
        [SerializeField] private CeilingSensors ceilingSensors;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private enum ClimbingOptions
        {
            Climb,
            Descend,
        }
        
        /// <summary>
        /// Whether the character is currently climbing.
        /// </summary>
        private bool ClimbingInProgress => _climbable != null;
        
        private Climbable _climbable;
        
        
        private void FixedUpdate()
        {
            switch (characterStatus.CurrentState)
            {
                case CharacterStatus.States.Climbing:
                    if (!ClimbingInProgress) StartCoroutine(Climb());
                    break;
            }
        }

        private IEnumerator Climb()
        {
            _climbable = ceilingSensors.Ledge.GetComponentInChildren<Climbable>();
            if (_climbable != null)
            {
                this.Log($"(ClimberInteractions - {transform.parent.transform.gameObject.name}) Starting climbing.", showLogs);
                Climbable.HangableLedges hangingLedge = (characterStatus.LookingRightWards)
                    ? Climbable.HangableLedges.Left
                    : Climbable.HangableLedges.Right;
                yield return _climbable.Hang(hangingLedge);
                UpdateCharacterPosition(hangingLedge, ClimbingOptions.Climb);
                // yield return new WaitUntil(() => !_climbable.PlayingAnimations);
                stateMachine.SetTrigger("ClimbingFinished");
                _climbable = null;
                this.Log($"(ClimberInteractions - {transform.parent.transform.gameObject.name}) Climbing finished.", showLogs);
            }
        }

        /// <summary>
        /// Translates the character to the correct position after climbing or descending.
        /// </summary>
        /// <param name="hangingLedge">The side we are climbing or descending.</param>
        /// <param name="climbingOption">Whether we are climbing or descending.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void UpdateCharacterPosition(Climbable.HangableLedges hangingLedge, ClimbingOptions climbingOption)
        {
            Vector3 newPosition = hangingLedge switch {
                Climbable.HangableLedges.Left => (climbingOption == ClimbingOptions.Climb)
                    ? _climbable.LeftLedgeTransform.position
                    : _climbable.LeftDescendingTransform.position,
                Climbable.HangableLedges.Right => (climbingOption == ClimbingOptions.Climb)
                    ? _climbable.RightLedgeTransform.position
                    : _climbable.RightDescendingTransform.position,
                _ => throw new ArgumentOutOfRangeException(nameof(hangingLedge), hangingLedge, null)
            };
            characterTransform.position = new Vector3(newPosition.x, newPosition.y, characterTransform.position.z);
        }

    }
}
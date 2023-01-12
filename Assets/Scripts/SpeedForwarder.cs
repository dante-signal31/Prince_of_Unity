using UnityEngine;

namespace Prince
{
    public class SpeedForwarder : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Used to link animation speed curves to speed float values at MovementProfile.")]
        [SerializeField] CharacterMovement characterMovement;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;

        
        // Value from 0 to 1. This value is set by current animation curve and is multiplied 
        // with this state maximum movement speed.
        // Not every animation set this value through an animation curve, only those that have
        // a non constant movement speed value along its frames.
        [HideInInspector]
        [SerializeField] private float currentSpeedProportion;

        private MovementProfile _movementProfile;

        private void Awake()
        {
            _movementProfile = characterMovement.CharacterMovementProfile;
        }

        private void FixedUpdate()
        {
            _movementProfile.CurrentSpeedProportion = currentSpeedProportion;
            this.Log($"(SpeedForwarder - {transform.root.name}) Current speed proportion {currentSpeedProportion} and movement profile speed proportion {_movementProfile.CurrentSpeedProportion}", showLogs);
        }
    }
}
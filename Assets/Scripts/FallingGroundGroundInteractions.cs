using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component detects when a falling ground lands and makes it disappear leaving garbage.
    /// </summary>
    public class FallingGroundGroundInteractions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know how are we to ground below.")]
        [SerializeField] private FallingGroundCrashSensor fallingGroundCrashSensor;
        [Tooltip("Needed to know when we are falling.")]
        [SerializeField] private FallingGroundStatus fallingGroundStatus;
        [Tooltip("Needed to signal state machine when to crash.")] 
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to play crashing sound.")] 
        [SerializeField] private SoundController soundController;

        [Header("CONFIGURATION:")] 
        [Tooltip("Distance to ground below to activate crash.")] 
        [SerializeField] private float crashDistance;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;

        private bool _trackingEnabled;
        private GameObject _groundBelow;
        private float _distanceToGroundBelow;

        private void Awake()
        {
            _trackingEnabled = false;
        }

        private void FixedUpdate()
        {
            _trackingEnabled = (fallingGroundStatus.CurrentState == FallingGroundStatus.FallingGroundStates.Falling);
            if (_trackingEnabled && fallingGroundCrashSensor.GroundBelowDetected)
            {
                UpdateDistanceToGroundBelow();
                if (_distanceToGroundBelow < crashDistance) Crash();
            }
            
        }

        /// <summary>
        /// Get distance to brick we are falling over.
        /// </summary>
        /// <returns>Distance to brick.</returns>
        private void UpdateDistanceToGroundBelow()
        {
            _distanceToGroundBelow = fallingGroundCrashSensor.DistanceToGroundBelow;
            _groundBelow = fallingGroundCrashSensor.CrashingOverGround;
            this.Log($"(FallingGroundGroundInteractions - {transform.root.name}) Distance to ground below: {_distanceToGroundBelow}.", showLogs);
        }

        /// <summary>
        /// Manages crashing over below floor.
        /// </summary>
        private void Crash()
        {
            stateMachine.SetTrigger("Crash");
            soundController.PlaySound("ground_crashing");
            _groundBelow.GetComponentInChildren<GroundRubbishAppearance>().PlaceThingsOverGround(ThingsOverGround.Garbage);
            this.Log($"(FallingGroundGroundInteractions - {transform.root.name}) Crash triggered.", showLogs);
        }
    }
}
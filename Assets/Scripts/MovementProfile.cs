using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This ScriptableObject is loaded into a character to give it its movement speeds.
    /// </summary>
    [CreateAssetMenu(fileName = "MovementProfile", menuName = "ScriptableObjects/MovementProfile", order = 2)]
    public class MovementProfile : ScriptableObject
    {
        public float AdvanceWithSwordSpeed;
        public float RetreatSpeed;
        public float HitBySwordSpeed;
        public float BlockSwordSpeed;
        public float MaximumRunningSpeed;
        public float MaximumWalkingSpeed;
        public float MaximumStandingSpeed;
        public float MaximumCrouchWalkingSpeed;
        public float FallingHorizontalSpeed;
        public float RunningJumpingImpulseSpeed;
        public float RunningJumpingSpeed;

        // Next value is automatically set by SpeedForwarder in every FixedUpdate. So, it should not be shown 
        // in inspector. 
        private float _currentSpeedProportion;
        public float CurrentSpeedProportion
        {
            get=> _currentSpeedProportion;
            set
            {
                _currentSpeedProportion = Mathf.Clamp(value, 0.0f, 1.0f);
            }
        }

        /// <summary>
        /// Movement speed at current point of running animation.
        /// </summary>
        public float CurrentRunningSpeed => Lerp(CurrentSpeedProportion, MaximumRunningSpeed);

        /// <summary>
        /// Movement speed at current point of walking animation.
        /// </summary>
        public float CurrentWalkingSpeed => Lerp(CurrentSpeedProportion, MaximumWalkingSpeed);

        /// <summary>
        /// Movement speed at current point of walking animation.
        /// </summary>
        public float CurrentStandingSpeed => Lerp(CurrentSpeedProportion, MaximumStandingSpeed);
        
        /// <summary>
        /// Movement speed at current point of crouching walking animation.
        /// </summary>
        public float CurrentCrouchWalkingSpeed => Lerp(CurrentSpeedProportion, MaximumCrouchWalkingSpeed);
        
        /// <summary>
        /// Linear interpolation of maximum speed depending on current speed proportion.
        ///
        /// Actually is a wrapper of Math.Lerp() but I've read that method has problem at the
        /// edges, so here a set those edges values manually.
        /// </summary>
        /// <param name="currentSpeedProportion"></param>
        /// <param name="maximumSpeed"></param>
        /// <returns></returns>
        private static float Lerp(float currentSpeedProportion, float maximumSpeed)
        {
            if (currentSpeedProportion > 0 && currentSpeedProportion < 1)
            {
                return Mathf.Lerp(0, maximumSpeed, currentSpeedProportion);
            }
            else if (currentSpeedProportion <= 0)
            {
                return 0;
            }
            else
            {
                return maximumSpeed;
            }
        }
    }
}
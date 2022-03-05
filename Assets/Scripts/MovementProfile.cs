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

        public float CurrentRunningSpeed
        {
            get
            {
                if (CurrentSpeedProportion > 0 && CurrentSpeedProportion < 1)
                {
                    return Mathf.Lerp(0, MaximumRunningSpeed ,CurrentSpeedProportion);
                } 
                else if (CurrentSpeedProportion == 0)
                {
                    return 0;
                }
                else
                {
                    return MaximumRunningSpeed;
                }
            }
        }
    }
}
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
        
    }
}
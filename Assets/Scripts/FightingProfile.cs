using UnityEngine;

namespace Prince
{
    [CreateAssetMenu(fileName = "FightingProfile", menuName = "ScriptableObjects/GuardFightingProfile", order = 3)]
    public class FightingProfile : ScriptableObject
    {
        [Tooltip("Probability this guard is going to Prince straightly. The lesser probability the higher likeness this guard stays in his position.")]
        [Range(0,1)]
        public float boldness;

        [Tooltip("Probability this guard is going to attack when he is at hitting range. The lesser probability the higher likeness this guard stays idle.")]
        [Range(0,1)]
        public float attack;

        [Tooltip("Probability this guard is going to block when he is attacked. The lesser probability the higher likeness this guard receives a hit.")]
        [Range(0,1)]
        public float defense;
    }
}
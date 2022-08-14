using UnityEngine;

namespace Prince
{
    public class TrapAnimationEventsForwarder : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to play sound effects.")]
        [SerializeField] private SoundController soundController;
        [Tooltip("Needed to signal when trap can kill or not.")] 
        [SerializeField] private TrapStatus trapStatus;

        public void PlaySound(string clipName)
        {
            soundController.PlaySound(clipName);
        }

        public void CanKill(bool isLethal)
        {
            trapStatus.CanKill = isLethal;
        }
    }
}
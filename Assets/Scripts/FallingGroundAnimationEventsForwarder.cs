using UnityEngine;

namespace Prince
{
    public class FallingGroundAnimationEventsForwarder : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to show and hide occluder.")] 
        [SerializeField] private SpriteRenderer occluderSprite;
        [Tooltip("Needed to play sounds.")]
        [SerializeField] private SoundController soundController;

        public void HideOccluder()
        {
            occluderSprite.enabled = false;
        }

        public void ShowOccluder()
        {
            occluderSprite.enabled = true;
        }
        
        public void PlaySound(string clipName)
        {
            soundController.PlaySound(clipName);
        }
    }
}
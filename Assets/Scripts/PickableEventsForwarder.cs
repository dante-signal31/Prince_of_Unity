using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Animators can send animations events only to components in the same transform. I don't want
    /// to overload the Appearance subtransform, so this component is next to Animator to
    /// forward events to every other component scattered through the game object.
    /// </summary>
    public class PickableEventsForwarder : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to trigger flash in its precise instant.")] 
        [SerializeField] private PickableFlash flashController;
        [Tooltip("Needed to play not music sounds.")]
        [SerializeField] private SoundController soundController;
        
        private EventBus _eventBus;
        
        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponent<EventBus>();
        }

        public void ShowFlash()
        {
            flashController.ShowFlashes();
        }

        public void RaisePotionTakenEvent()
        {
            _eventBus.TriggerEvent(new GameEvents.SmallPotionTaken(), this);
        }

        public void RaiseSwordTaken()
        {
            _eventBus.TriggerEvent(new GameEvents.SwordTaken(), this);
        }

        public void PlaySound(string clip)
        {
            soundController.PlaySound(clip);
        }
    }
}
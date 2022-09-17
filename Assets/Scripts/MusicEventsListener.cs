using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Listens for music events and plays required clip at level camera sound controller.
    /// </summary>
    public class MusicEventsListener : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to receive music requests")]
        [SerializeField] private EventBus eventBus;
        [Tooltip("Needed to play requested music.")]
        [SerializeField] private SoundController soundController;

        private void Start()
        {
            eventBus.AddListener<GameEvents.SmallPotionTaken>(OnSmallPotionTaken);
            eventBus.AddListener<GameEvents.SwordTaken>(OnSwordTaken);
        }

        // private void OnEnable()
        // {
        //     
        // }

        private void OnDisable()
        {
            eventBus.RemoveListener<GameEvents.SmallPotionTaken>(OnSmallPotionTaken);
            eventBus.RemoveListener<GameEvents.SwordTaken>(OnSwordTaken);
        }

        private void OnSmallPotionTaken(object _, GameEvents.SmallPotionTaken __)
        {
            soundController.PlaySound("small_potion");
        }

        private void OnSwordTaken(object _, GameEvents.SwordTaken __)
        {
            soundController.PlaySound("sword_victory");
        }
    }
}
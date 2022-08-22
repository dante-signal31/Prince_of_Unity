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
            eventBus.AddListener<GameEvents.MusicEvent>(PlayMusic);
        }

        private void PlayMusic(object _, GameEvents.MusicEvent ev)
        {
            soundController.PlaySound(ev.ClipToPlay);
        }
    }
}
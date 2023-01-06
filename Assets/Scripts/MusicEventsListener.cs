using System;
using UnityEngine;

namespace Prince
{
    // TODO: Check if music should be played when guards deads.

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

        [Header("CONFIGURATION:")] 
        [Tooltip("Time in seconds to delay playing dead music start.")] 
        [SerializeField] private float deadMusicDelay;

        private void Start()
        {
            eventBus.AddListener<GameEvents.SmallPotionTaken>(OnSmallPotionTaken);
            eventBus.AddListener<GameEvents.SwordTaken>(OnSwordTaken);
            eventBus.AddListener<GameEvents.LevelLoaded>(OnLevelLoaded);
            eventBus.AddListener<GameEvents.LevelReloaded>(OnLevelLoaded);
            eventBus.AddListener<GameEvents.PrinceDead>(OnPrinceDead);
        }

        private void OnDisable()
        {
            eventBus.RemoveListener<GameEvents.SmallPotionTaken>(OnSmallPotionTaken);
            eventBus.RemoveListener<GameEvents.SwordTaken>(OnSwordTaken);
            eventBus.RemoveListener<GameEvents.LevelLoaded>(OnLevelLoaded);
            eventBus.RemoveListener<GameEvents.LevelReloaded>(OnLevelLoaded);
            eventBus.RemoveListener<GameEvents.PrinceDead>(OnPrinceDead);
        }

        private void OnSmallPotionTaken(object _, GameEvents.SmallPotionTaken __)
        {
            soundController.PlaySound("small_potion");
        }

        private void OnSwordTaken(object _, GameEvents.SwordTaken __)
        {
            soundController.PlaySound("sword_victory");
        }

        private void OnLevelLoaded(object _, GameEvents.LevelLoaded ev)
        {
            switch (ev.LevelName)
            {
                case "Level 1":
                    Invoke(nameof(PlayLevel1Intro), 2);
                    break;
            }
        }

        private void PlayLevel1Intro()
        {
            soundController.PlaySound("level1_intro");
        }

        private void OnPrinceDead(object _, GameEvents.PrinceDead ev)
        {
            if (ev.DeadBySword)
            {
                Invoke(nameof(PlayDeadBySwordMusic), deadMusicDelay);
            }
            else
            {
                Invoke(nameof(PlayDeadMusic), deadMusicDelay);
            }
        }

        private void PlayDeadMusic()
        { 
            soundController.PlaySound("regular_death");
        }

        private void PlayDeadBySwordMusic()
        {
            soundController.PlaySound("fight_death");
        }
    }
}
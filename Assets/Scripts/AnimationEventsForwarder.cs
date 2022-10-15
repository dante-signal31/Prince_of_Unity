using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Animators can send animations events only to components in the same transform. I don't want
    /// to overload the Appearance subtransform, so this component is next to Animator to
    /// forward events to every other component scattered through the game object.
    /// </summary>
    public class AnimationEventsForwarder : MonoBehaviour
        {
                [Header("WIRING:")]
                [Tooltip("Needed to signal fighting events to stay in synch with animations.")]
                [SerializeField] private FightingInteractions fightingInteractions;
                [Tooltip("Needed to play sounds at certain points of animations")] 
                [SerializeField] private SoundController soundController;
                [Tooltip("Needed to play music at certain points of animation.")]
                [SerializeField] private MusicController musicController;
                [Tooltip("Needed to emit vibrations event.")] 
                [SerializeField] private VibrationsController vibrationsController;

                private EventBus _eventBus;

                private void Awake()
                {
                    _eventBus = GameObject.Find("GameManagers").GetComponentInChildren<EventBus>();
                }

                ///////// My strike can be blocked.
                public void StrikeStart()
                {
                    fightingInteractions.StrikeStart();
                }

                public void BlockingChanceEnded()
                {
                    fightingInteractions.BlockingChanceEnded();
                }

                public void StrikeHit()
                {
                    fightingInteractions.StrikeHit();
                }

//////// I block my enemy attack and now I have chance to counter attack.
                public void BlockSwordStarted()
                {
                    fightingInteractions.BlockSwordStarted();
                }

                public void CounterAttackChanceEnded()
                {
                    fightingInteractions.CounterAttackChanceEnded();
                }

//////// My attack has been blocked but my enemy counterattacks, now I have an small chance to block him.        
                public void BlockedSwordStarted()
                {
                    fightingInteractions.BlockedSwordStarted();
                }

                public void CounterBlockSwordChanceEnded()
                {
                    fightingInteractions.CounterBlockSwordChanceEnded();
                }

                public void PlaySound(string soundName)
                {
                    soundController.PlaySound(soundName);
                }

                public void PlayMusic(string musicName)
                {
                    musicController.PlayMusic(musicName);
                }

                public void EmitEvent(string name)
                {
                    switch (name)
                    {
                        
                    }
                }

                public void EmitVibration()
                {
                    vibrationsController.TriggerVibrationEvent();
                }
        }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component manages every sound that gameobject produces.
    /// </summary>
    public class SoundController : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to play selected audio clips.")]
        [SerializeField] private AudioSource audioSource;
        
        [Header("CONFIGURATION:")]
        [Tooltip("Sounds this character plays at certain moments.")]
        [SerializeField] private SoundList audioClips;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;

        /// <summary>
        /// Play given sound.
        ///
        /// Sound is played asynchronously.
        /// </summary>
        /// <param name="soundName"></param>
        public void PlaySound(string soundName)
        {
            StartCoroutine(PlaySoundAsync(soundName));
        }

        private IEnumerator PlaySoundAsync(string soundName)
        {
            this.Log($"(SoundController - {transform.root.name}) Playing {soundName}", showLogs);
            audioSource.clip = audioClips[soundName];
            audioSource.Play();
            yield return null;
        }

        /// <summary>
        /// Get sound length in seconds.
        /// </summary>
        /// <param name="soundName">Name of sound.</param>
        /// <returns>Length in seconds.</returns>
        public float getSoundLength(string soundName)
        {
            return audioClips[soundName].length;
        }
    }
}


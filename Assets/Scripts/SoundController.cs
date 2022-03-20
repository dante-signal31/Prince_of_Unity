using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
    // [Serializable]
    // public class NamedAudioClip
    // {
    //     public string name;
    //     public AudioClip audioClip;
    //
    //     public NamedAudioClip(string name, AudioClip audioClip)
    //     {
    //         this.name = name;
    //         this.audioClip = audioClip;
    //     }
    // }
    
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

        // // Start is called before the first frame update
        // void Start()
        // {
        //
        // }
        //
        // // Update is called once per frame
        // void Update()
        // {
        //
        // }
    }
}


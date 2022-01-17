using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
    [Serializable]
    public class NamedAudioClip
    {
        public string name;
        public AudioClip audioClip;

        public NamedAudioClip(string name, AudioClip audioClip)
        {
            this.name = name;
            this.audioClip = audioClip;
        }
    }
    
    /// <summary>
    /// Dict cannot be serialized so be cannot edit them at inspector. We have to create a dict alike
    /// structure.
    ///
    /// Programatically this scriptable object behaves like a dict.
    /// </summary>
    [CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableObjects/SoundList",
        order = 4)]
    public class SoundList : ScriptableObject
    {
        public List<NamedAudioClip> sounds;

        public AudioClip this[string key]
        {
            get => GetValue(key);

            set => SetValue(key, value);
        }

        private AudioClip GetValue(string key)
        {
            foreach (var namedAudioClip in sounds)
            {
                if (namedAudioClip.name == key) return namedAudioClip.audioClip;
            }

            return null;
        }

        private void SetValue(string key, AudioClip value)
        {
            NamedAudioClip namedAudioClipFound = null;
            foreach (var namedAudioClip in sounds)
            {
                if (namedAudioClip.name == key)
                {
                    namedAudioClipFound = namedAudioClip;
                    break;
                };
            }

            if (namedAudioClipFound == null)
            {
                sounds.Add(new NamedAudioClip(key, value));
            }
            else
            {
                namedAudioClipFound.audioClip= value;
            }
            
        }
    }
}
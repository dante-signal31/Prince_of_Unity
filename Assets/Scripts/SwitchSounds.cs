using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This components plays activation sound when switch get activated state.
    /// </summary>
    public class SwitchSounds : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when we get activated state.")]
        [SerializeField] private SwitchStatus switchStatus;
        [Tooltip("Needed to play sounds.")]
        [SerializeField] private SoundController soundController;

        private bool _soundAlreadyPlayed = false;

        private void Update()
        {
            switch (switchStatus.CurrentState)
            {
                case SwitchStatus.States.Activated:
                    if (!_soundAlreadyPlayed)
                    {
                        soundController.PlaySound("switch_activated");
                        _soundAlreadyPlayed = true;
                    }
                    break;
                case SwitchStatus.States.Idle:
                    if (_soundAlreadyPlayed) _soundAlreadyPlayed = false;
                    break;
            }
        }
    }
}
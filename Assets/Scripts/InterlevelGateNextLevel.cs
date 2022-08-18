using System;
using UnityEngine;

namespace Prince
{
    public class InterlevelGateNextLevel : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when level has been entered.")]
        [SerializeField] private InterlevelGateStatus gateStatus;
        [Tooltip("Needed to play end level music.")]
        [SerializeField] private SoundController soundController;
        
        [Header("CONFIGURATION:")]
        [Tooltip("Level to load when gate is entered. Use name registered at current LevelLoader.")]
        [SerializeField] private string levelToLoad;

        private LevelLoader _levelLoader;
        private bool _alreadyLoadingNextLevel = false;

        private void Awake()
        {
            _levelLoader = GameObject.Find("GameManagers").GetComponentInChildren<LevelLoader>();
        }

        private void Update()
        {
            if (!_alreadyLoadingNextLevel && gateStatus.CurrentState == InterlevelGateStatus.GateStates.Entered)
            {
                playMusicAndLoadNextLevel();
                _alreadyLoadingNextLevel = true;
            }
        }

        private void playMusicAndLoadNextLevel()
        {
            string end_level_music = "end_level_music";
            soundController.PlaySound(end_level_music);
            float soundLength = soundController.getSoundLength(end_level_music);
            Action nextLevelFunction = () => _levelLoader.LoadScene(levelToLoad);
            Invoke(nextLevelFunction.Method.Name, soundLength);
        }
    }
}
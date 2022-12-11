using UnityEngine;

namespace Prince
{
    // TODO: Take from game the music that is played when Prince is defeated by a guard.
    
    /// <summary>
    /// This component let gameobjects ask to level camera sound controller to play
    /// specific music clips.
    /// </summary>
    public class MusicController : MonoBehaviour
    {
        private SoundController _cameraSoundController;

        private void Awake()
        {
            _cameraSoundController = GameObject.Find("LevelCamera").GetComponentInChildren<SoundController>();
        }

        /// <summary>
        /// Play an specific music audio clip.
        /// </summary>
        /// <param name="name">Music audio clip name.</param>
        public void PlayMusic(string name)
        {
            _cameraSoundController.PlaySound(name);
        }
    }
}
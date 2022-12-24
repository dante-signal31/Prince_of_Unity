using UnityEngine;

namespace Prince
{
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
        /// <param name="_name">Music audio clip name.</param>
        public void PlayMusic(string _name)
        {
            _cameraSoundController.PlaySound(_name);
        }
    }
}
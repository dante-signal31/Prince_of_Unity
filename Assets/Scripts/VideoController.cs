using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Prince
{
    /// <summary>
    /// This component manages full screen video playing.
    ///
    /// It is going to be used usually for interlevel videos.
    ///
    /// Video will be started at awake and will fire an videoPlayingEnded when it reaches
    /// video end.
    /// </summary>
    public class VideoController : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to play video.")]
        [SerializeField] private VideoPlayer videoPlayer;

        [Header("CONFIGURATION:")] 
        [Tooltip("Video to play.")] 
        [SerializeField] private VideoClip videoToPlay;
        [Tooltip("Event to call when video gets its end. Used with level local game objects.")] 
        [SerializeField] private UnityEvent videoPlayingEnded;
        [Tooltip("Load next level when video ends.")]
        [SerializeField] private bool loadNextLevelAtEnd;

        private Camera _currentCamera;
        private LevelLoader _levelLoader;

        private void Awake()
        {
            videoPlayer.loopPointReached += OnVideoPlayingEnded;
            _currentCamera = GameObject.Find("LevelCamera").GetComponentInChildren<Camera>();
            videoPlayer.targetCamera = _currentCamera;
            videoPlayer.clip = videoToPlay;
            _levelLoader = GameObject.Find("LevelLoader").GetComponentInChildren<LevelLoader>();
        }

        private void OnVideoPlayingEnded(VideoPlayer vp)
        {
            if (videoPlayingEnded != null) videoPlayingEnded.Invoke();
            if (loadNextLevelAtEnd) _levelLoader.LoadNextScene();
        }

        public void SkipVideo()
        {
            videoPlayer.time = videoToPlay.length;
        }
    }
}
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
        [Tooltip("Event to call when video gets its end.")] 
        [SerializeField] private UnityEvent videoPlayingEnded;

        private Camera currentCamera;

        private void Awake()
        {
            videoPlayer.loopPointReached += OnVideoPlayingEnded;
            currentCamera = GameObject.Find("LevelCamera").GetComponentInChildren<Camera>();
            videoPlayer.targetCamera = currentCamera;
            videoPlayer.clip = videoToPlay;
        }

        private void OnVideoPlayingEnded(VideoPlayer vp)
        {
            if (videoPlayingEnded != null) videoPlayingEnded.Invoke();
        }
    }
}
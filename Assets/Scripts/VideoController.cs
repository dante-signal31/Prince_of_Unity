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
        private EventBus _eventBus;
        private bool _startEventTriggered;

        private void Awake()
        {
            SetUpVideoPlayer();
            _levelLoader = GameObject.Find("LevelLoader").GetComponentInChildren<LevelLoader>();
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
        }

        private void SetUpVideoPlayer()
        {
            videoPlayer.loopPointReached += OnVideoPlayingEnded;
            _currentCamera = GameObject.Find("LevelCamera").GetComponentInChildren<Camera>();
            videoPlayer.targetCamera = _currentCamera;
            videoPlayer.clip = videoToPlay;
        }

        private void Update()
        {
            if (!_startEventTriggered)
            {
                _eventBus.TriggerEvent(new GameEvents.VideoPlayStart(videoToPlay), this);
                _startEventTriggered = true;
            }
        }

        private void OnVideoPlayingEnded(VideoPlayer vp)
        {
            if (videoPlayingEnded != null) videoPlayingEnded.Invoke();
            _eventBus.TriggerEvent(new GameEvents.VideoPlayEnd(videoToPlay), this);
            if (loadNextLevelAtEnd) _levelLoader.LoadNextScene();
        }

        public void SkipVideo()
        {
            videoPlayer.time = videoToPlay.length;
        }
    }
}
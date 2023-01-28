using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This manager components manages game screen resolution.
    /// </summary>
    public class ScreenResolution : MonoBehaviour
    {
        public enum ScreenResolutions
        {
            r640_480,
            r800_600,
            r1920_1080,
            r2560_1080    
        }

        [Header("WIRING")]
        [Tooltip("Needed to know which resolution user desires.")]
        [SerializeField] private GameConfiguration gameConfiguration;
        
        private struct Dimension
        {
            public int Width { get; }
            public int Height { get; }
            
            public Dimension(int width, int height)
            {
                Width = width;
                Height = height;
            }
        }

        /// <summary>
        /// Get a dimension struct for given resolution tag.
        /// </summary>
        /// <param name="resolution">Desired resolution tag.</param>
        /// <returns></returns>
        private Dimension GetResolutionDimensions(ScreenResolutions resolution)
        {
            switch (resolution)
            {
                case ScreenResolutions.r640_480:
                    return new Dimension(640, 480);
                case ScreenResolutions.r800_600:
                    return new Dimension(800, 600);
                case ScreenResolutions.r1920_1080:
                    return new Dimension(1920, 1080);
                case ScreenResolutions.r2560_1080:
                    return new Dimension(2560, 1080);
                default:
                    throw new ArgumentOutOfRangeException(nameof(resolution), resolution, null);
            }
        }

        private void Start()
        {
            Dimension screenDimension = GetResolutionDimensions(gameConfiguration.ScreenResolution);
#if UNITY_EDITOR || UNITY_STANDALONE_LINUX
            // On Linux FullScreenMode stretches image, ignoring set resolution. So, I have to play
            // game in windowed mode. But in windowed mode you must count the window bar so in a full hd
            // screen (1080p) the lower side of the window stays under screen lower border. To avoid that
            // in Linux I keep the game windowed at 720p, until a find a better solution.
            if (screenDimension.Height > 720) screenDimension = new Dimension(1280, 720);
            Screen.SetResolution(screenDimension.Width, screenDimension.Height, FullScreenMode.Windowed);
#else
            Screen.SetResolution(screenDimension.Width, screenDimension.Height, gameConfiguration.FullScreen);
#endif
        }
    }
}
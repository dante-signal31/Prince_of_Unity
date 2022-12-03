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
            Screen.SetResolution(screenDimension.Width, screenDimension.Height, gameConfiguration.FullScreen);
        }
    }
}
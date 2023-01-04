using UnityEngine;

namespace Prince
{
    // TODO: Create a windows installer.
    
    // TODO: Add project README.
    
    // TODO: Create wiki pages with game instructions.

    /// <summary>
    /// This component gather every game specific configuration to start game with.
    /// </summary>
    public class GameConfiguration : MonoBehaviour
    {
        [Header("GAME:")]
        [Tooltip("Total time in seconds to complete game.")]
        [SerializeField] private int gameTotalTime;
        [Tooltip("Show game in full screen.")]
        [SerializeField] private bool fullScreen;
        [Tooltip("Screen resolution for game.")]
        [SerializeField] private ScreenResolution.ScreenResolutions screenResolution;

        [Header("PLAYER:")] 
        [Tooltip("Maximum starting life.")] 
        [SerializeField] private int maximumStartingLife;
        [Tooltip("Maximum life roof (i.e maximum life units that can be shown on hud).")]
        [SerializeField] private int maximumLifeRoof;
        [Tooltip("Starting life.")] 
        [SerializeField] private int startingLife;
        [Tooltip("Whether player starts game with a sword.")] 
        [SerializeField] private bool startsWithSword;

        /// <summary>
        /// Total time in seconds to complete game.
        /// </summary>
        public float GameTotalTime => gameTotalTime;

        /// <summary>
        /// Whether to show this game in full screen.
        /// </summary>
        public bool FullScreen => fullScreen;

        /// <summary>
        /// Screen resolution for the game.
        /// </summary>
        public ScreenResolution.ScreenResolutions ScreenResolution => screenResolution;

        /// <summary>
        /// Maximum life to start with.
        /// </summary>
        public int PlayerMaximumStartingLife => maximumStartingLife;

        /// <summary>
        /// Maximum life roof (i.e maximum life units that can be shown on hud).
        /// </summary>
        public int PlayerMaximumLifeRoof => maximumLifeRoof;

        /// <summary>
        /// Current life to start with.
        /// </summary>
        public int PlayerStartingLife => startingLife;

        /// <summary>
        /// Whether player starts game with a sword.
        /// </summary>
        public bool PlayerStartsWithSword => startsWithSword;
        
    }
}
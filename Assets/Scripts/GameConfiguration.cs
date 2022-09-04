using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component gather every game specific configuration to start game with.
    /// </summary>
    public class GameConfiguration : MonoBehaviour
    {
        [Header("GAME:")]
        [Tooltip("Total time in minutes to complete game.")]
        [SerializeField] private int gameTotalTime;

        [Header("PLAYER:")] 
        [Tooltip("Maximum starting life.")] 
        [SerializeField] private int maximumStartingLife;
        [Tooltip("Starting life.")] 
        [SerializeField] private int startingLife;
        [Tooltip("Whether player starts game with a sword.")] 
        [SerializeField] private bool startsWithSword;

        /// <summary>
        /// Total time un minutes to complete game.
        /// </summary>
        public int GameTotalTime => gameTotalTime;

        /// <summary>
        /// Maximum life to start with.
        /// </summary>
        public int PlayerMaximumStartingLife => maximumStartingLife;

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
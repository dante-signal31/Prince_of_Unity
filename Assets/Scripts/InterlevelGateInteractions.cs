using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component is used by Prince to interact with interlevel gates.
    /// </summary>
    public class InterlevelGateInteractions : MonoBehaviour
    {
        /// <summary>
        /// Interlevel gate set this to true if Prince character is in front of one of them and gate is open
        /// and ready to be entered.
        /// </summary>
        public bool InterlevelGateAvailable { get; set; }
    }
}
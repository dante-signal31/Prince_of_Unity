using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This LevelSpecifics prefab component is intended to give data configuration that is common to every level.
    /// </summary>
    public class LevelConfiguration : MonoBehaviour
    {
        [Header("CONFIGURATION:")] [Tooltip("Whether to show HUD bar when this scene is loaded.")] 
        [SerializeField] private bool showHudBarAtStart;

        /// <summary>
        /// Whether this scene should show HUD bar when it is loaded.
        /// </summary>
        public bool ShowHudBarAtStart => showHudBarAtStart;
    }
}
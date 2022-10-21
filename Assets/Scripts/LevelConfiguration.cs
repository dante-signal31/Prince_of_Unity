using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This LevelSpecifics prefab component is intended to give data configuration that is common to every level.
    /// </summary>
    public class LevelConfiguration : MonoBehaviour
    {
        [Header("CONFIGURATION:")] 
        [Tooltip("Whether to show HUD bar when this scene is loaded.")] 
        [SerializeField] private bool showHudBarAtStart;
        [Tooltip("Whether time counter should be activated in this level.")] 
        [SerializeField] private bool timeCounterEnabled;
        [Tooltip("Whether Prince status should be saved when this level is loaded.")] 
        [SerializeField] private bool savePrinceStatusWhenLevelLoaded;

        /// <summary>
        /// Whether this scene should show HUD bar when it is loaded.
        /// </summary>
        public bool ShowHudBarAtStart => showHudBarAtStart;

        /// <summary>
        /// Whether time counter should be active in this scene. This should be false at video scenes.
        /// </summary>
        public bool TimeCounterEnabled => timeCounterEnabled;

        /// <summary>
        /// Whether Prince status should be saved when this level is loaded. This should be false at video scenes.
        /// </summary>
        public bool SavePrinceStatusWhenLevelLoaded => savePrinceStatusWhenLevelLoaded;
    }
}
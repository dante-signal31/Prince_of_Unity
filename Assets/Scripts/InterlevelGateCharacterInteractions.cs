using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This is an interlevel gate component used by Prince character to interact with
    /// gate.
    /// </summary>
    public class InterlevelGateCharacterInteractions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to signal we Prince is entering the gate.")]
        [SerializeField] private Animator stateMachine;
        
        /// <summary>
        /// Called from Prince character when he wants to enter inter level gate.
        /// </summary>
        public void EnterGate()
        {
            stateMachine.SetTrigger("Enter");
        }
    }
}
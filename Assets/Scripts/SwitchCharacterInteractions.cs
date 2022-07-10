using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to manage character interactions with this switch.
    /// </summary>
    public class SwitchCharacterInteractions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to send signal of activation and deactivation.")] 
        [SerializeField] private Animator stateMachine;

        public void ActivateSwitch()
        {
            stateMachine.SetBool("Activated", true);
        }

        public void DeactivateSwitch()
        {
            stateMachine.SetBool("Activated", false);
        }
    }
}
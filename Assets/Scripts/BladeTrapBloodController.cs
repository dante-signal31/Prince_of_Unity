using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component traks when a character is butchered to show blood in trap's blades.
    /// </summary>
    public class BladeTrapBloodController : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to set blodied flag at state machine.")] 
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to know when a character has been hunted.")]
        [SerializeField] private TrapStatus trapStatus;

        private bool _blodied;

        public bool Blodied
        {
            get => _blodied;
            set
            {
                // This is actually run once because blades just get blodied once.
                if (!_blodied && value)
                {
                    _blodied = value;
                    stateMachine.SetBool("Blodied", value);
                }
            }
        }

        private void Update()
        {
            if (trapStatus.CurrentState == TrapStatus.States.Killed)
            {
                Blodied = true;
            }
        }
    }
}
using UnityEngine;

namespace Prince
{
    public class CharacterSensors : MonoBehaviour
    {
        [SerializeField] private Animator stateMachine;

        private bool _enemySeen;
        
        public bool EnemySeen
        {
            // TODO: Remove this true when testing ends.
            get => true;
            set
            {
                _enemySeen = value;
                stateMachine.SetBool("enemySeen", value);
            }
        }
        
        /// <summary>
        /// UpdateAnimator flags that depend on character sensors.
        /// </summary>
        private void UpdateStateMachineFlags()
        {
            stateMachine.SetBool("enemySeen", EnemySeen);
        }
        
        private void Awake()
        {
            UpdateStateMachineFlags();
        }
    }
}
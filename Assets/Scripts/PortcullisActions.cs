using UnityEngine;

namespace Prince
{
    /// <summary>
    /// A simple forwarder to be placed at portcullis root transform.
    ///
    /// At UnityEvent you can only associate methods from root transform game object. As
    /// IronCurtainController is way deep inside Portcullis game object, I need some kind
    /// of forwarder.
    /// </summary>
    public class PortcullisActions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to operate iron curtain.")] 
        [SerializeField] private Animator stateMachine;

        /// <summary>
        /// Start portcullis opening sequence.
        /// </summary>
        public void Open()
        {
            stateMachine.SetTrigger("Open");    
        }

        /// <summary>
        /// Start portcullis closing sequence.
        /// </summary>
        public void CloseSlowly()
        {
            stateMachine.SetTrigger("Close");    
        }

        /// <summary>
        /// Start portcullis fast closing sequence.
        /// </summary>
        public void CloseFast()
        {
            stateMachine.SetTrigger("CloseFast");
        }
    }
}
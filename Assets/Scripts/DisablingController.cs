using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component disables his game object when we reach an specific state.
    ///
    /// This way we avoid further interactions and process after that state.
    /// </summary>
    public class DisablingController<T> : MonoBehaviour where T: Enum
    {
        [Header("CONFIGURATION:")] 
        [Tooltip("Target state when game object is going to be disabled.")]
        [SerializeField] private T disablingState;
        [Tooltip("How long we should wait to disable game object after reaching target state.")]
        [SerializeField] private float waitingTime;

        private IStateMachineStatus<T> _gameObjectStatus;

        private void Awake()
        {
            _gameObjectStatus = gameObject.GetComponentInChildren<IStateMachineStatus<T>>();
        }

        /// <summary>
        /// Disable current game object.
        /// </summary>
        protected virtual void DisableGameObject()
        {
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            // Comparison operators cannot be used with generic. Instead use CompareTo. 
            if (_gameObjectStatus.CurrentState.CompareTo(disablingState) == 0)
            {
                // We cannot disable this component at once because we would interrupt 
                // crashing sound. To let crashing sound end we wait a while before disabling
                // object.
                Invoke(nameof(DisableGameObject), waitingTime);
            }
        }
        
    }
}
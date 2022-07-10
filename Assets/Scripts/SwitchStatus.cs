using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to register current switch state.
    /// </summary>
    public class SwitchStatus : MonoBehaviour, IStateMachineStatus<SwitchStatus.States>
    {
        public enum States
        {
            Idle,
            Activated
        }

        public States CurrentState { get; set; }
    }
}
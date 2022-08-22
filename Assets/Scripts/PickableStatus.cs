using UnityEngine;

namespace Prince
{
    public class PickableStatus : MonoBehaviour, IStateMachineStatus<PickableStatus.States>
    {
        public enum States
        {
            Idle,
            BeingTaken,
            Taken
        }

        public States CurrentState { get; set; }
    }
}
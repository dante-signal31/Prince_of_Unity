using UnityEngine;

namespace Prince
{
    public class PickableStatus : MonoBehaviour, IStateMachineStatus<PickableStatus.States>
    {
        public enum States
        {
            Idle,
            BeingTaken,
            Taken,
            Disabled
        }

        public States CurrentState { get; set; }
    }
}
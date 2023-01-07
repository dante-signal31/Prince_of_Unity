using System.Collections;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Specific functionality to open and close interlevel gates.
    /// </summary>
    [ExecuteAlways]
    public class InterlevelGateCurtainController : CurtainController<InterlevelGateStatus.GateStates>
    {
        [Header("WIRING (InterlevelGateCurtainController)")] 
        [Tooltip("Needed to know current state.")] 
        [SerializeField] private InterlevelGateStatus gateStatus;

        [Header("CONFIGURATION (InterlevelGateCurtainController)")] 
        [Tooltip("Delay to sync closing fast sound with curtain position.")]
        [SerializeField] private float closingFastSoundDelay;
        
        protected override IStateMachineStatus<InterlevelGateStatus.GateStates> GateStatus => gateStatus;
        protected override InterlevelGateStatus.GateStates CurrentState { get; set; }
        
        protected override void ReactToNewState(InterlevelGateStatus.GateStates newState)
        {
            switch (newState)
            {
                case InterlevelGateStatus.GateStates.Opening:
                    OpenGate();
                    break;
                case InterlevelGateStatus.GateStates.ClosingFast:
                    closeGateFast();
                    break;
                case InterlevelGateStatus.GateStates.Open:
                    setGateOpen();
                    break;
                case InterlevelGateStatus.GateStates.Closed:
                    SetGateClosed();
                    break;
            }
        }
        
        /// <summary>
        /// Coroutine to open the gate.
        /// </summary>
        protected override IEnumerator OpenGateAsync()
        {
            soundController.PlaySound("opening");
            yield return base.OpenGateAsync();
        }
        
        /// <summary>
        /// Coroutine to fast close the portcullis.
        /// </summary>
        protected override IEnumerator CloseGateFastAsync()
        {
            Invoke(nameof(PlayClosingFastSound), closingFastSoundDelay);
            yield return base.CloseGateFastAsync();
        }

        private void PlayClosingFastSound()
        {
            soundController.PlaySound("closing_fast");
        }

        public override bool IsOpening()
        {
            return CurrentState == InterlevelGateStatus.GateStates.Opening;
        }

        public override bool IsClosingSlowly()
        {
            // Inter level gates never close slowly.
            return false;
        }

        public override bool IsClosingFast()
        {
            return CurrentState == InterlevelGateStatus.GateStates.ClosingFast;
        }

        public override bool IsOpen()
        {
            return CurrentState == InterlevelGateStatus.GateStates.Open;
        }
    }
}
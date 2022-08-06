using System.Collections;
using UnityEngine;

namespace Prince
{
    [ExecuteAlways]
    public class IronCurtainController : CurtainController<PortcullisStatus.PortcullisStates>
    {
        [Header("WIRING (IronCurtainController):")] 
        [Tooltip("Needed to know current portcullis status.")]
        [SerializeField] private new PortcullisStatus gateStatus;
        [Tooltip("Needed to block way when curtain is closed.")]
        [SerializeField] private BoxCollider2D curtainCollider;
        
        [Header("CONFIGURATION (IronCurtainController):")] 
        [Tooltip("Needed to calculate Y offsets.")]
        [SerializeField] private float colliderInitialYPosition;
        [Tooltip("Minimum Y size for curtain collider.")]
        [SerializeField] private float colliderMinimumHeight;
        [Tooltip("Maximum Y size for curtain collider.")]
        [SerializeField] private float colliderMaximumHeight;

        protected override IStateMachineStatus<PortcullisStatus.PortcullisStates> GateStatus => gateStatus;

        protected override PortcullisStatus.PortcullisStates CurrentState { get; set; }

        /// <summary>
        /// <p>Set current opening for iron curtain.</p>
        ///
        /// <p>Opening percentage used here goes from 0 (closed) to 1 (open). Values over 1 will be
        /// converted to 1 and values under 0 will converted to 0.</p>
        /// </summary>
        /// <param name="newOpening">Opening, from 0 (closed) to 1 (open)</param>
        protected override void setOpening(float newOpening)
        {
            base.setOpening(newOpening);
            updateColliderSize();
        }

        /// <summary>
        /// Make collider longer while curtain closes to block the way.
        /// </summary>
        /// <param name="portcullisOpening">Opening, from 0 (closed) to 1 (open)</param>
        private void updateColliderSize()
        {
            // Collider size rate is the inverse of opening rate.
            float colliderSize = (1 - GateOpening);
            float newColliderHeight = Mathf.Lerp(colliderMinimumHeight, colliderMaximumHeight, colliderSize);
            curtainCollider.size = new Vector2(curtainCollider.size.x, newColliderHeight);
            curtainCollider.transform.localPosition = new Vector3(curtainCollider.transform.localPosition.x,
                colliderInitialYPosition - ((newColliderHeight - colliderMinimumHeight) / 2),
                curtainCollider.transform.localPosition.z);
        }
        
        /// <summary>
        /// Coroutine to open the gate.
        /// </summary>
        protected override IEnumerator OpenGateAsync()
        {
            soundController.PlaySound("portcullis_opening");
            yield return base.OpenGateAsync();
            soundController.PlaySound("portcullis_end");
        }

        /// <summary>
        /// Coroutine to slow close the portcullis.
        /// </summary>
        protected override IEnumerator CloseGateSlowlyAsync()
        {
            soundController.PlaySound("portcullis_closing_slowly");
            yield return base.CloseGateSlowlyAsync();
            soundController.PlaySound("portcullis_end");
        }

        /// <summary>
        /// Coroutine to fast close the portcullis.
        /// </summary>
        protected override IEnumerator CloseGateFastAsync()
        {
            yield return base.CloseGateFastAsync();
            soundController.PlaySound("portcullis_closing_fast");
        }
        
        protected override void ReactToNewState(PortcullisStatus.PortcullisStates newState)
        {
            switch (newState)
            {
                case PortcullisStatus.PortcullisStates.Opening:
                    OpenGate();
                    break;
                case PortcullisStatus.PortcullisStates.ClosingFast:
                    closeGateFast();
                    break;
                case PortcullisStatus.PortcullisStates.ClosingSlow:
                    CloseGateSlowly();
                    break;
                case PortcullisStatus.PortcullisStates.Open:
                    setGateOpen();
                    break;
                case PortcullisStatus.PortcullisStates.Closed:
                    SetGateClosed();
                    break;
            }
        }

        protected override void Awake()
        {
            CurrentState = PortcullisStatus.PortcullisStates.Closed;
            base.Awake();
        }

        public override bool IsOpening()
        {
            return gateStatus.CurrentState == PortcullisStatus.PortcullisStates.Opening;
        }

        public override bool IsClosingSlowly()
        {
            return gateStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingSlow;
        }

        public override bool IsClosingFast()
        {
            return gateStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingFast;
        }

        public override bool IsOpen()
        {
            return gateStatus.CurrentState == PortcullisStatus.PortcullisStates.Open;
        }
    }
}
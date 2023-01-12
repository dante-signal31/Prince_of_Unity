using UnityEngine;

namespace Prince
{
    /// <summary>
    /// When a falling ground finally falls its left hand neighbour must show its border. This
    /// component is intended to notify neighbour in that moment.
    /// </summary>
    public class FallingGroundNeighbourInteractions : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to know if there's any neighbour to notify we are falling.")]
        [SerializeField] private FallingGroundNeighbourSensor neighbourSensor;
        [Tooltip("Needed to know if we are falling.")]
        [SerializeField] private FallingGroundStatus fallingGroundStatus;

        private bool _alreadyNoticed = false;
        
        private void FixedUpdate()
        {
            switch (fallingGroundStatus.CurrentState)
            {
                case FallingGroundStatus.FallingGroundStates.Falling: 
                    NoticeNeighbourWeAreFalling();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Notice neighbour we are falling making him to show its border.
        /// </summary>
        private void NoticeNeighbourWeAreFalling()
        {
            if (neighbourSensor.NeighbourDetected && !_alreadyNoticed)
            {
                IBorder borderManager = neighbourSensor.Neighbour.GetComponentInChildren<IBorder>();
                if (borderManager != null) borderManager.ShowBorder(true);
                _alreadyNoticed = true;
            }
        }
    }
}
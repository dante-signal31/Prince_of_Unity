using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component control colliders that are at rooms bounds to change camera location
    /// when player leaves a room and enters in another.
    /// </summary>
    public class CameraChangerGate : MonoBehaviour
    {
        private enum ChangerGateTypes
        {
            Horizontal,
            Vertical
        }
        
        [Header("WIRING:")]
        [Tooltip("The room this component is attached to.")]
        [SerializeField] private Room currentRoom;

        [Header("CONFIGURATION:")]
        [Tooltip("Whether this gate moves camera horizontally or vertically.")]
        [SerializeField] private ChangerGateTypes currentCameraChangerGateType;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private Room _externalRoom;
        private CameraController _levelCamera;
        private EventBus _eventBus;
        
        private void Start()
        {
            SetExternalRoom();
            _levelCamera = currentRoom.LevelCamera.GetComponentInChildren<CameraController>();
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
        }

        /// <summary>
        /// Detect the external room this gate changes level camera from/to.
        /// </summary>
        private void SetExternalRoom()
        {
            switch (currentCameraChangerGateType)
            {
                case ChangerGateTypes.Horizontal:
                    _externalRoom = currentRoom.RightRoomNeighbour;
                    break;
                case ChangerGateTypes.Vertical:
                    _externalRoom = currentRoom.UpRoomNeighbour;
                    break;
            }
        }

        /// <summary>
        /// Whether this player is out of current room or not.
        /// </summary>
        /// <returns>True is player is out of current room and entering in it. False otherwhise.</returns>
        private bool IsPlayerOutOfCurrentRoom(GameObject player)
        {
            Vector3 positionDifference = player.transform.position - transform.position;
            switch (currentCameraChangerGateType)
            {
                case ChangerGateTypes.Horizontal:
                    return positionDifference.x > 0;
                default:
                    return positionDifference.y > 0;
            }
        }

        /// <summary>
        /// Change camera location when entering gate.
        /// </summary>
        /// <param name="col">Character collider.</param>
        private void OnTriggerEnter2D(Collider2D col)
        {
            GameObject other = col.transform.root.gameObject;
            // I want the gate to be activated by character body collider not by his sensors colliders.
            if (other.CompareTag("Player") && !col.transform.CompareTag("Sensor"))
            {
                if (IsPlayerOutOfCurrentRoom(other))
                {
                    // Player is entering in current room.
                    if (_levelCamera!= null) _levelCamera.PlaceInRoom(currentRoom);
                    this.Log($"(CameraChangerGate - {transform.root.name}) Player seems to be entering in me.", showLogs);
                    _eventBus.TriggerEvent(new GameEvents.PrinceEnteredNewRoom(currentRoom), this);
                }
                else
                {
                    // Player is leaving current room.
                    if (_levelCamera!= null) _levelCamera.PlaceInRoom(_externalRoom);
                    this.Log($"(CameraChangerGate - {transform.root.name}) Player seems to be leaving from me.", showLogs);
                    _eventBus.TriggerEvent(new GameEvents.PrinceEnteredNewRoom(_externalRoom), this);
                }
                
            }
        }

        /// <summary>
        /// A further check is needed when player leaves gate. Normal behaviour is that player
        /// leaves gate through the opposite side to the one he entered it. But there is an
        /// edge behaviour to check: if player stops inside gate and leave it through the same side
        /// he entered it. If that behaviour happens we must switch back camera to its original room.
        /// </summary>
        /// <param name="col">Character collider.</param>
        private void OnTriggerExit2D(Collider2D col)
        {
            GameObject other = col.transform.root.gameObject;
            // I want the gate to be activated by character body collider not by his sensors colliders.
            if (other.CompareTag("Player") && !col.transform.CompareTag("Sensor"))
            {
                if (IsPlayerOutOfCurrentRoom(other))
                {
                    // Player is returning to his former room.
                    if (_levelCamera.CurrentRoom != _externalRoom)
                    {
                        _levelCamera.PlaceInRoom(_externalRoom);
                        _eventBus.TriggerEvent(new GameEvents.PrinceEnteredNewRoom(_externalRoom), this);
                    }
                    this.Log($"(CameraChangerGate - {transform.root.name}) Player is finally leaving from me.", showLogs);
                }
                else
                {
                    // Player is finally entering in current room.
                    if (_levelCamera.CurrentRoom != currentRoom)
                    {
                        _levelCamera.PlaceInRoom(currentRoom);
                        _eventBus.TriggerEvent(new GameEvents.PrinceEnteredNewRoom(currentRoom), this);
                    }
                    this.Log($"(CameraChangerGate - {transform.root.name}) Player finally is entering in me.", showLogs);
                }
                
            }
        }
        
    }
}
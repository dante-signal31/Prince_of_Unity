using UnityEngine;

namespace Prince
{
    /// <summary>
    /// <p>A room is the portion of level that is focused by a camera.</p>
    ///
    /// <p>In game terms, a room has a 10x3 dimension while in Unity terms is 10x6.</p>
    /// </summary>
    public class Room : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Transform position for this room camera.")] 
        [SerializeField] private Transform cameraTransform;
        
        [Header("CONFIGURATION:")]
        [Tooltip("Room that is above of this one.")]
        [SerializeField] private Room upRoomNeighbour;
        [Tooltip("Room that is at right side of this one.")]
        [SerializeField] private Room rightRoomNeighbour;
        [Tooltip("Needed to know if an enemy is present at current room.")]
        [SerializeField] private PopulationSensor populationSensor;

        private Vector3 _roomDimensions = new Vector3(10, 6, 0);

        /// <summary>
        /// Enemy present at current room.
        /// </summary>
        public GameObject EnemyInTheRoom => populationSensor.EnemyCharacter;

        /// <summary>
        /// Whether there is an enemy present at current room.
        /// </summary>
        public bool IsThereAnEnemyInTheRoom => populationSensor.EnemyCharacter != null;
        
        public GameObject LevelCamera {get; private set; }

        private CameraController _cameraController;

        private void Awake()
        {
            LevelCamera = GameObject.Find("LevelCamera");
            _cameraController = LevelCamera.GetComponentInChildren<CameraController>();
        }

        /// <summary>
        /// This room's camera position.
        /// </summary>
        public Vector3 RoomCameraPosition => cameraTransform.position;

        /// <summary>
        /// Room's neighbour above.
        /// </summary>
        public Room UpRoomNeighbour => upRoomNeighbour;
        
        /// <summary>
        /// Room's neighbour at right.
        /// </summary>
        public Room RightRoomNeighbour => rightRoomNeighbour;

        /// <summary>
        /// This room's name.
        /// </summary>
        public string Name => transform.root.name;
        
        /// <summary>
        /// Is this room the one with current camera focus?
        /// </summary>
        /// <returns>True if this room has the camera focus. False otherwise.</returns>
        public bool IsActiveRoom()
        {
            return ((LevelCamera != null) && (LevelCamera.transform.position == RoomCameraPosition));
        }

        public void OnPrinceClimbedInRoom()
        {
            if (!IsActiveRoom()) 
                if (_cameraController != null) _cameraController.PlaceInRoom(this);
        }

#if UNITY_EDITOR
        private void DrawRoomCameraPosition()
        {
            float gizmoRadius = 0.2f;
            Gizmos.color = IsActiveRoom() ? Color.green : Color.yellow;
            Gizmos.DrawWireSphere(RoomCameraPosition, gizmoRadius);
        }

        private void DrawRoomBounds()
        {
            Gizmos.color = IsActiveRoom() ? Color.green : Color.yellow;
            Gizmos.DrawWireCube(RoomCameraPosition, _roomDimensions);
        }
        
        private void OnDrawGizmosSelected()
        {
            DrawRoomCameraPosition();
            DrawRoomBounds();
        }
#endif
    }
}
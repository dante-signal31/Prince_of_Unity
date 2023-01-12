using UnityEngine;
using UnityEngine.Events;

namespace Prince
{
    /// <summary>
    /// Component to keep a registry of enemy characters in current room.
    /// </summary>
    public class PopulationSensor : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to find guards that might already be inside sensor when it is created.")]
        [SerializeField] private BoxCollider2D sensorBox;

        [Header("CONFIGURATION:")]
        [Tooltip("Event triggered when Prince climbs or descends into room.")]
        [SerializeField] private UnityEvent princeClimbedInRoom;
        
        /// <summary>
        /// Enemy present at current room.
        /// </summary>
        public GameObject EnemyCharacter { get; private set; }
        
        /// <summary>
        /// Prince present at current room.
        /// </summary>
        // public bool PrinceClimbedInRoom { get; private set; }

        private EventBus _eventBus;

        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
        }

        private void Start()
        {
            RegisterInitialPopulation();
            _eventBus.AddListener<GameEvents.PrinceHanged>(OnPrinceHanged);
            _eventBus.AddListener<GameEvents.PrinceClimbingEnded>(OnPrinceClimbingEnded);
        }
        
        private void OnDisable()
        {
            _eventBus.RemoveListener<GameEvents.PrinceHanged>(OnPrinceHanged);
            _eventBus.RemoveListener<GameEvents.PrinceClimbingEnded>(OnPrinceClimbingEnded);
        }

        public void OnPrinceHanged(object sender, GameEvents.PrinceHanged ev)
        {
            if (PrinceInSensor(ev.Position) && princeClimbedInRoom != null)
                princeClimbedInRoom.Invoke();
        }

        public void OnPrinceClimbingEnded(object sender, GameEvents.PrinceClimbingEnded ev)
        {
            if (PrinceInSensor(ev.Position) && princeClimbedInRoom != null)
                princeClimbedInRoom.Invoke();
        }

        private bool PrinceInSensor(Vector3 princePosition)
        {
            return sensorBox.OverlapPoint(princePosition);
        }

        /// <summary>
        /// This method find guards that are already at room when this one is created. 
        /// </summary>
        private void RegisterInitialPopulation()
        {
            Collider2D[] hitColliders =
                Physics2D.OverlapBoxAll(gameObject.transform.position, sensorBox.size, 0);
            bool guardInSensor = false;
            foreach (Collider2D hitCollider in hitColliders)
            {
                guardInSensor = RegisterIfGuard(hitCollider);
            }
            if (!guardInSensor) _eventBus.TriggerEvent(new GameEvents.NoGuardInTheRoom(null), this);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            RegisterIfGuard(col);
        }

        /// <summary>
        /// It provide collider belongs to a guards, then his registered at EnemyCharacter and a GuardEnteredRoom event is
        /// raised.
        /// </summary>
        /// <param name="col">Provided colider.</param>
        /// <returns>True if provided collider was a guard, or false otherwise.</returns>
        private bool RegisterIfGuard(Collider2D col)
        {
            if (col.tag == "Sensor") return false;
            GameObject character = col.transform.root.gameObject;
            CharacterStatus characterStatus = character.GetComponentInChildren<CharacterStatus>();
            if (characterStatus != null && !characterStatus.IsPrince)
            {
                EnemyCharacter = character;
                _eventBus.TriggerEvent(new GameEvents.GuardEnteredTheRoom(character), this);
                return true;
            }
            return false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Sensor") return;
            GameObject character = other.transform.root.gameObject;
            CharacterStatus characterStatus = character.GetComponentInChildren<CharacterStatus>();
            if (characterStatus != null && !character.GetComponentInChildren<CharacterStatus>().IsPrince)
            {
                if (EnemyCharacter == character)
                {
                    EnemyCharacter = null;
                    _eventBus.TriggerEvent(new GameEvents.NoGuardInTheRoom(character), this);
                }
            }
        }
    }
}
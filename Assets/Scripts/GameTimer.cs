using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Prince
{
    /// <summary>
    /// Game component to keep track of time passed and launch events at specified moments.
    /// </summary>
    public class GameTimer : MonoBehaviour
    {
        // TODO: Implement an ingame pause.
        public enum TriggeringTimeTypes
        {
            ElapsedSeconds,
            GameTotalTimePercentage
        }

        /// <summary>
        /// Event to be launched at specific moments.
        /// </summary>
        [Serializable]
        public class PlannedEvent: IComparable<PlannedEvent>
        {
            [Tooltip("What we want to set, whether seconds or game percentage.")]
            public TriggeringTimeTypes triggeringType;
            [Tooltip("Second to let pass since the beginning to trigger this event.")]
            public float elapsedSeconds;
            [Tooltip("Game percentage to let pass before triggering this event.")]
            [Range(0.0f, 100.0f)]
            public float elapsedPercentage;
            [Tooltip("Event to trigger when elapsed seconds are reached.")] 
            public UnityEvent eventToTrigger;

            public int CompareTo(PlannedEvent other)
            {
                return elapsedSeconds.CompareTo(other.elapsedSeconds);
            }

            /// <summary>
            /// Make planned event coherent between their elapsedSeconds and elapsedPercentage data.
            ///
            /// By coherent I mean calculating elapsedSeconds for each planned event with GameTotalTimePercentage triggeringType
            /// and calculating elapsedPercentage for each planned event with ElapsedSeconds treiggeringType.
            /// </summary>
            public void Normalize(float gameTotalTime)
            {
                switch (triggeringType)
                {
                    case TriggeringTimeTypes.ElapsedSeconds:
                        elapsedPercentage = (elapsedSeconds / gameTotalTime) * 100;
                        break;
                    case TriggeringTimeTypes.GameTotalTimePercentage:
                        elapsedSeconds = (elapsedPercentage / 100) * gameTotalTime;
                        break;
                }
            }
        }

        [Header("WIRING:")] 
        [Tooltip("Needed to know game total time.")]
        [SerializeField] private GameConfiguration gameConfiguration;
        [Tooltip("Needed to know when a level is loaded.")]
        [SerializeField] private EventBus eventBus;
        
        [Header("CONFIGURATION:")]
        [Tooltip("List of events to trigger at specified moments.")]
        [SerializeField] private List<PlannedEvent> plannedEvents;

        /// <summary>
        /// Whether this counter is currently enabled and counting time.
        /// </summary>
        public bool TimerEnabled { get; private set; }
        
        private int _eventIndex;
        private PrinceStatus _princePersistentStatus;

        private void Awake()
        {
            ReindexPlannedEventsList();
            _princePersistentStatus = GameObject.Find("GameManagers").GetComponentInChildren<PrinceStatus>();
        }

        private void Start()
        {
            eventBus.AddListener<GameEvents.LevelLoaded>(OnLevelLoaded);
            eventBus.AddListener<GameEvents.LevelReloaded>(OnLevelReloaded);
        }

        private void OnDisable()
        {
            eventBus.AddListener<GameEvents.LevelLoaded>(OnLevelLoaded);
            eventBus.RemoveListener<GameEvents.LevelReloaded>(OnLevelReloaded);
        }

        /// <summary>
        /// Listener for LevelLoaded events.
        /// </summary>
        /// <param name="_">Sender of this event. Usually a LevelLoader.</param>
        /// <param name="__">Event data.</param>
        private void OnLevelLoaded(object _, GameEvents.LevelLoaded __)
        {
            LevelConfiguration levelConfiguration =
                GameObject.Find("LevelSpecifics").GetComponentInChildren<LevelConfiguration>();
            TimerEnabled = levelConfiguration.TimeCounterEnabled;
        }

        /// <summary>
        /// Listener for LevelReloaded events.
        /// </summary>
        /// <param name="_">Sender of this event. Usually a LevelLoader.</param>
        /// <param name="__">Event data.</param>
        private void OnLevelReloaded(object _, GameEvents.LevelReloaded __)
        {
            ElapsedSeconds = _princePersistentStatus.LevelStartsStats.ElapsedSeconds;
        }

        /// <summary>
        /// Normalize elapsedSeconds and sort the entire list depending on elapsedSeconds.
        ///
        /// When you update list with new elements.
        /// </summary>
        private void ReindexPlannedEventsList()
        {
            // Normalize first to make every planned event elapsedSeconds coherent with those set by percentage.
            // After normalizing, every elapsedSeconds will be correct and can be used as index to sort planned events.
            NormalizePlannedEvents();
            plannedEvents.Sort();
        }

        /// <summary>
        /// Make every planned event coherent between their elapsedSeconds and elapsedPercentage data.
        ///
        /// By coherent I mean calculating elapsedSeconds for each planned event with GameTotalTimePercentage triggeringType
        /// and calculating elapsedPercentage for each planned event with ElapsedSeconds treiggeringType.
        /// </summary>
        private void NormalizePlannedEvents()
        {
            foreach (PlannedEvent plannedEvent in plannedEvents)
            {
                plannedEvent.Normalize(gameConfiguration.GameTotalTime);
            }
        }
        
        /// <summary>
        /// Elapsed time since the beginning of game.
        /// </summary>
        public float ElapsedSeconds { get; private set; }

        /// <summary>
        /// Elapsed game percentage since the beginning of game.
        /// </summary>
        public float ElapsedGamePercentage => ((ElapsedSeconds / gameConfiguration.GameTotalTime) * 100);

        /// <summary>
        /// How much time leaves to complete the game.
        /// </summary>
        public float RemainingSeconds => gameConfiguration.GameTotalTime - ElapsedSeconds;

        private void FixedUpdate()
        {
            if (TimerEnabled) ElapsedSeconds += Time.deltaTime;
            if (_eventIndex < plannedEvents.Count && plannedEvents[_eventIndex].elapsedSeconds <= ElapsedSeconds)
            {
                if (plannedEvents[_eventIndex].eventToTrigger != null)
                {
                    plannedEvents[_eventIndex].eventToTrigger.Invoke();
                }
                _eventIndex++;
            }
        }

        /// <summary>
        /// Set timer to zero.
        /// </summary>
        public void ResetTimer()
        {
            ElapsedSeconds = 0;
        }
        
    }
}
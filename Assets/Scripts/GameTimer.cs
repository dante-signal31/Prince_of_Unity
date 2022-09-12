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
        
        [Header("CONFIGURATION:")] 
        [Tooltip("List of events to trigger at specified moments.")]
        [SerializeField] private List<PlannedEvent> plannedEvents;

        private int _eventIndex;
        
        private void Awake()
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

        private void FixedUpdate()
        {
            ElapsedSeconds += Time.deltaTime;
            if (_eventIndex < plannedEvents.Count && plannedEvents[_eventIndex].elapsedSeconds <= ElapsedSeconds)
            {
                if (plannedEvents[_eventIndex].eventToTrigger != null)
                {
                    plannedEvents[_eventIndex].eventToTrigger.Invoke();
                }
                _eventIndex++;
            }
        }

        private void OnValidate()
        {
            // plannedEvents.Sort();
        }
    }
}
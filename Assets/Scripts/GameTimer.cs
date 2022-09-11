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
            plannedEvents.Sort();
        }
        
        /// <summary>
        /// Elapsed time since the beginning of game.
        /// </summary>
        public float ElapsedSeconds { get; private set; }

        private void FixedUpdate()
        {
            ElapsedSeconds += Time.deltaTime;
            if (plannedEvents[_eventIndex].elapsedSeconds <= ElapsedSeconds)
            {
                if (plannedEvents[_eventIndex].eventToTrigger != null)
                {
                    plannedEvents[_eventIndex].eventToTrigger.Invoke();
                }
            }
        }

        private void OnValidate()
        {
            plannedEvents.Sort();
        }
    }
}
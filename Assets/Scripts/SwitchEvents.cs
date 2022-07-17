using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Prince
{
    /// <summary>
    /// Component to activate things from this switch.
    /// </summary>
    public class SwitchEvents : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when this switch is activated.")]
        [SerializeField] private SwitchStatus switchStatus;
        
        [Header("CONFIGURATION:")]
        [Tooltip("Event launched by this brick when it is activated.")]
        [SerializeField] private UnityEvent activated;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        [Tooltip("Color used to draw debug gizmo.")]
        [SerializeField] private Color gizmoColor;
        [Tooltip("Size for starting and ending gizmo.")]
        [SerializeField] private float gizmoSize;
        
        #if UNITY_EDITOR
        /// <summary>
        /// List of listeners that have subscribed to this switch.
        /// </summary>
        public List<GameObject> Listeners { get; private set; }

        /// <summary>
        /// Color used to paint gizmos related to this game object.
        /// </summary>
        public Color GizmoColor => gizmoColor;
        #endif
        
        private bool _eventAlreadyTriggered;
        private void Update()
        {
            if (switchStatus.CurrentState == SwitchStatus.States.Activated && !_eventAlreadyTriggered)
            {
                if (activated != null) activated.Invoke();
                _eventAlreadyTriggered = true;
            } 
            else if (switchStatus.CurrentState == SwitchStatus.States.Idle && _eventAlreadyTriggered)
            {
                _eventAlreadyTriggered = false;
            }
        }

        #if UNITY_EDITOR
        /// <summary>
        /// Update list of game object registered as listeners of activated event.
        /// </summary>
        private void UpdateListenerList()
        {
            Listeners.Clear();
            int listenerCount = activated.GetPersistentEventCount();
            this.Log($"(SwitchEvents - {transform.root.name}) There are {listenerCount} listeners.", showLogs);
            for (int i=0; i<listenerCount; i++)
            {
                this.Log($"(SwitchEvents - {transform.root.name}) Listener {i} is of type {activated.GetPersistentTarget(i).GetType()}.", showLogs);
                MonoBehaviour listener = (MonoBehaviour) activated.GetPersistentTarget(i);
                Listeners.Add(listener.gameObject);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            DrawConnectionsToListeners();
        }

        private void DrawConnectionsToListeners()
        {
            if (Listeners.Count == 0) return;
            gizmoColor.a = 1;
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(transform.position, new Vector3(gizmoSize, gizmoSize));
            this.Log($"(SwitchEvents - {transform.root.name}) Drawing connections with {Listeners.Count} listeners.", showLogs);
            foreach (GameObject listener in Listeners)
            {
                this.Log(
                    $"(SwitchEvents - {transform.root.name}) Drawing connections with listener from {transform.position} to {listener.transform.position}.",
                    showLogs);
                Gizmos.DrawSphere(listener.transform.position, gizmoSize / 2);
                Gizmos.DrawLine(transform.position, listener.transform.position);
            }
        }

        private void OnValidate()
        {
            UpdateListenerList();
        }
        #endif
    }
}
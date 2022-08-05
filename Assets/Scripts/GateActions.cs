using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// A simple forwarder to be placed at gate root transform.
    ///
    /// At UnityEvent you can only associate methods from root transform game object. As
    /// IronCurtainController is way deep inside gate game object, I need some kind
    /// of forwarder.
    /// </summary>
    [ExecuteAlways]
    public class GateActions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to operate iron curtain.")] 
        [SerializeField] private Animator stateMachine;

        [Header("DEBUG:")] 
        [Tooltip("Color to apply to gizmos.")]
        [SerializeField] private Color gizmoColor;
        [Tooltip("Size for gizmos.")]
        [SerializeField] private float gizmoSize;

        #if UNITY_EDITOR
        private List<GameObject> ClosingSwitchesList = new List<GameObject>();
        private List<GameObject> OpeningSwitchesList = new List<GameObject>();
        #endif
        
        /// <summary>
        /// Start gate opening sequence.
        /// </summary>
        public void Open()
        {
            stateMachine.SetTrigger("Open");    
        }

        /// <summary>
        /// Start gate closing sequence.
        /// </summary>
        public void CloseSlowly()
        {
            stateMachine.SetTrigger("Close");    
        }

        /// <summary>
        /// Start gate fast closing sequence.
        /// </summary>
        public void CloseFast()
        {
            stateMachine.SetTrigger("CloseFast");
        }

        private void Awake()
        {
            if (Application.isPlaying)
            {
                // Play mode logic.
            }
            else
            {
                // Editor mode logic.
#if UNITY_EDITOR
                UpdateAllSwitchesList();
#endif
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateAllSwitchesList();
        }

        private void UpdateAllSwitchesList()
        {
            UpdateClosingSwitchesList();
            UpdateOpeningSwitchesList();
        }

        /// <summary>
        /// Update list of switches that close this gate.
        /// </summary>
        private void UpdateClosingSwitchesList()
        {
            ClosingSwitchesList.Clear();
            List<GameObject> closingSwitches = new List<GameObject>(GameObject.FindGameObjectsWithTag("ClosingSwitch"));
            foreach (GameObject _switch in closingSwitches)
            {
                if (_switch.GetComponentInChildren<SwitchEvents>().Listeners.Contains(gameObject))
                    ClosingSwitchesList.Add(_switch);
            }
        }

        /// <summary>
        /// Update list of switches that close this gate.
        /// </summary>
        private void UpdateOpeningSwitchesList()
        {
            OpeningSwitchesList.Clear();
            List<GameObject> openingSwitches = new List<GameObject>(GameObject.FindGameObjectsWithTag("OpeningSwitch"));
            foreach (GameObject _switch in openingSwitches)
            {
                if (_switch.GetComponentInChildren<SwitchEvents>().Listeners.Contains(gameObject))
                    OpeningSwitchesList.Add(_switch);
            }
        }

        private void OnDrawGizmosSelected()
        {
            DrawConnectionsToSwitches();
        }

        private void DrawConnectionsToList(List<GameObject> listToConnect)
        {
            if (listToConnect.Count == 0) return;
            gizmoColor.a = 1;
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoSize / 2);
            foreach (GameObject _switch in listToConnect)
            {
                Color switchGizmoColor = _switch.GetComponentInChildren<SwitchEvents>().GizmoColor;
                switchGizmoColor.a = 1;
                Gizmos.color = switchGizmoColor;
                Gizmos.DrawCube(_switch.transform.position, new Vector3(gizmoSize, gizmoSize));
                Gizmos.DrawLine(transform.position, _switch.transform.position);
            }
        }

        private void DrawConnectionsToSwitches()
        {
            DrawConnectionsToList(ClosingSwitchesList);
            DrawConnectionsToList(OpeningSwitchesList);
        }
        #endif
    }
}
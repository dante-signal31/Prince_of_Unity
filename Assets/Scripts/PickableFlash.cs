using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to perform flashes when this pickable is taken.
    /// </summary>
    public class PickableFlash : MonoBehaviour
    {
        [Header("CONFIGURATION:")] 
        [Tooltip("Flashes to show when this pickable is taken.")]
        [SerializeField] private List<CameraController.Flash> flashes;

        private CameraController _currentCameraController;

        private void Awake()
        {
            _currentCameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
        }

        /// <summary>
        /// Show flashes on current camera.
        /// </summary>
        public void ShowFlashes()
        {
            if (_currentCameraController != null)
            {
                _currentCameraController.ShowFlashes(flashes.ToArray());
            }
        }
    }
}
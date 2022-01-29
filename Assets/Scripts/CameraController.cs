using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component controls level camera behaviour.
/// </summary>
[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// A flash is an instant when background changes is color. It happens in
    /// special events like when Prince is hit or it finds a sword.
    /// </summary>
    [Serializable]
    public struct Flash
    {
        [Tooltip("How much time passes since last flash to show this one.")]
        public float startDelay;
        [Tooltip("Color to show at background.")]
        public Color flashColor;
        [Tooltip("How much time this flash should last.")]
        public float duration;
    }

    [Header("WIRING:")]
    [Tooltip("This prefab camera component.")]
    [SerializeField] private Camera camera;
    
    [Header("CONFIGURATION:")]
    [Tooltip("How many units this camera should show as height.")]
    [SerializeField] private int height;
    [Tooltip("How many units this camera should show as width.")]
    [SerializeField] private int width;

    private bool _updateNeeded = false;
    private Flash[] _currentFlashSequence;
    
    public float AspectRatio => (float) width / (float) height;
    public float OrthographicSize => (float) height / 2;
    
    /// <summary>
    /// Set camera settings.
    /// </summary>
    private void UpdateCameraSettings()
    {
        camera.aspect = AspectRatio;
        camera.orthographicSize = OrthographicSize;
        FixPositionOffset();
    }

    /// <summary>
    /// When ortographic size is changed its position is slightly offset so we fix that.
    /// </summary>
    private void FixPositionOffset()
    {
        camera.transform.localPosition = new Vector3((float)width / 2,
                (float) height / 2,
                camera.transform.localPosition.z);
    }
    
    private void Awake()
    {
        UpdateCameraSettings();
    }

    /// <summary>
    /// Show given flash sequence.
    /// </summary>
    /// <param name="flashes">Flash sequence.</param>
    public void ShowFlashes(Flash[] flashes)
    {
        StartCoroutine(ShowFlashesAsync(flashes));
    }

    private IEnumerator ShowFlashesAsync(Flash[] flashes)
    {
        Color previousColor = camera.backgroundColor;
        foreach (Flash flash in flashes)
        {
            yield return new WaitForSeconds(flash.startDelay);
            camera.backgroundColor = flash.flashColor;
            yield return new WaitForSeconds(flash.duration);
            camera.backgroundColor = previousColor;
        }
    }

    /// <summary>
    /// When a value changes on Inspector, update camera settings.
    /// </summary>
    private void LateUpdate()
    {
        #if UNITY_EDITOR
        if (_updateNeeded)
        {
            UpdateCameraSettings();
            _updateNeeded = false;
        }
        #endif
    }

    private void OnValidate()
    {
        _updateNeeded = true;
    }
}

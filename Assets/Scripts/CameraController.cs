using System;
using System.Collections;
using System.Collections.Generic;
using Prince;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [Tooltip("Needed to signal current guard if kill cheat key has been pressed..")]
    [SerializeField] private EventBus eventBus;
    
    [Header("CONFIGURATION:")]
    [Tooltip("How many units this camera should show as height.")]
    [SerializeField] private int height;
    [Tooltip("How many units this camera should show as width.")]
    [SerializeField] private int width;
    [Tooltip("In which room place this camera when level loads.")]
    [SerializeField] private string defaultRoom = "Room_0_0";
    [Tooltip("Default background color.")]
    [SerializeField] private Color defaultBackgroundColor;

    private bool _updateNeeded = false;
    private Flash[] _currentFlashSequence;
    
    public float AspectRatio => (float) width / (float) height;
    public float OrthographicSize => (float) height / 2;
    
    public Room CurrentRoom { get; private set; }

    /// <summary>
    /// Place this level camera over given room.
    /// </summary>
    /// <param name="room">Room to place this camera over.</param>
    public void PlaceInRoom(Room room)
    {
        transform.position = room.RoomCameraPosition;
        CurrentRoom = room;
    }
    
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
        camera.transform.localPosition = new Vector3(0,0, camera.transform.localPosition.z);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += PlaceAtDefaultRoom;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= PlaceAtDefaultRoom;
        if (eventBus.HasRegisteredEvent<GameEvents.KillCurrentGuardKeyPressed>()) 
            eventBus.RemoveListener<GameEvents.KillCurrentGuardKeyPressed>(OnKillCurrentGuardKeyPressed);
    }
    
    private void OnKillCurrentGuardKeyPressed(object sender, GameEvents.KillCurrentGuardKeyPressed e)
    {
        if (CurrentRoom.IsThereAnEnemyInTheRoom)
        {
            CurrentRoom.EnemyInTheRoom.GetComponentInChildren<HealthController>().KilledByCheatKey();
        }
    }

    private void Awake()
    {
        UpdateCameraSettings();
    }

    private void Start()
    {
        if (eventBus.HasRegisteredEvent<GameEvents.KillCurrentGuardKeyPressed>()) 
            eventBus.AddListener<GameEvents.KillCurrentGuardKeyPressed>(OnKillCurrentGuardKeyPressed);
    }

    /// <summary>
    /// Place camera at default location.
    ///
    /// If current scene has no room with default name then do nothing.
    /// </summary>
    private void PlaceAtDefaultRoom(Scene _, LoadSceneMode __)
    {
        GameObject roomGameObject = GameObject.Find(defaultRoom);
        Room room = roomGameObject != null ? roomGameObject.GetComponentInChildren<Room>() : null;
        if (room != null) PlaceInRoom(room);
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
        // Color previousColor = camera.backgroundColor;
        foreach (Flash flash in flashes)
        {
            yield return new WaitForSeconds(flash.startDelay);
            camera.backgroundColor = flash.flashColor;
            yield return new WaitForSeconds(flash.duration);
            // camera.backgroundColor = previousColor;
            camera.backgroundColor = defaultBackgroundColor;
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

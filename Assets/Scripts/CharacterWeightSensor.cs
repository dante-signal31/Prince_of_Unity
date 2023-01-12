using System.Collections.Generic;
using Prince;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This component is intended to be used in prefabs where a character can get over it
/// activating a behaviour.
///
/// It allows to set an activation time so a character can stay over this ground brick
/// for a while before activating its triggering event. 
/// </summary>
public class CharacterWeightSensor : MonoBehaviour
{
    [Header("CONFIGURATION:")] 
    [Tooltip("Time a character must stay over this component to activate trigger behaviour.")]
    [SerializeField] private float activationTime;
    // I could have used polling to know if sensor is triggered, but as this sensor is only
    // activated few times I found interesting using unity events. That way I'm not going 
    // to waste CPU polling a sensor that is idle most of the time.
    //
    // In this specific use case (intra-prefab communication) I could have wired
    // FallingGroundCharacterInteractions to call its public methods directly. Probably
    // the result would have been the same, and would have been more performant. But
    // this way at least I have a chance to practice with UnityEvents.
    [Tooltip("Callbacks to activate when sensor is triggered.")]
    [SerializeField] private UnityEvent weightSensorActivated;
    [Tooltip("Callbacks to activate when sensor is no longer triggered.")]
    [SerializeField] private UnityEvent weightSensorDeactivated;
    
    /// <summary>
    /// <p>Reference to Characters game object that is over sensor.</p>
    ///
    /// <p>I use a HashSet for cases when Prince and an enemy are fighting over falling ground.</p>
    /// </summary>
    public HashSet<GameObject> CharactersOverSensor { get; private set; }

    /// <summary>
    /// Whether this sensor has detected any character over it.
    /// </summary>
    public bool CharacterDetected => CharactersOverSensor.Count > 1;

    private bool _countingTime = false;
    private float _elapsedTime = 0;
    private bool _activated = false;
    private BoxCollider2D _sensorBox;

    /// <summary>
    /// Character states that should not activate this sensor.
    /// </summary>
    private HashSet<CharacterStatus.States>
        dontActivateInTheseCharacterStates = new HashSet<CharacterStatus.States>
        {
            CharacterStatus.States.RunningJump,
            CharacterStatus.States.WalkingJump
        };

    private void Awake()
    {
        _sensorBox = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CharactersOverSensor = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_countingTime)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= activationTime)
            {
                if (weightSensorActivated != null) weightSensorActivated.Invoke();
                CounterStop();
            }
        }
    }

    /// <summary>
    /// Start counting time to trigger sensor.
    /// </summary>
    private void CounterStart()
    {
        _countingTime = true;
    }

    /// <summary>
    /// <p>Stop counting time to trigger sensor.</p>
    ///
    /// <p>Next time this counter is activated, it will start from 0.</p>
    /// </summary>
    private void CounterStop()
    {
        _countingTime = false;
        _elapsedTime = 0;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Trigger volumes should not be detected to avoid fighting sensors to activate spikes.
        // But falling collider should be detected although is a trigger to keep spikes up
        // when a character is falling over them.
        if (col.isTrigger && col.name != "FallingCollider") return;
        GameObject detectedGameObject = GameObjectTools.Collider2GameObject(col);
        if (GameObjectTools.IsACharacter(detectedGameObject))
        {
            if (GameObjectTools.CharacterInState(detectedGameObject, dontActivateInTheseCharacterStates)) return;
            CharactersOverSensor.Add(detectedGameObject);
            CounterStart();
            _activated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject detectedGameObject = GameObjectTools.Collider2GameObject(other);
        if (GameObjectTools.IsACharacter(detectedGameObject))
        {
            if (GameObjectTools.CharacterInState(detectedGameObject, dontActivateInTheseCharacterStates)) return;
            RemoveGameObjectFromSensor(detectedGameObject);
        }
    }

    /// <summary>
    /// Remove given object from detected game object list.
    /// </summary>
    /// <param name="gameObjectToRemove">Game objet to remove from sensor.</param>
    private void RemoveGameObjectFromSensor(GameObject gameObjectToRemove)
    {
        CharactersOverSensor.Remove(gameObjectToRemove);
        if (CharactersOverSensor.Count == 0)
        {
            CounterStop();
            if (_activated)
            {
                if (weightSensorDeactivated != null) weightSensorDeactivated.Invoke();
                _activated = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // If Prince jumps from a switch then his collider is disabled so OnTriggerExit is not called. We must
        // check if our CharacterOverSensor list is actually up to date.
        HashSet<GameObject> detectedCharacters = new HashSet<GameObject>(CharactersOverSensor);
        foreach (GameObject o in detectedCharacters)
        {
            foreach (Collider2D col in o.GetComponentsInChildren<Collider2D>())
            {
                if (col.isTrigger && col.name != "FallingCollider") continue;
                if (_sensorBox.IsTouching(col))
                {
                    _activated = true;
                    return;
                }
            }
            RemoveGameObjectFromSensor(o);
        }
    }
}

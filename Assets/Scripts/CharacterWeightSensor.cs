using System;
using System.Collections;
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
    [Tooltip("Callbacks to activate when sensor is triggered.")]
    [SerializeField] private UnityEvent weightSensorActivated;
    
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
                weightSensorActivated.Invoke();
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

    /// <summary>
    /// Assess given game object to find out it it is a Character.
    /// </summary>
    /// <param name="detectedGameObject">Given game object to assess.</param>
    /// <returns>True if given game object is a Character.</returns>
    private bool IsACharacter(GameObject detectedGameObject)
    {
        CharacterStatus characterStatus = detectedGameObject.GetComponentInChildren<CharacterStatus>();
        return characterStatus != null;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject detectedGameObject = col.transform.root.gameObject;
        if (IsACharacter(detectedGameObject))
        {
            CharactersOverSensor.Add(detectedGameObject);
            CounterStart();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject detectedGameObject = other.transform.root.gameObject;
        if (IsACharacter(detectedGameObject))
        {
            CharactersOverSensor.Remove(detectedGameObject);
            if (CharactersOverSensor.Count == 0)
            {
                CounterStop();
            }
        }
    }
}

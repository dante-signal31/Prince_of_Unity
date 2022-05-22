using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to detect if this ground is falling over a character.
    /// </summary>
    public class FallingGroundCharacterDetector : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to notify that we are crashing over a character.")] 
        [SerializeField] private FallingGroundCharacterInteractions fallingGroundCharacterInteractions;

        // private void OnTriggerEnter2D(Collider2D col)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // private void OnTriggerExit2D(Collider2D other)
        // {
        //     throw new NotImplementedException();
        // }
    }
}
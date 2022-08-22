using System;
using Prince;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    /// <summary>
    /// This event is raised by Prince characters to signal an small potion has been taken.
    /// </summary>
    public class SmallPotionTaken : EventArgs { }

    /// <summary>
    /// This event is raised by sword when it is taken by Prince.
    /// </summary>
    public class SwordTaken : EventArgs { }

    [Header("WIRING:")] 
    [Tooltip("Needed to register game wide events.")]
    [SerializeField] private EventBus eventBus;
        
    private void Awake()
    {
        eventBus.RegisterEvent<SmallPotionTaken>();
        eventBus.RegisterEvent<SwordTaken>();
    }
}

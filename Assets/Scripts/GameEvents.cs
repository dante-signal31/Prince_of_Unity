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

    /// <summary>
    /// This event is raised every time a character changes his life points.
    /// </summary>
    public class CharacterLifeUpdated : EventArgs
    {
        /// <summary>
        /// New current life.
        /// </summary>
        public int CurrentLife { get; private set; }
        
        /// <summary>
        /// New maximum life.
        /// </summary>
        public int MaximumLife { get; private set; }
        
        public CharacterLifeUpdated(int currentLife, int maximumLife)
        {
            CurrentLife = currentLife;
            MaximumLife = maximumLife;
        }
    }

    /// <summary>
    /// This event is raised every time a guard enters a room.
    /// </summary>
    public class GuardEnteredTheRoom : EventArgs
    {
        public GameObject Guard { get; private set; }

        public GuardEnteredTheRoom(GameObject guard)
        {
            Guard = guard;
        }
    }

    /// <summary>
    /// This event is raised every time a guards enters a room.
    /// </summary>
    public class GuardLeftTheRoom : GuardEnteredTheRoom
    {
        public GuardLeftTheRoom(GameObject guard): base(guard) { }
    }

    /// <summary>
    /// This event is raised every time Prince changes of room.
    /// </summary>
    public class PrinceEnteredNewRoom : EventArgs { }



    [Header("WIRING:")] 
    [Tooltip("Needed to register game wide events.")]
    [SerializeField] private EventBus eventBus;
        
    private void Awake()
    {
        eventBus.RegisterEvent<SmallPotionTaken>();
        eventBus.RegisterEvent<SwordTaken>();
        eventBus.RegisterEvent<CharacterLifeUpdated>();
        eventBus.RegisterEvent<GuardEnteredTheRoom>();
        eventBus.RegisterEvent<GuardLeftTheRoom>();
        eventBus.RegisterEvent<PrinceEnteredNewRoom>();
    }
}

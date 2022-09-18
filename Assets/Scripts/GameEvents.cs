using System;
using Prince;
using UnityEngine;
using UnityEngine.Video;

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
    public class NoGuardInTheRoom : GuardEnteredTheRoom
    {
        public NoGuardInTheRoom(GameObject guard): base(guard) { }
    }

    /// <summary>
    /// This event is raised every time Prince changes of room.
    /// </summary>
    public class PrinceEnteredNewRoom : EventArgs { }

    /// <summary>
    /// This event is raised to announce a video clip is being played.
    /// </summary>
    public class VideoPlayStart : EventArgs
    {
        public VideoClip Clip { get; private set; }

        public VideoPlayStart(VideoClip clip)
        {
            Clip = clip;
        }
    }
    
    /// <summary>
    /// This event is raised when a video that was being played reached its end.
    /// </summary>
    public class VideoPlayEnd : VideoPlayStart
    {
        public VideoPlayEnd(VideoClip clip): base(clip){}
    }

    /// <summary>
    /// This event is raised when Prince is present at current scene.
    /// </summary>
    public class PrinceInTheScene : EventArgs { }

    /// <summary>
    /// This event is raised when LevelLoader loads a new level.
    /// </summary>
    public class LevelLoaded : EventArgs
    {
        public string LevelName { get; }

        public LevelLoaded(string levelName)
        {
            LevelName = levelName;
        }
    }



    [Header("WIRING:")] 
    [Tooltip("Needed to register game wide events.")]
    [SerializeField] private EventBus eventBus;
        
    private void Awake()
    {
        eventBus.RegisterEvent<SmallPotionTaken>();
        eventBus.RegisterEvent<SwordTaken>();
        eventBus.RegisterEvent<CharacterLifeUpdated>();
        eventBus.RegisterEvent<GuardEnteredTheRoom>();
        eventBus.RegisterEvent<NoGuardInTheRoom>();
        eventBus.RegisterEvent<PrinceEnteredNewRoom>();
        eventBus.RegisterEvent<VideoPlayStart>();
        eventBus.RegisterEvent<VideoPlayEnd>();
        // eventBus.RegisterEvent<PrinceInTheScene>();
        eventBus.RegisterEvent<LevelLoaded>();
    }
}

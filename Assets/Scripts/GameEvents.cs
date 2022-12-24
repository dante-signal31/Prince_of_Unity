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
    /// This event is raised when Prince is killed at first level after taking sword.
    /// </summary>
    public class SwordLost : EventArgs {}

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
    public class PrinceEnteredNewRoom : EventArgs
    {
        public Room NewRoom { get; }

        public PrinceEnteredNewRoom(Room newRoom)
        {
            NewRoom = newRoom;
        }
    }

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

    /// <summary>
    /// This event is raised when LevelLoader reloads current level.
    /// </summary>
    public class LevelReloaded : LevelLoaded
    {
        public LevelReloaded(string levelName):base(levelName){}
    }

    /// <summary>
    /// This event is raised when time to read a text screen ends.
    /// </summary>
    public class TextScreenTimeout : EventArgs
    {
        public string LevelName { get; }
        
        public TextScreenTimeout(string levelName)
        {
            LevelName = levelName;
        }
    }

    /// <summary>
    /// This event is raised when Prince hangs from a climbable.
    /// </summary>
    public class PrinceHanged : EventArgs
    {
        public Vector3 Position { get; }

        public PrinceHanged(Vector3 position)
        {
            Position = position;
        }
    }

    /// <summary>
    /// This event is raised when Prince ends his climbing or descending from a climbable.
    /// </summary>
    public class PrinceClimbingEnded : PrinceHanged
    {
        public PrinceClimbingEnded(Vector3 position) : base(position) { }
    }

    /// <summary>
    /// This event raises when game has ended.
    /// </summary>
    public class GameEnded : EventArgs { }

    /// <summary>
    /// This event is raised when Prince deads.
    /// </summary>
    public class PrinceDead : EventArgs
    {
        public bool DeadBySword { get; }

        public PrinceDead(bool deadBySword = false)
        {
            DeadBySword = deadBySword;
        }
    }

    /// <summary>
    /// This event is raised when pause key is pressed. 
    /// </summary>
    public class PauseKeyPressed : EventArgs { }
    
    /// <summary>
    /// This event is raised when time increase cheat key is used.
    /// </summary>
    public class TimeIncreaseKeyPressed: EventArgs {}
    
    /// <summary>
    /// This event is raised when time decrease cheat key is used.
    /// </summary>
    public class TimeDecreaseKeyPressed: EventArgs {}

    /// <summary>
    /// This event is raised when kill current guard cheat key is used.
    /// </summary>
    public class KillCurrentGuardKeyPressed : EventArgs { }
    
    /// <summary>
    /// This event is raised when user ask to leave the game.
    /// </summary>
    public class QuitRequested: EventArgs { }

    /// <summary>
    /// This event is raised when user confirms when asked.
    /// </summary>
    public class UserConfirmation : EventArgs { }
    
    /// <summary>
    /// This event is raised when user cancels what initially asked.
    /// </summary>
    public class UserCancelation : EventArgs { }

    // /// <summary>
    // /// This event is raised whenever timer is disabled.
    // /// </summary>
    // public class TimerPaused : EventArgs { }
    //
    // /// <summary>
    // /// This event is raised whenever timer is enabled.
    // /// </summary>
    // public class TimerResumed: EventArgs {}


    [Header("WIRING:")] 
    [Tooltip("Needed to register game wide events.")]
    [SerializeField] private EventBus eventBus;
        
    private void Awake()
    {
        eventBus.RegisterEvent<SmallPotionTaken>();
        eventBus.RegisterEvent<SwordTaken>();
        eventBus.RegisterEvent<SwordLost>();
        eventBus.RegisterEvent<CharacterLifeUpdated>();
        eventBus.RegisterEvent<GuardEnteredTheRoom>();
        eventBus.RegisterEvent<NoGuardInTheRoom>();
        eventBus.RegisterEvent<PrinceEnteredNewRoom>();
        eventBus.RegisterEvent<VideoPlayStart>();
        eventBus.RegisterEvent<VideoPlayEnd>();
        eventBus.RegisterEvent<LevelLoaded>();
        eventBus.RegisterEvent<LevelReloaded>();
        eventBus.RegisterEvent<TextScreenTimeout>();
        eventBus.RegisterEvent<GameEnded>();
        eventBus.RegisterEvent<PrinceHanged>();
        eventBus.RegisterEvent<PrinceClimbingEnded>();
        eventBus.RegisterEvent<PrinceDead>();
        eventBus.RegisterEvent<PauseKeyPressed>();
        eventBus.RegisterEvent<TimeIncreaseKeyPressed>();
        eventBus.RegisterEvent<TimeDecreaseKeyPressed>();
        eventBus.RegisterEvent<KillCurrentGuardKeyPressed>();
        eventBus.RegisterEvent<QuitRequested>();
        eventBus.RegisterEvent<UserConfirmation>();
        eventBus.RegisterEvent<UserCancelation>();
    }
}

﻿using System.Collections;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// <p>This controller takes commands and send them to movement and physics components.</p>
    /// <br/>
    /// <p>This way you can use user generated commands while playing and recorded commands while testing.</p>
    /// <br/>
    /// <p>Being aware of that, Prince input execution follows this order:<br/>
    /// InputActionController --> InputController --> CommandController</p>
    /// <br/>
    /// <p>While guard input execution follows this order:<br/>
    /// GuardController --> InputController --> CommandController</p>
    /// </summary>
    public class CommandController : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to set state flags depending on commands.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to signal changes in climbing/hangings conditions.")] 
        // Enemy does not climb, so he will keep this value to null.
        [SerializeField] private ClimberInteractions climbingInteractions;
        [Tooltip("Needed to increment life through cheat keys.")] 
        [SerializeField] private CharacterStatus characterStatus;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private CommandSequence _commandQueue = new CommandSequence();
        private LevelLoader _levelLoader;
        private EventBus _eventBus;

        private void Awake()
        {
            _levelLoader = GameObject.Find("GameManagers").GetComponentInChildren<LevelLoader>();
            _eventBus = GameObject.Find("GameManagers").GetComponentInChildren<EventBus>();
        }

        /// <summary>
        /// Add a command to pending commands to execute queue.
        /// </summary>
        /// <param name="command">Command to add.</param>
        public IEnumerator ExecuteCommand(Command command)
        {
            switch (command.Action)
            {
                case Command.CommandType.Action:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed Action at {Time.time}", showLogs);
                    stateMachine.SetTriggerOneFrame("ActionPressed", this);
                    stateMachine.SetBool("ActionHoldPressed", true);
                    if (climbingInteractions != null) climbingInteractions.ActionPushed();
                    break;
                case Command.CommandType.Block:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed Block at {Time.time}", showLogs);
                    // stateMachine.SetTriggerOneFrame("Block", this);
                    stateMachine.SetTrigger("Block");
                    break;
                case Command.CommandType.Strike:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Execute strike at {Time.time}", showLogs);
                    // stateMachine.SetTriggerOneFrame("Strike", this);
                    stateMachine.SetTrigger("Strike");
                    break;
                case Command.CommandType.WalkRightWithSword:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Execute  walk right with sword at {Time.time}", showLogs);
                    stateMachine.SetTriggerOneFrame("WalkRightWithSword", this);
                    break;
                case Command.CommandType.WalkLeftWithSword:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Execute  walk left with sword at {Time.time}", showLogs);
                    stateMachine.SetTriggerOneFrame("WalkLeftWithSword", this);
                    break;
                case Command.CommandType.Sheathe:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed Sheathe at {Time.time}", showLogs);
                    stateMachine.SetTriggerOneFrame("Sheathe", this);
                    break;
                case Command.CommandType.Duck:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed Duck at {Time.time}", showLogs);
                    stateMachine.SetTriggerOneFrame("Duck", this);
                    break;
                case Command.CommandType.CrouchWalk:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed CrouchWalk at {Time.time}", showLogs);
                    stateMachine.SetTriggerOneFrame("CrouchWalk", this);
                    break;
                case Command.CommandType.Stand:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed Stand at {Time.time}", showLogs);
                    // stateMachine.SetTriggerOneFrame("Stand", this);
                    stateMachine.SetTrigger("Stand");
                    break;
                case Command.CommandType.Jump:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed Jump at {Time.time}", showLogs);
                    stateMachine.SetTrigger("Jump");
                    climbingInteractions.JumpPushed();
                    break;
                case Command.CommandType.StopJump:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed Jump at {Time.time}", showLogs);
                    if (climbingInteractions != null)
                    {
                        if (climbingInteractions.ClimbingInProgress) stateMachine.SetTrigger("StopJump");
                        climbingInteractions.JumpReleased();
                    }
                    break;
                case Command.CommandType.Stop:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed Stop at {Time.time}", showLogs);
                    // stateMachine.SetTriggerOneFrame("Stop", this);
                    stateMachine.SetTrigger("Stop");
                    if (stateMachine.GetBool("RunHoldPressed")) stateMachine.SetBool("RunHoldPressed", false);
                    break;
                case Command.CommandType.StopMovingRight:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed Stop at {Time.time}", showLogs);
                    // stateMachine.SetTriggerOneFrame("Stop", this);
                    stateMachine.SetTrigger("Stop");
                    if (stateMachine.GetBool("MoveRightHoldPressed")) stateMachine.SetBool("MoveRightHoldPressed", false);
                    break;
                case Command.CommandType.StopMovingLeft:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed Stop at {Time.time}", showLogs);
                    // stateMachine.SetTriggerOneFrame("Stop", this);
                    stateMachine.SetTrigger("Stop");
                    if (stateMachine.GetBool("MoveLeftHoldPressed")) stateMachine.SetBool("MoveLeftHoldPressed", false);
                    break;
                case Command.CommandType.StopAction:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed StopAction at {Time.time}", showLogs);
                    stateMachine.SetBool("ActionHoldPressed", false);
                    if (climbingInteractions != null) climbingInteractions.ActionReleased();
                    break;
                case Command.CommandType.RunLeft:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed RunLeft at {Time.time}", showLogs);
                    // Must keep RunLeft trigger for turning backs.
                    stateMachine.SetTriggerOneFrame("RunLeft", this);
                    stateMachine.SetBool("RunHoldPressed", true);
                    stateMachine.SetBool("MoveLeftHoldPressed", true);
                    break;
                case Command.CommandType.RunRight:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed RunRight at {Time.time}", showLogs);
                    // Must keep RunRight trigger for turning backs.
                    stateMachine.SetTriggerOneFrame("RunRight", this);
                    stateMachine.SetBool("RunHoldPressed", true);
                    stateMachine.SetBool("MoveRightHoldPressed", true);
                    break;
                case Command.CommandType.WalkLeft:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed WalkLeft at {Time.time}", showLogs);
                    stateMachine.SetTriggerOneFrame("WalkLeft", this);
                    break;
                case Command.CommandType.WalkRight:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed WalkRight at {Time.time}", showLogs);
                    stateMachine.SetTriggerOneFrame("WalkRight", this);
                    break;
                case Command.CommandType.SkipToNextLevel:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed cheat SkipToNextLevel at {Time.time}", showLogs);
                    _levelLoader.LoadNextScene();
                    break;
                case Command.CommandType.Pause:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed pause at {Time.time}", showLogs);
                    _eventBus.TriggerEvent(new GameEvents.PauseKeyPressed(), this);
                    break;
                case Command.CommandType.AddExtraBarOfLife:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed add extra bar of life at {Time.time}", showLogs);
                    characterStatus.MaximumLife++;
                    break;
                case Command.CommandType.HealLifePoint:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed heal life point at {Time.time}", showLogs);
                    characterStatus.Life++;
                    break;
                case Command.CommandType.IncreaseTime:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed increase time at {Time.time}", showLogs);
                    _eventBus.TriggerEvent(new GameEvents.TimeIncreaseKeyPressed(), this);
                    break;
                case Command.CommandType.DecreaseTime:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed decrease time at {Time.time}", showLogs);
                    _eventBus.TriggerEvent(new GameEvents.TimeDecreaseKeyPressed(), this);
                    break;
                case Command.CommandType.KillCurrentGuard:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed kill current guard at {Time.time}", showLogs);
                    _eventBus.TriggerEvent(new GameEvents.KillCurrentGuardKeyPressed(), this);
                    break;
                case Command.CommandType.Quit:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed quit application at {Time.time}", showLogs);
                    _eventBus.TriggerEvent(new GameEvents.QuitRequested(), this);
                    break;
                case Command.CommandType.Confirm:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed user confirmation at {Time.time}", showLogs);
                    _eventBus.TriggerEvent(new GameEvents.UserConfirmation(), this);
                    break;
                case Command.CommandType.Cancel:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed user cancelation at {Time.time}", showLogs);
                    _eventBus.TriggerEvent(new GameEvents.UserCancelation(), this);
                    break;
                case Command.CommandType.GetSword:
                    this.Log($"(CommandController - {transform.parent.transform.gameObject.name}) Executed cheat key for get sword at {Time.time}", showLogs);
                    characterStatus.HasSword = true;
                    _eventBus.TriggerEvent(new GameEvents.SwordTaken(), this);
                    break;
            }
            yield return null;
        }

        /// <summary>
        /// Replay a sequence of stored commands over current command.
        /// </summary>
        /// <param name="commandSequence">Command sequence to execute.</param>
        public IEnumerator ReplayCommandSequence(CommandSequence commandSequence)
        {
            _commandQueue.PushCommandSequence(commandSequence);
            while (_commandQueue.Count > 0)
            {
                Command command = PopCommand();
                this.Log($"(CommandController - {transform.root.name}) Replay recorded command {command.Action} with delay {command.Delay}", showLogs);
                // Don't use usual WaitForSeconds or you won't be able to unpause game after calling a pause command. 
                yield return new WaitForSecondsRealtime(command.Delay);
                StartCoroutine(ExecuteCommand(command));
            }
        }

        /// <summary>
        /// Extract a command from pending commands to execute queue.
        /// </summary>
        /// <returns>A command pending to execute.</returns>
        private Command PopCommand()
        {
            return _commandQueue.PopCommand();
        }

    } 
}

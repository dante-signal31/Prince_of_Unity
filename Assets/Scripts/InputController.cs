using UnityEditor;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// <p>This component translates Input calls received from a InputActionController, or an AI controller,
    /// to commands. Those commands are sent to a CommandController which is intended to submit them to
    /// destination components.</p>
    /// <br/>
    /// <p>This component is shared between main characters and enemy characters. Its main utility is that
    /// it allows to record commands received by character so they can be replayed at tests.</p>
    /// <br/>
    /// <p>Being aware of that, Prince input execution follows this order:<br/>
    /// InputActionController --> InputController --> CommandController</p>
    /// <br/>
    /// <p>While guard input execution follows this order:<br/>
    /// GuardController --> InputController --> CommandController</p>
    /// </summary>
    public class InputController : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to submit command to different components.")]
        [SerializeField] private CommandController commandController;
        
        [Header("CONFIGURATION:")]
        [Tooltip("ONLY FOR TESTING. Command recording: If set to Recording, every command sent to character will be recorded in given file.")]
        [HelpBar("ONLY FOR TESTING. If set to Recording, every command sent to character will be recorded in given file.", MessageType.Warning)]
        [SerializeField] private InputControllerStates recordCommandAction;
        [Tooltip("Where recorded commands should be recorded.")]
        [SerializeField] private string recordedCommandsFile;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        

        private bool _isRecording;
        private float _startTime;
        private CommandSequence _recordedCommandSequence = new CommandSequence();

        private void Start()
        {
            _startTime = Time.time;
            if (recordCommandAction == InputControllerStates.Replaying)
            {
                ReplayRecordedCommands();    
            }
            else
            {
                _isRecording = (recordCommandAction == InputControllerStates.Recording);
            }
        }

        /// <summary>
        /// Deserialize recorded command from given file and send it to command controller for execution.
        /// </summary>
        private void ReplayRecordedCommands()
        {
            _recordedCommandSequence = CommandSequence.Load(recordedCommandsFile);
            StartCoroutine(commandController.ReplayCommandSequence(_recordedCommandSequence));
        }

        /// <summary>
        /// Serialize recorded commands and save them to given files.
        /// </summary>
        private void SaveRecordedCommands()
        {
            _recordedCommandSequence.Save(recordedCommandsFile);
        }

        /// <summary>
        /// Returns elapsed time and resets counter to zero.
        /// </summary>
        /// <returns>Elapsed time.</returns>
        private float GetElapsedTime()
        {
            float elapsedTime = Time.time - _startTime;
            _startTime = Time.time;
            return elapsedTime;
        }

        /// <summary>
        /// Add command to command controller for execution.
        /// </summary>
        /// <param name="commandType">Command type to execute.</param>
        private void ExecuteCommand(Command.CommandType commandType)
        {
            float waitingTime = (_isRecording) ? GetElapsedTime() : 0;
            Command command = new Command(waitingTime, commandType);
            if (_isRecording) _recordedCommandSequence.PushCommand(command);
            if (gameObject.activeSelf) StartCoroutine(commandController.ExecuteCommand(command));
        }

        private void OnApplicationQuit()
        {
            if (_isRecording)
            {
                this.Log("(InputController) Saving commands.", showLogs);
                SaveRecordedCommands();
            }
        }

        public void RunRight()
        {
            ExecuteCommand(Command.CommandType.RunRight);
        }

        public void RunLeft()
        {
            ExecuteCommand(Command.CommandType.RunLeft);
        }

        public void WalkRight()
        {
            ExecuteCommand(Command.CommandType.WalkRight);
        }

        public void WalkLeft()
        {
            ExecuteCommand(Command.CommandType.WalkLeft);
        }

        public void CrouchWalkRight()
        {
            ExecuteCommand(Command.CommandType.CrouchWalk);
        }

        public void CrouchWalkLeft()
        {
            ExecuteCommand(Command.CommandType.CrouchWalk);
        }

        public void Stop()
        {
            ExecuteCommand(Command.CommandType.Stop);
        }

        public void StopJump()
        {
            ExecuteCommand(Command.CommandType.StopJump);
        }

        public void StopAction()
        {
            ExecuteCommand(Command.CommandType.StopAction);
        }

        public void Duck()
        {
            ExecuteCommand(Command.CommandType.Duck);
        }

        public void Stand()
        {
            ExecuteCommand(Command.CommandType.Stand);
        }

        public void Action()
        {
            ExecuteCommand(Command.CommandType.Action);
        }

        public void Jump()
        {
            ExecuteCommand(Command.CommandType.Jump);
        }

        public void Block()
        {
            ExecuteCommand(Command.CommandType.Block);
        }

        public void Sheathe()
        {
            ExecuteCommand(Command.CommandType.Sheathe);
        }

        public void Strike()
        {
            ExecuteCommand(Command.CommandType.Strike);
        }
        
        public void WalkRightWithSword()
        {
            ExecuteCommand(Command.CommandType.WalkRightWithSword);
        }
        
        public void WalkLeftWithSword()
        {
            ExecuteCommand(Command.CommandType.WalkLeftWithSword);
        }

        public void SkipToNextLevel()
        {
            ExecuteCommand(Command.CommandType.SkipToNextLevel);
        }

        public void Pause()
        {
            ExecuteCommand(Command.CommandType.Pause);
        }

        public void AddExtraBarOfLife()
        {
            ExecuteCommand(Command.CommandType.AddExtraBarOfLife);
        }

        public void HealLifePoint()
        {
            ExecuteCommand(Command.CommandType.HealLifePoint);
        }

        public void IncreaseTime()
        {
            ExecuteCommand(Command.CommandType.IncreaseTime);
        }

        public void DecreaseTime()
        {
            ExecuteCommand(Command.CommandType.DecreaseTime);
        }

        public void KillCurrentGuard()
        {
            ExecuteCommand(Command.CommandType.KillCurrentGuard);
        }
    }
}

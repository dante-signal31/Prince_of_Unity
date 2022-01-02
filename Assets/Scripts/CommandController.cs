using System.Collections;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This controller takes commands and send them to movement and physics components.
    ///
    /// This way you can use user generated commands while playing and recorded commands while testing.
    /// </summary>
    public class CommandController : MonoBehaviour
    {
        [Tooltip("Needed to set state flags depending on commands.")]
        [SerializeField] private Animator stateMachine;
        
        private CommandSequence _commandQueue = new CommandSequence();

        /// <summary>
        /// Add a command to pending commands to execute queue.
        /// </summary>
        /// <param name="command">Command to add.</param>
        public IEnumerator ExecuteCommand(Command command)
        {
            yield return new WaitForSeconds(command.Delay);
            switch (command.Action)
            {
                case Command.CommandType.Action:
                    Debug.Log($"(CommandController) Executed Action at {Time.time}");
                    stateMachine.SetTrigger("ActionPressed");
                    break;
                case Command.CommandType.Block:
                    Debug.Log($"(CommandController) Executed Block at {Time.time}");
                    stateMachine.SetTrigger("Block");
                    break;
                case Command.CommandType.Strike:
                    Debug.Log($"(CommandController) Execute strike at {Time.time}");
                    stateMachine.SetTrigger("Strike");
                    break;
                case Command.CommandType.WalkRightWithSword:
                    Debug.Log($"(CommandController) Execute  walk right with sword at {Time.time}");
                    stateMachine.SetTrigger("WalkRightWithSword");
                    break;
                case Command.CommandType.WalkLeftWithSword:
                    Debug.Log($"(CommandController) Execute  walk left with sword at {Time.time}");
                    stateMachine.SetTrigger("WalkLeftWithSword");
                    break;
                case Command.CommandType.Sheathe:
                    Debug.Log($"(CommandController) Executed Sheathe at {Time.time}");
                    stateMachine.SetTrigger("Sheathe");
                    break;
                case Command.CommandType.Duck:
                    Debug.Log($"(CommandController) Executed Duck at {Time.time}");
                    break;
                case Command.CommandType.Jump:
                    Debug.Log($"(CommandController) Executed Jump at {Time.time}");
                    break;
                case Command.CommandType.Stop:
                    Debug.Log($"(CommandController) Executed Stop at {Time.time}");
                    break;
                case Command.CommandType.StopAction:
                    Debug.Log($"(CommandController) Executed StopAction at {Time.time}");
                    break;
                case Command.CommandType.RunLeft:
                    Debug.Log($"(CommandController) Executed RunLeft at {Time.time}");
                    stateMachine.SetTrigger("RunLeft");
                    break;
                case Command.CommandType.RunRight:
                    Debug.Log($"(CommandController) Executed RunRight at {Time.time}");
                    stateMachine.SetTrigger("RunRight");
                    break;
                case Command.CommandType.WalkLeft:
                    Debug.Log($"(CommandController) Executed WalkLeft at {Time.time}");
                    stateMachine.SetTrigger("WalkLeft");
                    break;
                case Command.CommandType.WalkRight:
                    Debug.Log($"(CommandController) Executed WalkRight at {Time.time}");
                    stateMachine.SetTrigger("WalkRight");
                    break;
            }
        }

        // /// <summary>
        // /// Add a command sequence to pending commands to execute queue.
        // /// </summary>
        // /// <param name="commandSequence">Command sequence to add.</param>
        // public void PushCommandSequence(CommandSequence commandSequence)
        // {
        //     _commandQueue.PushCommandSequence(commandSequence);
        // }

        public void ReplayCommandSequence(CommandSequence commandSequence)
        {
            _commandQueue.PushCommandSequence(commandSequence);
            while (_commandQueue.Count > 0)
            {
                Command command = PopCommand();
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

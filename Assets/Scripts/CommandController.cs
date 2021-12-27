using System.Collections;
using System.Collections.Generic;
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
        private CommandSequence _commandQueue;

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
                    break;
                case Command.CommandType.Block:
                    Debug.Log($"(CommandController) Executed Block at {Time.time}");
                    break;
                case Command.CommandType.Duck:
                    Debug.Log($"(CommandController) Executed Duck at {Time.time}");
                    break;
                case Command.CommandType.Jump:
                    Debug.Log($"(CommandController) Executed Jump at {Time.time}");
                    break;
                case Command.CommandType.Sheathe:
                    Debug.Log($"(CommandController) Executed Sheathe at {Time.time}");
                    break;
                case Command.CommandType.Stop:
                    Debug.Log($"(CommandController) Executed Stop at {Time.time}");
                    break;
                case Command.CommandType.StopAction:
                    Debug.Log($"(CommandController) Executed StopAction at {Time.time}");
                    break;
                case Command.CommandType.RunLeft:
                    Debug.Log($"(CommandController) Executed RunLeft at {Time.time}");
                    break;
                case Command.CommandType.RunRight:
                    Debug.Log($"(CommandController) Executed RunRight at {Time.time}");
                    break;
                case Command.CommandType.WalkLeft:
                    Debug.Log($"(CommandController) Executed WalkLeft at {Time.time}");
                    break;
                case Command.CommandType.WalkRight:
                    Debug.Log($"(CommandController) Executed WalkRight at {Time.time}");
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

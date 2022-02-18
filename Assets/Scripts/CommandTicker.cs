using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Fighting commands should be synchronized to keep animation interactions smooth. So fighting commands
    /// have to be stored in a buffer to be executed at a constant pace.
    ///
    /// This component keeps a command sequence by character and execute a command from every
    /// sequence at a fixed period.
    /// </summary>
    public class CommandTicker : MonoBehaviour
    {
        [Header("CONFIGURATION:")]
        [Tooltip("Time period for command execution.")]
        [SerializeField] private float tickTime;

        
        public delegate void CommandExecuter(Command.CommandType command);
        
        /// <summary>
        /// A helper class to associate a CommandSequence with GameObject's method that must
        /// execute those commands.
        /// </summary>
        private class CommandSequenceExecuter
        {
            private CommandSequence _commandSequence;

            private CommandExecuter _commandExecuter;

            public CommandSequenceExecuter(CommandSequence commandSequence, CommandExecuter commandExecuter)
            {
                _commandSequence = commandSequence;
                _commandExecuter = commandExecuter;
            }

            /// <summary>
            /// Adds a new command to FIFO queue.
            /// </summary>
            /// <param name="command">Command to add.</param>
            public void PushCommand(Command.CommandType command)
            {
                // Ticked commands have no delays because their timing are managed by CommandTicker.
                Command _command = new Command(0, command);
                _commandSequence.PushCommand(_command);
            }

            /// <summary>
            /// Extract from FIFO queue the oldest command and execute it. 
            /// </summary>
            public void ExecuteNextCommand()
            {
                Command _command = _commandSequence.PopCommand();
                if (_command != null) _commandExecuter(_command.Action);
            }
        }

        private Dictionary<int, CommandSequenceExecuter> _characterCommandSequence;

        private void Awake()
        {
            _characterCommandSequence = new Dictionary<int, CommandSequenceExecuter>();
        }

        private void Start()
        {
            StartCoroutine(Ticker());
        }

        /// <summary>
        /// Create a new command sequence executer associated to an specific GameObject ID. 
        /// </summary>
        /// <param name="gameObjectID">The character's GameObject ID. Obtained from GameObject's GetInstanceID method.</param>
        /// <param name="commandExecuter">The characters method to pass extracted command to.</param>
        public void Register(int gameObjectID, CommandExecuter commandExecuter)
        {
            if (!_characterCommandSequence.ContainsKey(gameObjectID))
            {
                _characterCommandSequence.Add(
                    gameObjectID,
                    new CommandSequenceExecuter(new CommandSequence(), commandExecuter));
            }
        }

        /// <summary>
        /// Add command to this GameObject's command sequence.
        /// </summary>
        /// <param name="gameObjectID">The character's GameObject ID. Obtained from GameObject's GetInstanceID method.</param>
        /// <param name="command">Command to add to the queue.</param>
        public void PushCommand(int gameObjectID, Command.CommandType command)
        {
            _characterCommandSequence[gameObjectID].PushCommand(command);
        }

        /// <summary>
        /// Command execution loop.
        /// </summary>
        private IEnumerator Ticker()
        {
            while (true)
            {
                foreach (int ID in _characterCommandSequence.Keys)
                {
                    _characterCommandSequence[ID].ExecuteNextCommand();
                }
                yield return new WaitForSeconds(tickTime);
            }
        }
    }
}
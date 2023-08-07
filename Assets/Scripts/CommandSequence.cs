using System;
using System.Collections.Generic;
using System.IO;
// using System.Linq;
using UnityEngine;

namespace Prince
{
    public class CommandSequence: IEquatable<CommandSequence>
    {
        private const string FileExtension = "cse";
        
        private Queue<Command> CommandQueue = new Queue<Command>();
    
        public int Count => CommandQueue.Count;
    
        /// <summary>
        /// Add command to pending to execute command queue.
        ///
        /// Command queue is a FIFO queue.
        /// </summary>
        /// <param name="command">Command to add.</param>
        public void PushCommand(Command command)
        {
            //Debug.Log($"(CommandSequence) Registering command: {command.Action}");
            CommandQueue.Enqueue(command);
        }
    
        /// <summary>
        /// Add multiple commands in one go to command queue.
        /// </summary>
        /// <param name="commandSequenceToAdd">A queue of commands to add to CommandController current queue.</param>
        public void PushCommandSequence(CommandSequence commandSequenceToAdd)
        {
            while (commandSequenceToAdd.Count > 0)
            {
                Command commandToAdd = commandSequenceToAdd.PopCommand();
                CommandQueue.Enqueue(commandToAdd);
            }
        }
            
        /// <summary>
        /// Take a command from FIFO queue.
        /// </summary>
        /// <returns>Oldest command.</returns>
        public Command PopCommand()
        {
            return CommandQueue.Dequeue();
        }
        
        /// <summary>
        /// Unity built-in serialization is rather limited. Arrays and List apart it cannot serialize any other collection
        /// and only if it is included in a class. As we want to serialize a field not an entire class we need to include
        /// that field in a dummy wrapper class.
        ///
        /// See: https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
        /// </summary>
        /// <typeparam name="T">Type of the collection this class wraps.</typeparam>
        [Serializable]
        private class WrappedArray<T>
        {
            public T[] Items;
        }

        /// <summary>
        /// Unity built-in serialization cannot serialize Queues so we must convert that field in something Unity can
        /// work with, like an array.
        /// </summary>
        /// <returns></returns>
        private WrappedArray<Command> WrapCommandQueue()
        {
            Command[] commandArray = CommandQueue.ToArray();
            WrappedArray<Command> wrappedCommandArray = new WrappedArray<Command>();
            wrappedCommandArray.Items = commandArray;
            return wrappedCommandArray;
        }

        /// <summary>
        /// Deserialize jsonString and return saved CommandSequence after removing wrapping class.
        /// </summary>
        /// <param name="jsonString">JSON string to deserialize.</param>
        /// <returns>Recovered command sequence.</returns>
        private static CommandSequence UnWrapCommandQueue(string jsonString)
        {
            WrappedArray<Command> wrappedCommandArray = JsonUtility.FromJson<WrappedArray<Command>>(jsonString);
            CommandSequence sequence = new CommandSequence();
            foreach (Command command in wrappedCommandArray.Items)
            {
                sequence.PushCommand(command);
            }
            return sequence;
        }

        /// <summary>
        /// Save this command sequence in a file.
        ///
        /// File will have a .cse extension.
        /// </summary>
        /// <param name="filePathname">Path to file where command should be recorded. Don't include .cse extension. </param>
        public void Save(string filePathname)
        {
            // Unity built-in serialization cannot serialize Queues so we must convert that field in something Unity can
            // work with, like an array.
            WrappedArray<Command> wrappedCommandArray = WrapCommandQueue();
            string jsonString = JsonUtility.ToJson(wrappedCommandArray, true);
            string filePathnameWithExtension = Path.ChangeExtension(filePathname, FileExtension);
            //Debug.Log($"(CommandSequence) Saving commands at {filePathnameWithExtension}");
            File.WriteAllText(filePathnameWithExtension, jsonString);
        }
    
        /// <summary>
        /// Get a command sequence from a file.
        /// </summary>
        /// <param name="filePathname">Path to file where command is recorded. Don't include .cse extension.</param>
        /// <returns>Sequence of commands stored at file.</returns>
        public static CommandSequence Load(string filePathname)
        {
            string filePathnameWithExtension = Path.ChangeExtension(filePathname, FileExtension);
            string absoluteFilePathname = Path.GetFullPath(filePathnameWithExtension);
            
#if UNITY_STANDALONE_LINUX
            // Test paths are coded using Windows standard, if we are at linux we must change paths.
            absoluteFilePathname = absoluteFilePathname.Replace('\\', '/');
#endif
            
            // Debug.Log($"(CommandSequence) Loading commands from {filePathnameWithExtension}");
            Debug.Log($"(CommandSequence) Loading commands from {absoluteFilePathname}");
            // string jsonString = File.ReadAllText(filePathnameWithExtension);
            string jsonString = File.ReadAllText(absoluteFilePathname);
            CommandSequence sequence = UnWrapCommandQueue(jsonString);
            return sequence;
        }

        public bool Equals(CommandSequence other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CommandSequence)obj);
        }

        public override int GetHashCode()
        {
            int accumulatorHash = 0;
            if (CommandQueue != null)
            {
                Command[] commandArray = CommandQueue.ToArray();
                foreach (Command command in commandArray)
                {
                    int commandHash = command.GetHashCode();
                    accumulatorHash = HashCode.Combine(accumulatorHash, commandHash);
                }
            }
            return accumulatorHash;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Prince;
using Tests.UnitTests.Tools.Fs;
using UnityEditor;

namespace Tests.UnitTests
{
    public class CommandTests
    {
        /// <summary>
        /// Test we can compare individual commands.
        /// </summary>
        [Test]
        public void TestCommandEquality()
        {
            // Test setup.
            Command command = new Command(1, Command.CommandType.Jump);
            Command commandEqual = new Command(1, Command.CommandType.Jump);
            Command commandNotEqual = new Command(1, Command.CommandType.Block);
            Command commandNotEqual2 = new Command(2, Command.CommandType.Jump);
            Command commandNotEqual3 = new Command(2, Command.CommandType.Block);
            // Perform tests.
            Assert.True(command.Equals(commandEqual));
            Assert.False(command.Equals(commandNotEqual));
            Assert.False(command.Equals(commandNotEqual2));
            Assert.False(command.Equals(commandNotEqual3));
        }

        /// <summary>
        /// Test we can compare command sequences.
        /// </summary>
        [Test]
        public void TestCommandSequenceEquality()
        {
            // Test setup.
            Command command1 = new Command(0, Command.CommandType.Jump);
            Command command2 = new Command(1, Command.CommandType.RunRight);
            Command command3 = new Command(2, Command.CommandType.Duck);
            Command command4 = new Command(3, Command.CommandType.RunLeft);
            CommandSequence commandSequence = new CommandSequence();
            commandSequence.PushCommand(command1);
            commandSequence.PushCommand(command2);
            commandSequence.PushCommand(command3);
            CommandSequence commandSequenceEqual = new CommandSequence();
            commandSequenceEqual.PushCommand(command1);
            commandSequenceEqual.PushCommand(command2);
            commandSequenceEqual.PushCommand(command3);
            CommandSequence commandSequenceNotEqual = new CommandSequence();
            commandSequenceNotEqual.PushCommand(command1);
            commandSequenceNotEqual.PushCommand(command2);
            commandSequenceNotEqual.PushCommand(command4);
            CommandSequence commandSequenceNotEqual2 = new CommandSequence();
            commandSequenceNotEqual2.PushCommand(command1);
            commandSequenceNotEqual2.PushCommand(command2);
            commandSequenceNotEqual2.PushCommand(command3);
            commandSequenceNotEqual2.PushCommand(command4);
            // Perform tests.
            Assert.True(commandSequence.Equals(commandSequenceEqual));
            Assert.False(commandSequence.Equals(commandSequenceNotEqual));
            Assert.False(commandSequence.Equals(commandSequenceNotEqual2));
        }
        
        /// <summary>
        /// Test we recover what we save to disk.
        /// </summary>
        [Test]
        public void TestCommandSave()
        {
            using (Temp tempfile = new Temp(Temp.TempType.File))
            {
                // Test setup.
                CommandSequence sequence = new CommandSequence();
                sequence.PushCommand(new Command(0, Command.CommandType.RunRight));
                sequence.PushCommand(new Command(1, Command.CommandType.Jump));
                sequence.PushCommand(new Command(2, Command.CommandType.WalkRight));
                sequence.Save(tempfile.TempPath);
                // Perform test.
                CommandSequence recoveredSequence = CommandSequence.Load(tempfile.TempPath);
                Assert.True(recoveredSequence.Equals(sequence));
            }
        }

        // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // // `yield return null;` to skip a frame.
        // [UnityTest]
        // public IEnumerator CommandTestWithEnumeratorPasses()
        // {
        //     // Use the Assert class to test conditions.
        //     // Use yield to skip a frame.
        //     yield return null;
        // }
    }
}


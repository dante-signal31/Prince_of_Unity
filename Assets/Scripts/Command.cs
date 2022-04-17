using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// To make control interactions repeatable, they are encapsulated in Commands instances. Those commands
    /// are serializable and can be file stored to be reused later at tests.
    /// </summary>
    [Serializable]
    public class Command: IEquatable<Command>
    {
        [Serializable]
        public enum CommandType
        {
            RunRight,
            RunLeft,
            Jump,
            Duck,
            Stop,
            WalkRight,
            WalkLeft,
            Action,
            StopAction,
            Block,
            Sheathe,
            Strike,
            WalkRightWithSword,
            WalkLeftWithSword,
            Stand,
            CrouchWalk,
            StopJump,
        }

        [SerializeField] private float _delay;
        [SerializeField] private CommandType _action;

        public float Delay => _delay;
        public CommandType Action => _action;
    
        public Command(float Delay, CommandType Action)
        {
            this._delay = Delay;
            this._action = Action;
        }

        public bool Equals(Command other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _delay.Equals(other._delay) && _action == other._action;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Command)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_delay, (int)_action);
        }
    }
    
}

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
            RunRight = 0,
            RunLeft = 1,
            Jump = 2,
            Duck = 3,
            Stop = 4,
            WalkRight = 5,
            WalkLeft = 6,
            Action = 7,
            StopAction = 8,
            Block = 9,
            Sheathe = 10,
            Strike = 11,
            WalkRightWithSword = 12,
            WalkLeftWithSword = 13,
            Stand = 14,
            CrouchWalk = 15,
            StopJump = 16,
            SkipToNextLevel = 17,
            Pause = 18,
            AddExtraBarOfLife = 19,
            HealLifePoint = 20,
            IncreaseTime = 21,
            DecreaseTime = 22,
            KillCurrentGuard = 23,
            Quit = 24,
            Confirm = 25,
            Cancel = 26,
            GetSword = 27,
            StopMovingRight = 28,
            StopMovingLeft = 29
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

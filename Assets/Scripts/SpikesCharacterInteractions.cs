using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Trap component used to interact with characters.
    /// </summary>
    public class SpikesCharacterInteractions: TrapCharacterInteractions
    {
        protected override bool InvulnerableCharacter(GameObject character)
        {
            CharacterStatus characterStatus = character.GetComponentInChildren<CharacterStatus>();
            return characterStatus.CurrentState == CharacterStatus.States.Walk ||
                   characterStatus.CurrentState == CharacterStatus.States.RunningJump ||
                   characterStatus.CurrentState == CharacterStatus.States.VerticalJumpStart ||
                   characterStatus.CurrentState == CharacterStatus.States.VerticalJump ||
                   characterStatus.CurrentState == CharacterStatus.States.VerticalJumpEnd ||
                   characterStatus.CurrentState == CharacterStatus.States.Climbing ||
                   characterStatus.CurrentState == CharacterStatus.States.Descending ||
                   characterStatus.CurrentState == CharacterStatus.States.TurnBack ||
                   characterStatus.CurrentState == CharacterStatus.States.Idle;
        }
    }
}
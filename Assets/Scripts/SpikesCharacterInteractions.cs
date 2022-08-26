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
                   characterStatus.CurrentState == CharacterStatus.States.Idle;
        }
    }
}
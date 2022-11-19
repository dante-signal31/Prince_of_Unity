using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Trap component used to interact with characters.
    /// </summary>
    public class SpikesCharacterInteractions: TrapCharacterInteractions
    {
        // TODO: If prince is ending a vertical jump it should not be killed.
        protected override bool InvulnerableCharacter(GameObject character)
        {
            CharacterStatus characterStatus = character.GetComponentInChildren<CharacterStatus>();
            return characterStatus.CurrentState == CharacterStatus.States.Walk ||
                   characterStatus.CurrentState == CharacterStatus.States.RunningJump ||
                   characterStatus.CurrentState == CharacterStatus.States.Idle;
        }
    }
}
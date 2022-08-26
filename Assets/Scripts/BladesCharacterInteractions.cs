using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Trap component used to interact with characters.
    /// </summary>
    public class BladesCharacterInteractions: TrapCharacterInteractions
    {
        protected override bool InvulnerableCharacter(GameObject character)
        {
            // There is no way to survive a blade if it is activated over you.
            return false;
        }
        
    }
}
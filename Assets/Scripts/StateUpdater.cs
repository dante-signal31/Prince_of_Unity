using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// There is not an easy way to ask Animator which is the current state. So we make every state update
    /// an annotation at CharacterStatus. 
    /// </summary>
    public class StateUpdater : GenericStateUpdater<CharacterStatus.States, CharacterStatus>
    { }
}
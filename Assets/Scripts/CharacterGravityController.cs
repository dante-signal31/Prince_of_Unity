using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component controls how gravity affects character.
    ///
    /// It is used mainly to deactivate gravity for this character in special moments
    /// like jumps or death.
    /// </summary>
    public class CharacterGravityController: GravityController
    {
        [Header("WIRING:")]
        [Tooltip("Needed to know current character state.")]
        [SerializeField] private CharacterStatus stateMachineStatus;
        
        private void FixedUpdate()
        {

            switch (stateMachineStatus.CurrentState)
            {
                // TODO: Try to unify this first bunch of conditions with second one.
                case CharacterStatus.States.Dead:
                case CharacterStatus.States.DeadByFall:
                    DisableGravity();
                    break;
                case CharacterStatus.States.RunningJump:
                case CharacterStatus.States.WalkingJump:
                case CharacterStatus.States.Climbing:
                    if (GravityEnabled) DisableGravity();
                    break;
                default:
                    if (!GravityEnabled) EnableGravity();
                    break;
            }
        }
    }
}
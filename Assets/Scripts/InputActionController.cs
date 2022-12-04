using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prince
{
    /// <summary>
    /// <p>This component translates Input events received from a Player Input to an InputController.
    /// InputController translates those calls to Commands and send them to a CommandController to
    /// their execution. That way, both Guards and Prince can use InputController while their input comes
    /// from different sources, Prince from player input while guard input comes from an AI component.</p>
    /// <br/>
    /// <p>Action map switching is done from here.</p>
    /// <br/>
    /// <p>Being aware of that, Prince input execution follows this order:<br/>
    /// InputActionController --> InputController --> CommandController</p>
    /// <br/>
    /// <p>While guard input execution follows this order:<br/>
    /// GuardController --> InputController --> CommandController</p>
    ///
    /// </summary>
    public class InputActionController : MonoBehaviour
    {
        [Tooltip("Needed to switch action map between fighting and normal mode.")]
        [SerializeField] private PlayerInput playerInput;
        [Tooltip("Needed to convert actions into commands.")]
        [SerializeField] private InputController inputController;
        [Tooltip("Needed to give context. Some actions depends on sensors lectures to be converted to a command or another.")]
        [SerializeField] private EnemySensors sensors;
        [Tooltip("Needed to not duplicate inputs in some states.")]
        [SerializeField] private CharacterStatus characterStatus;
        
        private bool _actionPressed;
        private bool _jumpPressed;

        public void MoveRight(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (characterStatus.CurrentState == CharacterStatus.States.Crouch)
                {
                    inputController.CrouchWalkRight();
                }
                else if (_actionPressed)
                {
                    inputController.WalkRight();
                }
                else
                {
                    inputController.RunRight();
                }
            }

            if (context.canceled)
            {
                // if (!(_actionPressed || characterStatus.CurrentState == CharacterStatus.States.CrouchWalking)) 
                if (characterStatus.CurrentState != CharacterStatus.States.CrouchWalking)
                    inputController.Stop();
            }
        }
        
        public void MoveLeft(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (characterStatus.CurrentState == CharacterStatus.States.Crouch)
                {
                    inputController.CrouchWalkLeft();
                }
                else if (_actionPressed)
                {
                    inputController.WalkLeft();
                }
                else
                {
                    inputController.RunLeft();
                }
            }

            if (context.canceled)
            {
                if (!(_actionPressed ||
                      characterStatus.CurrentState == CharacterStatus.States.CrouchWalking))
                    inputController.Stop();
            }
        }

        public void Duck(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.Duck();
            if (context.canceled) inputController.Stand();
        }

        public void Action(InputAction.CallbackContext context)
        {
            // New input system does not have yet multiple keys support for the same binding, although
            // it is planned:
            //
            // https://forum.unity.com/threads/do-you-consider-walk-and-sprint-to-be-the-same-action.1231047/#post-7845540
            //
            // While that support arrives the only option we have is using flags to combine keys action. 
            if (context.started)
            {
                _actionPressed = true;
            }
                
            if (context.performed)
            {
                if (sensors.EnemySeen)
                {
                    if (playerInput.currentActionMap.name != "FightingActionMap")
                        playerInput.SwitchCurrentActionMap("FightingActionMap");
                }
                inputController.Action();
            }

            if (context.canceled)
            {
                // if (sensors.EnemySeen)
                //     inputController.StopAction();
                inputController.StopAction();
                _actionPressed = false;
            }
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _jumpPressed = true;
            }

            if (context.performed)
            {
                inputController.Jump();
            }
            
            if (context.canceled)
            {
                inputController.StopJump();
                _jumpPressed = false;
            }
        }

        public void Block(InputAction.CallbackContext context)
        {
            // If we are already blocking, don't send an block sword signal again.
            if (context.performed && characterStatus.CurrentState != CharacterStatus.States.BlockSword) 
                inputController.Block();
        }

        public void Sheathe(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                inputController.Sheathe();
                playerInput.SwitchCurrentActionMap("PrinceActionMap");
            }
        }

        public void Strike(InputAction.CallbackContext context)
        {
            // If we are already attacking, don't send an strike signal again.
            if (context.performed && characterStatus.CurrentState != CharacterStatus.States.AttackWithSword) 
                inputController.Strike();
        }

        public void WalkRightWithSword(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.WalkRightWithSword();
            if (context.canceled) inputController.Stop();
        }
        
        public void WalkLeftWithSword(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.WalkLeftWithSword();
            if (context.canceled) inputController.Stop();
        }
        
        public void SkipToNextLevel(InputAction.CallbackContext ctx)
        {
            if (ctx.started) inputController.SkipToNextLevel();
        }
        
        public void AddExtraBarOfLife(InputAction.CallbackContext ctx)
        {
            if (ctx.started) inputController.AddExtraBarOfLife();
        }
        
        public void HealLifePoint(InputAction.CallbackContext ctx)
        {
            if (ctx.started) inputController.HealLifePoint();
        }
        
        public void KillGuardInRoom(InputAction.CallbackContext ctx)
        {
            if (ctx.started) inputController.KillCurrentGuard();
        }
        
        public void IncreaseTime(InputAction.CallbackContext ctx)
        {
            if (ctx.started) inputController.IncreaseTime();
        }
        
        public void DecreaseTime(InputAction.CallbackContext ctx)
        {
            if (ctx.started) inputController.DecreaseTime();
        }

        public void Pause(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.Pause();
        }
        
        public void Quit(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.Quit();
        }
        
        public void Confirm(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.Confirm();
        }
        
        public void Cancel(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.Cancel();
        }
        
    }
}

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

        public void RunRight(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.RunRight();
            if (context.canceled) inputController.Stop();
        }
        
        public void RunLeft(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.RunLeft();
            if (context.canceled) inputController.Stop();
        }

        public void WalkRight(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.WalkRight();
            if (context.canceled) inputController.Stop();
        }

        public void WalkLeft(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.WalkLeft();
            if (context.canceled) inputController.Stop();
        }

        public void Duck(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.Duck();
            if (context.canceled) inputController.Stop();
        }

        public void Action(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (sensors.EnemySeen) playerInput.SwitchCurrentActionMap("FightingActionMap");
                inputController.Action();
            }
            if (context.canceled) inputController.StopAction();
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.Jump();
            if (context.canceled) inputController.Stop();
        }

        public void Block(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.Block();
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
            if (context.performed) inputController.Strike();
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
        
    }
}

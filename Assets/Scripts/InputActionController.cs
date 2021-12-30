using UnityEngine;
using UnityEngine.InputSystem;

namespace Prince
{
    /// <summary>
    /// This component translates Input events received from a Player Input to commands.
    /// Those commands are sent to character InputController.
    /// </summary>
    public class InputActionController : MonoBehaviour
    {
        [SerializeField] private InputController inputController;

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
            if (context.performed) inputController.Action();
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
            if (context.canceled) inputController.Stop();
        }

        public void Sheathe(InputAction.CallbackContext context)
        {
            if (context.performed) inputController.Sheathe();
            if (context.canceled) inputController.Stop();
        }
    }
}

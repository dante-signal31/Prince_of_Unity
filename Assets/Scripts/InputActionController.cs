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
            inputController.RunRight();
        }
        
        public void RunLeft(InputAction.CallbackContext context)
        {
            inputController.RunLeft();
        }

        public void WalkRight(InputAction.CallbackContext context)
        {
            inputController.WalkRight();
        }

        public void WalkLeft(InputAction.CallbackContext context)
        {
            inputController.WalkLeft();
        }

        public void Stop(InputAction.CallbackContext context)
        {
            inputController.Stop();
        }

        public void StopAction(InputAction.CallbackContext context)
        {
            inputController.StopAction();
        }

        public void Duck(InputAction.CallbackContext context)
        {
            inputController.Duck();
        }

        public void Action(InputAction.CallbackContext context)
        {
            inputController.Action();
        }

        public void Jump(InputAction.CallbackContext context)
        {
            inputController.Jump();
        }

        public void Block(InputAction.CallbackContext context)
        {
            inputController.Block();
        }

        public void Sheathe(InputAction.CallbackContext context)
        {
            inputController.Sheathe();
        }

    }
}


namespace Prince
{
    /// <summary>
    /// This component disables his game object when falling ground crashes.
    ///
    /// This way we avoid further interactions after ground has crashed.
    /// </summary>
    public class FallingGroundDisablingController : DisablingController<FallingGroundStatus.FallingGroundStates>
    { }
}
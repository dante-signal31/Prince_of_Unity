
namespace Prince
{
    /// <summary>
    /// There is not an easy way to ask Animator which is the current state. So we make every state update
    /// an annotation at ClimbableStatus. 
    /// </summary>
    public class ClimbableStateUpdater : GenericStateUpdater<ClimbableStatus.States, ClimbableStatus>
    { }
}
namespace Prince
{
    /// <summary>
    /// This component disables his game object when character enters a given state.
    ///
    /// This way we avoid further interactions while playing inter level animations or after dying in a trap.
    /// </summary>
    public class CharacterDisablingController : DisablingController<CharacterStatus.States>
    { }
}
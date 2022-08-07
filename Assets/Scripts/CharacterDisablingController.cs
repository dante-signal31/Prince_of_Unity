namespace Prince
{
    /// <summary>
    /// This component disables his game object when Prince enters an interlevel gate.
    ///
    /// This way we avoid further interactions while playing inter level animations.
    /// </summary>
    public class CharacterDisablingController : DisablingController<CharacterStatus.States>
    { }
}
namespace Prince
{
    /// <summary>
    /// Small potion component to make changes over Prince when hi drinks potion.
    /// </summary>
    public class SmallPotionCharacterInteractions: PickableCharacterInteractions
    {
        protected override void DoSomethingOverTaker(PickableInteractions taker)
        {
            taker.RestoreOneHealthPoint();
        }
    }
}
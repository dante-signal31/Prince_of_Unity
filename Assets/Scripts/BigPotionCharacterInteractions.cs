namespace Prince
{
    public class BigPotionCharacterInteractions: PickableCharacterInteractions
    {
        protected override void DoSomethingOverTaker(PickableInteractions taker)
        {
            taker.EnhanceHealth();
        }
    }
}
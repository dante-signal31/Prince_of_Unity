namespace Prince
{
    public class SwordCharacterInteractions: PickableCharacterInteractions
    {
        protected override void DoSomethingOverTaker(PickableInteractions taker)
        {
            taker.TakeSword();
        }
    }
}
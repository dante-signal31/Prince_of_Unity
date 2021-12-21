/// <summary>
/// Prefabs that have ground can have things over it (bones, garbage, etc), so we need methods to show it.
/// </summary>
public interface IGround
{
        public void PlaceThingsOverGround(ThingsOverGround thing);
}
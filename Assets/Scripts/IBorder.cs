/// <summary>
/// Prefabs that have ground should show border at right if it is at a very edge, but not if it has more
/// ground at right hand side.
/// </summary>
public interface IBorder
{
        /// <summary>
        /// Has this prefab an active border?.
        /// </summary>
        /// <returns>True if yes.</returns>
        public bool IsBorderShown();
        
        /// <summary>
        /// Set this prefab to show a border.
        /// </summary>
        /// <param name="showIt">True if prefab must show its border.</param>
        public void ShowBorder(bool showIt);
}
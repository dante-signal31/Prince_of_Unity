using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component manages sprites shown by a trap.
    /// </summary>
    public class TrapAppearance : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to show Prince corpse looking to right.")] 
        [SerializeField] private SpriteRenderer princeCorpseLookingRight;
        [Tooltip("Needed to show Prince corpse looking to left.")] 
        [SerializeField] private SpriteRenderer princeCorpseLookingLeft;
        [Tooltip("Needed to show Guard corpse looking to right.")] 
        [SerializeField] private SpriteRenderer guardCorpseLookingRight;
        [Tooltip("Needed to show Guard corpse looking to left.")] 
        [SerializeField] private SpriteRenderer guardCorpseLookingLeft;

        /// <summary>
        /// Show given corpse looking in given direction.
        /// </summary>
        /// <param name="corpse">Corpse to show.</param>
        /// <param name="lookingRight">Whether corpse should look rightwards.</param>
        private void ShowPrinceCorpse(Sprite corpse, bool lookingRight)
        {
            if (lookingRight)
            {
                princeCorpseLookingRight.sprite = corpse;
            }
            else
            {
                // By default corpses sprites look to right, but there is no need to flip them because 
                // sprite renderers looking to left are already flipped in their editor configuration.
                princeCorpseLookingLeft.sprite = corpse;
            }
        }
        
        /// <summary>
        /// Show given corpse looking in given direction.
        /// </summary>
        /// <param name="corpse">Corpse to show.</param>
        /// <param name="lookingRight">Whether corpse should look rightwards.</param>
        private void ShowGuardCorpse(Sprite corpse, bool lookingRight)
        {
            if (lookingRight)
            {
                guardCorpseLookingRight.sprite = corpse;
            }
            else
            {
                // By default corpses sprites look to right, but there is no need to flip them because 
                // sprite renderers looking to left are already flipped in their editor configuration.
                guardCorpseLookingLeft.sprite = corpse;
            }
        }

        /// <summary>
        /// Show given corpse looking in given direction.
        /// </summary>
        /// <param name="isPrince">Whether this coprse is from prince or not.</param>
        /// <param name="lookingRight">Whether corpse should look rightwards.</param>
        /// <param name="corpse">Corpse to show.</param>
        public void ShowCorpse(bool isPrince, bool lookingRight, Sprite corpse)
        {
            if (isPrince)
            {
                ShowPrinceCorpse(corpse, lookingRight);
            }
            else
            {
                ShowGuardCorpse(corpse, lookingRight);
            }
        }
    }
}
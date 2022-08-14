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
                princeCorpseLookingLeft.sprite = corpse;
                // By default corpses sprites look to right.
                princeCorpseLookingLeft.flipY = true;
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
                guardCorpseLookingLeft.sprite = corpse;
                // By default corpses sprites look to right.
                guardCorpseLookingLeft.flipY = true;
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
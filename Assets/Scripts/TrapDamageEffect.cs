using System.Collections;
using UnityEngine;

namespace Prince
{
    public class TrapDamageEffect : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to show damage sprite when character comes from left.")]
        [SerializeField] private SpriteRenderer rightDamageSpriteRenderer;
        [Tooltip("Needed to show damage sprite when character comes from right.")]
        [SerializeField] private SpriteRenderer leftDamageSpriteRenderer;
        [Tooltip("Needed to play sounds.")]
        [SerializeField] private SoundController soundController;
        
        [Header("CONFIGURATION:")] 
        [Tooltip("Background flash to show when damage is received by a character.")]
        [SerializeField] private CameraController.Flash damageFlash;
        [Tooltip("Sprite to show when damage is received.")]
        [SerializeField] private Sprite damageSprite;
        [Tooltip("Time to wait before showing damage icon.")]
        [SerializeField] private float damageDelay;
        [Tooltip("Time to show damage sprite.")]
        [SerializeField] private float damageTime;

        private CameraController _currentCameraController;
        private Color _currentCameraBackgroundColor;

        public enum DamageEffectType
        {
            CharacterCameFromLeft,
            CharacterCameFromRight
        }
        private void Awake()
        {
            rightDamageSpriteRenderer.sprite = damageSprite;
            leftDamageSpriteRenderer.sprite = damageSprite;
            HideDamage();
        }
        
        // Start is called before the first frame update
        void Start()
        {
            _currentCameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
        }
        
        /// <summary>
        /// Show damage sprite.
        /// </summary>
        private void ShowDamage(DamageEffectType effectType)
        {
            switch (effectType)
            {
                case DamageEffectType.CharacterCameFromLeft:
                    rightDamageSpriteRenderer.enabled = true;
                    break;
                case DamageEffectType.CharacterCameFromRight:
                    leftDamageSpriteRenderer.enabled = true;
                    break;
            }
        }
        
        /// <summary>
        /// Hide damage sprite.
        /// </summary>
        private void HideDamage()
        {
            rightDamageSpriteRenderer.enabled = false;
            leftDamageSpriteRenderer.enabled = false;
        }
        
        /// <summary>
        ///Make a background flash with defined color.
        /// </summary>
        private void ShowDamageFlash()
        {
            if (_currentCameraController != null)
            {
                _currentCameraController.ShowFlashes(new[] { damageFlash });
            }
        }
        
        /// <summary>
        /// Show damage produced by a trap.
        /// </summary>
        /// </param>
        public void ShowTrapHit(DamageEffectType effectType, bool isPrince = true)
        {
            StartCoroutine(ShowHitAsync(effectType, "chaft", isPrince));
        }
        
        private IEnumerator ShowHitAsync(DamageEffectType effectType, string soundToPlay = null, bool isPrince = true)
        {
            if (isPrince) ShowDamageFlash();
            yield return new WaitForSeconds(damageDelay);
            ShowDamage(effectType);
            if (soundToPlay != null) soundController.PlaySound(soundToPlay);
            yield return new WaitForSeconds(damageTime);
            HideDamage();
        }
        
        
    }
}
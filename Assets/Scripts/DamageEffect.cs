using System;
using System.Collections;
using System.Collections.Generic;
using Prince;
using UnityEngine;


/// <summary>
/// This component manages visual effects when a character is damaged.
/// </summary>
public class DamageEffect : MonoBehaviour
{
    [Header("WIRING:")]
    [Tooltip("Needed to show up damage sprite.")]
    [SerializeField] private SpriteRenderer upDamageSprite;
    [Tooltip("Needed to show down damage sprite.")]
    [SerializeField] private SpriteRenderer downDamageSprite;
    [Tooltip("Needed to play sounds.")]
    [SerializeField] private SoundController soundController;

    [Header("CONFIGURATION:")] 
    [Tooltip("Background flash to show when damage is received as Prince.")]
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
        UpDamage,
        DownDamage
    }
    
    private void Awake()
    {
        upDamageSprite.sprite = damageSprite;
        downDamageSprite.sprite = damageSprite;
        HideDamage();
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentCameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
    }

    /// <summary>
    /// Show damage sprite depending on whether received damage is up or down.
    /// </summary>
    /// <param name="effectType">Received damage type</param>
    private void ShowDamage(DamageEffectType effectType)
    {
        switch (effectType)
        {
            case DamageEffectType.UpDamage:
                upDamageSprite.enabled = true;
                break;
            case DamageEffectType.DownDamage:
                downDamageSprite.enabled = true;
                break;
        }
    }

    /// <summary>
    /// Hide every damage sprite.
    /// </summary>
    private void HideDamage()
    {
        upDamageSprite.enabled = false;
        downDamageSprite.enabled = false;
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
    /// Show damage produced by a sword hit.
    /// </summary>
    /// <param name="hitOverGuard">If true then we are hitting over a guard. Guards do not show
    /// screen red tint when hit.
    /// </param>
    public void ShowSwordHit(bool hitOverGuard)
    {
        StartCoroutine(ShowSwordHitAsync(hitOverGuard));
    }

    private IEnumerator ShowSwordHitAsync(bool hitOverGuard)
    {
        if (!hitOverGuard) ShowDamageFlash();
        yield return new WaitForSeconds(damageDelay);
        ShowDamage(DamageEffectType.UpDamage);
        soundController.PlaySound("hit_by_sword");
        yield return new WaitForSeconds(damageTime);
        HideDamage();
    }
    
    /// <summary>
    /// Show damage produced falling landing.
    /// </summary>
    /// <param name="hitOverGuard">If true then we are hitting over a guard. Guards do not show
    /// screen red tint when hit.
    /// </param>
    public void ShowLandingHit(bool hitOverGuard)
    {
        StartCoroutine(ShowLandingHitAsync(hitOverGuard));
    }

    private IEnumerator ShowLandingHitAsync(bool hitOverGuard)
    {
        if (!hitOverGuard) ShowDamageFlash();
        yield return new WaitForSeconds(damageDelay);
        ShowDamage(DamageEffectType.DownDamage);
        // In this case sound effects are called from animations, because those effects are
        // different depending on animation.
        // soundController.PlaySound("hit_by_sword");
        yield return new WaitForSeconds(damageTime);
        HideDamage();
    }
}

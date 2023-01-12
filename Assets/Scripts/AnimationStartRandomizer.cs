using UnityEngine;

/// <summary>
/// When you have many prefabs together with the same idle animation playing it is
/// odd if they appear in sync. So this component randomize idle animation start
/// to avoid that effect.
/// </summary>
public class AnimationStartRandomizer : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        RandomizedStartPlay(CurrentAnimationName());
    }

    /// <summary>
    /// Start animation from a random frame.
    ///
    /// This way you can avoid that multiple instances of the same animation stay in sync (e.g. flames animations).
    /// </summary>
    /// <param name="animationName">Animation to play at a randomized start.</param>
    private void RandomizedStartPlay(string animationName)
    {
        float startPosition = Random.Range(0.0f, 1.0f);
        animator.Play(animationName, 0, startPosition);
    }

    /// <summary>
    /// Get current animation name.
    /// </summary>
    /// <returns>Current animation name.</returns>
    private string CurrentAnimationName()
    {
        var currAnimName = "";
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
            if (animator.GetCurrentAnimatorStateInfo (0).IsName (clip.name)) {
                currAnimName = clip.name.ToString();
            }
        }
        return currAnimName;
    }
    
}

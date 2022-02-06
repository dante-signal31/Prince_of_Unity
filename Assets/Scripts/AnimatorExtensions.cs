using System.Collections;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// <p>By default animator triggers get enqueued if they are fired while in a state that cannot consume it. That
    /// is not the behaviour I want. I'd like trigger to vanish after a frame independently they are consumed or not.</p>
    ///
    /// <p>To do that, a method extension over Animator is needed to implement an special trigger call.</p>
    ///
    /// <p>Solution got from: https://forum.unity.com/threads/mecanim-trigger-stays-down-queued.314742/</p>
    /// </summary>
    public static class AnimatorExtensions
    {
        /// <summary>
        /// Set trigger just for one frame and reset it afterwards.
        /// </summary>
        /// <param name="anim">Animator our state machine is running on.</param>
        /// <param name="coroutineRunner">MonoBehavior that is calling this method.</param>
        /// <param name="trigger">Name of trigger we want to call.</param>
        public static void SetTriggerOneFrame(this Animator anim, string trigger, MonoBehaviour coroutineRunner) {
            coroutineRunner.StartCoroutine(TriggerOneFrame(anim, trigger));
        }
 
        private static IEnumerator TriggerOneFrame(Animator anim, string trigger) {
            anim.SetTrigger(trigger);
            yield return null;
            if (anim != null) {
                anim.ResetTrigger(trigger);
            }
        }
    }
}
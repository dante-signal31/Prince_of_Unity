using System.Collections;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Pickable component to interact with Prince character.
    ///
    /// This component should not be used directly but subclassed instead.
    /// </summary>
    public abstract class PickableCharacterInteractions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to signal when this pickable is being taken and from where.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to show flashes when this pickable is taken.")]
        [SerializeField] private PickableFlash flashController;
        [Tooltip("Needed to know when picking animation is being played.")]
        [SerializeField] private PickableStatus pickableStatus;

        /// <summary>
        /// Whether this pickable is still being playing it picking animation.
        /// </summary>
        protected bool PickingAnimationEnded => pickableStatus.CurrentState == PickableStatus.States.Taken;
        
        public void Take(PickableInteractions taker)
        {
            StartCoroutine(TakePickable(taker));
        }

        /// <summary>
        /// Whether Prince taker is at right from pickable.
        /// </summary>
        /// <param name="taker">PickableInteractions Prince component.</param>
        /// <returns>True if taker is at right from pickable.</returns>
        private bool BeingTakenFromRight(PickableInteractions taker)
        {
            return (taker.transform.position.x - transform.position.x) >= 0;
        }

        private IEnumerator TakePickable(PickableInteractions taker)
        {
            taker.PickingAnimationStarted();
            // Wait until character has disabled its sprite to avoid a frame with two prince sprites, the one from
            // character and the one from pickable. WaitUntil character is in TakingPickable state or skipping a frame
            // does not seem to work, so I have to wait time between frames at 8 fps.
            yield return new WaitForSeconds(0.125f);
            stateMachine.SetBool("PrinceAtRight", BeingTakenFromRight(taker));
            stateMachine.SetTrigger("Taken");
            flashController.ShowFlashes();
            DoSomethingOverTaker(taker);
            yield return new WaitUntil(() => PickingAnimationEnded);
            taker.PickingAnimationEnded();
            stateMachine.SetTrigger("Disable");
        }

        /// <summary>
        /// Apply this pickable effect over taker.
        /// </summary>
        /// <param name="taker">PickableInteractions Prince component.</param>
        protected abstract void DoSomethingOverTaker(PickableInteractions taker);
    }
}
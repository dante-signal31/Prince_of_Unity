using UnityEngine;

namespace Tests.PlayTests.Scripts
{
    public class CircleEventReceiver2 : CircleEventReceiver
    {
        /// <summary>
        /// Callback for circle activated event.
        /// </summary>
        /// <param name="sender">Event emitter.</param>
        /// <param name="color">Color chosen by emitter.</param>
        public override void OnCircleActivated(object sender, CircleEventEmitter.CircleEventArgs args)
        {
            SetColor(args.ActivationValue);
            Debug.Log("I'm receiver2, and I've been activated by circle.");
        }
        
        /// <summary>
        /// Callback for box activated event.
        ///
        /// Set random value equal to the one at emitter.
        /// </summary>
        /// <param name="sender">Event emitter.</param>
        /// <param name="args">Color chosen by emitter.</param>
        public override void OnBoxActivated(object sender, BoxEventEmitter.BoxEventArgs args)
        {
            SetRandomValue(args.Value);
            Debug.Log("I'm receiver2, and I've been activated by box.");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
        public class AnimationEventsForwarder : MonoBehaviour
        {
                [Header("WIRING:")]
                [Tooltip("Needed to signal fighting events to stay in synch with animations.")]
                [SerializeField]
                private FightingInteractions fightingInteractions;

///////// My strike can be blocked.
                public void StrikeStart()
                {

                }

                public void BlockingChanceEnded()
                {

                }

                public void StrikeHit()
                {

                }

//////// I block my enemy attack and now I have chance to counter attack.
                public void BlockSwordStarted()
                {

                }

                public void CounterAttackChanceEnded()
                {

                }

//////// My attack has been blocked but my enemy counterattacks, now I have an small chance to block him.        
                public void BlockedSwordStarted()
                {
                }

                public void CounterBlockSwordChanceEnded()
                {
                }
        }
}

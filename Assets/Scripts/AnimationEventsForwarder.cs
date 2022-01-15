using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
    public class AnimationEventsForwarder : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to signal fighting events to stay in synch with animations.")]
        [SerializeField] private FightingInteractions fightingInteractions;
        
        public void StrikeStart()
        {
        
        }

        public void BlockingChanceEnded()
        {
        
        }

        public void StrikeHit()
        {
        
        }
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}

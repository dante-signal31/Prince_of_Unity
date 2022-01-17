using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Prince
{
    public class FightingInteractions : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to contact with enemy when in hitting range and get its respective FightingInteractions")]
        [SerializeField] private FightingSensors fightingSensors;
        [Tooltip("Needed to play sound depending on combat conditions.")]
        [SerializeField] private SoundController soundEmitter;
        
        private FightingInteractions _currentEnemyInteractions;
        
        /// <summary>
        /// Get FightingInteractions component from enemy currently detected by fighting sensors.
        /// </summary>
        /// <returns>FightingInteractions component from enemy currently detected by fighting sensors.</returns>
        private FightingInteractions GetCurrentEnemyInteractions()
        {
            GameObject enemyGameObject = fightingSensors.CurrentEnemy;
            return (enemyGameObject == null)? null: enemyGameObject.GetComponentInChildren<FightingInteractions>();
        }

        private void Awake()
        {
            _currentEnemyInteractions = GetCurrentEnemyInteractions();
        }

        private void FixedUpdate()
        {
            _currentEnemyInteractions = GetCurrentEnemyInteractions();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        /// <summary>
        /// Signal enemy we are starting an attack to give him a chance to block it.
        /// </summary>
        public void StrikeStart()
        {

        }

        /// <summary>
        /// Method used by enemies to signal us they're starting an attack.
        /// </summary>
        public void NoticeStrikeStart()
        {
            
        }
        
        /// <summary>
        /// Enemy chance to block our attack has ended.
        /// </summary>
        public void BlockingChanceEnded()
        {
            if (_currentEnemyInteractions == null)
            {
                soundEmitter.PlaySound("missed_attack");
            }
        }

        /// <summary>
        /// Signal enemy we are hitting him.
        /// </summary>
        public void StrikeHit()
        {

        }

        /// <summary>
        /// Method used by enemies to signal us they're hitting us.
        /// </summary>
        public void NoticeStrikeHit()
        {
            
        }
        
        /// <summary>
        /// Signal enemy we're blocking his attack. Now we have an small chance to counter attack. 
        /// </summary>
        public void BlockSwordStarted()
        {

        }

        /// <summary>
        /// Method used by enemies to signal us they've blocked our attack.
        /// </summary>
        public void NoticeBlockSwordStarted()
        {
            
        }

        /// <summary>
        /// Our chance to counter attack after blocking an enemy attack has ended.
        /// </summary>
        public void CounterAttackChanceEnded()
        {

        }
        
        /// <summary>
        /// Our attack was blocked, and enemy may be going to strike us back. We still
        /// have an small chance to block that attack.
        /// </summary>
        public void BlockedSwordStarted()
        {
        }

        /// <summary>
        /// Our chance to block our enemy attack has ended.
        /// </summary>
        public void CounterBlockSwordChanceEnded()
        {
        }
    }
}

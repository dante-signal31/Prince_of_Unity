using UnityEngine;


namespace Prince
{
    /// <summary>
    /// This is the main interface to communicate with current enemy while sword fighting.
    ///
    /// Through this component we can know when we are being attacked and if we still have a blocking chance.
    /// On the other hand this component is the way to signal our enemy that we are starting an attack against him.
    /// </summary>
    public class FightingInteractions : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to contact with enemy when in hitting range and get its respective FightingInteractions")]
        [SerializeField] private FightingSensors fightingSensors;
        [Tooltip("Needed to signal our state machine when our strikes are blocked.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to play sound depending on combat conditions.")]
        [SerializeField] private SoundController soundEmitter;
        [Tooltip("Needed to show damage effects when hit.")]
        [SerializeField] private DamageEffect damageEffects;
        [Tooltip("Needed to update health when hit.")]
        [SerializeField] private HealthController healthController;
        
        [Header("CONFIGURATION:")]
        [Tooltip("This component is in a guard?")]
        [SerializeField] private bool iAmGuard;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        /// <summary>
        /// Used by attacker to track he has an attack on course.
        /// </summary>
        public bool StrikeBlockable { get; private set; }
        
        /// <summary>
        /// Used by defender to track an attack is on its way and he has a chance to block it.
        /// </summary>
        public bool BlockingStrikePossible { get; private set; } 
        
        public bool CounterAttackPossible { get; private set; }
        

        private FightingInteractions _currentEnemyInteractions;
        private bool _strikeMissed = false;
        private float _elapsedBlockingTime;
        
        
        
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
            _elapsedBlockingTime += Time.fixedDeltaTime;
            
            // This is for an edge case. If you start an attack just before your enemy enters hitting range then
            // your enemy won't receive the StrikeStart() signal. So by the time your strike gets to hitting point,
            // your enemy will be already in hitting range but he won't have had any notice of your strike nor
            // any chance to block it.
            //
            // To fix that, at once both character are at hitting range we check that StrikeBlockable variable from attacker
            // and BlockingStrikePossible are synced.
            if (_currentEnemyInteractions != null)
            {
                if (StrikeBlockable && !_currentEnemyInteractions.BlockingStrikePossible) _currentEnemyInteractions.NoticeStrikeStart();
            }
        }

        /// <summary>
        /// Signal enemy we are starting an attack to give him a chance to block it.
        /// </summary>
        public void StrikeStart()
        {
            if (_currentEnemyInteractions != null)
            {
                this.Log($"(FightingInteractions - {transform.root.name}) Noticing {_currentEnemyInteractions.transform.root.name} that I'm going to attack him.", showLogs);
                _currentEnemyInteractions.NoticeStrikeStart();
                this.Log($"(FightingInteractions - {transform.root.name}) Starting attack against {_currentEnemyInteractions.transform.root.name}", showLogs);
            }
            _strikeMissed = false;
            StrikeBlockable = true;
            _elapsedBlockingTime = 0;
        }

        /// <summary>
        /// Method used by enemies to signal us they're starting an attack.
        ///
        /// Since this moment, enemy has an small chance to block our attack until 
        /// </summary>
        public void NoticeStrikeStart()
        {
            this.Log($"(FightingInteractions - {transform.root.name}) Entering NoticeStrikeStart.", showLogs);
            if (_currentEnemyInteractions != null)
            {
                this.Log($"(FightingInteractions - {transform.root.name}) {_currentEnemyInteractions.transform.root.name} notice us he is going to attack us.", showLogs);
            }
            else
            {
                this.Log($"(FightingInteractions - {transform.root.name}) We have been noticed we are going to be attacked", showLogs);
            }
            BlockingStrikePossible = true;
        }
        
        /// <summary>
        /// Enemy chance to block our attack has ended.
        /// </summary>
        public void BlockingChanceEnded()
        {
            if (_currentEnemyInteractions == null)
            {
                this.Log($"(FightingInteractions - {transform.root.name}) No enemy at range, so this strike is going to hit no one.", showLogs);
                soundEmitter.PlaySound("missed_attack");
                _strikeMissed = true;
            }
            else
            {
                this.Log($"(FightingInteractions - {transform.root.name}) Blocking chance ended. Blocking window was of {_elapsedBlockingTime} seconds.", showLogs);
                _currentEnemyInteractions.NoticeBlockingChanceEnded();
            }
            StrikeBlockable = false;
        }

        /// <summary>
        /// Method used by enemies to signal us our chance to block their attack has ended..
        /// </summary>
        public void NoticeBlockingChanceEnded()
        {
            if (_currentEnemyInteractions != null)
            {
                this.Log(
                    $"(FightingInteractions - {transform.root.name}) {_currentEnemyInteractions.transform.root.name} notices us our chance to block his attack has ended.", showLogs);
            }
            else
            {
                this.Log(
                    $"(FightingInteractions - {transform.root.name}) We have been noticed our chance to block his attack has ended.", showLogs);
            }
            
            BlockingStrikePossible = false;
        }

        /// <summary>
        /// Signal enemy we are hitting him.
        /// </summary>
        public void StrikeHit()
        {
            if (_strikeMissed) return;
            if (_currentEnemyInteractions != null)
            {
                this.Log($"(FightingInteractions - {gameObject.transform.parent.name}) Hitting to {_currentEnemyInteractions.transform.parent.name} ", showLogs);
                _currentEnemyInteractions.StrikeReceived();
            }
        }

        /// <summary>
        /// Used over an enemy to signal him he has been hit.
        /// </summary>
        public void StrikeReceived()
        {
            damageEffects.ShowSwordHit(iAmGuard);
            healthController.SwordHit();
        }
        
        /// <summary>
        /// Signal enemy we're blocking his attack. Now we have an small chance to counter attack. 
        /// </summary>
        public void BlockSwordStarted()
        {
            if (_currentEnemyInteractions != null)
            {
                this.Log($"(FightingInteractions - {transform.root.name}) Noticing {_currentEnemyInteractions.transform.root.name} we are going to block his attack.", showLogs);
                _currentEnemyInteractions.NoticeBlockSwordStarted();
            }
            CounterAttackPossible = true;
        }

        /// <summary>
        /// Method used by enemies to signal us they've blocked our attack.
        /// </summary>
        public void NoticeBlockSwordStarted()
        {
            if (_currentEnemyInteractions != null) this.Log($"(FightingInteractions - {transform.root.name}) {_currentEnemyInteractions.transform.root.name} notice us he is going to block our attack.", showLogs);
            stateMachine.SetTrigger("Blocked");
        }

        /// <summary>
        /// Our chance to counter attack after blocking an enemy attack has ended.
        /// </summary>
        public void CounterAttackChanceEnded()
        {
            if (_currentEnemyInteractions != null) this.Log($"(FightingInteractions - {transform.root.name}) {_currentEnemyInteractions.transform.root.name} notices us our chance to counter attack has ended.", showLogs);
            CounterAttackPossible = false;
        }
        
        /// <summary>
        /// Our attack was blocked, and enemy may be going to strike us back. We still
        /// have an small chance to block that attack.
        /// </summary>
        public void BlockedSwordStarted()
        {
            this.Log($"(FightingInteractions - {transform.root.name}) Our attack was blocked.", showLogs);
            BlockingStrikePossible = true;
        }

        /// <summary>
        /// Our chance to block our enemy attack has ended.
        /// </summary>
        public void CounterBlockSwordChanceEnded()
        {
            this.Log($"(FightingInteractions - {transform.root.name}) Our chance to counter block has ended.", showLogs);
            BlockingStrikePossible = false;
        }
    }
}

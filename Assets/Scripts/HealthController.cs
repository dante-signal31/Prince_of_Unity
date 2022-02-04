using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component centralizes every health change for character and signals every state
    /// change that depends on that life.
    /// </summary>
    public class HealthController : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to get and update current life.")]
        [SerializeField] private CharacterStatus characterStatus;
        [Tooltip("Needed to signal state machine state changes related to health conditions.")]
        [SerializeField] private Animator stateMachine;

        /// <summary>
        /// Current character life.
        ///
        /// Actually, just a forwarder to CharacterStatus life property.
        /// </summary>
        private int Life
        {
            get => characterStatus.Life;
            set => characterStatus.Life = value;
        }

        /// <summary>
        /// Current character maximum life.
        ///
        /// Actually, just a forwarder to CharacterStatus maximum life property.
        /// </summary>
        private int MaximumLife
        {
            get => characterStatus.MaximumLife;
            set => characterStatus.MaximumLife = value;
        }

        /// <summary>
        /// Character has been hit by an enemy's sword.
        /// </summary>
        public void SwordHit()
        {
            switch (characterStatus.CurrentState)
            {
                // If character has sword unsheathed then only loses a life.
                case CharacterStatus.States.IdleSword:
                case CharacterStatus.States.AdvanceSword:
                case CharacterStatus.States.AttackWithSword:
                case CharacterStatus.States.BlockedSword:
                case CharacterStatus.States.CounterBlockSword:
                case CharacterStatus.States.BlockSword:
                case CharacterStatus.States.Retreat:
                    Life--;
                    break;
                // In every other case character is directly killed.
                default:
                    Life = 0;
                    break;
            }
            // Life--;
            if (characterStatus.IsDead)
            {
                Debug.Log($"(HealthController - {gameObject.transform.parent.name}) Dead by sword.");
                stateMachine.SetBool("isDead", true);
                stateMachine.SetTrigger("Hit");
            } 
            else
            {
                Debug.Log($"(HealthController - {gameObject.transform.parent.name}) Hit by sword. New current life: {Life}");
                stateMachine.SetTrigger("Hit");
            }
        }
    }
}
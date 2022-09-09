using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to follow Prince status across game.
    ///
    /// While in scene you can ask to Prince character for his data, but that game object is destroyed and recreated
    /// each time level changes. So a game management component is needed to keep his data across levels.
    /// </summary>
    public class PrinceStatus : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to get player starting configuration.")]
        [SerializeField] private GameConfiguration gameConfiguration;
        [Tooltip("Needed to register listeners for Prince events.")]
        [SerializeField] private EventBus eventBus;

        /// <summary>
        /// Player current maximum life.
        /// </summary>
        public int CurrentPlayerMaximumLife { get; private set; }
        
        /// <summary>
        /// Player current life.
        /// </summary>
        public int CurrentPlayerLife { get; private set; }
        
        /// <summary>
        /// Whether player has a sword or not.
        /// </summary>
        public bool HasSword { get; private set; }
        
        private void Awake()
        {
            GetStartingConfiguration();
        }

        private void Start()
        {
            eventBus.AddListener<GameEvents.SwordTaken>(OnSwordTaken);
            eventBus.AddListener<GameEvents.CharacterLifeUpdated>(OnLifeUpdated);
        }

        private void GetStartingConfiguration()
        {
            CurrentPlayerMaximumLife = gameConfiguration.PlayerMaximumStartingLife;
            CurrentPlayerLife = gameConfiguration.PlayerStartingLife;
            HasSword = gameConfiguration.PlayerStartsWithSword;
        }

        private void OnSwordTaken(object _, GameEvents.SwordTaken __)
        {
            HasSword = true;
        }

        /// <summary>
        /// Listener for listener update events.
        /// </summary>
        /// <param name="sender">Sender game object. Usually a character monobehaviour</param>
        /// <param name="ev">Event data.</param>
        private void OnLifeUpdated(object sender, GameEvents.CharacterLifeUpdated ev)
        {
            // We only keep track of Prince across levels.
            CharacterStatus senderStatus = ((MonoBehaviour) sender).transform.root.gameObject.GetComponentInChildren<CharacterStatus>();
            if (senderStatus != null && senderStatus.IsPrince)
            {
                CurrentPlayerMaximumLife = ev.MaximumLife;
                CurrentPlayerLife = Mathf.Clamp(ev.CurrentLife, 0, CurrentPlayerMaximumLife);
            }
        }
        
    }
}
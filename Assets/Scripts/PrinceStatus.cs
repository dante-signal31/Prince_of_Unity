using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// GameManagers component to follow Prince status across game.
    ///
    /// While in scene you can ask to Prince character for his data, but that game object is destroyed and recreated
    /// each time level changes. So a game management component is needed to keep his data across levels.
    /// </summary>
    public class PrinceStatus : MonoBehaviour
    {
        
        public struct Stats
        {
            public int MaximumLife;
            public int CurrentLife;
            public float ElapsedSeconds;
            public bool HasSword;
        }
        
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
        /// <p>Roof for maximum life.</p>
        ///
        /// <p>i.e the maximum life units that can be shown on HUD.</p>
        /// </summary>
        public int MaximumLifeRoof { get; private set; }
        
        /// <summary>
        /// Whether player has a sword or not.
        /// </summary>
        public bool HasSword { get; private set; }

        private Stats _levelStartStats = new Stats();
        
        /// <summary>
        /// Prince stats when he started current level.
        /// </summary>
        public Stats LevelStartsStats => _levelStartStats;
        
        private void Awake()
        {
            GetStartingConfiguration();
        }

        private void Start()
        {
            eventBus.AddListener<GameEvents.SwordTaken>(OnSwordTaken);
            eventBus.AddListener<GameEvents.SwordLost>(OnSwordLost);
            eventBus.AddListener<GameEvents.CharacterLifeUpdated>(OnLifeUpdated);
            eventBus.AddListener<GameEvents.LevelLoaded>(OnLevelLoaded);
            eventBus.AddListener<GameEvents.LevelReloaded>(OnLevelReloaded);
        }

        // private void OnEnable()
        // {
        //     
        // }

        private void OnDisable()
        {
            eventBus.RemoveListener<GameEvents.SwordTaken>(OnSwordTaken);
            eventBus.RemoveListener<GameEvents.SwordLost>(OnSwordLost);
            eventBus.RemoveListener<GameEvents.CharacterLifeUpdated>(OnLifeUpdated);
            eventBus.RemoveListener<GameEvents.LevelLoaded>(OnLevelLoaded);
            eventBus.RemoveListener<GameEvents.LevelReloaded>(OnLevelReloaded);
        }

        private void GetStartingConfiguration()
        {
            CurrentPlayerMaximumLife = gameConfiguration.PlayerMaximumStartingLife;
            CurrentPlayerLife = gameConfiguration.PlayerStartingLife;
            HasSword = gameConfiguration.PlayerStartsWithSword;
            MaximumLifeRoof = gameConfiguration.PlayerMaximumLifeRoof;
        }

        private void OnSwordTaken(object _, GameEvents.SwordTaken __)
        {
            HasSword = true;
        }

        private void OnSwordLost(object _, GameEvents.SwordLost __)
        {
            HasSword = false;
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

        /// <summary>
        /// Listener for LevelLoaded events.
        /// </summary>
        /// <param name="_">Sender game object. Usually a LevelLoader monobehaviour.</param>
        /// <param name="__">Event data.</param>
        private void OnLevelLoaded(object _, GameEvents.LevelLoaded __)
        {
            bool shouldSavePrinceStats = GameObject.Find("LevelSpecifics").GetComponentInChildren<LevelConfiguration>()
                .SavePrinceStatusWhenLevelLoaded;
            if (shouldSavePrinceStats)
            {
                _levelStartStats.MaximumLife = CurrentPlayerMaximumLife;
                _levelStartStats.CurrentLife = CurrentPlayerLife;
                _levelStartStats.HasSword = HasSword;
                GameTimer timer = GameObject.Find("GameManagers").GetComponentInChildren<GameTimer>();
                _levelStartStats.ElapsedSeconds = timer.ElapsedSeconds;
            }
        }

        /// <summary>
        /// Listener for LevelReloaded events.
        /// </summary>
        /// <param name="_">Sender game object. Usually a LevelLoader monobehaviour.</param>
        /// <param name="__">Event data.</param>
        private void OnLevelReloaded(object _, GameEvents.LevelReloaded __)
        {
            CurrentPlayerMaximumLife = _levelStartStats.MaximumLife;
            CurrentPlayerLife = _levelStartStats.CurrentLife;
            HasSword = _levelStartStats.HasSword;
        }
    }
}
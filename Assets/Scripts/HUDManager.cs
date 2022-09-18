using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

namespace Prince
{
    /// <summary>
    /// Component to display Prince life, message text and Enemy life in HUD bar.
    ///
    /// Be aware that this HUD is only ready to show just one enemy life at a time, so at level design you must check
    /// only an enemy can attack Prince at the same time.
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to change HUD graphics.")]
        [SerializeField] private UIDocument hud;
        [Tooltip("Needed to add listeners")]
        [SerializeField] private EventBus eventBus;
        [Tooltip("Needed to know if an enemy is present at current screen.")] 
        [SerializeField] private CameraController cameraController;

        [Header("CONFIGURATION:")] 
        [Tooltip("Sprite to show as Prince life point.")]
        [SerializeField] private Sprite princeLifePoint;
        [Tooltip("Sprite to show for empty prince life points.")]
        [SerializeField] private Sprite princeHollowLifePoint;
        [Tooltip("Sprite to show for enemy life points.")]
        [SerializeField] private Sprite enemyLifePoint;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;

        /// <summary>
        /// Current message displayed on bar.
        /// </summary>
        public string CurrentMessage => _textBar.text;

        private VisualElement _rootVisualElement;
        private VisualElement[] _princeLifes;
        private VisualElement[] _enemyLifes;
        private Label _textBar;
        private PrinceStatus _princePersistentStatus;

        private EventBus _eventBus;

        private void Awake()
        {
            _princePersistentStatus = GameObject.Find("GameManagers").GetComponentInChildren<PrinceStatus>();
            _eventBus = GameObject.Find("GameManagers").GetComponentInChildren<EventBus>();
        }

        private void OnEnable()
        {
            _rootVisualElement = hud.rootVisualElement;
            _textBar = _rootVisualElement.Q<Label>("TextMessage");
            _princeLifes = GetLifeElements("PrinceBar");
            _enemyLifes = GetLifeElements("EnemyBar");
        }

        private void OnDisable()
        {
            eventBus.RemoveListener<GameEvents.CharacterLifeUpdated>(OnCharacterLifeUpdated);
            eventBus.RemoveListener<GameEvents.GuardEnteredTheRoom>(OnGuardEnteredTheRoom);
            eventBus.RemoveListener<GameEvents.NoGuardInTheRoom>(OnNoGuardInTheRoom);
            eventBus.RemoveListener<GameEvents.PrinceEnteredNewRoom>(OnPrinceEnteredNewRoom);
            // eventBus.RemoveListener<GameEvents.VideoPlayStart>(OnVideoPlayStart);
            // eventBus.RemoveListener<GameEvents.PrinceInTheScene>(OnPrinceInTheScene);
        }

        private void Start()
        {
            eventBus.AddListener<GameEvents.CharacterLifeUpdated>(OnCharacterLifeUpdated);
            eventBus.AddListener<GameEvents.GuardEnteredTheRoom>(OnGuardEnteredTheRoom);
            eventBus.AddListener<GameEvents.NoGuardInTheRoom>(OnNoGuardInTheRoom);
            eventBus.AddListener<GameEvents.PrinceEnteredNewRoom>(OnPrinceEnteredNewRoom);
            // eventBus.AddListener<GameEvents.VideoPlayStart>(OnVideoPlayStart);
            // eventBus.AddListener<GameEvents.PrinceInTheScene>(OnPrinceInTheScene);
            eventBus.AddListener<GameEvents.LevelLoaded>(OnLevelLoaded);
            SetPrinceLife(_princePersistentStatus.CurrentPlayerLife, _princePersistentStatus.CurrentPlayerMaximumLife);
            ShowHudIfLevelsAskIt();
        }
        
        

        /// <summary>
        /// Get life point placeholders array.
        ///
        /// They are ordered alphabetically by their name.
        /// </summary>
        /// <param name="rootLifeName">Root element por life point placeholders.</param>
        /// <returns>Array of life point placeholders.</returns>
        private VisualElement[] GetLifeElements(string rootLifeName)
        {
            VisualElement rootLife = _rootVisualElement.Q<VisualElement>(rootLifeName);
            return rootLife.Children().OrderBy(element => element.name).ToArray();
        }

        /// <summary>
        /// Hide hud.
        ///
        /// Useful for video scenes.
        /// </summary>
        public void HideHud()
        {
            _rootVisualElement.visible = false;
        }

        /// <summary>
        /// Listener on LevelLoaded events.
        /// </summary>
        /// <param name="_">Actually not used here.</param>
        /// <param name="__">Actually not used here.</param>
        private void OnLevelLoaded(object _, GameEvents.LevelLoaded __)
        {
            ShowHudIfLevelsAskIt();
        }

        private void ShowHudIfLevelsAskIt()
        {
            LevelConfiguration currentLevelConfiguration =
                GameObject.Find("LevelSpecifics").GetComponentInChildren<LevelConfiguration>();
            if (currentLevelConfiguration.ShowHudBarAtStart)
            {
                ShowHud();
            }
            else
            {
                HideHud();
            }
        }

        // /// <summary>
        // /// Listener for VideoPlayStart events.
        // /// </summary>
        // /// <param name="_">Actually not used here.</param>
        // private void OnVideoPlayStart(object _, GameEvents.VideoPlayStart __)
        // {
        //     HideHud();
        // }

        /// <summary>
        /// Show hud.
        ///
        /// Nedd to be used after video scenes.
        /// </summary>
        public void ShowHud()
        {
            _rootVisualElement.visible = true;
        }

        // /// <summary>
        // /// Listener for PrinceInTheScene events.
        // /// </summary>
        // /// <param name="_">Actually not used here.</param>
        // private void OnPrinceInTheScene(object _, GameEvents.PrinceInTheScene __)
        // {
        //     ShowHud();
        // }
        
        
        /// <summary>
        /// Set text bar message.
        /// </summary>
        /// <param name="text">Message to display at text bar.</param>
        public void SetMessage(string text)
        {
            _textBar.text = text;
        }

        /// <summary>
        /// Remove any text from message bar.
        /// </summary>
        public void ClearMessage()
        {
            SetMessage("");
        }

        /// <summary>
        /// Set text bar message just for a time.
        /// </summary>
        /// <param name="text">Message to display at text bar.</param>
        /// <param name="timeInSeconds">Time to show message.</param>
        public void SetMessageForATime(string text, float timeInSeconds)
        {
            SetMessage(text);
            Invoke(nameof(ClearMessage), timeInSeconds);
        }

        /// <summary>
        /// Listener for character update events.
        /// </summary>
        /// <param name="sender">Sender of event. Usually a character monobehaviour.</param>
        /// <param name="ev">Event data.</param>
        public void OnCharacterLifeUpdated(object sender, GameEvents.CharacterLifeUpdated ev)
        {
            GameObject senderGameObject = ((MonoBehaviour)sender).transform.root.gameObject;
            CharacterStatus characterStatus =
                senderGameObject.GetComponentInChildren<CharacterStatus>();
            if (characterStatus != null)
            {
                if (characterStatus.IsPrince)
                {
                    SetPrinceLife(ev.CurrentLife, ev.MaximumLife);
                }
                else
                {
                    if (senderGameObject == cameraController.CurrentRoom.EnemyInTheRoom) SetEnemyLife(ev.CurrentLife);
                }
            }
        }

        /// <summary>
        /// Listener to update enemy bar if Prince enters a new room.
        /// </summary>
        /// <param name="sender">Sender of event. Usually a camera changer gate monobehaviour.</param>
        /// <param name="_">Event data. In this case actually empty.</param>
        public void OnPrinceEnteredNewRoom(object sender, GameEvents.PrinceEnteredNewRoom _)
        {
            GameObject enemyInTheRoom = cameraController.CurrentRoom.EnemyInTheRoom;
            if (enemyInTheRoom == null)
            {
                SetEnemyLife(0);
            }
            else
            {
                SetEnemyLife(enemyInTheRoom.GetComponentInChildren<CharacterStatus>().Life);
            } 
        }

        /// <summary>
        /// Listener for guard entered a room events.
        /// </summary>
        /// <param name="sender">Sender of event. Usually a room monobehaviour.</param>
        /// <param name="ev">Event data.</param>
        public void OnGuardEnteredTheRoom(object sender, GameEvents.GuardEnteredTheRoom ev)
        {
            GameObject senderGameObject = ((MonoBehaviour)sender).transform.root.gameObject;
            Room senderRoom = senderGameObject.GetComponentInChildren<Room>();
            if (senderRoom != null && cameraController.CurrentRoom == senderRoom)
            {
                GameObject guardGameObject = ev.Guard;
                SetEnemyLife(guardGameObject.GetComponentInChildren<CharacterStatus>().Life);
            }
        }
        
        /// <summary>
        /// Listener for guard left a room events.
        /// </summary>
        /// <param name="sender">Sender of event. Usually a room.</param>
        /// <param name="ev">Event data.</param>
        public void OnNoGuardInTheRoom(object sender, GameEvents.GuardEnteredTheRoom _)
        {
            GameObject senderGameObject = ((MonoBehaviour)sender).transform.root.gameObject;
            Room senderRoom = senderGameObject.GetComponentInChildren<Room>();
            if (senderRoom != null && cameraController.CurrentRoom == senderRoom)
            {
                SetEnemyLife(0);
            }
        }
        
        

        /// <summary>
        /// Update Prince life bar.
        /// </summary>
        /// <param name="currentLife">Current active life points.</param>
        /// <param name="maximumLife">Current maximum life points.</param>
        private void SetPrinceLife(int currentLife, int maximumLife)
        {
            if (currentLife < 0)
            {
                this.Log($"(HUDManager - {transform.root.name}) Hud not updated because a Prince life under 0 requested.", showLogs);
                return;
            }

            if (maximumLife < currentLife)
            {
                this.Log($"(HUDManager - {transform.root.name}) Hud not updated because a maximum Prince life under current life requested.", showLogs);
                return;
            }

            if (maximumLife < 1)
            {
                this.Log($"(HUDManager - {transform.root.name}) Hud not updated because a maximum Prince life under 1 requested.", showLogs);
                return;
            }

            if (maximumLife > _princeLifes.Length)
            {
                this.Log($"(HUDManager - {transform.root.name}) Hud not updated because a maximum Prince life over hud maximum has been requested.", showLogs);
                return;
            }
            
            // Active life points.
            for (int i = 0; i < currentLife; i++)
            {
                _princeLifes[i].style.backgroundImage = new StyleBackground(princeLifePoint);
            }
            // Empty life points.
            if (maximumLife > currentLife)
            {
                for (int i = currentLife; i < maximumLife; i++)
                {
                    _princeLifes[i].style.backgroundImage = new StyleBackground(princeHollowLifePoint);
                }
            }
            // Remove points above maximum.
            if (maximumLife < _princeLifes.Length)
            {
                for (int i = maximumLife; i < _princeLifes.Length; i++)
                {
                    _princeLifes[i].style.backgroundImage = null;
                }
            }
        }

        /// <summary>
        /// Update enemy life bar.
        /// </summary>
        /// <param name="currentLife">Current enemy life bar.</param>
        private void SetEnemyLife(int currentLife)
        {
            if (currentLife > _enemyLifes.Length)
            {
                this.Log($"(HUDManager - {transform.root.name}) Hud not updated because a maximum guard life over hud maximum has been requested.", showLogs);
                return;
            }
            
            // Active life points.
            for (int i = 0; i < currentLife; i++)
            {
                _enemyLifes[i].style.backgroundImage = new StyleBackground(enemyLifePoint);
            }
            // Remove points above current life.
            if (currentLife < _enemyLifes.Length)
            {
                for (int i = currentLife; i < _enemyLifes.Length; i++)
                {
                    _enemyLifes[i].style.backgroundImage = null;
                }
            }
        }
    }
}
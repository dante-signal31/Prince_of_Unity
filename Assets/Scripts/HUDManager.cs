using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prince
{
    /// <summary>
    /// Component to display Prince life, message text and Enemy life in HUD bar.
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to change HUD graphics.")]
        [SerializeField] private UIDocument hud;
        [Tooltip("Needed to add listeners")]
        [SerializeField] private EventBus eventBus;

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

        private VisualElement _rootVisualElement;
        private VisualElement[] _princeLifes;
        private VisualElement[] _enemyLifes;
        private Label _textBar;
        private PrinceStatus _princePersistentStatus;

        private void Awake()
        {
            _princePersistentStatus = GameObject.Find("GameManagers").GetComponentInChildren<PrinceStatus>();
        }

        private void OnEnable()
        {
            _rootVisualElement = hud.rootVisualElement;
            _textBar = _rootVisualElement.Q<Label>("TextMessage");
            _princeLifes = GetLifeElements("PrinceBar");
            _enemyLifes = GetLifeElements("EnemyBar");
        }

        private void Start()
        {
            eventBus.AddListener<GameEvents.CharacterLifeUpdated>(OnCharacterLifeUpdated);
            SetPrinceLife(_princePersistentStatus.CurrentPlayerLife, _princePersistentStatus.CurrentPlayerMaximumLife);
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
        /// Show hud.
        ///
        /// Nedd to be used after video scenes.
        /// </summary>
        public void ShowHud()
        {
            _rootVisualElement.visible = true;
        }
        
        
        /// <summary>
        /// Set text bar message.
        /// </summary>
        /// <param name="text">Message to display at text bar.</param>
        public void SetMessage(string text)
        {
            _textBar.text = text;
        }

        /// <summary>
        /// Listener for character update events.
        /// </summary>
        /// <param name="sender">Sender of event.</param>
        /// <param name="ev">Event data.</param>
        public void OnCharacterLifeUpdated(object sender, GameEvents.CharacterLifeUpdated ev)
        {
            CharacterStatus characterStatus =
                ((GameObject)sender).transform.root.gameObject.GetComponentInChildren<CharacterStatus>();
            if (characterStatus != null)
            {
                if (characterStatus.IsPrince)
                {
                    SetPrinceLife(ev.CurrentLife, ev.MaximumLife);
                }
                else
                {
                    SetEnemyLife(ev.CurrentLife);
                }
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
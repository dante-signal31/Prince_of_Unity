using UnityEngine;
using UnityEngine.UIElements;

namespace Prince
{
    /// <summary>
    /// Component to manage full screen texts, like the disclaimer one.
    ///
    /// Basically it just counts time to let text to be read and afterwards hides texts and announces at eventBus
    /// that time has expired.
    /// </summary>
    public class TextScreen : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Text UI element.")] 
        [SerializeField] private UIDocument textUI;

        [Header("CONFIGURATION:")]
        [Tooltip("Time in seconds to let the text be read before automatic fadeout.")]
        [SerializeField] private float timeToFadeOut;
        
        private VisualElement _rootVisualElement;
        private Label _textBox;
        private float _elapsedTime;
        private bool _timeToBeReadExpired;
        private bool _alreadyFading;
        private EventBus _eventBus;
        private LevelLoader _levelLoader;

        private void Awake()
        {
            _eventBus = GameObject.Find("GameManagers").GetComponentInChildren<EventBus>();
            _levelLoader = GameObject.Find("GameManagers").GetComponentInChildren<LevelLoader>();
        }

        private void OnEnable()
        {
            _rootVisualElement = textUI.rootVisualElement;
            _textBox = _rootVisualElement.Q<Label>("Text");
        }

        public void LaunchTextScreenTimeoutEvent()
        {
            _eventBus.TriggerEvent(new GameEvents.TextScreenTimeout(_levelLoader.CurrentSceneName), this);
        }

        private void FixedUpdate()
        {
            _elapsedTime += Time.fixedDeltaTime;
            if ((_elapsedTime >= timeToFadeOut) && !_timeToBeReadExpired)
            {
                _timeToBeReadExpired = true;
                // Let ease transition to perform. Keep below value synced with .DisclaimerText transition animation duration.
                Invoke(nameof(LaunchTextScreenTimeoutEvent), 2.0f);
            }
        }

        private void Update()
        {
            if (_timeToBeReadExpired && !_alreadyFading)
            {
                _textBox.AddToClassList("DisclaimerTextFadeOut");
                _alreadyFading = true;
            }
        }
    }
}
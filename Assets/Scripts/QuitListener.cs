using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Prince
{
    /// <summary>
    /// This component listen for user quit requests and ask for confirmation before closing application.
    /// </summary>
    public class QuitListener : MonoBehaviour
    {
        [Header("WIRING")]
        [Tooltip("Needed to subscribe to events.")]
        [SerializeField] private EventBus eventBus;

        private bool _quitPendingOfConfirmation;
            
        private void Start()
        {
            eventBus.AddListener<GameEvents.QuitRequested>(OnQuitRequested);
            eventBus.AddListener<GameEvents.UserConfirmation>(OnUserConfirmation);
            eventBus.AddListener<GameEvents.UserCancelation>(OnUserCancelation);
        }

        private void OnDisable()
        {
            eventBus.RemoveListener<GameEvents.QuitRequested>(OnQuitRequested);
            eventBus.RemoveListener<GameEvents.UserConfirmation>(OnUserConfirmation);
            eventBus.RemoveListener<GameEvents.UserCancelation>(OnUserCancelation);
        }

        /// <summary>
        /// Listener for user cancelation events.
        /// </summary>
        /// <param name="sender">Sender of event. Usually a character monobehaviour.</param>
        /// <param name="_">Event data. Actually not used here.</param>
        private void OnUserCancelation(object sender, GameEvents.UserCancelation _)
        {
            _quitPendingOfConfirmation = false;
        }

        /// <summary>
        /// Listener for user confirmation events.
        /// </summary>
        /// <param name="sender">Sender of event. Usually a character monobehaviour.</param>
        /// <param name="_">Event data. Actually not used here.</param>
        private void OnUserConfirmation(object sender, GameEvents.UserConfirmation _)
        {
            if (_quitPendingOfConfirmation) QuitApplication();
        }

        /// <summary>
        /// Listener for quit requested events.
        /// </summary>
        /// <param name="sender">Sender of event. Usually a character monobehaviour.</param>
        /// <param name="_">Event data. Actually not used here.</param>
        private void OnQuitRequested(object sender, GameEvents.QuitRequested _)
        {
            _quitPendingOfConfirmation = true;
        }

        /// <summary>
        /// Close game and leave to desktop.
        /// </summary>
        private void QuitApplication()
        {
#if UNITY_EDITOR
            // Aplication.Quit() is ignored when playing at editor. So at editor you must se EditorApplication.isPlaying 
            // to false to end game and return to editor. But as EditorApplication comes from UnityEditor namespace it has
            // to be enclosed in #if directives.
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
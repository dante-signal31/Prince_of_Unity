using System;
using System.Collections;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to print at HUD message bar time related messages.
    /// </summary>
    public class GameTimerPrinter : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know remaining time")] 
        [SerializeField] private GameTimer gameTimer;
        [Tooltip("Needed to print how much time leaves.")]
        [SerializeField] private HUDManager hudManager;

        [Header("CONFIGURATION:")] 
        [Tooltip("How much seconds should time message stay at HUD bar.")] 
        [SerializeField] private float messageTime;
        
        /// <summary>
        /// Print at message bar how many minutes leaves to end game.
        /// </summary>
        public void PrintRemainingTimeInMinutes()
        {
            if (gameTimer.RemainingSeconds > 60)
            {
                hudManager.SetMessageForATime($"{(int)Mathf.Floor(gameTimer.RemainingSeconds/60)} MINUTES LEFT", messageTime);
            }
            else
            {
                PrintRemainingTimeInSeconds();
            }
            
        }

        /// <summary>
        /// Print at message bar how many seconds leaves to end game.
        /// </summary>
        public void PrintRemainingTimeInSeconds(float printingTime = float.NaN)
        {
            if (float.IsNaN(printingTime)) printingTime = messageTime;
            hudManager.SetMessageForATime($"{(int)(gameTimer.RemainingSeconds)} SECONDS LEFT", printingTime);
        }

        /// <summary>
        /// Print every second how much time leaves.
        /// </summary>
        public void StartCountDown()
        {
            StartCoroutine(CountDown());
        }

        private IEnumerator CountDown()
        {
            while (gameTimer.RemainingSeconds >= 1)
            {
                PrintRemainingTimeInSeconds(printingTime: 1.0f);
                yield return new WaitForSeconds(1.0f);
            }
        }
        
        /// <summary>
        /// Listener for GameTimer TimerPaused events.
        /// </summary>
        /// <param name="sender">Sender of event. Usually a GameTimer.</param>
        /// <param name="ev">Event data.</param>
        public void OnGameTimerPaused()
        {
            hudManager.SetMessage("GAME PAUSED");
        }

        /// <summary>
        /// Listener for GameTimer TimerResumed events.
        /// </summary>
        /// <param name="sender">Sender of event. Usually a GameTimer.</param>
        /// <param name="ev">Event data.</param>
        public void OnGameTimerResumed()
        {
            hudManager.SetMessage("");
        }
    }
}
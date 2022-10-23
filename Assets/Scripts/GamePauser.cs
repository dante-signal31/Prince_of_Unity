using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This GameTimer component pauses the game when timer is disabled.
    /// </summary>
    public class GamePauser : MonoBehaviour
    {
        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
        }
    }
}
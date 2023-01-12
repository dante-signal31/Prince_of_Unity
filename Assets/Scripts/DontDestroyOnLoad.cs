using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Add this component to any game object that should not be destroyed between scenes.
    ///
    /// Add it to game objects root transform. It affects to every children of root transform.
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
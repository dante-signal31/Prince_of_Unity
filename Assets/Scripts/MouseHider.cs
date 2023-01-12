using UnityEngine;

namespace Prince
{
    // This component hides mouse cursor when game starts
    public class MouseHider : MonoBehaviour
    {
        private void Start()
        {
#if !UNITY_EDITOR
            Cursor.visible = false;
#endif
        }
    }
}
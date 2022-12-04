using System;
using UnityEngine;

namespace Prince
{
    // This component hides mouse cursor when game starts
    public class MouseHider : MonoBehaviour
    {
        private void Start()
        {
            Cursor.visible = false;
        }
    }
}
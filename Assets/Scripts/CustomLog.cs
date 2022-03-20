using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Common custom log implementation used in various behaviours extensions.
    /// </summary>
    public static class CustomLog
    {
        public static void Log(string logString)
        {
            DateTime currentTime = System.DateTime.Now;
            Debug.Log($"[{currentTime.Hour}:{currentTime.Minute}:{currentTime.Second}:{currentTime.Millisecond}] {logString}");
        }
    }
}
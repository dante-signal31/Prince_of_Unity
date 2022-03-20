// using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// <p>If every component logs simultaneously is hard to find the specific thing you need.</p>
    ///
    /// <p>This extension over usual Debug.Log() allows you to associate your log call to a custom
    /// StateMachineBehaviour boolean field so you can enable or disable logging per script. You can activate
    /// that field through inspector if you want that component logs and use that field value for
    /// showScriptLogs when Log() is called.</p>
    /// 
    /// <p>That way your console log window won't be so cluttered and will show only what you're interested in.</p>
    /// </summary>
    public static class StateMachineBehaviourExtensions
    {
        /// <summary>
        /// <p>Show given log string if showScriptLogs is true.</p>
        ///
        /// <p>Besides a detailed timestamp is prepended to given logString. Be aware that default Unity
        /// timestamp goes only to seconds, so if you need milliseconds then you are out of look. This
        /// extension prepend your logs with a timestamps with milliseconds.</p>
        /// </summary>
        /// <param name="script">We are extending StateMachineBehaviour.</param>
        /// <param name="logString">String to log.</param>
        /// <param name="showScriptLogs">True if we want to activate this log, false otherwise.</param>
        public static void Log(this StateMachineBehaviour script, string logString, bool showScriptLogs)
        {
            if (showScriptLogs)
            {
                CustomLog.Log(logString);
                // DateTime currentTime = System.DateTime.Now;
                // Debug.Log($"[{currentTime.Hour}:{currentTime.Minute}:{currentTime.Second}:{currentTime.Millisecond}] {logString}");
            }
        }
        
    }
}
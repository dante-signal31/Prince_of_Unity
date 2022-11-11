using UnityEditor;
using UnityEngine;

namespace Prince
{
    // Most documentation and tutorials about custom property attributes include its definition along with 
    // ist corresponding property drawer a Editor folder.
    //
    // That approach does not work in my case because I have an specific assembly at my Scripts folder to
    // reference it from my play tests. I guess tutorials assume you are using default assemblies but when
    // you are not problem is that your property attribute is not seen from your classes at Scripts folder.
    //
    // So, the only solution I've found is to take custom properties definitions to Scripts folder, and
    // leave it corresponding property drawer at Editor folder.
    
    /// <summary>
    /// Show a help box at inspector with a message about decorated field.
    /// </summary>
    public class HelpBarAttribute : PropertyAttribute
    {
        public string Message;
        public MessageType MessageLevel;

        public HelpBarAttribute(string message, MessageType messageLevel)
        {
            Message = message;
            MessageLevel = messageLevel;
        }
    }
}
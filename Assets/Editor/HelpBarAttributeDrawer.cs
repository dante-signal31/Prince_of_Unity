using Prince;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    // TODO: Migrate to new UI Toolkit support.

    [CustomPropertyDrawer(typeof(HelpBarAttribute))]
    public class HelpBarAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            // Rect where we are going to show help message.
            Rect helpBarRect = position;
            helpBarRect.height = GetHelpBarHeight();
            
            // Get message text.
            string message = ((HelpBarAttribute)attribute).Message;
            MessageType messageLevel = ((HelpBarAttribute)attribute).MessageLevel;
            
            // Now draw help box at calculated position and populate it with given message.
            EditorGUI.HelpBox(helpBarRect, message, messageLevel);

            // Rect where we are going to show our decorated property after our message.
            Rect propertyRect = position;
            propertyRect.y += EditorGUIUtility.standardVerticalSpacing + helpBarRect.height;
            propertyRect.height = EditorGUI.GetPropertyHeight(property, includeChildren: true);
            
            // Draw decorated property at calculated rect.
            EditorGUI.PropertyField(propertyRect, property, includeChildren: true);
        }

        private float GetHelpBarHeight()
        {
            float width = EditorGUIUtility.currentViewWidth;
            HelpBarAttribute helpBarAttribute = (HelpBarAttribute)attribute;
            GUIContent content = new GUIContent(helpBarAttribute.Message);
            float helpBarHeight = EditorStyles.helpBox.CalcHeight(content, width) + EditorGUIUtility.singleLineHeight;
            return helpBarHeight;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Total property height is: HelpBar height + 1 line vertical space + original property height.
            float totalSpace = GetHelpBarHeight() + EditorGUIUtility.standardVerticalSpacing +
                               EditorGUI.GetPropertyHeight(property, includeChildren: true);
            return totalSpace;
        }
    }
}
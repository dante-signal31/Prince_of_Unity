using Prince;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomPropertyDrawer(typeof(HelpBarAttribute))]
    public class HelpBarAttributeDrawer : PropertyDrawer
    {
        // public override void OnGUI(Rect position, SerializedProperty property,
        //     GUIContent label)
        // {
        //     // Rect where we are going to show help message.
        //     Rect helpBarRect = position;
        //     helpBarRect.height = GetHelpBarHeight();
        //     
        //     // Get message text.
        //     string message = ((HelpBarAttribute)attribute).Message;
        //     MessageType messageLevel = ((HelpBarAttribute)attribute).MessageLevel;
        //     
        //     // Now draw help box at calculated position and populate it with given message.
        //     EditorGUI.HelpBox(helpBarRect, message, messageLevel);
        //
        //     // Rect where we are going to show our decorated property after our message.
        //     Rect propertyRect = position;
        //     propertyRect.y += EditorGUIUtility.standardVerticalSpacing + helpBarRect.height;
        //     propertyRect.height = EditorGUI.GetPropertyHeight(property, includeChildren: true);
        //     
        //     // Draw decorated property at calculated rect.
        //     EditorGUI.PropertyField(propertyRect, property, includeChildren: true);
        // }
        //
        // private float GetHelpBarHeight()
        // {
        //     float width = EditorGUIUtility.currentViewWidth;
        //     HelpBarAttribute helpBarAttribute = (HelpBarAttribute)attribute;
        //     GUIContent content = new GUIContent(helpBarAttribute.Message);
        //     float helpBarHeight = EditorStyles.helpBox.CalcHeight(content, width) + EditorGUIUtility.singleLineHeight;
        //     return helpBarHeight;
        // }
        //
        // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        // {
        //     // Total property height is: HelpBar height + 1 line vertical space + original property height.
        //     float totalSpace = GetHelpBarHeight() + EditorGUIUtility.standardVerticalSpacing +
        //                        EditorGUI.GetPropertyHeight(property, includeChildren: true);
        //     return totalSpace;
        // }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create container element.
            VisualElement container = new VisualElement();
            
            // Get message text.
            string message = ((HelpBarAttribute)attribute).Message;
            MessageType messageLevel = ((HelpBarAttribute)attribute).MessageLevel;
            
            // Add help bubble with given message to container.
            HelpBox helpBox = new HelpBox(message, (HelpBoxMessageType) messageLevel);
            container.Add(helpBox);
            
            // And now add decorated property to container.
            PropertyField decoratedProperty = new PropertyField(property);
            container.Add(decoratedProperty);
            
            // Return container to be shown on inspector.
            return container;
        }
    }
}
using Prince;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    // TODO: Migrate to new UI Toolkit support.
    
    [CustomPropertyDrawer(typeof(GameTimer.PlannedEvent))]
    public class PlannedEventDrawer : PropertyDrawer
    {
        private float _lineCount;
        private float _initialHeight;
        readonly float _lineHeight = EditorGUIUtility.singleLineHeight;
        private float CurrentHeight => _initialHeight + (_lineCount * (_lineHeight + EditorGUIUtility.standardVerticalSpacing));
        
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);
            _lineCount = 0;
            // Draw label.
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            int originalIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            // Calculate rects. I want to show first triggeringType combo and after a property field showing
            // only the one that is needed for this triggering type.
            _initialHeight = position.y;
            Rect triggeringTypeRect = new Rect(position.x, CurrentHeight, position.width, _lineHeight);
            _lineCount++;
            Rect triggeringTimeRect = new Rect(position.x, CurrentHeight, position.width, _lineHeight);
            _lineCount++;

            // Keep track on any changes in this inspector values.
            EditorGUI.BeginChangeCheck();
            
            // Draw fields.
            // Show triggering time.
            SerializedProperty serializedTriggeringType = property.FindPropertyRelative("triggeringType");
            GameTimer.TriggeringTimeTypes selectedTimeType = (GameTimer.TriggeringTimeTypes) serializedTriggeringType.enumValueIndex;
            selectedTimeType = (GameTimer.TriggeringTimeTypes) EditorGUI.EnumPopup(triggeringTypeRect, serializedTriggeringType.displayName, selectedTimeType);
            serializedTriggeringType.enumValueIndex = (int) selectedTimeType;
            EditorGUI.indentLevel++;
            switch (selectedTimeType)
            {
                case GameTimer.TriggeringTimeTypes.ElapsedSeconds:
                    SerializedProperty serializedElapsedSeconds = property.FindPropertyRelative("elapsedSeconds");
                    EditorGUI.PropertyField(triggeringTimeRect, serializedElapsedSeconds);
                    break;
                case GameTimer.TriggeringTimeTypes.GameTotalTimePercentage:
                    SerializedProperty serializedElapsedPercentage = property.FindPropertyRelative("elapsedPercentage");
                    EditorGUI.Slider(triggeringTimeRect, serializedElapsedPercentage, 0, 100);
                    break;
            }
            EditorGUI.indentLevel--;
            // Now show triggered event.
            SerializedProperty serializedEventToTrigger = property.FindPropertyRelative("eventToTrigger");
            
            Rect eventsRect = new Rect(position.x, CurrentHeight, position.width, EditorGUI.GetPropertyHeight(serializedEventToTrigger, includeChildren: true));
            EditorGUI.PropertyField(eventsRect, property.FindPropertyRelative("eventToTrigger"), includeChildren: true);
            _lineCount += EditorGUI.GetPropertyHeight(serializedEventToTrigger) / _lineHeight;
            // Set indent back to what it was.
            EditorGUI.indentLevel = originalIndent;
            // Store any change.
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }

        // Needed to return global property size to parents like lists drawers.
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Here I use my own local lineCount because if I use global _lineCount it gets messy at lists
            // with multiple items.
            
            // First the line count for the static part: triggeringType and triggeringTime.
            float lineCount = 2;
            // Now calculate dynamic part.
            SerializedProperty serializedEventToTrigger = property.FindPropertyRelative("eventToTrigger");
            lineCount += EditorGUI.GetPropertyHeight(serializedEventToTrigger) / _lineHeight;
            
            // With total line count, now you can get total property height.
            // Vertical spacing is multiplied by three because we have 3 vertical element that need
            // spacing between them: triggeringType, triggeringTime and eventToTrigger.
            return lineCount * _lineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
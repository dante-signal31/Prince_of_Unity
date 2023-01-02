using System;
using System.Collections.Generic;
using System.Linq;
using Prince;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomPropertyDrawer(typeof(GameTimer.PlannedEvent))]
    public class PlannedEventDrawer : PropertyDrawer
    {
        // private float _lineCount;
        // private float _initialHeight;
        // readonly float _lineHeight = EditorGUIUtility.singleLineHeight;
        // private float CurrentHeight => _initialHeight + (_lineCount * (_lineHeight + EditorGUIUtility.standardVerticalSpacing));
        //
        // public override void OnGUI(Rect position, SerializedProperty property,
        //     GUIContent label)
        // {
        //     // Using BeginProperty / EndProperty on the parent property means that
        //     // prefab override logic works on the entire property.
        //     EditorGUI.BeginProperty(position, label, property);
        //     _lineCount = 0;
        //     // Draw label.
        //     position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        //     int originalIndent = EditorGUI.indentLevel;
        //     EditorGUI.indentLevel = 0;
        //     
        //     // Calculate rects. I want to show first triggeringType combo and after a property field showing
        //     // only the one that is needed for this triggering type.
        //     _initialHeight = position.y;
        //     Rect triggeringTypeRect = new Rect(position.x, CurrentHeight, position.width, _lineHeight);
        //     _lineCount++;
        //     Rect triggeringTimeRect = new Rect(position.x, CurrentHeight, position.width, _lineHeight);
        //     _lineCount++;
        //
        //     // Keep track on any changes in this inspector values.
        //     EditorGUI.BeginChangeCheck();
        //     
        //     // Draw fields.
        //     // Show triggering time.
        //     SerializedProperty serializedTriggeringType = property.FindPropertyRelative("triggeringType");
        //     GameTimer.TriggeringTimeTypes selectedTimeType = (GameTimer.TriggeringTimeTypes) serializedTriggeringType.enumValueIndex;
        //     selectedTimeType = (GameTimer.TriggeringTimeTypes) EditorGUI.EnumPopup(triggeringTypeRect, serializedTriggeringType.displayName, selectedTimeType);
        //     serializedTriggeringType.enumValueIndex = (int) selectedTimeType;
        //     EditorGUI.indentLevel++;
        //     switch (selectedTimeType)
        //     {
        //         case GameTimer.TriggeringTimeTypes.ElapsedSeconds:
        //             SerializedProperty serializedElapsedSeconds = property.FindPropertyRelative("elapsedSeconds");
        //             EditorGUI.PropertyField(triggeringTimeRect, serializedElapsedSeconds);
        //             break;
        //         case GameTimer.TriggeringTimeTypes.GameTotalTimePercentage:
        //             SerializedProperty serializedElapsedPercentage = property.FindPropertyRelative("elapsedPercentage");
        //             EditorGUI.Slider(triggeringTimeRect, serializedElapsedPercentage, 0, 100);
        //             break;
        //     }
        //     EditorGUI.indentLevel--;
        //     // Now show triggered event.
        //     SerializedProperty serializedEventToTrigger = property.FindPropertyRelative("eventToTrigger");
        //     
        //     Rect eventsRect = new Rect(position.x, CurrentHeight, position.width, EditorGUI.GetPropertyHeight(serializedEventToTrigger, includeChildren: true));
        //     EditorGUI.PropertyField(eventsRect, property.FindPropertyRelative("eventToTrigger"), includeChildren: true);
        //     _lineCount += EditorGUI.GetPropertyHeight(serializedEventToTrigger) / _lineHeight;
        //     // Set indent back to what it was.
        //     EditorGUI.indentLevel = originalIndent;
        //     // Store any change.
        //     if (EditorGUI.EndChangeCheck())
        //     {
        //         property.serializedObject.ApplyModifiedProperties();
        //     }
        //     EditorGUI.EndProperty();
        // }
        //
        // // Needed to return global property size to parents like lists drawers.
        // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        // {
        //     // Here I use my own local lineCount because if I use global _lineCount it gets messy at lists
        //     // with multiple items.
        //     
        //     // First the line count for the static part: triggeringType and triggeringTime.
        //     float lineCount = 2;
        //     // Now calculate dynamic part.
        //     SerializedProperty serializedEventToTrigger = property.FindPropertyRelative("eventToTrigger");
        //     lineCount += EditorGUI.GetPropertyHeight(serializedEventToTrigger) / _lineHeight;
        //     
        //     // With total line count, now you can get total property height.
        //     // Vertical spacing is multiplied by three because we have 3 vertical element that need
        //     // spacing between them: triggeringType, triggeringTime and eventToTrigger.
        //     return lineCount * _lineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
        // }

        private Length _leftMargin = new Length(3.0f, LengthUnit.Percent);

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create main container
            VisualElement container = new VisualElement();
            
            // Four vertical elements: Prefix label, triggering type, elapsed amount and event to trigger..
            container.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            
            // Prefix label.
            Label prefixLabel = new Label();
            container.Add(prefixLabel);
            prefixLabel.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft);
            prefixLabel.text = property.displayName;
            
            // Data container.
            VisualElement dataContainer = new VisualElement();
            container.Add(dataContainer);
            dataContainer.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            dataContainer.style.alignContent = new StyleEnum<Align>(Align.FlexEnd);
            dataContainer.style.marginLeft = new StyleLength(_leftMargin);
            
            // Triggering container.
            VisualElement triggeringContainer = new VisualElement();
            triggeringContainer.name = $"triggeringContainer - {property.displayName}";
            triggeringContainer.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.ColumnReverse);
            dataContainer.Add(triggeringContainer);
            
            
            // Elapsed container.
            VisualElement elapsedContainer = new VisualElement();
            elapsedContainer.name = $"elapsedContainer - {property.displayName}";
            elapsedContainer.style.marginLeft = new StyleLength(_leftMargin);
            elapsedContainer.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            triggeringContainer.Add(elapsedContainer);
            
            SerializedProperty serializedTriggeringType = property.FindPropertyRelative("triggeringType");
            GameTimer.TriggeringTimeTypes selectedTimeType = (GameTimer.TriggeringTimeTypes) serializedTriggeringType.enumValueIndex;
            PopupField<GameTimer.TriggeringTimeTypes> triggeringTimePopup = new PopupField<GameTimer.TriggeringTimeTypes>(serializedTriggeringType.displayName);
            triggeringTimePopup.choices = new List<GameTimer.TriggeringTimeTypes>(Enum.GetValues(typeof(GameTimer.TriggeringTimeTypes)).Cast<GameTimer.TriggeringTimeTypes>().ToList());
            triggeringTimePopup.value = selectedTimeType;
            triggeringTimePopup.RegisterValueChangedCallback(evt =>
            {
                selectedTimeType = (GameTimer.TriggeringTimeTypes)evt.newValue;
                serializedTriggeringType.enumValueIndex = (int) selectedTimeType;
                serializedTriggeringType.serializedObject.ApplyModifiedProperties();
                ShowElapsedField(property, selectedTimeType, elapsedContainer);
            });
            triggeringContainer.Add(triggeringTimePopup);
            ShowElapsedField(property, selectedTimeType, elapsedContainer);

            
            // Now show triggered event.
            SerializedProperty serializedEventToTrigger = property.FindPropertyRelative("eventToTrigger");
            dataContainer.Add(new PropertyField(serializedEventToTrigger));
            
            // Return created container to be shown on inspector.
            return container;
        }

        
        private void ShowElapsedField(SerializedProperty property, GameTimer.TriggeringTimeTypes selectedTimeType,
            VisualElement parentContainer)
        {
            parentContainer.Clear();

            switch (selectedTimeType)
            {
                case GameTimer.TriggeringTimeTypes.ElapsedSeconds:
                    SerializedProperty serializedElapsedSeconds = property.FindPropertyRelative("elapsedSeconds");
                    // I don't know why a PropertyField is now correctly redrawn when combo was updated, so in this case 
                    // I had to use a FloatField.
                    FloatField elapsedSeconds = new FloatField("Elapsed Seconds");
                    elapsedSeconds.name = $"elapsedSecondsField - {property.displayName}";
                    elapsedSeconds.value = serializedElapsedSeconds.floatValue;
                    elapsedSeconds.RegisterValueChangedCallback(evt =>
                    {
                        float desiredSeconds = evt.newValue;
                        serializedElapsedSeconds.floatValue = desiredSeconds;
                        serializedElapsedSeconds.serializedObject.ApplyModifiedProperties();
                    });
                    parentContainer.Add(elapsedSeconds);
                    break;
                case GameTimer.TriggeringTimeTypes.GameTotalTimePercentage:
                    SerializedProperty serializedElapsedPercentage = property.FindPropertyRelative("elapsedPercentage");
                    Slider elapsedPercentage = new Slider("Elapsed Percentage", 0, 100);
                    elapsedPercentage.name = $"elapsedPercentageField - {property.displayName}";
                    elapsedPercentage.showInputField = true;
                    elapsedPercentage.value = serializedElapsedPercentage.floatValue;
                    elapsedPercentage.RegisterValueChangedCallback(evt =>
                    {
                        float desiredPercentage = evt.newValue;
                        serializedElapsedPercentage.floatValue = desiredPercentage;
                        serializedElapsedPercentage.serializedObject.ApplyModifiedProperties();
                    });
                    parentContainer.Add(elapsedPercentage);
                    break;
            }
        }
    }
}
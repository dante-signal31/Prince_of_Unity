using System;
using Prince;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(TrapStatus))]
    public class TrapStatusInspector : UnityEditor.Editor
    {
        private SerializedProperty _stateMachineProperty;
        private SerializedProperty _soundControllerProperty;
        private SerializedProperty _killModeProperty;
        private SerializedProperty _loopProperty;
        private SerializedProperty _loopWaitingTime;

        private Length _leftMargin = new Length(3.0f, LengthUnit.Percent);
        
        private void OnEnable()
        {
            _stateMachineProperty = serializedObject.FindProperty("stateMachine");
            _soundControllerProperty = serializedObject.FindProperty("soundController");
            _killModeProperty = serializedObject.FindProperty("killMode");
            _loopProperty = serializedObject.FindProperty("loop");
            _loopWaitingTime = serializedObject.FindProperty("loopWaitingTime");
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            // Add elements in a descending column.
            root.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            
            // Just add those properties we want to be shown in its default view.
            root.Add(new PropertyField(_stateMachineProperty));
            root.Add(new PropertyField(_soundControllerProperty));
            root.Add(new PropertyField(_killModeProperty));
            
            // Here, I want to customize the way this part of inspector is shown, so I
            // use a toogle instead of a PropertyField to access its value.
            Toggle isLoop = new Toggle("Loop");
            isLoop.BindProperty(_loopProperty);
            root.Add(isLoop);

            // loopWaitingTimePanel should show any content only if Loop value  is true.
            VisualElement loopWaitingTimePanel = new VisualElement();
            root.Add(loopWaitingTimePanel);
            // When adding fields dynamically use and specific field instead of PropertyField.
            // I've detected repainting problems when using PropertyFields.
            FloatField loopWaitingTime = new FloatField("Loop Waiting Time");
            loopWaitingTime.BindProperty(_loopWaitingTime);
            loopWaitingTime.style.marginLeft = new StyleLength(_leftMargin);
            if (_loopProperty.boolValue) loopWaitingTimePanel.Add(loopWaitingTime);

            // Add a callback to refresh loopWaitingTimePanel depending on Loop value changes.
            isLoop.RegisterValueChangedCallback(evt =>
            {
                // make sure panel is empty.
                loopWaitingTimePanel.Clear();
                
                // Fill panel with loopWaitingTimeProperty only if Loop is true.
                if (evt.newValue) loopWaitingTimePanel.Add(loopWaitingTime);
            });
            
            return root;
        }
    }
}
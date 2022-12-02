using Prince;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// <p>This property drawer is needed because I want to drag and drop scene assets to inspector. Problem is
    /// that SceneAssets are under UnityEditor namespace. Remember that final build fails if you use UnityEditor
    /// namespace outside from Editor folder.</p>
    ///
    /// <p> So, I need a custom property drawer to take dropped scenes and take their name and store at Level
    /// struct just scene name string, not an SceneAsset.</p>
    /// </summary>
    [CustomPropertyDrawer(typeof(LevelLoader.Level))]
    public class LevelDrawer : PropertyDrawer
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
            
            // Calculate rects. I want to show first levelName field and after a property field to get
            // an scene object. That scene object will be read and its name will be levelSceneName.
            _initialHeight = position.y;
            _lineCount++;
            Rect levelNameRect = new Rect(position.x, CurrentHeight, position.width, _lineHeight);
            _lineCount++;
            Rect levelSceneNameRect = new Rect(position.x, CurrentHeight, position.width, _lineHeight);
            _lineCount++;
            
            // Keep track on any changes in this inspector values.
            EditorGUI.BeginChangeCheck();
            
            // Draw fields.
            // Show levelName.
            SerializedProperty serializedLevelNameProperty = property.FindPropertyRelative("levelName");
            EditorGUI.PropertyField(levelNameRect, serializedLevelNameProperty);
            // Show scene field.
            SerializedProperty serializedLevelSceneNameProperty = property.FindPropertyRelative("levelSceneName");
            string serializedLevelSceneName = serializedLevelSceneNameProperty.stringValue;
            SceneAsset currentScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(serializedLevelSceneName);
            SceneAsset selectedScene = (SceneAsset)EditorGUI.ObjectField(levelSceneNameRect, "Level Scene", currentScene,
                typeof(SceneAsset), true);
            
            // Store any change.
            if (EditorGUI.EndChangeCheck())
            {
                if (selectedScene != null) serializedLevelSceneNameProperty.stringValue = AssetDatabase.GetAssetPath(selectedScene);
                Debug.Log($"Selected scene with path: {serializedLevelSceneNameProperty.stringValue}");
                property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }

        // Needed to return global property size to parents like lists drawers.
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // return base.GetPropertyHeight(property, label);
            
            // Level drawer only has two lines, the one with the name and the one with the object.
            // Vertical spacing is multiplied by two because we have 2 vertical element that need
            // spacing between them: levelName and levelScene.
            return 3 * _lineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;;
        }
    }
}
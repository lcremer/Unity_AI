using UnityEditor;
using UnityEngine;
using BehaviorLibrary;

[CustomPropertyDrawer(typeof(BehaviorTree))]
public class AIPreviewer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (GUI.Button(position, "Open Behavior Tree Preview"))
        {
            // TODO: Fix workaround by deriving BehaviorTree from ScriptableObject
            BehaviorTree behavior = property.objectReferenceValue as System.Object as BehaviorTree;
            AIPreviewWindow.Launch(behavior);
        }
    }
}
using System.Collections;
using BehaviorLibrary.Components.Composites;
using UnityEditor;
using UnityEngine;
using BehaviorLibrary;
using System.Reflection;

[CustomPropertyDrawer(typeof(BehaviorTree))]
public class AIPreviewer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (GUI.Button(position, "Open Behavior Tree Preview"))
        {
            BehaviorTree behavior = property.GetPropertyData<BehaviorTree>();
            AIPreviewWindow.Launch(behavior);
        }
    }
}
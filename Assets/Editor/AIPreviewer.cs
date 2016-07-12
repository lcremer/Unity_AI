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
            BehaviorTree behavior = property.objectReferenceValue as BehaviorTree;
            if (behavior != null)
            {
                AIPreviewWindow.Launch( behavior );
            }
            else
            {
                Debug.LogError( "AIPreviewer SerializedProperty ObjectReferenceValue cast to BehaviorTree result was null!" );
            }
        }
    }
}
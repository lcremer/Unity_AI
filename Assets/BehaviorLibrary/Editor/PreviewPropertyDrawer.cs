using UnityEditor;
using UnityEngine;
using BehaviorLibrary;

namespace BehaviorLibary.Editor
{
    [CustomPropertyDrawer( typeof( BehaviorTree ) )]
    public class PreviewPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            if ( !UnityEditor.EditorApplication.isPlaying )
            {
                GUI.enabled = false;
            }

            if ( GUI.Button( position, "Preview Behavior Tree" ) )
            {
                BehaviorTree behavior = property.objectReferenceValue as BehaviorTree;
                if ( behavior != null )
                {
                    PreviewEditorWindow.Launch( behavior );
                }
                else
                {
                    Debug.LogWarning( "AIPreviewer SerializedProperty ObjectReferenceValue cast to BehaviorTree result was null!" );
                }
            }
        }
    }
}
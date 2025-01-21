using UnityEditor;
using UnityEngine;

namespace CHG.Utilities.Attribute
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute), true)]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ReadOnlyAttribute _attribute = attribute as ReadOnlyAttribute;
            if((_attribute.OnRuntime && Application.isPlaying) || (_attribute.OnEditor && !Application.isPlaying))
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(position, property, label, true);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);    
            }            
        }
    }
}

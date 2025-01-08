using UnityEditor;
using UnityEngine;

namespace CHG.Utilities.EditorExpansion
{
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect fieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            if(fieldInfo.FieldType.IsArray || fieldInfo.FieldType.IsGenericType)
            {
                EditorGUI.HelpBox(fieldRect, "Required Attribute doesn't work with Array / Generic Type", MessageType.Error);
                return;
            }
            
            EditorGUI.PropertyField(fieldRect, property, label);
            RequiredAttribute _attribute = attribute as RequiredAttribute;
            
            if (property.propertyType == SerializedPropertyType.String && string.IsNullOrEmpty(property.stringValue))
            {
                _attribute.isSafe = false;
            }
            else if(property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null)
            {
                
                _attribute.isSafe = false;
            }
            else
            {
                _attribute.isSafe = true;
            }

            if(!_attribute.isSafe)
            {
                Rect warningRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, 20);
                EditorGUI.HelpBox(warningRect, property.displayName + "'s Value is Required!", MessageType.Warning);

                if(_attribute.showWarning)
                {
                    Debug.LogWarning(property.serializedObject.targetObject.name + ": " + property.displayName + "'s Value is Required!");
                }
            }
        }        
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            RequiredAttribute _attribute = attribute as RequiredAttribute;
            if (_attribute != null && !_attribute.isSafe)
            {
                return base.GetPropertyHeight(property, label) + 20;                
            }
            return base.GetPropertyHeight(property, label);
        }
    }
}
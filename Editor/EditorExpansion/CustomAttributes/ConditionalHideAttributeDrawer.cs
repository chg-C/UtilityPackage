using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.iOS;

namespace CHG.Utilities.EditorExpansion
{
    [CustomPropertyDrawer(typeof(ConditionalHideAttribute), true)]
    public class ConditionalHideDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute _attribute = attribute as ConditionalHideAttribute;
          
            if (_attribute != null && property.serializedObject.targetObject)
            {
                SerializedProperty refProperty = property.serializedObject.FindProperty(_attribute.refProperty);
                if (refProperty != null)
                {
                    switch(refProperty.propertyType)
                    {
                        case SerializedPropertyType.Boolean:
                            _attribute.hideProperty = refProperty.boolValue;
                        break;
                        case SerializedPropertyType.String:
                            _attribute.hideProperty = String.IsNullOrEmpty(refProperty.stringValue);
                        break;
                        case SerializedPropertyType.ObjectReference:
                            _attribute.hideProperty = refProperty.objectReferenceValue != null;
                        break;
                        default:
                            EditorGUI.LabelField(position, "ConditionalHide Working With Bool / String / Object References only");
                            Debug.LogWarning("ConditionalHide Working With Bool / String / Object References only");
                        return;
                    }
                    _attribute.hideProperty = _attribute.inverted == _attribute.hideProperty;
                }
                else
                {
                    Debug.LogWarning("There is No " + _attribute.refProperty + " Property!");
                    _attribute.hideProperty = false;
                }
            }
            
            label = EditorGUI.BeginProperty(position, label, property);
            if(!_attribute.hideProperty)
                EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute _attribute = attribute as ConditionalHideAttribute;
            if (_attribute != null)
            {
                if(_attribute.hideProperty)
                    return 0;
            }
            return base.GetPropertyHeight(property, label);
        }
    }
}
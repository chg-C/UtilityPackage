using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CHG.Utilities.Attribute
{
    [CustomPropertyDrawer(typeof(ConditionalHideAttribute), true)]
    public class ConditionalHideDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute conditionalHide = attribute as ConditionalHideAttribute;
          
            if (conditionalHide != null && property.serializedObject.targetObject)
            {
                CheckConditions(ref conditionalHide, property);
            }
            
            label = EditorGUI.BeginProperty(position, label, property);
            if(!conditionalHide.NeedHide)
                EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }
        
        private void CheckConditions(ref ConditionalHideAttribute attribute, SerializedProperty property)
        {
            if(CheckSerialized(ref attribute, property))
                return;
            if(CheckProperty(ref attribute, property))
                return;

            Debug.LogWarning(property.serializedObject.targetObject.GetType() + " - " + attribute.refProperty + "를 찾을 수 없습니다!");
        }
        /// <summary>
        /// 직렬화(public 혹은 SerializeField Attribute) Field 체크
        /// </summary>
        private bool CheckSerialized(ref ConditionalHideAttribute attribute, SerializedProperty property)
        {
            SerializedProperty refProperty = property.serializedObject.FindProperty(attribute.refProperty);
            if (refProperty != null)
            {
                switch(refProperty.propertyType)
                {
                    case SerializedPropertyType.Boolean:
                        attribute.hideProperty = refProperty.boolValue;
                        break;
                    case SerializedPropertyType.String:
                        attribute.hideProperty = String.IsNullOrEmpty(refProperty.stringValue);
                        break;
                    case SerializedPropertyType.ObjectReference:
                        attribute.hideProperty = refProperty.objectReferenceValue != null;
                        break;
                    default:
                        Debug.LogWarning(property.serializedObject.targetObject.GetType() + " - ConditionalHide Attribute는 bool, string, Object Reference Field를 지원합니다.");
                        return false;
                    
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Reflection을 사용해서 Property 값 체크하기
        /// </summary>
        private bool CheckProperty(ref ConditionalHideAttribute attribute, SerializedProperty property)
        {  
            Type type = property.serializedObject.targetObject.GetType();
            //public + instance의 Property 가져오기
            var info = type.GetProperty(attribute.refProperty, BindingFlags.Instance | BindingFlags.Public);
            
            if(info != null)
            {
                if(info.PropertyType == typeof(bool))
                {
                    attribute.hideProperty = (bool)info.GetValue(property.serializedObject.targetObject);
                    return true;
                }
                else if(info.PropertyType == typeof(string))
                {
                    attribute.hideProperty = string.IsNullOrEmpty((string)info.GetValue(property.serializedObject.targetObject));
                    return true;
                }
                else
                {
                    Debug.LogWarning(property.serializedObject.targetObject.GetType() + " - ConditionalHide Attribute는 bool, string Property를 지원합니다.");
                    return false;
                }
            }

            return false;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute _attribute = attribute as ConditionalHideAttribute;
            if (_attribute != null)
            {
                if(_attribute.NeedHide)
                    return 0;
            }
            return base.GetPropertyHeight(property, label);
        }
    }
}
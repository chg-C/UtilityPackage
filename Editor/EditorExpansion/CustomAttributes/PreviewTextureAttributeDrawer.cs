using UnityEditor;
using UnityEngine;

namespace CHG.Utilities.EditorExpansion
{
    [CustomPropertyDrawer(typeof(PreviewTextureAttribute), true)]
    public class PreviewTextureAttributeDrawer : PropertyDrawer
    {
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PreviewTextureAttribute _attribute = attribute as PreviewTextureAttribute;
            if(_attribute.isReady == false)
            {
                if(_attribute.propertyType == PreviewTextureAttribute.PropertyType.UNKNOWN)
                {
                    System.Type type = GetObjectType(property);
                    if(type == typeof(string))
                    {
                        _attribute.propertyType = PreviewTextureAttribute.PropertyType.STRING;
                    }
                    else if(type == typeof(Texture2D))
                    {
                        _attribute.propertyType = PreviewTextureAttribute.PropertyType.TEXTURE2D;                        
                    }
                    else if(type == typeof(Texture))
                    {
                        _attribute.propertyType = PreviewTextureAttribute.PropertyType.TEXTURE;                        
                    }
                    else if(type == typeof(Sprite))
                    {
                        _attribute.propertyType = PreviewTextureAttribute.PropertyType.SPRITE;
                    }
                    else
                    {
                        _attribute.propertyType = PreviewTextureAttribute.PropertyType.INVALID;

                        _attribute.errorMessage = "PreviewTexture Attribute는 Texture, Texture2D, Sprite, string만 사용할 수 있습니다.";
                    }
                }
                
                if(_attribute.propertyType == PreviewTextureAttribute.PropertyType.STRING)
                {
                    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(property.stringValue);
                    if(texture == null)
                    {
                        _attribute.errorMessage = "유효하지 않은 텍스처 경로입니다.";
                    }
                    else
                    {
                        _attribute.errorMessage = "";
                        _attribute.PreviewTexture = texture;
                    }
                }
                else if(_attribute.propertyType == PreviewTextureAttribute.PropertyType.SPRITE)
                {
                    Sprite sprite = property.objectReferenceValue as Sprite;
                    if(sprite != null)
                        _attribute.PreviewTexture = sprite.texture;
                    else
                        _attribute.PreviewTexture = null;
                    
                    _attribute.errorMessage = "";
                }
                else if(_attribute.propertyType == PreviewTextureAttribute.PropertyType.TEXTURE ||
                        _attribute.propertyType == PreviewTextureAttribute.PropertyType.TEXTURE2D)
                {
                    _attribute.PreviewTexture = property.objectReferenceValue as Texture;
                    _attribute.errorMessage = "";
                }

                _attribute.isReady = true;
            }
            EditorGUILayout.BeginVertical();
            //Default Property
            Rect propertyRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(propertyRect, property, label);
            //Error Message(if Has)
            if(!string.IsNullOrEmpty(_attribute.errorMessage))
            {
                Rect errorMessageRect = new Rect(position.x, position.y + 2 + EditorGUIUtility.singleLineHeight, position.width,
                    PreviewTextureAttribute.ErrorMessageHeight);
                EditorGUI.HelpBox(errorMessageRect, _attribute.errorMessage, MessageType.Warning);
            }
            else if(_attribute.PreviewTexture != null)
            {
                Rect previewRect = new Rect(position.x, position.y + 2 + EditorGUIUtility.singleLineHeight,
                    _attribute.width, _attribute.height);
                switch(_attribute.drawType)
                {
                    case PreviewRenderType.TEXTURE:
                        EditorGUI.DrawPreviewTexture(previewRect, _attribute.PreviewTexture);
                        break;
                    case PreviewRenderType.ALPHA:
                        EditorGUI.DrawTextureAlpha(previewRect, _attribute.PreviewTexture);
                        break;
                    default:
                    case PreviewRenderType.TRANSPARENT:
                        EditorGUI.DrawTextureTransparent(previewRect, _attribute.PreviewTexture);
                        break;
                }
                
            }
            EditorGUILayout.EndVertical();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            
            PreviewTextureAttribute _attribute = attribute as PreviewTextureAttribute;
            if(!string.IsNullOrEmpty(_attribute.errorMessage))
            {
                return base.GetPropertyHeight(property, label) + 2 + PreviewTextureAttribute.ErrorMessageHeight;
            }
            else if(_attribute.PreviewTexture != null)
            {
                return base.GetPropertyHeight(property, label) + 2 + _attribute.height;
            }
            return base.GetPropertyHeight(property, label);
 
        }  
        private System.Type GetObjectType(SerializedProperty property)
        {
            var targetObject = property.serializedObject.targetObject;
            var fieldInfo = targetObject.GetType().GetField(property.name);

            if (fieldInfo != null)
            {
                return fieldInfo.FieldType;
            }

            return null;
        }
    }
}
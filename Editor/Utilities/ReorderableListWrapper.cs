using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
using Unity.Android.Gradle;

namespace CHG.Utilities.EditorExpansion
{
    public class ReorderableListWrapper<T>
    {
        ReorderableList list;
        SerializedProperty property;
        public string listName;

        public ReorderableListWrapper(SerializedProperty property, string listName)
        {
            this.property = property;
            this.listName = listName;

            list = new ReorderableList(property.serializedObject, property, true, true, true, true);
            list.drawHeaderCallback =
                (rect) => {
                    EditorGUI.LabelField(rect, listName);
                };
            
            list.drawElementCallback = 
                (rect, index, isActivated, isFocused) => {
                    SerializedProperty element = property.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, element);
                };
            
            list.onAddCallback =
                (list) => {
                    property.InsertArrayElementAtIndex(property.arraySize);
                    SerializedProperty newProperty = property.GetArrayElementAtIndex(property.arraySize-1);
                };
            
            list.onRemoveCallback =
                (list) =>
                {
                    //list.selectedIndices
                    property.DeleteArrayElementAtIndex(list.index);
                };
        }
        public void DrawLayoutList()
        {
            list.DoLayoutList();            
        }
    }
}
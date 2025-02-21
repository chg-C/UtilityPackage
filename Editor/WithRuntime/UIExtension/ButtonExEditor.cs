using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace CHG.Utilities.UI {
    [CustomEditor(typeof(ButtonEx))]
    public class ButtonExEditor : ButtonEditor
    {
        private SerializedProperty onPointerDownProperty;
        private SerializedProperty onPointerUpProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            onPointerDownProperty = base.serializedObject.FindProperty("onPointerDownEvent");
            onPointerUpProperty = base.serializedObject.FindProperty("onPointerUpEvent");
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            base.serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(onPointerDownProperty);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(onPointerUpProperty);
            
            base.serializedObject.ApplyModifiedProperties();
        }
    }
}
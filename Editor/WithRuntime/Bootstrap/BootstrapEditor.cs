namespace CHG.Utilities.Bootstrap.Editor
{
    using UnityEditor;
    [CustomEditor(typeof(Bootstrap))]
    public class BootstrapEditor : Editor
    {
        SerializedProperty profileProperty;
        Editor profileEditor = null;
        void OnEnable()
        {
            profileProperty = serializedObject.FindProperty("profile");
        }        
        private void OnDisable()
        {
            if (profileEditor != null)
            {
                DestroyImmediate(profileEditor);
            }
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(profileProperty);
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            
            var profile = profileProperty.objectReferenceValue;
            

            if(profileEditor == null && profileProperty != null)
            {
                profileEditor = CreateEditor(profile);

            }
            else if(profileEditor.target != profileProperty.objectReferenceValue)
            {
                DestroyImmediate(profileEditor);
                profileEditor = CreateEditor(profile);
            }
            

            if(profileEditor != null)
            {
                profileEditor.OnInspectorGUI();
            }
        }
    }
}
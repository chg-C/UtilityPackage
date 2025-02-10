namespace CHG.Utilities.Bootstrap.Editor
{
    using CHG.Utilities.EditorExpansion;
    using UnityEditor;
    
    [CustomEditor(typeof(BootstrapProfile))]
    public class BootstrapProfileEditor : Editor
    {
        ReorderableListWrapper list;
        private void OnEnable()
        {
            list = new ReorderableListWrapper(serializedObject, serializedObject.FindProperty("Prefabs"), "Prefabs");
            //prefabList = new PrefabList(serializedObject, serializedObject.FindProperty("Prefabs"));
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            list.DrawLayoutList();
            
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
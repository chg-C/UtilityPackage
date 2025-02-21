using UnityEngine;
using UnityEditor;
using CHG.Utilities.Datas;

namespace CHG.Editor.ScriptTemplator
{
    using Editor = UnityEditor.Editor;
    [CustomEditor(typeof(ScriptTemplates))]
    public class ScriptTemplatesEditor : Editor
    {
        ScriptTemplates data;
        SerializedProperty keywordsProperty;
        Editor keywordsEditor = null;        
        
        void OnEnable()
        {
            data = (ScriptTemplates)target;
            keywordsProperty = serializedObject.FindProperty("keywordsData");
        }
        public override void OnInspectorGUI()
        {
            FilePathSetting();
            EditorGUILayout.Space(15);
            KeywordsSetting();
        }
        void FilePathSetting()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Templates File Path:", data.templatesPath);
            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("Show Folder"))
            {
                if(System.IO.Directory.Exists(data.templatesPath))
                {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(data.templatesPath);
                    EditorGUIUtility.PingObject(Selection.activeObject);
                }
            }
            if(GUILayout.Button("Change Path"))
            {
                string newPath = EditorUtility.OpenFolderPanel("ScriptTemplates File Path", data.templatesPath, "");
                if(!string.IsNullOrEmpty(newPath))
                {
                    newPath = FileUtility.ConvertToRelativePath(newPath);
                    data.templatesPath = newPath;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        void KeywordsSetting()
        {            
            EditorGUILayout.LabelField("Pre-defined Keywords");
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(keywordsProperty);
                if(EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }
                
                var keywords = keywordsProperty.objectReferenceValue;
                

                if(keywordsEditor == null && keywordsProperty != null)
                {
                    keywordsEditor = CreateEditor(keywords);

                }
                else if(keywordsEditor.target != keywordsProperty.objectReferenceValue)
                {
                    DestroyImmediate(keywordsEditor);
                    keywordsEditor = CreateEditor(keywords);
                }
                
                if(keywordsEditor != null)
                {
                    keywordsEditor.OnInspectorGUI();
                }
            }
            EditorGUILayout.EndVertical();
        }
        private void OnDisable()
        {
            if (keywordsEditor != null)
            {
                DestroyImmediate(keywordsEditor);
            }
        }
    }
}
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace CHG.Utilities.Texts
{
    /// <summary>
    /// Inspector에서 직접 Text Asset을 수정할 수 있는 Editor Class
    /// </summary>
    
    [CustomEditor(typeof(TextAsset))]
    public class DirectTextEditor : UnityEditor.Editor
    {
        private DirectTextEditHeader header = new DirectTextEditHeader();
        private ITextAssetEditor defaultEditor = new CommonTextEditor();


        private TextAsset targetAsset;
        private string initialText;
        private string lastText;

        private bool isDirty = false;

        private void OnEnable() {
            targetAsset = (TextAsset)target;
            initialText = targetAsset.text;
            lastText = initialText;

            isDirty = false;
        }
        private void OnDisable() {
            if(initialText != lastText && isDirty)
            {
                if(EditorUtility.DisplayDialog("변경 사항 저장",
                    "텍스트 어셋의 내용이 변경되었습니다.\n저장하시겠습니까?",
                    "예", "아니오"))
                {
                    UpdateAsset(targetAsset);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            TextAsset textAsset = (TextAsset)target;

            bool wasEnabled = GUI.enabled;
            GUI.enabled = true;

            DrawInspectorGUI(defaultEditor, textAsset, false);

            GUI.enabled = wasEnabled;
        }
        void DrawInspectorGUI(ITextAssetEditor editor, TextAsset asset, bool useCustomEdit)
        {
            //Header
            GUILayout.BeginVertical();
            HeaderInfo headerResult = header.DrawHeader(useCustomEdit);

            GUILayout.Box("", GUILayout.Height(1), GUILayout.ExpandWidth(true));
            //Body
            EditorGUI.BeginDisabledGroup(headerResult.IsReadOnly);
            EditorGUI.BeginChangeCheck();

            // if(headerResult.UseCustomEdit)
            // {

            // }
            string text = editor.DrawEditor(lastText);

            if(EditorGUI.EndChangeCheck())
            {
                lastText = text;
                isDirty = true;
            }
            GUILayout.BeginHorizontal();
            GUI.enabled = isDirty;
            if(GUILayout.Button("저장"))
            {
                UpdateAsset(targetAsset);
            }
            // if(GUILayout.Button("REVERT"))
            // {
            //     lastText = initialText;
            //     isDirty = false;
            // }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
            GUILayout.EndVertical();
        }

        private void UpdateAsset(TextAsset textAsset)
        {
            var path = AssetDatabase.GetAssetPath(textAsset);
            File.WriteAllText(path, lastText);

            isDirty = false;

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }
}
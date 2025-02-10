using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace CHG.Editor.Texts
{
    /// <summary>
    /// Inspector에서 직접 Text Asset을 수정할 수 있는 Editor Class
    /// </summary>
    
    [CustomEditor(typeof(TextAsset))]
    public class DirectTextEditor : UnityEditor.Editor
    {
        private DirectTextEditHeader header = new DirectTextEditHeader();
        private DirectTextEditFooter footer = new DirectTextEditFooter();

        private ITextAssetEditor[] customEditors =
        {
            new CSVTextEditor(),
            new JSONTextEditor()
        };
        private ITextAssetEditor defaultEditor = new CommonTextEditor();


        private TextAsset targetAsset;
        private FileInfo targetFileInfo;
        private string initialText;
        private string lastText;

        private bool isBodyFold = false;
        private bool isFootFold = false;

        private bool isDirty = false;

        private void OnEnable() {
            targetAsset = (TextAsset)target;
            initialText = targetAsset.text;
            lastText = initialText;

            isDirty = false;

            ChangeTarget();
        }
        void ChangeTarget()
        {
            header?.OnTargetChanged();
            for(int i = 0; i < customEditors.Length; ++i)
            {
                customEditors[i]?.OnTargetChanged();
            }
            defaultEditor.OnTargetChanged();

            isBodyFold = true;

            string path = AssetDatabase.GetAssetPath(target);
            try
            {
                targetFileInfo = new FileInfo(path);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return;
            }
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
            
            ITextAssetEditor editor = defaultEditor;
            for(int i = 0; i < customEditors.Length; ++i)            
            {
                if(customEditors[i].IsTarget(textAsset))
                {
                    editor = customEditors[i];
                    break;
                }
            }

            DrawInspectorGUI(editor, textAsset, editor != defaultEditor);

            GUI.enabled = wasEnabled;
        }
        void DrawInspectorGUI(ITextAssetEditor editor, TextAsset asset, bool useCustomEdit)
        {
            //Header
            GUILayout.BeginVertical();
            HeaderInfo headerResult = header.DrawHeader(useCustomEdit);

            //GUILayout.Box("", GUILayout.Height(1), GUILayout.ExpandWidth(true));
            EditorGUILayout.Space(10);
            //Body
            isBodyFold = EditorGUILayout.Foldout(isBodyFold, "텍스트 데이터", true);
            if(isBodyFold)
            {
                EditorGUI.BeginDisabledGroup(targetFileInfo.IsReadOnly);
                EditorGUI.BeginChangeCheck();

                string text = "";
                if(headerResult.UseCustomEdit)
                {
                    text = editor.DrawEditor(lastText);                
                }
                else
                {
                    text = defaultEditor.DrawEditor(lastText);
                }

                if(EditorGUI.EndChangeCheck())
                {
                    lastText = text;
                    isDirty = true;
                }
                GUILayout.BeginHorizontal();
                GUI.enabled = isDirty && !targetFileInfo.IsReadOnly;
                if(GUILayout.Button("텍스트 저장"))
                {
                    UpdateAsset(targetAsset);
                }
                if(GUILayout.Button("다른 이름으로 저장"))
                {
                    SaveAs();
                }
                GUILayout.EndHorizontal();
                GUI.enabled = true;
                EditorGUI.EndDisabledGroup();
            }
            isFootFold = EditorGUILayout.Foldout(isFootFold, "파일 설정", true);
            if(isFootFold)
            {
                //Footer
                footer.DrawFoot(targetFileInfo);
            }

            GUILayout.EndVertical();
        }
        private void SaveAs()
        {
            
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
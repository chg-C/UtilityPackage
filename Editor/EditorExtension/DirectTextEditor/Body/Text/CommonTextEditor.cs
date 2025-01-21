using UnityEngine;
using UnityEditor;

namespace CHG.Editor.Texts
{
    public class CommonTextEditor : ITextAssetEditor
    {
        public bool UseCustomHeader => false;

        public string DrawEditor(string text)
        {
            var style = new GUIStyle(EditorStyles.textArea)
            {
                wordWrap = true
            };

            return EditorGUILayout.TextArea(text, style);
        }

        public bool IsTarget(TextAsset textAsset)
        {
            return true;
        }

        public void OnTargetChanged()
        {
        }
    }
}
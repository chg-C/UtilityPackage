using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CHG.Editor.Texts
{
    public class CSVTextEditor : ITextAssetEditor
    {
        public bool UseCustomHeader => true;
        bool isFirstLineHeader = true;
        
        public string DrawEditor(string text)
        {
            isFirstLineHeader = GUILayout.Toggle(isFirstLineHeader, "첫 줄은 헤더");
            CSVSplitter newSplitter = (CSVSplitter)EditorGUILayout.EnumPopup("텍스트 구분자", splitter);
            if(splitter != newSplitter)
            {
                splitter = newSplitter;
            }

            return text;
        }
        void DrawCSVRow(List<string> fields, StringBuilder builder)
        {
            
        }

        public bool IsTarget(TextAsset textAsset)
        {
            string path = AssetDatabase.GetAssetPath(textAsset);
            if(path != null && path.EndsWith(".csv"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        CSVSplitter splitter = CSVSplitter.Comma;
        List<string> header;
        List<List<string>> body;
        enum CSVSplitter {Comma, Semicolon, Tab, VerticalBar, Space, Colon}
        char SplitterToChar(CSVSplitter splitter)
        {
            switch(splitter)
            {
                default:
                case CSVSplitter.Comma:
                    return ',';
                case CSVSplitter.Semicolon:
                    return ';';
                case CSVSplitter.Tab:
                    return '\t';
                case CSVSplitter.VerticalBar:
                    return '|';
                case CSVSplitter.Space:
                    return ' ';
                case CSVSplitter.Colon:
                    return ':';
            }
        }

        public void OnTargetChanged()
        {
            isFirstLineHeader = true;
        }
    }
}
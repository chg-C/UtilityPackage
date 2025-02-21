using UnityEngine;
using UnityEditor;

namespace CHG.Editor.Texts
{
    public class JSONTextEditor : ITextAssetEditor
    {
        public bool UseCustomHeader => true;

        public bool IsTarget(TextAsset textAsset)
        {
            string path = AssetDatabase.GetAssetPath(textAsset);
            if(path != null && path.EndsWith(".json"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string DrawEditor(string text)
        {
            
            return text;
        }

        public void OnTargetChanged()
        {
            
        }
    }
}
using UnityEngine;
using UnityEditor;

namespace CHG.Utilities.Texts
{
    public class JSONTextEditor : ITextAssetEditor
    {
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
            
            return "";
        }
    }
}
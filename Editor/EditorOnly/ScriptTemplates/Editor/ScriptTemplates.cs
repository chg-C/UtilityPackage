using CHG.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace CHG.Editor.ScriptTemplator
{
    public sealed class ScriptTemplates : ScriptableObject
    {
        public const string SettingPath = "Assets/Settings/ScriptTemplates";
        public const string SettingName = "ScriptTemplatesSetting";
        public const string FullName = SettingPath + "/" + SettingName + ".asset";
        
        public ScriptTemplateKeywords keywordsData;
        public string templatesPath;
        
        public int priority = 80;

        void Reset()
        {
            Debug.Log("Resetting");
            keywordsData = null;
            templatesPath = null;

            Init();
        }        
        public void Init()
        {
            if(keywordsData == null)
            {
                keywordsData = AssetDatabase.LoadAssetAtPath<ScriptTemplateKeywords>(SettingPath+"/Keywords/ScriptTemplateKeywords.asset");
                if(keywordsData == null)
                {
                    keywordsData = CreateInstance<ScriptTemplateKeywords>();
                    RecursiveFolderGenerator.CreateFolderRecursively(SettingPath+"/Keywords");
                    AssetDatabase.CreateAsset(keywordsData, SettingPath+"/Keywords/ScriptTemplateKeywords.asset");
                    AssetDatabase.SaveAssets();
                }
            }
            if(!AssetDatabase.IsValidFolder(templatesPath))
            {
                if(!AssetDatabase.IsValidFolder(SettingPath+"/TemplateFiles"))
                    AssetDatabase.CreateFolder(SettingPath, "TemplateFiles");
                
                templatesPath = SettingPath+"/TemplateFiles";
            }
        }
    }
}
using System.IO;
using CHG.Utilities.Datas;
using UnityEditor;
using UnityEngine;



namespace CHG.Utilities.Bootstrap.Editor
{
    public static class BootstrapSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateBootstrapProvider()
        {
            Bootstrap bootstrap = Bootstrap.FindBootstrap();
            if(bootstrap == null)
            {
                Debug.LogWarning("Generate Default Bootstrap Setting On Assets/Resources/"
                     + Bootstrap.SettingPath);
                RecursivoeFolderGenerator.CreateFolderRecursively("Assets/Resources/" + Bootstrap.SettingPath);
                bootstrap = ScriptableObject.CreateInstance<Bootstrap>();
                bootstrap.Profile = GenerateDefaultProfile();
                AssetDatabase.CreateAsset(bootstrap, "Assets/Resources/" + Bootstrap.SettingPath + "/" + Bootstrap.DefaultName + ".asset");
                AssetDatabase.CreateAsset(bootstrap.Profile, "Assets/Resources/"+Bootstrap.SettingPath+"/" + BootstrapProfile.DefaultName+".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
            return CreateProvider("Project/Bootstrap", bootstrap);
        }
        [MenuItem("Tools/CHG/Bootstrap")]
        static void OpenBootstrap()
        {
            EditorApplication.ExecuteMenuItem("Project/Bootstrap");
        }
        static BootstrapProfile GenerateDefaultProfile()
        {
            BootstrapProfile defaultProfile = ScriptableObject.CreateInstance<BootstrapProfile>();
            AssetDatabase.CreateAsset(defaultProfile, "Assets/Resources/"+Bootstrap.SettingPath+"/" + BootstrapProfile.DefaultName+".asset");
            AssetDatabase.SaveAssets();

            return defaultProfile;
        }
        static SettingsProvider CreateProvider(string settingsWindowPath, Object asset)
        {
            var provider = AssetSettingsProvider.CreateProviderFromObject(settingsWindowPath, asset);

            provider.keywords = SettingsProvider.GetSearchKeywordsFromSerializedObject(new SerializedObject(asset));

            return provider;
        }
    }
}

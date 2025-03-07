using UnityEditor;
using UnityEngine;

using CHG.Editor.Utilities;

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
                bootstrap = InitBootstrap();
                
            }
            
            return CreateProvider("Project/CHG/Bootstrap", bootstrap);
        }
        public static Bootstrap InitBootstrap()
        {
            Debug.LogWarning("Generate Default Bootstrap Setting On Assets/Resources/"
                    + Bootstrap.SettingPath);
            RecursiveFolderGenerator.CreateFolderRecursively("Assets/Resources/" + Bootstrap.SettingPath);
            Bootstrap bootstrap = ScriptableObject.CreateInstance<Bootstrap>();
            bootstrap.Profile = GenerateDefaultProfile();
            AssetDatabase.CreateAsset(bootstrap, "Assets/Resources/" + Bootstrap.SettingPath + "/" + Bootstrap.SettingName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return bootstrap;
        }

        [MenuItem("Tools/CHG/Bootstrap", priority = 62)]
        static void OpenBootstrap()
        {
            SettingsService.OpenProjectSettings("Project/CHG/Bootstrap");
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
